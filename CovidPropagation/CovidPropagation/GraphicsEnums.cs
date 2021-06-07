using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CovidPropagation
{
    public enum UIType
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
        HeatMap,
        [Description("GUI")]
        GUI
    }

    public enum ChartsAxisData
    {
        [Description("Périodes")]
        TimeFrame,
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

    public enum ChartsDisplayData
    {
        [Description("Personnes")]
        Persons,
        [Description("Cas")]
        Cases,
        [Description("Infectieux")]
        Infectious,
        [Description("Incubation")]
        Incubations,
        [Description("Sains")]
        Healthy,
        [Description("Décès")]
        Death, 
        [Description("Immunisés")]
        Immune, 
        [Description("Hospitalisés")]
        Hospitalisation,
        [Description("Re")]
        Re,
        [Description("Contamination")]
        Contamination
    }

    public enum ChartsDisplayInterval
    {
        [Description("Jour")]
        Day,
        [Description("Semaine")]
        Week,
        [Description("Mois")]
        Month,
        [Description("Total")]
        Total
    }
}
