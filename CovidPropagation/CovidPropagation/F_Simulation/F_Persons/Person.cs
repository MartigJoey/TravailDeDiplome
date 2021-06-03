/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
using System;
using System.Linq;
using System.Collections.Generic;

namespace CovidPropagation
{
    /// <summary>
    /// ID Documentation : Person_Class
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

        private const int MIN_TIME_TO_DIE = 5;  // En jours
        private const int MAX_TIME_TO_DIE = 13; // En jours

        private const int MIN_IMMUNITY_DURATION = 3 * 30; // En jours
        private const int MAX_IMMUNITY_DURATION = 8 * 30; // En jours

        private static int ids = 0;
        private int id;

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
        private bool _mustLeaveHospital;
        private int _timeBeforeDeath;

        public PersonState CurrentState { get => _state; set => _state = value; }
        public double QuantaExhalationRate { get => quantaExhalationRate; }
        public bool HasMask { get => _hasMask; }
        public double ExhalationMaskEfficiency { get => _mask.ExhalationMaskEfficiency; }
        public double InhalationMaskEfficiency { get => _mask.InhalationMaskEfficiency; }
        public int Age { get => _age; set => _age = value; }
        internal List<Ilness> Ilnesses { get => ilnesses; set => ilnesses = value; }
        public bool MustLeaveHospital { set => _mustLeaveHospital = value; }
        public int Id { get => id; set => id = value; }

        public Person(Planning planning, Hospital hospital, int age = GlobalVariables.DEFAULT_PERSON_AGE, PersonState state = PersonState.Healthy)
        {
            _planning = planning;
            _state = state;
            _rdm = GlobalVariables.rdm;
            Age = age;
            _hospitalCovid = hospital;
            MustLeaveHospital = false;
            Ilnesses = new List<Ilness>();
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
                    VirusIncubationOver();
            }

