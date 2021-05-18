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
        [Description("Périodes")]
        Period,
        [Description("Jour")]
        Day,
        [Description("Semaine")]
        Week,
        [Description("Nombre de personnes")]
        QuantityOfPeople,
        [Description("Nombre de cas")]
        QuantityOfCase,
        [Description("Nombre de décès")]
        QuantityOfDeath,
        [Description("Nombre d'hospitalisation")]
        QuantityOfHospitalisation,
        [Description("Nombre d'immunisés")]
        QuantityOfImmune,
        [Description("Nombre de contaminés")]
        QuantityOfContamination,
        [Description("Nombre de reproduction")]
        Re,

    }

    public enum GraphicsDisplayData
    {
        [Description("Personnes")]
        Persons,
        [Description("Cas")]
        Cases,
        [Description("Sains")]
        Healthy,
        [Description("Décès")]
        Death, 
        [Description("Immunisés")]
        Immune, 
        [Description("Hospitalisés")]
        Hospitalisation,
        [Description("Re")]
        Re
    }
}
