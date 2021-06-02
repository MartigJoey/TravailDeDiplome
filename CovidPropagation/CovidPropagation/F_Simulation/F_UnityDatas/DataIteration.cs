using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    public class DataIteration
    {
        public int[] personsNewSite;
        public int[] personsNewState;

        public DataIteration(int[] personsNewSite, int[] personsNewState)
        {
            this.personsNewSite = personsNewSite;
            this.personsNewState = personsNewState;
        }
    }
}
