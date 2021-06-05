/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CovidPropagation
{
    public delegate void GUIDataEventHandler(int[] personsNewSite, int[] personsNewState); 
    public delegate void InitializeGUIEventHandler(DataPopulation populationDatas, DataSites siteDatas);
    public delegate void DataUpdateEventHandler(SimulationDatas e);
    public delegate void DispalyChangeEventHandler(SimulationDatas e, bool isDisplayChange);

    /// <summary>
    /// ID Documentation : Simulation_Class
    /// </summary>
    public class Simulation : EventArgs
    {
        private const double PROBABILITY_OF_BEING_A_COMPANY = 0.7909d;
        private const double PROBABILITY_OF_BEING_A_STORE = 0.1d;
        private const double PROBABILITY_OF_BEING_A_RESTAURANT = 0.1d;
        private const double PROBABILITY_OF_BEING_A_SCHOOL = 0.0045d;
        private const double PROBABILITY_OF_BEING_A_HOSPITAL = 0.0004d;
        private const double PROBABILITY_OF_BEING_A_SUPERMARKET = 0.0042d;

        private const double DEFAULT_PROBABILITY_OF_BEING_MINOR = 0.22d;
        private const double DEFAULT_PROBABILITY_OF_BEING_RETIRED = 0.14d;

        private const double PROBABILITY_OF_BEING_VACCINATED_PER_TIMEFRAME = 0.00004472222d;

        private const double PROBABILITY_OF_USING_A_CAR = 0.36d;
        private const double PROBABILITY_OF_USING_A_BIKE = 0.37d;
        private const double PROBABILITY_OF_USING_A_BUS = 0.15d;
        private const double PROBABILITY_OF_WALKING = 0.27d;// Normalement 0.11 avec les bus

        private const int MAX_SCHOOL_AGE = 25;
        private const int MAX_WORKING_AGE = 65;


        public event DataUpdateEventHandler OnDataUpdate;
        public event DispalyChangeEventHandler OnDisplay;
        public event GUIDataEventHandler OnGUIUpdate;
        public event InitializeGUIEventHandler OnGUIInitialize;

        private Random rdm = new Random();
        private double _probabilityOfBeingInfected;
        private int _nbPersons;

        private Dictionary<SiteType, List<Site>> _sitesDictionnary;
        private List<Site> _sites;
        private Dictionary<Site, int> _sitesIds;
        private Outside _outside;
        private List<Person> _population;
        private Stopwatch _sp;
        private bool _startStop;
        private bool _isInitialized;
        private int _interval;

        SimulationDatas chartsDatas;

        public int Interval { get => _interval; set => _interval = value; }
        public bool IsInitialized { get => _isInitialized; }

        public Simulation()
        {
            _sites = new List<Site>();
            _sitesIds = new Dictionary<Site, int>();
            _sitesDictionnary = new Dictionary<SiteType, List<Site>>();
            _outside = new Outside();
            _population = new List<Person>();
            _sp = new Stopwatch();
            _isInitialized = false;
        }

        /// <summary>
        /// ID documentation: Initialize_Simulation
        /// Initialise la simulation, créé les bâtiments, créé les individus, Réinitialise le TimeManager et set les trigger des mesures.
        /// </summary>
        /// <param name="probabilityOfBeingInfected">Probabilités qu'un individu a d'être infecté lors de sa création</param>
        /// <param name="nbPersons">Nombre d'individus total dans la simulation</param>
        public void Initialize()
        {
            _sites.Clear();
            _sitesDictionnary.Clear();
            _population.Clear();
            _isInitialized = true;
            _probabilityOfBeingInfected = SimulationGeneralParameters.ProbabilityOfInfected;
            _nbPersons = SimulationGeneralParameters.NbPeople;
            _startStop = true;

            TimeManager.Init();
            Virus.Init();

            // Récupérer depuis les paramètres
            double minorProbability = DEFAULT_PROBABILITY_OF_BEING_MINOR;
            double retirementProbability = DEFAULT_PROBABILITY_OF_BEING_RETIRED;

            Stopwatch speedTest = new Stopwatch();
            speedTest.Start();
            CreateBuildings();
            speedTest.Stop();
            Debug.WriteLine("Building" + speedTest.ElapsedMilliseconds + "  Count" + _sites.Count);
            speedTest.Restart();
            CreatePublicTransports();
            speedTest.Stop();
            Debug.WriteLine("Transport" + speedTest.ElapsedMilliseconds);
            speedTest.Restart();

            CreatePopulation(_nbPersons, retirementProbability, minorProbability);

            speedTest.Stop();
            Debug.WriteLine("Population " + speedTest.ElapsedMilliseconds + "  Count " + _population.Count);
            //buildingSites.ForEach(b => b.SetMaskMeasure(true, true));
        }

        /// <summary>
        /// ID Documentation : Simulation_Iteration
        /// Itère la simulation. 
        /// Créé un "timer" à l'aide des stopwatch pour une meilleur précision.
        /// Ordonne à la population de changer d'activité,
        /// aux bâtiment de calculer les chances d'attraper le virus dans ceux-ci, 
        /// à la population de calculer si elle a été infectée.
        /// </summary>
        public async void Iterate()
        {
            chartsDatas = new SimulationDatas();
            chartsDatas.Initialize();
            chartsDatas.AddDatas(GetAllDatas());
            int[] personsNewSite = new int[_population.Count];
            int[] personsNewState = new int[_population.Count];

            // Informe si une mesure est active ou non
            bool isMaskMeasureOn = false;
            bool isDistanciationMeasureOn = false;
            bool isQuarantineMeasureOn = false;
            bool isVaccinationMeasureOn = false;

            int indexPerson = 0;

            // Initialise les données du GUI
            OnGUIInitialize?.Invoke(new DataPopulation(
                    _population.Count,
                    _population.Where(p => p.CurrentState == PersonState.Infected).Select(p => p.Id).ToArray()
                    ),
                new DataSites(
                    _sitesDictionnary[SiteType.Home].Count,
                    _sitesDictionnary[SiteType.WorkPlace].Where(s => s.GetType() == typeof(Company)).Count(),
                    _sitesDictionnary[SiteType.Hospital].Count,
                    _sitesDictionnary[SiteType.Eat].Where(s => s.GetType() == typeof(Restaurant)).Count(),
                    _sitesDictionnary[SiteType.WorkPlace].Where(s => s.GetType() == typeof(School)).Count(),
                    _sitesDictionnary[SiteType.Store].Where(s => s.GetType() == typeof(Store)).Count(),
                    _sitesDictionnary[SiteType.Store].Where(s => s.GetType() == typeof(Supermarket)).Count()
                    )
                );

            int sumEllapsedTime = 0;
            Stopwatch sp = new Stopwatch();
            // Boucle d'itération de la simulation
            while (true)
            {
                // Défini si la simulation est en pause ou non
                if (_startStop)
                {
                    _sp.Start();
                    int nbInfected = _population.Where(p => (int)p.CurrentState >= (int)PersonState.Infected).Count();
                    if (SimulationGeneralParameters.IsMaskMeasuresEnabled)
                        isMaskMeasureOn = SetMaskMeasure(nbInfected, isMaskMeasureOn);

                    if (SimulationGeneralParameters.IsDistanciationMeasuresEnabled)
                        isDistanciationMeasureOn = SetDistanciationMeasure(nbInfected, isDistanciationMeasureOn);

                    if (SimulationGeneralParameters.IsQuarantineMeasuresEnabled)
                        isQuarantineMeasureOn = SetQuarantineMeasure(nbInfected, isQuarantineMeasureOn);

                    if (SimulationGeneralParameters.IsVaccinationMeasuresEnabled)
                        isVaccinationMeasureOn = SetVaccinationMeasure(nbInfected, isVaccinationMeasureOn);

                    TimeManager.NextTimeFrame();

                    indexPerson = 0;
                    _population.ForEach(p => {
                        Site newSite = p.ChangeActivity();
                        if (_sitesIds.ContainsKey(newSite))
                            personsNewSite[indexPerson] =  _sitesIds[newSite];

                        if (isVaccinationMeasureOn && rdm.NextDouble() <= PROBABILITY_OF_BEING_VACCINATED_PER_TIMEFRAME && (int)p.CurrentState < (int)PersonState.Infected)
                            p.GetVaccinated();
                        indexPerson++;
                    });
                    _sites.ForEach(p => p.CalculateprobabilityOfInfection());
                    _sitesDictionnary[SiteType.Hospital].ForEach(h => ((Hospital)h).TreatPatients());

                    indexPerson = 0;
                    _population.ForEach(p => {
                        personsNewState[indexPerson] = (int)p.ChechState();
                        indexPerson++;
                    });
                    _population.RemoveAll(p => p.CurrentState == PersonState.Dead);

                    chartsDatas.AddDatas(GetAllDatas());
                    // Affiche au maximum une fois par seconde
                    if (sumEllapsedTime >= 1000)
                    {
                        // Trigger les évènements qui vont mettre à jour les graphiques
                        OnDisplay?.Invoke(chartsDatas, false);
                        OnDataUpdate?.Invoke(chartsDatas);

                        // Trigger l'évènement qui va mettre à jour le GUI et met à jour ses données.
                        OnGUIUpdate?.Invoke(personsNewSite, personsNewState);

                        sumEllapsedTime = 0;
                    }

                    _sp.Stop();
                    // Calule le temps à attendre pour correspondre au données du slider. (1 secondes par itération --> si l'itération se fait en 100ms alors on attend 900ms)
                    if (_sp.ElapsedMilliseconds < Interval)
                    {
                        long interval = Interval;
                        int delay = (int)(Interval - _sp.ElapsedMilliseconds);
                        await Task.Delay(delay);
                        sumEllapsedTime += delay;
                    }
                    sumEllapsedTime += (int)_sp.ElapsedMilliseconds;
                    _sp.Reset();
                }
                else
                {
                    await Task.Delay(100); // Ajoute un délai pour réduire la consommation CPU
                }
            }
        }

        private bool SetMaskMeasure(int nbInfected, bool isOn)
        {
            if (isOn == false && nbInfected > SimulationGeneralParameters.NbInfecetdForMaskActivation)
            {
                // Set masque dans bâtiments par types
                _sites.ForEach(s => s.SetMaskMeasure(true, true));
                isOn = true;
            }
            if (isOn == true && nbInfected < SimulationGeneralParameters.NbInfecetdForMaskDeactivation)
            {
                _sites.ForEach(b => b.SetMaskMeasure(false, false));
                isOn = false;
            }
            return isOn;
        }

        private bool SetDistanciationMeasure(int nbInfected, bool isOn)
        {
            if (isOn == false && nbInfected > SimulationGeneralParameters.NbInfecetdForDistanciationActivation)
            {
                _sites.ForEach(s => s.SetDistanciations(true));
                isOn = true;
            }
            if (isOn == true && nbInfected < SimulationGeneralParameters.NbInfecetdForDistanciationDeactivation)
            {
                _sites.ForEach(s => s.SetDistanciations(false));
                isOn = false;
            }
            return isOn;
        }

        private bool SetQuarantineMeasure(int nbInfected, bool isOn)
        {
            if (isOn == false && nbInfected > SimulationGeneralParameters.NbInfecetdForQuarantineActivation)
            {
                foreach (var person in _population)
                {
                        person.SetQuarantine(QuarantineParameters.IshealthyQuarantined, 
                                             QuarantineParameters.IsInfectedQuarantined, 
                                             QuarantineParameters.IsInfectiousQuarantined, 
                                             QuarantineParameters.IsImmuneQuarantined);
                }
                isOn = true;
            }
            if (isOn == true && nbInfected < SimulationGeneralParameters.NbInfecetdForQuarantineDeactivation)
            {
                foreach (var person in _population)
                {
                    person.SetQuarantine(false, false, false, false);
                }
                isOn = false;
            }
            return isOn;
        }

        private bool SetVaccinationMeasure(int nbInfected, bool isOn)
        {
            if (isOn == false && nbInfected > SimulationGeneralParameters.NbInfecetdForVaccinationActivation)
            {
                isOn = true;
            }
            if (isOn == true && nbInfected < SimulationGeneralParameters.NbInfecetdForVaccinationDeactivation)
            {
                isOn = false;
            }
            return isOn;
        }

        /// <summary>
        /// Trigger l'évènement qui réaffiche les graphiques.
        /// </summary>
        public void TriggerDisplayChanges()
        {
            OnDisplay?.Invoke(chartsDatas, true);
        }
        #region GetDatas

        /// <summary>
        /// Récupère les données lors d'un évènement qui est déclanché à chaque itération de la simulation.
        /// </summary>
        /// <returns>Données actuelles de la simulation.</returns>
        public SimulationDatas GetAllDatas()
        {
            SimulationDatas datas = new SimulationDatas();
            datas.Initialize();
            // MODIFIER LES REQUETES POUR DES VALEURS FIXES
            datas.NumberOfPeople.Add(_population.Count);
            datas.NumberOfInfected.Add(_population.Where(p => (int)p.CurrentState >= (int)PersonState.Infected).Count());
            datas.NumberOfImmune.Add(_population.Where(p => (int)p.CurrentState == (int)PersonState.Immune).Count());
            datas.NumberOfHospitalisation.Add(_sitesDictionnary[SiteType.Hospital].Sum(b => ((Hospital)b).CountPatients()));
            datas.NumberOfDeath.Add(SimulationGeneralParameters.NbPeople - _population.Count);
            datas.NumberOfContamination.Add(42);
            datas.NumberOfHealthy.Add(_population.Where(p => (int)p.CurrentState == (int)PersonState.Healthy).Count());
            datas.NumberOfReproduction.Add(_sites.Sum(b => b.VirusAraisingCases));
            return datas;
            /*return $"Nombre de personne      : {population.Count} {Environment.NewLine}" +
                   $"Average age             : {(double)population.Average(p => p.Age)} {Environment.NewLine}" +
                   $"Infecté(s)              : {(double)population.Where(p => (int)p.CurrentState >= (int)PersonState.Infected).Count()} {Environment.NewLine}" +
                   $"Moyenne quanta exhalé   : {(double)population.Average(p => p.QuantaExhalationRate)} {Environment.NewLine}" +
                   $"Probabilité d'infection : {(double)sites.Sum(b => b.ProbabilityOfInfection)} {Environment.NewLine}" +

                   $"Quanta concentre        : {(double)sites.Sum(b => b.AvgQuantaConcentration)} {Environment.NewLine}" +
                   $"inhal mask eff          : {(double)sites.Sum(b => b.InhalationMaskEfficiency)} {Environment.NewLine}" +
                   $"Fraction persons w mask : {(double)sites.Sum(b => b.FractionPersonsWithMask)} {Environment.NewLine}" +

                   $"Quanta inhalé par person: {(double)sites.Sum(b => b.QuantaInhaledPerPerson)} {Environment.NewLine}" +
                   $"Re                      : {sites.Sum(b => b.VirusAraisingCases)} {Environment.NewLine}" +
                   $"Wearmasks               : {sites.Sum(b => b.FractionPersonsWithMask) / sites.Where(b => b.GetType() != typeof(Home)).Count()}" +
                   $"Temps                   : {TimeManager.CurrentDayString} {TimeManager.CurrentHour}";*/
        }

        // Datas
        public int GetNumberOfPeople()
        {
            return 0;
        }

        public int GetNumberOfInfected()
        {
            return 1;
        }

        public int GetNumberOfImmune()
        {
            return 2;
        }

        public int GetNumberOfReproduction()
        {
            return 3;
        }

        public int GetNumberOfHealthy()
        {
            return 4;
        }

        public int GetNumberOfHospitalisation()
        {
            return 5;
        }

        public int GetNumberOfDeath()
        {
            return 6;
        }

        public int GetNumberOfContamination()
        {
            return 7;
        }

        #endregion

        /// <summary>
        /// Démarre/Redémarre la simulation.
        /// </summary>
        public void Start()
        {
            _startStop = true;
        }

        /// <summary>
        /// Arrête/Pause la simulation.
        /// </summary>
        public void Stop()
        {
            _startStop = false;
        }

        /// <summary>
        /// ID documentation: Create_Buildings
        /// Créé les bâtiments de la simulation en fonction du nombre de personnes.
        /// Peut importe le nombre de personnes, il existe un bâtiment de chaque.
        /// Les bâtiments sont créé proportionnelement à la taille de la population et à leur type.
        /// Ne créé par les maisons de la population.
        /// </summary>
        private void CreateBuildings()
        {
            // 6.8d est le rapport bâtiment / personne sans compter les habitations.
            int nbBuildings = (int)Math.Ceiling((6.8d - 100) / 100 * _nbPersons + _nbPersons);
            KeyValuePair<object, double>[] companyType = new KeyValuePair<object, double>[] {
                    new KeyValuePair<object, double>(typeof(Company), PROBABILITY_OF_BEING_A_COMPANY),
                    new KeyValuePair<object, double>(typeof(Store), PROBABILITY_OF_BEING_A_STORE),
                    new KeyValuePair<object, double>(typeof(Restaurant), PROBABILITY_OF_BEING_A_RESTAURANT),
                    new KeyValuePair<object, double>(typeof(School), PROBABILITY_OF_BEING_A_SCHOOL),
                    new KeyValuePair<object, double>(typeof(Hospital), PROBABILITY_OF_BEING_A_HOSPITAL),
                    new KeyValuePair<object, double>(typeof(Supermarket), PROBABILITY_OF_BEING_A_SUPERMARKET),
            };
            _sites.Add(_outside);
            // Permet d'ajouter les éléments dans le bon ordre pour permettre d'utiliser leur index comme id dans unity
            List<Site> hospitals = new List<Site>();
            List<Site> schools = new List<Site>();
            List<Site> stores = new List<Site>();
            List<Site> restaurants = new List<Site>();
            List<Site> supermarkets = new List<Site>();
            List<Site> companies = new List<Site>();

            #region list
            _sitesDictionnary.Add(SiteType.Hospital, new List<Site>());
            _sitesDictionnary.Add(SiteType.Store, new List<Site>());
            _sitesDictionnary.Add(SiteType.WorkPlace, new List<Site>());
            _sitesDictionnary.Add(SiteType.Eat, new List<Site>());
            _sitesDictionnary.Add(SiteType.School, new List<Site>());
            //buildingSitesDictionnaryArray.Add(typeof(Company), new Site[nbBuildings]);

            while (nbBuildings > 0)
            {
                Type result = (Type)rdm.NextProbability(companyType);
                Site site;

                if (result == typeof(Company))
                {
                    site = new Company();
                    _sitesDictionnary[SiteType.WorkPlace].Add(site);
                    companies.Add(site);
                }
                else if (result == typeof(Store))
                {
                    site = new Store();
                    _sitesDictionnary[SiteType.Store].Add(site);
                    _sitesDictionnary[SiteType.WorkPlace].Add(site);
                    stores.Add(site);
                }
                else if (result == typeof(Restaurant))
                {
                    site = new Restaurant();
                    _sitesDictionnary[SiteType.Eat].Add(site);
                    _sitesDictionnary[SiteType.WorkPlace].Add(site);
                    restaurants.Add(site);
                }
                else if (result == typeof(School))
                {
                    site = new School();
                    _sitesDictionnary[SiteType.School].Add(site);
                    _sitesDictionnary[SiteType.WorkPlace].Add(site);
                    schools.Add(site);
                }
                else if (result == typeof(Hospital))
                {
                    site = new Hospital();
                    _sitesDictionnary[SiteType.Hospital].Add(site);
                    _sitesDictionnary[SiteType.WorkPlace].Add(site);
                    hospitals.Add(site);
                }
                else
                {
                    site = new Supermarket();
                    _sitesDictionnary[SiteType.Store].Add(site);
                    _sitesDictionnary[SiteType.WorkPlace].Add(site);
                    supermarkets.Add(site);
                }
                nbBuildings--;
            }
            #endregion

            #region missingBuildings
            Site missingSite;
            if (_sitesDictionnary[SiteType.Hospital].Count == 0)
            {
                missingSite = new Hospital();
                _sitesDictionnary[SiteType.Hospital].Add(missingSite);
                companies.Add(missingSite);
            }

            if (_sitesDictionnary[SiteType.Store].Count == 0)
            {
                missingSite = new Store();
                _sitesDictionnary[SiteType.Store].Add(missingSite);
                stores.Add(missingSite);
            }
                
            if (_sitesDictionnary[SiteType.Eat].Count == 0)
            {
                missingSite = new Restaurant();
                _sitesDictionnary[SiteType.Eat].Add(missingSite);
                restaurants.Add(missingSite);
            }
                
            if (_sitesDictionnary[SiteType.School].Count == 0)
            {
                missingSite = new School();
                _sitesDictionnary[SiteType.School].Add(missingSite);
                schools.Add(missingSite);
            }

            #endregion

            _sites.AddRange(hospitals);
            _sites.AddRange(schools);
            _sites.AddRange(stores);
            _sites.AddRange(restaurants);
            _sites.AddRange(supermarkets);
            _sites.AddRange(companies);


        }

        /// <summary>
        /// Créé les transports publiques de la simulation.
        /// </summary>
        private void CreatePublicTransports()
        {
            double nbOfBus = (int)Math.Ceiling((0.85d - 100) / 100 * _nbPersons + _nbPersons);
            _sitesDictionnary.Add(SiteType.Transport, new List<Site>());

            for (int i = 0; i < nbOfBus; i++)
            {
                Bus bus = new Bus();
                _sites.Add(bus);
                _sitesDictionnary[SiteType.WorkPlace].Add(bus);
                _sitesDictionnary[SiteType.Transport].Add(bus);
            }
        }

        /// <summary>
        /// Créé la population, définit l'âge de la personne ainsi que les lieux dans lesquelles elle va aller.
        /// Définit si la personne est infectée dès le départ ou si elle est saine.
        /// </summary>
        /// <param name="nbPeople">Nombre de personnes à créer.</param>
        /// <param name="retirementProbability">pourcentage de chance que la personne soit retraitée.</param>
        /// <param name="minorProbability">Pourcentage de chance que la personne soit mineur.</param>
        private void CreatePopulation(int nbPeople, double retirementProbability, double minorProbability)
        {
            _sitesDictionnary.Add(SiteType.Home, new List<Site>());
            List<Site> houses = new List<Site>();
            while (nbPeople > 0)
            {
                int age;
                int nbWorkDays;
                Home home = new Home();
                Hospital hospital = (Hospital)_sitesDictionnary[SiteType.Hospital][rdm.Next(0, _sitesDictionnary[SiteType.Hospital].Count)];
                Dictionary<SiteType, List<Site>> personSites = new Dictionary<SiteType, List<Site>>();
                PersonState personState;

                if (rdm.NextBoolean(_probabilityOfBeingInfected))
                    personState = PersonState.Infectious;
                else
                    personState = PersonState.Healthy;

                // ID Documentation: Sites_Attribution, Create_Person
                // Sélectionne les lieux dans lesquels l'individu va se déplacer en fonction des probabilité qu'il a d'être soit retraité soit mineur soit en âge de travailler.
                if (GlobalVariables.rdm.NextBoolean(retirementProbability))
                {
                    personSites = new Dictionary<SiteType, List<Site>>() {
                        { SiteType.Home, new List<Site>{home} },
                        { SiteType.Store, new List<Site>{
                            _sitesDictionnary[SiteType.Store][rdm.Next(0, _sitesDictionnary[SiteType.Store].Count)],
                            _sitesDictionnary[SiteType.Store][rdm.Next(0, _sitesDictionnary[SiteType.Store].Count)],
                            _sitesDictionnary[SiteType.Store][rdm.Next(0, _sitesDictionnary[SiteType.Store].Count)]
                            }
                        },
                        { SiteType.Eat, new List<Site>{
                            _sitesDictionnary[SiteType.Eat][rdm.Next(0, _sitesDictionnary[SiteType.Eat].Count)],
                            _sitesDictionnary[SiteType.Eat][rdm.Next(0, _sitesDictionnary[SiteType.Eat].Count)],
                            _sitesDictionnary[SiteType.Eat][rdm.Next(0, _sitesDictionnary[SiteType.Eat].Count)]
                            }
                        },
                        { SiteType.Transport, new List<Site>{ GetVehicle() } }
                    };
                    age = rdm.NextInclusive(60, 100);
                    nbWorkDays = 0;
                }
                else if (GlobalVariables.rdm.NextBoolean(minorProbability))
                {
                    personSites = new Dictionary<SiteType, List<Site>>() {
                        { SiteType.Home, new List<Site>{home} },
                        { SiteType.Store, new List<Site>{
                            _sitesDictionnary[SiteType.Store][rdm.Next(0, _sitesDictionnary[SiteType.Store].Count)],
                            _sitesDictionnary[SiteType.Store][rdm.Next(0, _sitesDictionnary[SiteType.Store].Count)],
                            _sitesDictionnary[SiteType.Store][rdm.Next(0, _sitesDictionnary[SiteType.Store].Count)]
                            }
                        },
                        { SiteType.Eat, new List<Site>{
                            _sitesDictionnary[SiteType.Eat][rdm.Next(0, _sitesDictionnary[SiteType.Eat].Count)],
                            _sitesDictionnary[SiteType.Eat][rdm.Next(0, _sitesDictionnary[SiteType.Eat].Count)],
                            _sitesDictionnary[SiteType.Eat][rdm.Next(0, _sitesDictionnary[SiteType.Eat].Count)]
                            }
                        },
                        { SiteType.Transport, new List<Site>{ 
                            _outside 
                            } 
                        },
                        { SiteType.WorkPlace, new List<Site>{ 
                            _sitesDictionnary[SiteType.School][rdm.Next(0, _sitesDictionnary[SiteType.School].Count)] 
                            } 
                        },
                    };
                    age = rdm.NextInclusive(5, 24);
                    nbWorkDays = 5;
                }
                else
                {
                    personSites = new Dictionary<SiteType, List<Site>>() {
                        { SiteType.Home, new List<Site>{home} },
                        { SiteType.Store, new List<Site>{
                            _sitesDictionnary[SiteType.Store][rdm.Next(0, _sitesDictionnary[SiteType.Store].Count)],
                            _sitesDictionnary[SiteType.Store][rdm.Next(0, _sitesDictionnary[SiteType.Store].Count)],
                            _sitesDictionnary[SiteType.Store][rdm.Next(0, _sitesDictionnary[SiteType.Store].Count)]
                            }
                        },
                        { SiteType.Eat, new List<Site>{
                            _sitesDictionnary[SiteType.Eat][rdm.Next(0, _sitesDictionnary[SiteType.Eat].Count)],
                            _sitesDictionnary[SiteType.Eat][rdm.Next(0, _sitesDictionnary[SiteType.Eat].Count)],
                            _sitesDictionnary[SiteType.Eat][rdm.Next(0, _sitesDictionnary[SiteType.Eat].Count)]
                            }
                        },
                        { SiteType.Transport, new List<Site>{
                            _outside,
                            GetVehicle()
                            }
                        },
                        { SiteType.WorkPlace, new List<Site>{
                            FindWorkPlace()
                            }
                        }
                    };
                    age = rdm.NextInclusive(18, 70);
                    nbWorkDays = 5;
                }
                houses.Add(home);
                _sitesDictionnary[SiteType.Home].Add(home);
                Planning planning = new Planning(personSites, nbWorkDays);
                _population.Add(new Person(planning, hospital, age, personState));
                nbPeople--;
            }
            _sites.InsertRange(0, houses);
            for (int i = 1; i < _sites.Count; i++)
            {
                _sitesIds.Add(_sites[i], i);
            }
        }

        /// <summary>
        /// Méthode récursive permettant de trouver un lieu de travail pour un individu.
        /// </summary>
        /// <returns>Lieu de travail.</returns>
        private Site FindWorkPlace()
        {
            WorkSite workplace = (WorkSite)_sitesDictionnary[SiteType.WorkPlace][rdm.Next(0, _sitesDictionnary[SiteType.WorkPlace].Count)];
            return workplace.IsHiring() ? (Site)workplace : FindWorkPlace();
        }

        private int CreateFamilly()
        {
            KeyValuePair<object, double>[] famillyPresetsProbability = new KeyValuePair<object, double>[] {
                new KeyValuePair<object, double>("OnePerson", 0.28d),
                new KeyValuePair<object, double>("CoupleWithChild", 0.21d),
                new KeyValuePair<object, double>("CoupleWithoutChild", 0.40d),
                new KeyValuePair<object, double>("OneParentWithChild", 0.11d)
            };

            string famillyPreset = (string)GlobalVariables.rdm.NextProbability(famillyPresetsProbability);

            // Switch présets
            switch (famillyPreset)
            {
                default:
                case "OnePerson":
                    // Créer personne avec amis
                    break;
                case "CoupleWithChild":
                    // Créer deux personnes en couples avec 1 enfant, amis liés
                    break;
                case "CoupleWithoutChild":
                    // Créer deux personnes en couples sans enfants, amis liés
                    break;
                case "OneParentWithChild":
                    // Créer 1 personne avec 1 enfant, amis
                    break;
            }
            return 1;
        }

        /// <summary>
        /// Récupère le moyen de transport qu'un individu adulte utilisera.
        /// </summary>
        /// <returns></returns>
        private Site GetVehicle()
        {
            KeyValuePair<object, double>[] transportsProbability = new KeyValuePair<object, double>[] {
                new KeyValuePair<object, double>(new Car(), PROBABILITY_OF_USING_A_CAR),
                new KeyValuePair<object, double>(_outside, PROBABILITY_OF_WALKING),
                new KeyValuePair<object, double>(new Bike(), PROBABILITY_OF_USING_A_BIKE), 
                new KeyValuePair<object, double>(_sitesDictionnary[SiteType.Transport][rdm.Next(0, _sitesDictionnary[SiteType.Transport].Count)], PROBABILITY_OF_USING_A_BUS),
            };
            return (Site)rdm.NextProbability(transportsProbability);
        }

        /*
        private int CreateRetired()
        {
            KeyValuePair<object, double>[] retiredPresetsProbability = new KeyValuePair<object, double>[] {
                new KeyValuePair<object, double>("OnePerson", 0.35d),
                new KeyValuePair<object, double>("Couple", 0.65d)
            };
            int nbCreated;
            string retiredPreset = (string)GlobalVariables.rdm.NextProbability(retiredPresetsProbability);
            Dictionary<Type, Site> locations = new Dictionary<Type, Site>();

            // Switch présets
            switch (retiredPreset)
            {
                default:
                case "OnePerson":
                    // Créer personne seule
                    // Choisir dans la liste de lieux au moin 1 de chaque.
                    List<KeyValuePair<Site, SitePersonStatus>> personSitesFree = new List<KeyValuePair<Site, SitePersonStatus>>() {
                        new KeyValuePair<Site, SitePersonStatus>(new Home(), SitePersonStatus.Other),
                        new KeyValuePair<Site, SitePersonStatus>(new Store(), SitePersonStatus.Client),
                        new KeyValuePair<Site, SitePersonStatus>(new Restaurant(), SitePersonStatus.Client),
                        new KeyValuePair<Site, SitePersonStatus>(new Car(), SitePersonStatus.Other)
                    };
                    Planning planning = new Planning(personSitesFree, 0);
                    locations.Add(typeof(Home), new Home());

                    if (GlobalVariables.rdm.NextBoolean())
                        locations.Add(typeof(Car), new Car());
                    else
                        locations.Add(typeof(Car), new Outside());

                    locations.Add(typeof(Outside), sites.Where(b => typeof(Outside) == b.GetType()).First());
                    locations.Add(typeof(Company), sites.Where(b => typeof(Company) == b.GetType()).First());
                    locations.Add(typeof(Hospital), sites.Where(b => typeof(Hospital) == b.GetType()).First());
                    locations.Add(typeof(Restaurant), sites.Where(b => typeof(Restaurant) == b.GetType()).First());
                    locations.Add(typeof(School), sites.Where(b => typeof(School) == b.GetType()).First());
                    locations.Add(typeof(Store), sites.Where(b => typeof(Store) == b.GetType()).First());
                    locations.Add(typeof(Supermarket), sites.Where(b => typeof(Supermarket) == b.GetType()).First());

                    //population.Add(new Person(planning));
                    nbCreated = 1;
                    break;
                case "Couple":
                    // Créer deux personnes en couples
                    //Planning planning1 = new Planning();
                    //Planning planning2 = new Planning();
                    //planning1.CreateElderPlanning(); // Link both
                    //planning2.CreateElderPlanning();
                    //
                    //locations.Add(typeof(Home), new Home());
                    //
                    //if (GlobalVariables.rdm.NextBoolean())
                    //    locations.Add(typeof(Car), new Car());
                    //else
                    //    locations.Add(typeof(Car), new Outside());
                    //
                    //locations.Add(typeof(Outside), allBuildingSites.Where(b => typeof(Outside) == b.GetType()).First());
                    //locations.Add(typeof(Company), allBuildingSites.Where(b => typeof(Company) == b.GetType()).First());
                    //locations.Add(typeof(Hospital), allBuildingSites.Where(b => typeof(Hospital) == b.GetType()).First());
                    //locations.Add(typeof(Restaurant), allBuildingSites.Where(b => typeof(Restaurant) == b.GetType()).First());
                    //locations.Add(typeof(School), allBuildingSites.Where(b => typeof(School) == b.GetType()).First());
                    //locations.Add(typeof(Store), allBuildingSites.Where(b => typeof(Store) == b.GetType()).First());
                    //locations.Add(typeof(Supermarket), allBuildingSites.Where(b => typeof(Supermarket) == b.GetType()).First());
                    //
                    //population.Add(new Person(planning1, locations));
                    //population.Add(new Person(planning2, locations));
                    //nbCreated = 2;
                    break;
            }

            return nbCreated = 1;
        }
        */
    }
}
