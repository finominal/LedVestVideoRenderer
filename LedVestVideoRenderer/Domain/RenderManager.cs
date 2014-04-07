using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using LedVestVideoRenderer.Repository;

namespace LedVestVideoRenderer.Domain
{
    public class RenderManager
    {
        private VestManager vest;
        
        private byte[] m_buffer;
        private Bitmap m_lastImg;
        private int m_capturedFrames;
        private int m_pixelX, m_pixelY; //global to avoid continuous construction
        private VideoManager m_videoManager;

        public void RenderVideoToFile(string videoFileName, string saveFileName, int maxBrightness, bool smoothen, bool checkForDuplicates, bool twoFrames)
        {
            //instantiate core objects
            m_videoManager = new VideoManager(videoFileName);
            vest = new VestManager();

            //create a window with which to display the video as frames are rendered. 
            var videoWindow = new Form { Width = m_videoManager.Width(), Height = m_videoManager.Height() };
            videoWindow.Show();

            //expand the pixel x/y co-ordinates to match the size of the video. 
            vest.FactorVest(m_videoManager.Width(), m_videoManager.Height()); 
      
            //create a data buffer to store the pixel colors
            m_buffer = new byte[(m_videoManager.FrameCount() * 470 * 3)];

            //for each frame in the video
            
            for (var frameNo = 0; frameNo < m_videoManager.FrameCount(); frameNo++)
            {
                //get the next frame from the video file
                var frameImage = m_videoManager.GetFrameBitmap(frameNo);

                if(!(twoFrames && frameNo%2 == 1)) //skip if we are skipping every second frame
                {
                    var duplicate = false;
                    if (checkForDuplicates) duplicate = IsDuplicateFrame(frameImage);//compare three pixels.

                    if (!duplicate)
                    {
                        videoWindow.BackgroundImage = frameImage;
                        videoWindow.Text = frameNo + " / " + m_videoManager.FrameCount();
                        videoWindow.Update();

                        try
                        {
                            for (var i = 0; i < vest.leds.Length; i++)
                            {
                                //go to the current location in the render buffer 
                                var loc = (m_capturedFrames*470*3) + (i*3); 

                                //get the X and Y co-ordinate from the vest led index, and flip them (video xy starts at the top)
                                m_pixelX = m_videoManager.Width() - vest.leds[i].X - 2;
                                m_pixelY = m_videoManager.Height() - vest.leds[i].Y - 2;

                                //get the pixel that coresponds to the current VEST LED XY coordinate
                                var color = frameImage.GetPixel(m_pixelX, m_pixelY);

                                //extract the color RGB, and scale down brightness to that set in the app 
                                m_buffer[loc] = Map(color.R, maxBrightness);
                                m_buffer[loc + 1] = Map(color.G, maxBrightness);
                                m_buffer[loc + 2] = Map(color.B, maxBrightness);

                                if (smoothen) //get another nearby pixel and average them
                                {

                                    loc = (m_capturedFrames*470*3) + (i*3);
                                    m_pixelX = m_videoManager.Width() - vest.leds[i].X - 3;
                                    m_pixelY = m_videoManager.Height() - vest.leds[i].Y - 3;
                                    var color2 = frameImage.GetPixel(m_pixelX, m_pixelY);

                                    m_buffer[loc] = (byte) ((Map(color2.R, maxBrightness)/2) + (m_buffer[loc]/2));
                                    m_buffer[loc + 1] = (byte) ((Map(color2.G, maxBrightness)/2) + (m_buffer[loc + 1]/2));
                                    m_buffer[loc + 2] = (byte) ((Map(color2.B, maxBrightness)/2) + (m_buffer[loc + 2]/2));
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Render Error: pixelX = " + m_pixelX.ToString(CultureInfo.InvariantCulture) + ", pixelY = " +
                                                m_pixelY.ToString(CultureInfo.InvariantCulture) + " Ex = " + e);
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

        private bool IsDuplicateFrame(Bitmap image)
        {
            var result = (m_lastImg == image);
            m_lastImg = image;
            return result;
       /*
            var newFrameColors = new byte[9];

            for (var i = 0; i < 3; i++)
            {
                newFrameColors[i + 0] = image.GetPixel(image.Width / 2, (image.Height / (1 + i)) - 1).R;
                newFrameColors[i+1] = image.GetPixel(image.Width/2, (image.Height/(1+i))-1).G;
                newFrameColors[i+2] = image.GetPixel(image.Width/2, (image.Height/(1+i))-1).B;
            }

            var x = newFrameColors.SequenceEqual(m_lastFrameColors);

            m_lastFrameColors = newFrameColors;
            return x;
        */
        }

        private void WriteBufferToFile(string saveFileName)
        {
            var copyBuffer = new byte[(m_capturedFrames - 1) * 470 * 3];
            for (int i = 0; i < copyBuffer.Length; i++)
            {
                copyBuffer[i] = m_buffer[i];
            }

            FileManager.WriteBufferToFile(saveFileName, copyBuffer);
        }

        private static byte Map(double source, double newMaximum)
        {
            var newPercentage = newMaximum / 255;
            return (byte)(source * newPercentage);
        }
    }
}
