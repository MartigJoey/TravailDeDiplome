using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    class CoughSymptom : Symptom
    {
        private int _quantaAddedMin;
        private int _quantaAddedMax;

        public CoughSymptom(int quantaAddedMin = 100, int quantaAddedMax = 200)
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
