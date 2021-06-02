using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    public class DataSites
    {
        private int[] sitesType;
        private int[] sitesId;

        public int[] SitesType { get => sitesType; set => sitesType = value; }
        public int[] SitesId { get => sitesId; set => sitesId = value; }

        public DataSites(int[] types, int[] ids)
        {
            SitesType = types;
            SitesId = ids;
        }
    }
}
