/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 27.04.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using System;
using System.Linq;
using System.Collections.Generic;

namespace CovidPropagation
{
    /// <summary>
    /// État d'une personne concernant le virus.
    /// </summary>
    public enum PersonState
    {
        Healthy = 0,
        Immune = 1,
        Infected = 2,
        Infectious = 3,
        Asymptomatic = 4
    }

    /// <summary>
    /// Individus se déplacant dans différents lieu en fonction de son planning.
    /// Peut être infecté et modifier les chances d'être infecté dans un lieu.
    /// </summary>
    public class Person
    {
        private Planning _planning;
        private Site _currentSite; // Int temporaire --> batiment / véhicules
        private PersonState _state;
        private List<Ilness> ilnesses;
        private double virusResistance;
        private double baseVirusResistance;
        private int _age;
        private int virusDuration;
        private int virusIncubationDuration;
        private int immunityDuration;
        private Random _rdm;
        private double quantaExhalationRate;
        private bool _hasMask;
        private double _exhalationMaskEfficiency;
        private double _inhalationMaskEfficiency;

        // Ajouter respiration
        // Si toux
        // Augmenter respiration
        public PersonState CurrentState { get => _state; set => _state = value; }
        public double QuantaExhalationRate { get => quantaExhalationRate; }
        public bool HasMask { get => _hasMask; set => _hasMask = value; }
        public double ExhalationMaskEfficiency { get => _exhalationMaskEfficiency; set => _exhalationMaskEfficiency = value; }
        public double InhalationMaskEfficiency { get => _inhalationMaskEfficiency; set => _inhalationMaskEfficiency = value; }

        public Person(Planning planning, Random rdm, int age = GlobalVariables.DEFAULT_PERSON_AGE, PersonState state = PersonState.Healthy)
        {
            _planning = planning;
            _state = state;
            _rdm = rdm;
            _age = age;
            ilnesses = new List<Ilness>();

            // Initialise la résistance de base de la personne
            if (_rdm.Next(0, 100) < GlobalVariables.PERCENTAGE_OF_ASYMPTOMATIC)
            {
                baseVirusResistance = _rdm.Next(GlobalVariables.ASYMPTOMATIC_MIN_RESISTANCE, GlobalVariables.ASYMPTOMATIC_MAX_RESISTANCE);
            }
            else
            {
                baseVirusResistance = _rdm.Next(GlobalVariables.SYMPTOMATIC_MIN_RESISTANCE, GlobalVariables.SYMPTOMATIC_MAX_RESISTANCE);
            }
            virusResistance = baseVirusResistance;
            ExhalationMaskEfficiency = GlobalVariables.AVERAGE_EXHALATION_MASK_EFFICIENCY;
            _inhalationMaskEfficiency = GlobalVariables.AVERAGE_INHALATION_MASK_EFFICIENCY;
            _hasMask = false;
        }

        /// <summary>
        /// Change d'activité et recalcul les chances d'attraper le virus.
        /// Change d'état si besoin.
        /// Décrémente la durée d'incubation du virus si infecté.
        /// Décrémente la durée de vie du virus si infecté et que la durée d'incubation est terminée.
        /// Décrémente la durée d'immunité si la personne est immunisé.
        /// Calcul si l'individus attrape une maladie et decrémente la durée de celles déjà existantes.
        /// Retire les maladies dont la durée est terminée.
        /// </summary>
        public void ChangeActivity()
        {
            double contaminationProbability = 0;
            // Quitte le lieu précédent si il est différent, récupère le nouveau et entre dedans.
            Site newSite = _planning.GetActivity();
            if (_currentSite != newSite)
            {
                _currentSite.Leave(this);
                _currentSite = newSite;
                _currentSite.Enter(this);

                // Mettre le mask à true si besoin.
                quantaExhalationRate = _currentSite.GetAverageQuantaExhalationRate(); // Ajouter la possible Toux
            }

            contaminationProbability = _currentSite.GetProbabilityOfInfection();
            if (_state == PersonState.Healthy && contaminationProbability >= _rdm.NextDouble())
            {
                _state = PersonState.Infected;
                // Modifier pour récupérer directement les valeurs sur le virus. (Le virus calculera alors les valeurs)
                virusDuration = GlobalVariables.VIRUS_DURATION;
                virusIncubationDuration = GlobalVariables.VIRUS_INCUBATION_DURATION_MEDIAN;
            }

            ContractIlness();
            DecreaseImmunityDuration();
            DecreaseVirusDuration();
        }

        /// <summary>
        /// Décrémente la durée d'immunité si la personne est immunisé et change son état.
        /// </summary>
        private void DecreaseImmunityDuration()
        {
            if (_state == PersonState.Immune)
            {
                if (immunityDuration > 0)
                    immunityDuration--;
                else
                    _state = PersonState.Healthy;
            }
        }

        /// <summary>
        /// Décrémente la durée d'incubation du virus si infecté.
        /// Décrémente la durée de vie du virus si infecté et que la durée d'incubation est terminée.
        /// Et change l'état en fonction du résultat.
        /// </summary>
        private void DecreaseVirusDuration()
        {
            if (_state == PersonState.Infected)
            {
                if (virusIncubationDuration > 0)
                    virusIncubationDuration--;
                else
                {
                    if (virusResistance > GlobalVariables.ASYMPTOMATIC_MIN_RESISTANCE)
                        _state = PersonState.Asymptomatic;
                    else
                        _state = PersonState.Infectious;
                }
            }else if ((int)_state > 2)
            {
                if (virusDuration > 0)
                    virusDuration--;
                else
                    _state = PersonState.Healthy;
            }
        }

        /// <summary>
        /// Calcul si l'individus attrape une maladie et decrémente la durée de celles déjà existantes.
        /// Retire les maladies dont la durée est terminée.
        /// </summary>
        private void ContractIlness()
        {
            // SI attrape alors Modifier pour prendre en compte l'âge
            if (GlobalVariables.ILNESS_INFECTION_PROBABILITY > _rdm.NextDouble())
            {
                Ilness newIlness = new Ilness(_age, _rdm);
                ilnesses.Add(newIlness);
            }

            ilnesses.ForEach(i => i.DecrementTimeBeforeDesapearance());
            ilnesses.RemoveAll(i => i.Desapear());

            virusResistance = baseVirusResistance - ilnesses.Sum(i => i.Attack);
        }
    }
}
