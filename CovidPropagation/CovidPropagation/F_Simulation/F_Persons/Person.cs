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
using System.Diagnostics;

namespace CovidPropagation
{
    /// <summary>
    /// Individus se déplacant dans différents lieu en fonction de son planning.
    /// Peut être infecté et modifier les chances d'être infecté dans un lieu.
    /// </summary>
    public class Person
    {
        private const double PROBABILITY_OF_WEARING_CLOTH_MASK = 0.18;
        private const double PROBABILITY_OF_WEARING_FACESHIELD = 0.02;
        private const double PROBABILITY_OF_WEARING_N95_MASK = 0.01;
        private const double PROBABILITY_OF_WEARING_SURGICA_MASKL = 0.7;

        private const int MIN_RESISTANCE_BEFORE_HOSPITALISATION = 50;
        private const int MIN_RESISTANCE_BAFORE_DEATH = 10;

        private Planning _planning;
        private Site _currentSite;
        private PersonState _state;
        private List<Ilness> ilnesses;
        private List<Symptom> symptoms;
        private double virusResistance;
        private double baseVirusResistance;
        private int _age;
        private int virusDuration;
        private int virusIncubationDuration;
        private int immunityDuration;
        private Random _rdm;
        private double quantaExhalationRate;
        private bool _hasMask;
        private Mask _mask;
        private bool _isQuarantined;
        private int _quarantineDuration;
        private Home _quarantineLocation;
        private Hospital _hospitalCovid;

        public PersonState CurrentState { get => _state; set => _state = value; }
        public double QuantaExhalationRate { get => quantaExhalationRate; }
        public bool HasMask { get => _hasMask; }
        public double ExhalationMaskEfficiency { get => _mask.ExhalationMaskEfficiency; }
        public double InhalationMaskEfficiency { get => _mask.InhalationMaskEfficiency; }
        public int Age { get => _age; set => _age = value; }

        public Person(Planning planning, int age = GlobalVariables.DEFAULT_PERSON_AGE, PersonState state = PersonState.Healthy, Hospital hospital)
        {
            _planning = planning;
            _state = state;
            _rdm = GlobalVariables.rdm;
            Age = age;
            _hospitalCovid = hospital;
            ilnesses = new List<Ilness>();
            symptoms = new List<Symptom>();

            KeyValuePair<Object, double>[] probabilityOfMaskType = {
                new KeyValuePair<Object, double>(TypeOfMask.Cloth, PROBABILITY_OF_WEARING_CLOTH_MASK),
                new KeyValuePair<Object, double>(TypeOfMask.FaceShield, PROBABILITY_OF_WEARING_FACESHIELD),
                new KeyValuePair<Object, double>(TypeOfMask.N95, PROBABILITY_OF_WEARING_N95_MASK),
                new KeyValuePair<Object, double>(TypeOfMask.Surgical, PROBABILITY_OF_WEARING_SURGICA_MASKL),
            };

            _mask = new Mask((TypeOfMask)_rdm.NextProbability(probabilityOfMaskType));
            _hasMask = false;

            // Initialise la résistance de base de la personne
            if (_rdm.Next(0, 100) < GlobalVariables.PERCENTAGE_OF_ASYMPTOMATIC)
                baseVirusResistance = _rdm.Next(GlobalVariables.ASYMPTOMATIC_MIN_RESISTANCE, GlobalVariables.ASYMPTOMATIC_MAX_RESISTANCE);
            else
                baseVirusResistance = _rdm.Next(GlobalVariables.SYMPTOMATIC_MIN_RESISTANCE, GlobalVariables.SYMPTOMATIC_MAX_RESISTANCE);

            virusResistance = baseVirusResistance;
            _currentSite = _planning.GetActivity();
            _quarantineLocation = (Home)_currentSite; // Possible uniquement car tous les individus se trouve chez eux au démarrage.
            quantaExhalationRate = _currentSite.AverageQuantaExhalationRate;
            if ((int)_state >= 2)
            {
                SetInfectionDurations(_state);
                if ((int)_state > 2)
                    VirusIncubationIsOver();
            }
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
            //  - 50< => hospitalisations
            //  - 10 < => décès
            if (virusResistance < MIN_RESISTANCE_BEFORE_HOSPITALISATION)
            {
                if (_hospitalCovid.EnterForCovid(this))
                {
                    _currentSite = _hospitalCovid;
                }
                else
                {
                    _currentSite = _quarantineLocation;
                }
            }
            else if (_isQuarantined)
            {
                _currentSite = _quarantineLocation;
                if (_quarantineDuration <= 0)
                    _isQuarantined = false;


                _quarantineDuration--;
            }
            else
            {
                // Quitte le lieu précédent s'il est différent, récupère le nouveau et entre dedans.
                Site newSite = _planning.GetActivity();
                if (_currentSite != newSite)
                {
                    _currentSite.Leave(this);
                    _currentSite = newSite;
                    _currentSite.Enter(this, _planning.PersonTypeInActivity());

                    quantaExhalationRate = _currentSite.AverageQuantaExhalationRate;
                    if (symptoms.OfType<CoughSymptom>().Any())
                    {
                        quantaExhalationRate += symptoms.OfType<CoughSymptom>().First().QuantaAddedByCoughing();
                    }
                }
            }
        }

        public void GetCovidTreatment()
        {
            if (virusResistance < MIN_RESISTANCE_BAFORE_DEATH)
            {
                // Proceed to die
            }
            else
            {
                // guérir lentement ou stabiliser
                // Mélanger à CheckState.
            }
        }

        /// <summary>
        /// Vérifie l'état de contamination de la personne.
        /// </summary>
        public void ChechState()
        {
            double contaminationProbability = _currentSite.GetProbabilityOfInfection();
            
            if (_state == PersonState.Healthy && contaminationProbability >= _rdm.NextDouble())
            {
                _currentSite.HasEnvironnementChanged = true;
                SetInfectionDurations(PersonState.Infected);
            }

            ContractIlness();
            DecreaseImmunityDuration();
            DecreaseVirusDuration();
        }

        private void SetInfectionDurations(PersonState state)
        {
            _state = state;
            virusDuration = Virus.Duration * GlobalVariables.NUMBER_OF_TIMEFRAME;
            virusIncubationDuration = Convert.ToInt32(Virus.IncubationDurationMedian * GlobalVariables.NUMBER_OF_TIMEFRAME);
        }

        public void PutMaskOn()
        {
            _hasMask = true;
        }

        public void RemoveMask()
        {
            _hasMask = false;
        }

        public void SetQuarantine()
        {
            _isQuarantined = true;
            _quarantineDuration = Virus.Duration * GlobalVariables.NUMBER_OF_TIMEFRAME;
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
                    VirusIncubationIsOver();
                }
            }else if ((int)_state > 2)
            {
                if (virusDuration > 0)
                    virusDuration--;
                else
                    _state = PersonState.Healthy;
            }
        }

        private void VirusIncubationIsOver()
        {
            if (virusResistance > GlobalVariables.ASYMPTOMATIC_MIN_RESISTANCE)
            {
                _state = PersonState.Asymptomatic;
            }
            else
            {
                _state = PersonState.Infectious;
                symptoms.AddRange(Virus.GetCommonSymptoms());
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
                Ilness newIlness = new Ilness(Age, _rdm);
                ilnesses.Add(newIlness);
            }

            ilnesses.ForEach(i => i.DecrementTimeBeforeDesapearance());
            ilnesses.RemoveAll(i => i.Desapear());

            virusResistance = baseVirusResistance - ilnesses.Sum(i => i.Attack);
        }
    }
}
