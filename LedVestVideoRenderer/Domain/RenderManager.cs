using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using LedArrayVideoRenderer.Repository;

namespace LedArrayVideoRenderer.Domain
{
    public class RenderManager
    {
        private LedManager m_LedManager;
        private VideoManager m_videoManager;        
        private byte[] m_buffer1;
        private byte[] m_buffer2;
        private Bitmap m_lastImg;
        private int m_capturedFrames;
        private int m_pixelX, m_pixelY; //global to avoid continuous construction
        private Form videoWindow;

        public void RenderVideoToFile(string videoFileName, string ledIndexFileName,  string saveFileName, int maxBrightness, bool smoothen, bool checkForDuplicates, bool twoFrames)
        {
            //This method manages the extraction of data from the video, and triggers the execution of the file. 
            //It can manage up to TWO controllers. 
            //instantiate core objects

            m_videoManager = new VideoManager(videoFileName); //chooses the codex based on the file type
            m_LedManager = new LedManager(ledIndexFileName);  //imports the LED index

            if (m_LedManager.ImportOk)
            {
                //setup
                SetBufferSizes();
                CreateVideoWindow();
                RefactorLedsToFitVideo();

                //main routine
                ExtractPixelsFromVideo(maxBrightness, smoothen, checkForDuplicates, twoFrames);

                //conclusion
                videoWindow.Close();
                WriteBufferToFile(saveFileName);
            }
        }

        private void ExtractPixelsFromVideo(int maxBrightness, bool smoothen, bool checkForDuplicates, bool twoFrames)
        {
            //for each frame in the video
            for (var frameNo = 0; frameNo < m_videoManager.FrameCount(); frameNo++)
            {
                //get the next frame from the video file
                var frameImage = m_videoManager.GetFrameBitmap(frameNo);

                if (!(twoFrames && frameNo%2 == 1)) //skip if we are skipping every second frame
                {
                    var duplicate = false;
                    if (checkForDuplicates)
                        duplicate = IsDuplicateFrame(frameImage);
                            //compare to last frame, this helps prevent glitchyness when a frame is extracted twice. 

                    if (!duplicate)
                    {
                        videoWindow.BackgroundImage = frameImage;
                        videoWindow.Text = frameNo + " / " + m_videoManager.FrameCount();
                        videoWindow.Update();
                        Application.DoEvents();

                        //get the individual pixels
                        try
                        {
                            if (m_LedManager.numberOfControllers == 1)
                            {
                                ExtractPixelsFromFrame(maxBrightness, smoothen, frameImage, frameNo, 0, m_LedManager.leds.Count,m_buffer1);
                            }
                            if (m_LedManager.numberOfControllers == 2)
                            {
                                ExtractPixelsFromFrame(maxBrightness, smoothen, frameImage, m_capturedFrames, 0, m_LedManager.secondControllerStartsAt, m_buffer1);
                                ExtractPixelsFromFrame(maxBrightness, smoothen, frameImage, m_capturedFrames, m_LedManager.secondControllerStartsAt, m_LedManager.leds.Count, m_buffer2);
                            }
                        }
                        catch (Exception e)
                        {
                            videoWindow.Close();
                            using (var file = File.AppendText("./log.txt"))
                            {
                                file.Write(ErrorMessageString(e, frameNo));
                            }

                            throw new Exception(ErrorMessageString(e, frameNo));
                        }

                        //dispose of the current objects. 
                        frameImage.Dispose();
                        m_capturedFrames++; //nececary to skip duplicate frames. 
                    }
                }
            }
        }

