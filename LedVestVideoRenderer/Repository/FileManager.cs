using System.IO;

namespace LedVestVideoRenderer.Repository
{
    public static class FileManager 
    {        
        public static void WriteBufferToFile(string fileName, byte[] data )
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (var file = File.Create(fileName))
            {
                file.Write(data, 0, data.Length);
            }
        }
    }
}
