/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */

namespace CovidPropagation
{
    class CoughSymptom : Symptom
    {
        private int _quantaAddedMin;
        private int _quantaAddedMax;

        public CoughSymptom(int quantaAddedMin, int quantaAddedMax)
        {
            _quantaAddedMin = quantaAddedMin;
            _quantaAddedMax = quantaAddedMax;
        }

        /// <summary>
        /// Récupère le nombre de quanta que ce symptôme ajoutera.
        /// </summary>
        /// <returns>Valeur aléatoir entre le minimum et le maximum de quanta.</returns>
        public double QuantaAddedByCoughing()
        {
            return GlobalVariables.rdm.Next(_quantaAddedMin, _quantaAddedMax);
        }
    }
}
