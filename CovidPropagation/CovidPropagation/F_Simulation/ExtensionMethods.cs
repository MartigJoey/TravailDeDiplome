/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 06.05.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CovidPropagation
{
    static class ExtensionMethods
    {
        /// <summary>
        /// Convertis un booléen en int.
        /// </summary>
        /// <returns>1 ou 0 représentat la valeur du bool.</returns>
        public static int ConvertToInt(this bool value)
        {
            return value ? 1 : 0;
        }

        /// <summary>
        /// Choisis aléatoirement (50/50) si la valeur retournée est true ou false.
        /// </summary>
        /// <returns>Résultat du flipCoing</returns>
        public static bool NextBoolean(this Random value)
        {
            return Convert.ToBoolean(value.Next(0, 2));
        }

        /// <summary>
        /// Choisis aléatoirement une valeur qui résultera en true/false. Le true à un poid définit.
        /// </summary>
        /// <param name="trueWeight">Poid du true (format double 0 à 1)</param>
        /// <returns></returns>
        public static bool NextBoolean(this Random value, double trueWeight)
        {
            bool result = false;
            if (value.NextDouble() < trueWeight)
                result = true;

            return result;
        }

        /// <summary>
        /// Choisi en fonction des probabilité données un objet de du tableau et retourne celui sélectionné. Chaque objet à un poid qui défini les chances qu'il a d'être sélectionné.
        /// La somme des poids ne peut être plus grande ou plus petite que 1
        /// </summary>
        /// <param name="weight">Poid de l'objet</param>
        /// <returns>L'objet qui a été choisis</returns>
        public static object NextProbability(this Random value, KeyValuePair<object, double>[] weight)
        {
            double weightSum = weight.Sum(w => w.Value);
            if (weightSum > 1 && weightSum < 1)
                throw new ArgumentException("Weight array sum must be equal to 1.");

            object result = 0;
            double rdm = value.NextDouble();
            double sumPreviousWeight = 0;
            weight = weight.OrderBy(w => w.Value).ToArray();
            for (int i = 0; i < weight.Length; i++)
            {
                sumPreviousWeight += weight[i].Value;
                if (rdm < sumPreviousWeight)
                {
                    result = weight[i].Key;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Si le résultat obtenu est trop proche du maximum, la valeur resultante est modifiée pour obtenir un résultat suffisant.
        /// ⚠ Inclusif ⚠
        /// </summary>
        /// <param name="minValue">Valeurs minimum requise entre le résultat du random et le maximum possible</param>
        /// <returns></returns>
        public static int NextWithMinimum(this Random value, int minRdm, int maxRdm, int minValue)
        {
            int result = value.Next(minRdm, maxRdm + 1);

            if (result > (maxRdm - minValue) && result != maxRdm)
                result = maxRdm - minValue;

            return result;
        }

        /// <summary>
        /// Identique à Random.Next() mais la valeur la plus grande est incluse.
        /// </summary>
        /// <param name="minRdm">Valeur minimum à tirer au sort.</param>
        /// <param name="maxRdm">Valeur maximum à tirer au sort.</param>
        /// <returns></returns>
        public static int NextInclusive(this Random value, int minRdm, int maxRdm)
        {
            return value.Next(minRdm, maxRdm + 1);
        }

        /// <summary>
        /// Permet de mélanger une liste de tout type.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = GlobalVariables.rdm.Next(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }

        /// <summary>
        /// Convertit un caractère string en char. Si la string est plus grande qu'un caractère, seul le premier est récupéré.
        /// </summary>
        /// <returns>Premier caractère de la string en char</returns>
        public static char ToChar(this string value)
        {
            return value.ToCharArray()[0];
        }

        /// <summary>
        /// Récupère le dernier index d'une liste.
        /// Permet d'éviter d'écrire List.Count - 1 lors d'un for ou de récupérer le dernier élément d'une liste.
        /// </summary>
        /// <returns>Dernier index de la liste.</returns>
        public static int GetLastIndex<T>(this IEnumerable<T> value)
        {
            return value.Count() - 1;
        }
    }
}
