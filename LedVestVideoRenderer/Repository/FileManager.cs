


using System.IO;

namespace LedArrayVideoRenderer.Repository
{
    public static class FileManager
    {
        public static void WriteBufferToFile(string fileName, byte[] data, int ledCount,int capturedFramesCount)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var header = CreateHeader(ledCount); 
            var fileSize = 2 + (capturedFramesCount * ledCount * 3); //if skipping frames, the data will be smaller than the buffer

            using (var file = File.Create(fileName))
            {
                file.Write(header,0,header.Length);
                file.Write(data, 0, fileSize);
            }
        }

        private static byte[] CreateHeader(int number)
        {
            byte[] result = new byte[2];
            result[1] = (byte)number;
            result[0] = (byte)(number >> 8);
            return result;
        }
    }
}
