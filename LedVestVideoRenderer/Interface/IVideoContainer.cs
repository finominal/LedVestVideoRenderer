
using System.Drawing;

namespace LedArrayVideoRenderer.Interface
{
    interface IVideoContainer
    {
        double FrameRate();
        int FrameCount();
        Bitmap GetFrameBitmap(int frameNumber);
        int Width();
        int Height();
    }
}
