using System;
using System.Collections.Generic;
using System.Text;

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

        public double QuantaAddedByCoughing()
        {
            return GlobalVariables.rdm.Next(_quantaAddedMin, _quantaAddedMax);
        }
    }
}
