using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CovidPropagation
{
    public class WorkSite : Site
    {
        private int _nbPlaces;
        private int _nbWorker;
        public WorkSite(SiteType[] workPlaceTypes,
                       double length,
                       double width,
                       double height,
                       double ventilationWithOutside,
                       double additionalControlMeasures,
                       int nbPlaces) :
                 base(workPlaceTypes, length, width, height, ventilationWithOutside, additionalControlMeasures)
        {
            this._nbPlaces = nbPlaces;
            _nbWorker = 0;
        }

        public bool IsHiring()
        {
            bool result = false;
            if (_nbWorker < _nbPlaces)
            {
                _nbWorker++;
                result = true;
            }
            return result;
        }
    }
}
