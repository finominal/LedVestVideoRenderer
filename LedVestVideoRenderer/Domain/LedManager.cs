using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LedArrayVideoRenderer.Domain
{
    public class LedManager
    {
        public IList<LED> leds = new List<LED>();
        public int worldWidth, worldHeight;
        public int numberOfControllers;
        public int secondControllerStartsAt;
        public bool ImportOk;

        public LedManager(string ledIndexFileName)
        {
            InitializeLeds(ledIndexFileName);
        }

        public int LedCount => leds.Count;
        public void FactorLeds(double videoWidth, double videoHeight)
        {
            double widthFactor = (videoWidth*0.95) / worldWidth ; //plus two to help keep the expansion inside the width. 
            double heightFactor = (videoHeight*0.95) / worldHeight ;

            foreach (LED i in leds)
            {
                i.Factor(widthFactor, heightFactor);
            }
        }

        public void InitializeLeds(string ledFileName)
        {
            using (var reader = new StreamReader(File.OpenRead(ledFileName)))
            {
                ImportOk = false;
                try
                {
                    //first get the header, containing width and height info (helps with multiple controllers)
                    var header = reader.ReadLine();
                    var wh = header.Split(',');
                    numberOfControllers = int.Parse(wh[0]);
                    if (wh.Count() > 1) secondControllerStartsAt = int.Parse(wh[1]);

                    //get all the data into the led array
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        var led = new LED(int.Parse(values[1]), int.Parse(values[0]));
                        leds.Add(led);
                    }

                    //set the world parameters
                    worldWidth = GetWidth(); //strech by 5 to keep the leds inside the boarder, neat
                    worldHeight = GetHeight();

                    ImportOk = true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(@"There was an error importing the LED file. It must be a two column csv." + e);
                }
            }

        }

        private int GetWidth()
        {
            return leds.Max(led => led.X);
        }

        private int GetHeight()
        {
            return leds.Max(led => led.Y);
        }

    }
}
