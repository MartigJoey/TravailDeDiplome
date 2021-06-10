/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */

using System;

namespace CovidPropagation
{
    /// <summary>
    /// Différents type de masques ayant des propriétés différentes.
    /// </summary>
    public enum TypeOfMask
    {
        N95 = 0,
        Surgical = 1,
        FaceShield = 2,
        Cloth = 3
    }

    class Mask
    {
        private const double CLOTH_MASK_EXHALATION_EFFICIENCY = 0.5d;
        private const double CLOTH_MASK_INHALATION_EFFICIENCY_MIN = 0.2d;
        private const double CLOTH_MASK_INHALATION_EFFICIENCY_MAX = 0.5d;

        private const double SURGICAL_MASK_EXHALATION_EFFICIENCY = 0.65d;
        private const double SURGICAL_MASK_INHALATION_EFFICIENCY_MIN = 0.3d;
        private const double SURGICAL_MASK_INHALATION_EFFICIENCY_MAX = 0.5d;


        private const double FACE_SHIELD_MASK_EXHALATION_EFFICIENCY = 0.23d;
        private const double FACE_SHIELD_MASK_INHALATION_EFFICIENCY = 0.23d;


        private const double N95_MASK_EXHALATION_EFFICIENCY = 0.9d;
        private const double N95_MASK_INHALATION_EFFICIENCY = 0.9d;


        private double _exhalationMaskEfficiency;
        private double _inhalationMaskEfficiency;
        private Random rdm;
        public double ExhalationMaskEfficiency { get => _exhalationMaskEfficiency; }
        public double InhalationMaskEfficiency { get => _inhalationMaskEfficiency; }

        public Mask(TypeOfMask typeOfMask)
        {
            rdm = GlobalVariables.rdm;
            switch (typeOfMask)
            {
                default:
                case TypeOfMask.Cloth:
                    _exhalationMaskEfficiency = CLOTH_MASK_EXHALATION_EFFICIENCY;
                    _inhalationMaskEfficiency = rdm.NextDoubleInclusive(CLOTH_MASK_INHALATION_EFFICIENCY_MIN, CLOTH_MASK_INHALATION_EFFICIENCY_MAX);
                    break;
                case TypeOfMask.Surgical:
                    _exhalationMaskEfficiency = SURGICAL_MASK_EXHALATION_EFFICIENCY;
                    _inhalationMaskEfficiency = rdm.NextDoubleInclusive(SURGICAL_MASK_INHALATION_EFFICIENCY_MIN, SURGICAL_MASK_INHALATION_EFFICIENCY_MAX);
                    break;
                case TypeOfMask.FaceShield:
                    _exhalationMaskEfficiency = FACE_SHIELD_MASK_EXHALATION_EFFICIENCY;
                    _inhalationMaskEfficiency = FACE_SHIELD_MASK_INHALATION_EFFICIENCY;
                    break;
                case TypeOfMask.N95:
                    _exhalationMaskEfficiency = N95_MASK_EXHALATION_EFFICIENCY;
                    _inhalationMaskEfficiency = N95_MASK_INHALATION_EFFICIENCY;
                    break;
            }
        }
    }
}
