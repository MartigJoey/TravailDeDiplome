using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    public enum TypeOfMask
    {
        N95 = 0,
        Surgical = 1,
        FaceShield = 2,
        Cloth = 3
    }

    class Mask
    {
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
                    _exhalationMaskEfficiency = 0.5d;
                    _inhalationMaskEfficiency = rdm.NextDoubleInclusive(0.3d, 0.5d);
                    break;
                case TypeOfMask.Surgical:
                    _exhalationMaskEfficiency = 0.65d;
                    _inhalationMaskEfficiency = rdm.NextDoubleInclusive(0.3d, 0.5d);
                    break;
                case TypeOfMask.FaceShield:
                    _exhalationMaskEfficiency = 0.23d;
                    _inhalationMaskEfficiency = 0.23d;
                    break;
                case TypeOfMask.N95:
                    _exhalationMaskEfficiency = 0.9d;
                    _inhalationMaskEfficiency = 0.9d;
                    break;
            }
        }
    }
}
