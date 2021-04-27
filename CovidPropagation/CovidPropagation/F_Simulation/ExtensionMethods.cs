using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    static class ExtensionMethods
    {
        public static int ConvertToInt(this bool value)
        {
            return value ? 1 : 0;
        }
    }
}