        private void ExtractPixelsFromFrame(int maxBrightness, bool smoothen, Bitmap frameImage, int frameNo, int ledStartIndex, int ledEndIndex, byte[] _ledBuffer )
        {
            //Main Code to get the colors from the video based on the X/Y co-ordinates from the index.
            try
            {


            for (var i = 0; i < ledEndIndex - ledStartIndex; i++)
            {
                //go to the current location in the render buffer 
                //add two for the header bytes
                var loc = (frameNo * (ledEndIndex - ledStartIndex) * 3) + (i * 3) ;

                //get the X and Y co-ordinate from the vest led index, and flip them (video xy starts at the top)
                m_pixelX = ((m_videoManager.Width()-1) - m_LedManager.leds[i].X) ;
                m_pixelY =( (m_videoManager.Height()-1) - m_LedManager.leds[i].Y) ;

                //DEV - Catch Refactoring Indexing issues
                if (m_pixelX < 0 || m_pixelY < 0
                    || m_pixelX >= m_videoManager.Width() || m_pixelY >= m_videoManager.Height())
                {
                    var a = m_videoManager.Width();
                    var b =  m_LedManager.leds[i].X;
                    var c = a - b ;
                    var d = m_videoManager.Height();
                    var e =  m_LedManager.leds[i].Y;
                    var f = d - e ;
                    break;
                }

                //get the pixel that coresponds to the current VEST LED XY coordinate
                var color = frameImage.GetPixel(m_pixelX, m_pixelY);

                //extract the color RGB, and scale down brightness to that set in the app 
                _ledBuffer[loc] = Map(color.R, maxBrightness);
                _ledBuffer[loc + 1] = Map(color.G, maxBrightness);
                _ledBuffer[loc + 2] = Map(color.B, maxBrightness);

                if (smoothen) //get another nearby pixel and average them
                {
                    loc = (m_capturedFrames*470*3) + (i*3);
                    m_pixelX = m_videoManager.Width() - m_LedManager.leds[i].X - 5;
                    m_pixelY = m_videoManager.Height() - m_LedManager.leds[i].Y - 5;
                    var color2 = frameImage.GetPixel(m_pixelX, m_pixelY);

                    _ledBuffer[loc] = (byte)((Map(color2.R, maxBrightness) / 2) + (_ledBuffer[loc] / 2));
                    _ledBuffer[loc + 1] = (byte)((Map(color2.G, maxBrightness) / 2) + (_ledBuffer[loc + 1] / 2));
                    _ledBuffer[loc + 2] = (byte)((Map(color2.B, maxBrightness) / 2) + (_ledBuffer[loc + 2] / 2));
                }
            }
            }
            catch (Exception e)
            {
                var x = e;
            }
        }

        private string ErrorMessageString(Exception e, int frameNo)
        {
            return DateTime.Now.ToLongDateString() + " Render Error: pixelX = " +
                   m_pixelX.ToString(CultureInfo.InvariantCulture) + ", pixelY = " +
                   m_pixelY.ToString(CultureInfo.InvariantCulture) + ", frameNo = " +
                   frameNo.ToString(CultureInfo.InvariantCulture) + " Ex = " + e;
        }

        private void CreateVideoWindow()
        {
            //create a window with which to display the video as frames are rendered. 
            videoWindow = new Form { Width = m_videoManager.Width(), Height = m_videoManager.Height() };
            videoWindow.Show();
        }

        private void RefactorLedsToFitVideo()
        {
            //expand the pixel x/y co-ordinates to match the size of the video. 
            m_LedManager.FactorLeds(m_videoManager.Width(), m_videoManager.Height());
        }

        private void SetBufferSizes()
        {
            //setup the arrays and program flow based on number of controllers
            //This buffer size does not include the header, that is calculated at file creation time.   
            if (m_LedManager.numberOfControllers == 1)
            {
                m_buffer1 = new byte[(m_videoManager.FrameCount()*m_LedManager.leds.Count*3)];
            }
            else if (m_LedManager.numberOfControllers == 2)
            {
                m_buffer1 = new byte[(m_videoManager.FrameCount()*m_LedManager.secondControllerStartsAt*3)];
                m_buffer2 = new byte[(m_videoManager.FrameCount()*(m_LedManager.leds.Count - m_LedManager.secondControllerStartsAt)*3)];
                

            }
        }

        private bool IsDuplicateFrame(Bitmap image)
        {
            var result = (m_lastImg == image); 
            m_lastImg = image;//set for next check;
            return result;
        }

        private void WriteBufferToFile(string saveFileName)
        {
            if (m_LedManager.numberOfControllers == 1)
            {
                FileManager.WriteBufferToFile(saveFileName, m_buffer1, m_LedManager.leds.Count, m_capturedFrames);
            }
            if (m_LedManager.numberOfControllers == 2)
            {
                FileManager.WriteBufferToFile(saveFileName.Substring(0, saveFileName.Length - 4) + "-1.led", m_buffer1, m_LedManager.secondControllerStartsAt, m_capturedFrames);

                var fileName2 = saveFileName.Substring(0, saveFileName.Length - 4) + "-2.led";
                FileManager.WriteBufferToFile(fileName2, m_buffer2, m_LedManager.leds.Count - m_LedManager.secondControllerStartsAt, m_capturedFrames);
            }
        }

        private static byte Map(double source, double newMaximum)
        {
            var newPercentage = newMaximum / 255;
            return (byte)(source * newPercentage);
        }
    }
}
