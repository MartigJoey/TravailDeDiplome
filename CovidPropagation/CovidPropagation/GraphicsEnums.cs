using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CovidPropagation
{
    public enum GraphicsType
    {
        [Description("Linéaire")]
        Linear,
        [Description("Vertical")]
        Vertical,
        [Description("Horizontal")]
        Horizontal,
        [Description("Secteur")]
        PieChart,
        [Description("Carte thermique")]
        HeatMap
    }

    public enum GraphicsAxisData
    {
        [Description("Temps")]
        Time,
        [Description("Nombre de personnes")]
        NumberOfPeople
    }

    public enum GraphicsCurvesData
    {
        [Description("Cas")]
        cases,
        [Description("Sain")]
        healthy,
        [Description("Décès")]
        Death,
        [Description("Re")]
        Re
    }
}
