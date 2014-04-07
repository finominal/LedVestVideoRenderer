using System.Drawing;
using LedVestVideoRenderer.Interface;
using User.DirectShow;

namespace LedVestVideoRenderer.Domain
{
    public class Mp4VideoContainer : IVideoContainer
    {
        private FrameGrabber frameGrabber;


        public Mp4VideoContainer(string fileName)
        {
            frameGrabber = new FrameGrabber(fileName);
        }

        public double FrameRate()
        {
            return frameGrabber.FrameRate;
        }

        public int FrameCount()
        {
            return frameGrabber.FrameCount;
        }

        public Bitmap GetFrameBitmap(int frameNumber)
        {
            return frameGrabber.GetFrame(frameNumber).Image;
        }

        public int Width()
        {
            return frameGrabber.Width;
        }

        public int Height()
        {
            return frameGrabber.Height;
        }
    }
}
