
namespace LedArrayVideoRenderer.Domain
{
    public class LED 
    {
        public int X, Y; //seems overly simple, but this makes it easier to itterating through the vest, and also enables scalability to video of different sizes.

        public LED(int _x, int _y)
        {
            X = _y;
            Y = _x;
        }

        public void Factor(double factorX, double factorY)
        {

            X = (int) (X * factorX) ;
            Y =(int) (Y * factorY) +150;
        }
    }
}
