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
        private LedManager _ledManager;
        private VideoManager _videoManager;
        private FileManager _fileManager;

        private byte[] m_buffer1;
        private int m_capturedFrames;
        private int m_pixelX, m_pixelY; //global to avoid continuous construction
        private Form videoWindow;

        public void RenderVideoToFile(string videoFileName, string ledIndexFileName, string saveFileName, int maxBrightness)
        {
            //move to DI
            _videoManager = new VideoManager(videoFileName); //chooses the codex based on the file type
            _ledManager = new LedManager(ledIndexFileName);  //imports the LED index
            _fileManager = new FileManager();

            if (_ledManager.ImportOk)
            {
                //setup
                SetBufferSizes();
                CreateVideoWindow();
                RefactorLedsToFitVideo();

                //main routine
                ExtractPixelsFromVideo(maxBrightness);

                //conclusion
                videoWindow.Close();

                //_fileManager.WriteBufferToFile(saveFileName, m_buffer1, _ledManager.LedCount, m_capturedFrames);
                _fileManager.WriteBufferToStringArray(saveFileName, m_buffer1, _ledManager.LedCount, m_capturedFrames);
            }
        }

        private void ExtractPixelsFromVideo(int maxBrightness)
        {
            //for each frame in the video
            for (var frameNo = 0; frameNo < _videoManager.FrameCount(); frameNo++)
            {

                try
                {
                    //get the next frame from the video file
                    var frameImage = _videoManager.GetFrameBitmap(frameNo);

                    ExtractPixelsFromFrame(maxBrightness, frameImage, frameNo, 0, _ledManager.LedCount, m_buffer1);

                    //dispose of the current objects. 
                    frameImage.Dispose();
                    m_capturedFrames++; //nececary to skip duplicate frames. 
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
            }
        }


        private void ExtractPixelsFromFrame(int maxBrightness, Bitmap frameImage, int frameNo, int ledStartIndex, int ledEndIndex, byte[] _ledBuffer)
        {
            //Main Code to get the colors from the video based on the X/Y co-ordinates from the index.
            try
            {
                for (var i = 0; i < ledEndIndex - ledStartIndex; i++)
                {
                    //go to the current location in the render buffer 
                    //add two for the header bytes
                    var loc = (frameNo * (ledEndIndex - ledStartIndex) * 3) + (i * 3);

                    //artcar indexing is the same as an image. 
                    m_pixelX = (_ledManager.leds[i].X); //for the artcar zero starts at the top. same as an image
                    m_pixelY = (_ledManager.leds[i].Y);

                    //get the pixel that coresponds to the current VEST LED XY coordinate
                    var color = frameImage.GetPixel(m_pixelX, m_pixelY);

                    //extract the color RGB, and scale down brightness to that set in the app 
                    _ledBuffer[loc] = Map(color.R, maxBrightness);
                    _ledBuffer[loc + 1] = Map(color.G, maxBrightness);
                    _ledBuffer[loc + 2] = 0;// Map(color.B, maxBrightness); 

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ErrorMessageString(e, frameNo));
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
            videoWindow = new Form { Width = _videoManager.Width(), Height = _videoManager.Height() };
            videoWindow.Show();
        }

        private void RefactorLedsToFitVideo()
        {
            //expand the pixel x/y co-ordinates to match the size of the video. 
            _ledManager.FactorLeds(_videoManager.Width(), _videoManager.Height());
        }

        private void SetBufferSizes()
        {
            m_buffer1 = new byte[(_videoManager.FrameCount() * _ledManager.leds.Count * 3)];
        }

        private static byte Map(double source, double newMaximum)
        {
            var newPercentage = newMaximum / 255;
            return (byte)(source * newPercentage);
        }
    }
}
