using System;
using System.Drawing;
using System.Globalization;
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

        public void RenderVideoToFile(string videoFileName, string ledIndexFileName,  string saveFileName, int maxBrightness, bool smoothen, bool checkForDuplicates, bool twoFrames)
        {
            //instantiate core objects
            m_videoManager = new VideoManager(videoFileName);
            m_LedManager = new LedManager(ledIndexFileName);

            if (m_LedManager.ImportOk)
            {
                SetBufferSizes();

                var videoWindow = CreateVideoWindow();

                RefactorLedsToFitVideo();

                SetLedCountInBufferByte();

                //for each frame in the video
                for (var frameNo = 0; frameNo < m_videoManager.FrameCount(); frameNo++)
                {
                    //get the next frame from the video file
                    var frameImage = m_videoManager.GetFrameBitmap(frameNo);

                    if (!(twoFrames && frameNo%2 == 1)) //skip if we are skipping every second frame
                    {
                        var duplicate = false;
                        if (checkForDuplicates) duplicate = IsDuplicateFrame(frameImage); //compare to last frame, this helps prevent glitchyness when a frame is extracted twice. 

                        if (!duplicate)
                        {
                            videoWindow.BackgroundImage = frameImage;
                            videoWindow.Text = frameNo + " / " + m_videoManager.FrameCount();
                            videoWindow.Update();

                            try
                            {
                                if (m_LedManager.numberOfControllers == 1)
                                {
                                    ExtractPixelsFromFrame(maxBrightness, smoothen, frameImage, 0, m_LedManager.leds.Count, m_buffer1);
                                }
                                if (m_LedManager.numberOfControllers == 2)
                                {
                                    ExtractPixelsFromFrame(maxBrightness, smoothen, frameImage, 0, m_LedManager.secondControllerStartsAt, m_buffer1);
                                    ExtractPixelsFromFrame(maxBrightness, smoothen, frameImage, m_LedManager.secondControllerStartsAt, m_LedManager.leds.Count, m_buffer2);
                                    
                                }
                            }
                            catch (Exception e)
                            {
                                throw new Exception("Render Error: pixelX = " +
                                                    m_pixelX.ToString(CultureInfo.InvariantCulture) + ", pixelY = " +
                                                    m_pixelX.ToString(CultureInfo.InvariantCulture) + ", frameNo = " +
                                                    frameNo.ToString(CultureInfo.InvariantCulture) + " Ex = " + e);
                            }
                            //dispose of the current objects. 
                            frameImage.Dispose();
                            m_capturedFrames++;
                        }
                    }
                }
                videoWindow.Close();
                WriteBufferToFile(saveFileName);
            }
        }

        private void SetLedCountInBufferByte()
        {
            //The first two bytes in the protocol is the led count, used by the playback controller 
            var highbyte = (uint)m_LedManager.leds.Count;
            m_buffer1[1] = (byte)highbyte;//lsb
            m_buffer1[0] = (byte)(highbyte>>8);//msb
            
            if(m_LedManager.numberOfControllers == 2)
            {
                highbyte = (uint)(m_LedManager.leds.Count - m_LedManager.secondControllerStartsAt);
                m_buffer2[1] = (byte)(highbyte);
                m_buffer2[0] = (byte)(highbyte>>8); 
            }
        }

        private void ExtractPixelsFromFrame(int maxBrightness, bool smoothen, Bitmap frameImage, int ledStartIndex, int ledEndIndex, byte[] _ledBuffer )
        {
            //advance the index location one to make room for the header byte

            for (var i = ledStartIndex; i < ledEndIndex; i++)
            {
                //go to the current location in the render buffer 
                //add two for the header bytes
                var loc = (m_capturedFrames*m_LedManager.leds.Count*3) + (i*3) +2;

                //get the X and Y co-ordinate from the vest led index, and flip them (video xy starts at the top)
                m_pixelX = m_videoManager.Width() - m_LedManager.leds[i].X - 2;
                m_pixelY = m_videoManager.Height() - m_LedManager.leds[i].Y - 2;

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

        private Form CreateVideoWindow()
        {
            //create a window with which to display the video as frames are rendered. 
            var videoWindow = new Form {Width = m_videoManager.Width(), Height = m_videoManager.Height()};
            videoWindow.Show();
            return videoWindow;
        }

        private void RefactorLedsToFitVideo()
        {
            //expand the pixel x/y co-ordinates to match the size of the video. 
            m_LedManager.FactorLeds(m_videoManager.Width(), m_videoManager.Height());
        }

        private void SetBufferSizes()
        {
            //setup the arrays and program flow based on number of controllers
            //plus 2 is to reserve space for the led count which is the first item in the protocol.
            if (m_LedManager.numberOfControllers == 1)
            {
                m_buffer1 = new byte[(m_videoManager.FrameCount()*m_LedManager.leds.Count*3)+2];
            }
            else if (m_LedManager.numberOfControllers == 2)
            {
                m_buffer1 = new byte[(m_videoManager.FrameCount()*m_LedManager.secondControllerStartsAt*3)+2];
                m_buffer2 =
                    new byte[(m_videoManager.FrameCount()*(m_LedManager.leds.Count - m_LedManager.secondControllerStartsAt)*3)+2];
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
                FileManager.WriteBufferToFile(saveFileName, m_buffer1, m_LedManager.leds.Count);
            }
            if (m_LedManager.numberOfControllers == 2)
            {
                FileManager.WriteBufferToFile(saveFileName, m_buffer1, m_LedManager.secondControllerStartsAt);

                var fileName2 = saveFileName.Substring(0, saveFileName.Length - 4) + "-2.led";
                FileManager.WriteBufferToFile(fileName2, m_buffer2, m_LedManager.leds.Count - m_LedManager.secondControllerStartsAt);
            }
        }

        private static byte Map(double source, double newMaximum)
        {
            var newPercentage = newMaximum / 255;
            return (byte)(source * newPercentage);
        }
    }
}
