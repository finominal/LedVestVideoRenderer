using System.Collections;

namespace LedVestVideoRenderer.Domain
{
    public class LED 
    {
        public int X, Y;

        public LED(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }

        public void Factor(double factorX, double factorY)
        {

            X *= (int)(factorX );
            Y *= (int)(factorY );
            
        }
    }
}
