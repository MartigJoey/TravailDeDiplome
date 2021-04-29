using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CovidPropagation
{
    static class ExtensionMethods
    {
        public static int ConvertToInt(this bool value)
        {
            return value ? 1 : 0;
        }

        public static bool NextBoolean(this Random value)
        {
            return Convert.ToBoolean(value.Next(0, 2));
        }

        public static bool NextBoolean(this Random value, double trueWeight)
        {
            bool result = false;
            if (value.Next(0, 101) < trueWeight)
                result = true;

            return result;
        }

        /// La somme du tableau ne peut être plus grande ou plus petite que 1
        public static Type NextProbability(this Random value, KeyValuePair<Type, double>[] weight)
        {
            double weightSum = weight.Sum(w => w.Value);
            if (weightSum > 1 && weightSum < 1)
                throw new ArgumentException("Weight array sum must be equal to 1.");

            int result = 0;
            double rdm = value.NextDouble();
            double sumPreviousWeight = 0;
            weight = weight.OrderBy(w => w.Value).ToArray();
            for (int i = 0; i < weight.Length; i++)
            {
                sumPreviousWeight += weight[i].Value;
                if (rdm < sumPreviousWeight)
                {
                    result = i;
                    break;
                }
            }
            return weight[result].Key;
        }

        public static int NextProbability(this Random value, KeyValuePair<int, double>[] weight)
        {
            double weightSum = weight.Sum(w => w.Value);
            if (weightSum > 1 && weightSum < 1)
                throw new ArgumentException("Weight array sum must be equal to 1.");

            int result = 0;
            double rdm = value.NextDouble();
            double sumPreviousWeight = 0;
            weight = weight.OrderBy(w => w.Value).ToArray();
            for (int i = 0; i < weight.Length; i++)
            {
                sumPreviousWeight += weight[i].Value;
                if (rdm < sumPreviousWeight)
                {
                    result = i;
                    break;
                }
            }
            return weight[result].Key;
        }

        public static char ToChar(this string value)
        {
            return value.ToCharArray()[0];
        }
    }
}
