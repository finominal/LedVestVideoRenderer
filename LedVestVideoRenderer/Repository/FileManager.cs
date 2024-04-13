using System.IO;
using System.Text;

namespace LedArrayVideoRenderer.Repository
{
    public class FileManager
    {
        public  void WriteBufferToFile(string fileName, byte[] data, int ledCount,int capturedFramesCount)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var header = CreateHeaderFinController(ledCount); 
            var fileSize = (capturedFramesCount * ledCount * 3); //if skipping frames, the data will be smaller than the buffer

            using (var file = File.Create(fileName))
            {
                file.Write(header,0,header.Length);
                file.Write(data, 0, fileSize);
            }
        }

        private  byte[] CreateHeaderFinController(int number)
        {
            byte[] result = new byte[2];
            result[1] = (byte)number;
            result[0] = (byte)(number >> 8); //msb get pulled off first and pushed into an integer. 

            return result;
        }

        public void WriteBufferToStringArray(string fileName, byte[] data, int ledCount, int capturedFramesCount)
        {
            var stringBuilderMaster = new StringBuilder();

            stringBuilderMaster.AppendLine("{"); //open

            //build rows
            for (int frame = 0; frame < capturedFramesCount; frame++)
            {
                var frameLocation = frame * ledCount * 3;
                var rowBuilder = new StringBuilder();

                rowBuilder.Append("{"); //start row
                rowBuilder.Append("{"); //start row
                rowBuilder.Append("{"); //start row
                rowBuilder.Append("{"); //start row
                rowBuilder.Append("{"); //start row

                for (int led = 0; led < ledCount; led++)
                {
                    var ledStartLocation = led * 3;

                    rowBuilder.Append( data[frameLocation + ledStartLocation] + ",");
                    rowBuilder.Append( data[frameLocation + ledStartLocation + 1] + ",");
                    rowBuilder.Append( data[frameLocation + ledStartLocation + 2] ); 
                        if (led < ledCount - 1) rowBuilder.Append(",");//all but the last byte in this frame

                }

                rowBuilder.Append("}"); //end row

                if (frame < capturedFramesCount - 1) rowBuilder.Append(",");//all but the last row

                stringBuilderMaster.AppendLine(rowBuilder.ToString()); //Commit this line! 
            }

            stringBuilderMaster.AppendLine("};"); //close the array

            //write
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                    outputFile.Write(stringBuilderMaster.ToString());
            }
        }
    }
}
