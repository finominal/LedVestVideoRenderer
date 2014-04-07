using System.Drawing;
using LedVestVideoRenderer.Interface;
using User.DirectShow;

namespace LedVestVideoRenderer.Domain
{
    public class AviVideoContainer : IVideoContainer
    {
        private FrameGrabber frameGrabber;

        public AviVideoContainer(string fileName)
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
            return frameGrabber.GetImage(frameNumber);
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
