using System;
using System.Drawing;
using LedArrayVideoRenderer.Interface;

namespace LedArrayVideoRenderer.Domain
{
    class VideoManager
    {
        private IVideoContainer video;

        public VideoManager(string fileName)
        {
            switch(ResolveFileExtension(fileName))
            {
                case "avi":
                    video = new AviVideoContainer(fileName);
                    //video = new Mp4VideoContainer(fileName);
                    break;
                case "mp4":
                    video = new Mp4VideoContainer(fileName);
                    break;
                default:
                    throw new Exception("The video format is not able to be processed.");
                   
            }
        }

        internal double FrameRate()
        {
            return video.FrameRate();
        }

        internal int FrameCount()
        {
            return video.FrameCount();
        }

        internal Bitmap GetFrameBitmap(int frameNumber)
        {
            return video.GetFrameBitmap(frameNumber);
        }

        internal int Width() 
        {
            return video.Width();
        }

        internal int Height()
        {
            return video.Height();
        }

        private string ResolveFileExtension(string fileName)
        {
            var result = string.Empty;
            
            var endOfFile = fileName.Length - 1;
            if (fileName.Contains("."))
            {
                result = fileName.Substring(endOfFile-2,3);
            }

            return result;
        }
    } 
}
