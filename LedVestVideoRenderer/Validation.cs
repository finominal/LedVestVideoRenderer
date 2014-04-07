using System;

namespace LedVestVideoRenderer
{
    public static class Validation
    {

        public static Boolean IsValidBrightness(string text)
        {
            return text.IsNormalized();
        }

        //TODO add inspection of value to ensure it is  0-255 numeric

    }
}
