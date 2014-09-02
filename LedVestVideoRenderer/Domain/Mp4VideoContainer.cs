using System;
using System.Drawing;
using LedVestVideoRenderer.Interface;
using Microsoft.Expression.Encoder;

namespace LedVestVideoRenderer.Domain
{
    public class Mp4VideoContainer : IVideoContainer
    {
        private AudioVideoFile audioVideoFile;
        private MediaItem mediaItem;
        //private MetadataCollection metadata;

        public Mp4VideoContainer(string fileName)
        {
            audioVideoFile = new AudioVideoFile(fileName);
            mediaItem = new MediaItem(fileName);
           // metadata = audioVideoFile.Metadata;
        }

        public double FrameRate()
        {
            return mediaItem.OriginalFrameRate;
        }

        public int FrameCount()
        {
            return (int)(audioVideoFile.Duration.Seconds * FrameRate());
        }

        public Bitmap GetFrameBitmap(int frameNumber)
        {
            var totalSeconds = (1/FrameRate())*frameNumber;

            int hours = (int) totalSeconds/3600;
            int minutes = (int)((totalSeconds - (hours*3600))/60);
            int seconds = (int)(totalSeconds - (hours*3600) - minutes*60);
            int milliseconds = (int)((totalSeconds % 1 + (1 / FrameRate() / 2)) * 1000);

            TimeSpan t = new TimeSpan(0, hours, minutes, seconds, milliseconds);
            Size s = new Size(Width(), Height()); 

            return audioVideoFile.GetThumbnail(t, s);
        }

        public int Width()
        {
            return mediaItem.VideoSize.Width;
        }

        public int Height()
        {
            return mediaItem.VideoSize.Height;
        }
    }
}
