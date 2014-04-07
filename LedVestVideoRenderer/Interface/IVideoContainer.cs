
using System.Drawing;

namespace LedVestVideoRenderer.Interface
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
