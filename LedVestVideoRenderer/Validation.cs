using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LedVestVideoRenderer
{
    public static class Validation
    {

        public static Boolean IsValidBrightness(string text)
        {
            return text.IsNormalized();
        }

    }
}