            ids++;
            Id = ids;
        }

        /// <summary>
        /// ID Documentation : Change_Activity
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
            // Quitte l'hôpital
            if (_mustLeaveHospital)
            {
                _hospitalCovid.LeaveForCovid(this);
                _mustLeaveHospital = false;
            }

            // Si la résistance de l'individus est suffisament faible pour décèdé, qu'il ne se situe pas à l'hopital et que le virus s'est développé.
            if (virusResistance <= MIN_RESISTANCE_BAFORE_DEATH && _currentSite != _hospitalCovid && (int)_state > (int)PersonState.Infected)
            {
                DecrementUntilDeath(2);
            }

            // ID Documentation : Person_Hospital
            // Si la résistance de l'individus est suffisament faible pour être hôspitaliser, et que le virus s'est développé.
            if (virusResistance < MIN_RESISTANCE_BEFORE_HOSPITALISATION && (int)_state > (int)PersonState.Infected)
            {
                // S'il ne se situe pas déjà à l'hôpital
                if (_currentSite != _hospitalCovid)
                {
                    // Entre dans l'hôpital s'il y a de la place.
                    // Sinon, se confine.
                    if (_hospitalCovid.EnterForCovid(this))
                    {
                        _currentSite = _hospitalCovid;
                        _timeBeforeDeath = Convert.ToInt32((_rdm.Next(MIN_TIME_TO_DIE, MAX_TIME_TO_DIE) + _rdm.NextDouble()) * GlobalVariables.NUMBER_OF_TIMEFRAME);
                    }
                    else
                    {
                        _currentSite = _quarantineLocation;
                    }
                }
            }else if (_isQuarantined)
            {
                // Ajouter trajet
                _currentSite = _quarantineLocation;
                if (_quarantineDuration <= 0)
                    _isQuarantined = false;

                _quarantineDuration--;
            }else
            {
                // Quitte le lieu précédent s'il est différent, récupère le nouveau et entre dedans.
                Site newSite = _planning.GetActivity();
                if (_currentSite != newSite)
                {
                    _currentSite.Leave(this);
                    _currentSite = newSite;
                    _currentSite.Enter(this, _planning.PersonTypeInActivity());

                    // Calul les quantas exhalé en fonction des symptômes ainsi que du lieux.
                    quantaExhalationRate = _currentSite.AverageQuantaExhalationRate;
                    if (symptoms.OfType<CoughSymptom>().Any())
                    {
                        quantaExhalationRate += symptoms.OfType<CoughSymptom>().First().QuantaAddedByCoughing();
                    }
                }
            }
        }

        /// <summary>
        /// Décrémente le temps restant de l'individu de vivre lorsqu'il est hospitalisé.
        /// </summary>
        public void GetHospitalTreatment()
        {
            if (virusResistance <= MIN_RESISTANCE_BAFORE_DEATH)
            {
                DecrementUntilDeath(1);
            }
        }

        /// <summary>
        /// Décrémente la durée avant le décès de l'individu et change son état lorsque la durée atteint 0.
        /// </summary>
        /// <param name="decrement">Valeur décrémentant la durée avant le décès.</param>
        private void DecrementUntilDeath(int decrement)
        {
            if (_timeBeforeDeath <= 0)
            {
                _state = PersonState.Dead;
            }
            _timeBeforeDeath -= decrement;
        }

        /// <summary>
        /// ID Documentation : Check_State
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

        /// <summary>
        /// ID Documentation : Virus_Incubation
        /// Initialize la durée de vie du virus ainsi que sa durée d'incubation.
        /// </summary>
        /// <param name="state">Nouvel état de l'individu.</param>
        private void SetInfectionDurations(PersonState state)
        {
            _state = state;
            virusDuration = Virus.Duration * GlobalVariables.NUMBER_OF_TIMEFRAME;
            virusIncubationDuration = Convert.ToInt32(Virus.IncubationDuration * GlobalVariables.NUMBER_OF_TIMEFRAME);
        }

        /// <summary>
        /// Met le masque de la personne.
        /// </summary>
        public void PutMaskOn()
        {
            _hasMask = true;
        }

        /// <summary>
        /// Retire le masque de la personne.
        /// </summary>
        public void RemoveMask()
        {
            _hasMask = false;
        }

        /// <summary>
        /// Initialise la quarantaine ainsi que sa durée.
        /// </summary>
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
        /// ID Documentation : Incubation_Acitvity_Decrement
        /// Décrémente la durée d'incubation du virus si infecté.
        /// Décrémente la durée de vie du virus si infecté et que la durée d'incubation est terminée.
        /// Et change l'état en fonction du résultat.
        /// </summary>
        private void DecreaseVirusDuration()
        {
            // Si l'individu est infecté.
            if (_state == PersonState.Infected)
            {
                // Diminu la durée d'incubation du virus ou change l'état de l'individu et ajoute les symptômes.
                if (virusIncubationDuration > 0)
                    virusIncubationDuration--;
                else
                {
                    VirusIncubationOver();
                }
            }else if ((int)_state > (int)PersonState.Infected) // S'il est contagieux.
            {
                // Diminu la durée du virus ou change l'état de l'individu pour qu'il soit immunisé
                if (virusDuration > 0)
                    virusDuration--;
                else
                {
                    _state = PersonState.Immune;
                    immunityDuration = _rdm.Next(MIN_IMMUNITY_DURATION, MAX_IMMUNITY_DURATION) * GlobalVariables.NUMBER_OF_TIMEFRAME;
                }
            }
        }

        /// <summary>
        /// ID Documentation : Symptoms_After_Incubation
        /// Lorsque l'incubation du virus est terminée, change l'état en asymptomatique ou en infectieux.
        /// </summary>
        private void VirusIncubationOver()
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
        /// ID Documentation : Contract_Ilness
        /// Calcul si l'individus attrape une maladie et decrémente la durée de celles déjà existantes.
        /// Retire les maladies dont la durée est terminée.
        /// </summary>
        private void ContractIlness()
        {
            // Si attrape alors Modifier pour prendre en compte l'âge
            if (GlobalVariables.ILNESS_INFECTION_PROBABILITY > _rdm.NextDouble())
            {
                Ilness newIlness = new Ilness(Age, _rdm);
                Ilnesses.Add(newIlness);
            }
            RecalculateVirusResistance(1);
        }

        /// <summary>
        /// Recalcule la résistance au virus de l'individu.
        /// Vérifie si l'individu est toujours infecté par ses maladies.
        /// </summary>
        /// <param name="decrement">Valeur decrémentant la durée restante</param>
        public void RecalculateVirusResistance(int decrement)
        {
            Ilnesses.ForEach(i => i.DecrementTimeBeforeDesapearance(decrement));
            Ilnesses.RemoveAll(i => i.Desapear());
            virusResistance = baseVirusResistance - Ilnesses.Sum(i => i.Attack);
        }

        /// <summary>
        /// Récupère le prochain site de l'individu.
        /// </summary>
        /// <returns>Site contenu dans la prochaine activité</returns>
        public Site GetNextActivitySite()
        {
            return _planning.GetNextActivity();
        }
    }
}
