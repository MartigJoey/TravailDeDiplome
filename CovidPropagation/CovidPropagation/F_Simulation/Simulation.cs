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

        private const int NUMBER_OF_STORE_PER_PERSON = 3;
        private const int NUMBER_OF_PLACE_TO_EAT_PER_PERSON = 3;

        private const double DEFAULT_PROBABILITY_OF_BEING_MINOR = 0.22d;
        private const double DEFAULT_PROBABILITY_OF_BEING_RETIRED = 0.14d;

        private const double PROBABILITY_OF_USING_A_CAR = 0.36d;
        private const double PROBABILITY_OF_USING_A_BIKE = 0.37d;
        private const double PROBABILITY_OF_USING_A_BUS = 0.15d;
        private const double PROBABILITY_OF_WALKING = 0.27d; // Normalement 0.11 avec les bus
        private const double PERCENTAGE_OF_BUS = 0.085d;

        private const double PROBABILITY_OF_BEING_VACCINATED_PER_TIMEFRAME = 0.00004472222d;
        private const int NUMBER_OF_INFECTIOUS_VALUES_FOR_REPRODUCTION = 10;

        private int DELAY_BEFORE_CHECKING_IF_SIMULATION_SHOULD_START = 100;
        private int MAX_TIME_BEFORE_DISPLAYING_AGAIN = 1000;


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
            int[] personsNewSite = new int[_population.Count];
            int[] personsNewState = new int[_population.Count];

            // Informe si une mesure est active ou non
            bool isMaskMeasureOn = false;
            bool isDistanciationMeasureOn = false;
            bool isQuarantineMeasureOn = false;
            bool isVaccinationMeasureOn = false;

            int nbOfInfectious = 0;
            int nbOfIncubating = 0;
            int nbOfImmune = 0;
            int nbOfHealthy = 0;
            int nbOfDead = 0;
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

                    // Réinitialise les compteur de types d'individus
                    nbOfInfectious = 0;
                    nbOfIncubating = 0;
                    nbOfImmune = 0;
                    nbOfHealthy = 0;

                    // Applique les mesures ou les retires.
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

                    // Change l'activité de la population, change de lieu pour le GUI, applique la vaccination si celle-ci est appliquée.
                    indexPerson = 0;
                    _population.ForEach(p => {
                        Site newSite = p.ChangeActivity();
                        if (_sitesIds.ContainsKey(newSite))
                            personsNewSite[indexPerson] =  _sitesIds[newSite];

                        if (isVaccinationMeasureOn && rdm.NextDouble() <= PROBABILITY_OF_BEING_VACCINATED_PER_TIMEFRAME && (int)p.CurrentState < (int)PersonState.Infected)
                            p.GetVaccinated();
                        indexPerson++;
                    });
                    // Calcul les probabilités d'infections dans les lieux et traites les patients à l'hôpital.
                    _sites.ForEach(p => p.CalculateprobabilityOfInfection());
                    _sitesDictionnary[SiteType.Hospital].ForEach(h => ((Hospital)h).TreatPatients());

                    // Change l'état de l'individu s'il a été infectés et change l'état pour le GUI.
                    indexPerson = 0;
                    _population.ForEach(p => {
                        personsNewState[indexPerson] = (int)p.ChechState();
                        indexPerson++;
                        switch (p.CurrentState)
                        {
                            case PersonState.Dead:
                                nbOfDead++;
                                break;
                            case PersonState.Healthy:
                                nbOfHealthy++;
                                break;
                            case PersonState.Immune:
                                nbOfImmune++;
                                break;
                            case PersonState.Infected:
                                nbOfIncubating++;
                                break;
                            case PersonState.Asymptomatic:
                            case PersonState.Infectious:
                                nbOfInfectious++;
                                break;
                            default:
                                break;
                        }
                    });
                    // Retire les individus décédés au cours de l'itération.
                    _population.RemoveAll(p => p.CurrentState == PersonState.Dead);

                    // Trigger l'évènement OnTick qui va mettre à jour le GUI et met à jour ses données.
                    if (OnGUIUpdate != null)
                    {
                        OnGUIUpdate(personsNewSite, personsNewState);
                    }

                    chartsDatas.AddDatas(GetAllDatas(nbOfDead, nbOfHealthy, nbOfImmune, nbOfIncubating, nbOfInfectious));
                    // Affiche au maximum une fois par seconde
                    if (sumEllapsedTime >= MAX_TIME_BEFORE_DISPLAYING_AGAIN)
                    {
                        // Trigger les évènements qui vont mettre à jour les graphiques
                        OnDisplay?.Invoke(chartsDatas, false);
                        OnDataUpdate?.Invoke(chartsDatas);

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
                    await Task.Delay(DELAY_BEFORE_CHECKING_IF_SIMULATION_SHOULD_START); // Ajoute un délai pour réduire la consommation CPU
                }
            }
        }


        /// <summary>
        /// Trigger l'évènement qui réaffiche les graphiques.
        /// </summary>
        public void TriggerDisplayChanges()
        {
            OnDisplay?.Invoke(chartsDatas, true);
        }

        #region measures
        /// <summary>
        /// Définit si la mesure du masque doit être appliquée.
        /// </summary>
        /// <param name="nbInfected">Le nombre d'infectés dans la simulation.</param>
        /// <param name="isOn">Si la mesure est déjà active.</param>
        /// <returns>Si le mesure est active ou non.</returns>
        private bool SetMaskMeasure(int nbInfected, bool isOn)
        {
            // Si la mesure est déjà active, elle n'est pas réactivée.
            if (isOn == false && nbInfected > SimulationGeneralParameters.NbInfecetdForMaskActivation)
            {
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

        /// <summary>
        /// Définit si la mesure des distanciations doit être appliquée.
        /// </summary>
        /// <param name="nbInfected">Le nombre d'infectés dans la simulation.</param>
        /// <param name="isOn">Si la mesure est déjà active.</param>
        /// <returns>Si le mesure est active ou non.</returns>
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

        /// <summary>
        /// Définit si la quarantaine doit être mise en place.
        /// </summary>
        /// <param name="nbInfected">Le nombre d'infectés dans la simulation.</param>
        /// <param name="isOn">Si la mesure est déjà active.</param>
        /// <returns>Si le mesure est active ou non.</returns>
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

        /// <summary>
        /// Définit si la vaccination doit être mise en place ou non.
        /// </summary>
        /// <param name="nbInfected">Le nombre d'infectés dans la simulation.</param>
        /// <param name="isOn">Si la mesure est déjà active.</param>
        /// <returns>Si le mesure est active ou non.</returns>
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

        #endregion

        #region Datas

        /// <summary>
        /// Récupère les données lors d'un évènement qui est déclanché à chaque itération de la simulation.
        /// </summary>
        /// <returns>Données actuelles de la simulation.</returns>
        public SimulationDatas GetAllDatas(int nbOfDead, int nbOfHealthy, int nbOfImmune, int nbOfIncubating, int nbOfInfectious)
        {
            SimulationDatas datas = new SimulationDatas();
            datas.Initialize();

            datas.NumberOfPeople.Add(_population.Count);
            datas.NumberOfInfected.Add(nbOfInfectious + nbOfIncubating);
            datas.NumberOfInfectious.Add(nbOfInfectious);
            datas.NumberOfIncubation.Add(nbOfIncubating);
            datas.NumberOfImmune.Add(nbOfImmune);
            datas.NumberOfHospitalisation.Add(GetNumberOfHospitalisation());
            datas.NumberOfDeath.Add(nbOfDead);
            datas.NumberOfContamination.Add(GetNumberOfContamination());
            datas.NumberOfHealthy.Add(nbOfHealthy);
            datas.NumberOfReproduction.Add(GetNumberOfReproduction());
            return datas;
        }

        // Datas
        public int GetNumberOfHospitalisation()
        {
            return _sitesDictionnary[SiteType.Hospital].Sum(b => ((Hospital)b).CountPatients());
        }

        /// <summary>
        /// Compte le nombre de décès en soustrayant le nombre d'individus actuellement présents et les individus actuellement healthy.
        /// </summary>
        /// <returns>Nombre de décès dans le timeframe.</returns>
        public int GetNumberOfDeath()
        {
            return SimulationGeneralParameters.NbPeople - _population.Count;
        }

        /// <summary>
        /// Compte le nombre de contamination en soustrayant le nombre d'infectés du dernier timeframe et celui actuel.
        /// </summary>
        /// <returns>Nombre de contaminations dans le timeframe.</returns>
        public double GetNumberOfContamination()
        {
            double result = 0;
            if (chartsDatas.NumberOfInfected != null && chartsDatas.NumberOfInfected.Count > 1)
            {
                double currentNbOfInfected = chartsDatas.NumberOfInfected[chartsDatas.NumberOfInfected.GetLastIndex()];
                double lastNbOfInfected = chartsDatas.NumberOfInfected[chartsDatas.NumberOfInfected.GetLastIndex() - 1];
                if (lastNbOfInfected == 0)
                    result = currentNbOfInfected;
                else
                    result = currentNbOfInfected - lastNbOfInfected;

                if (result < 0)
                    result = 0;
            }
            return result;
        }

        /// <summary>
        /// Compte le nombre de reproduction en faisant la somme des dix dernières données de contaminations et en les divisant par le nombre de personnes infectieuses.
        /// </summary>
        /// <returns>Nombre de reproduction dans le timeframe.</returns>
        public double GetNumberOfReproduction()
        {
            double result = 0;
            if (chartsDatas.NumberOfInfectious != null && chartsDatas.NumberOfInfectious.Count > NUMBER_OF_INFECTIOUS_VALUES_FOR_REPRODUCTION)
            {
                double avgRecentContamination = 0;
                double currentNbOfInfectious = chartsDatas.NumberOfInfectious[chartsDatas.NumberOfInfectious.GetLastIndex()];

                for (int i = chartsDatas.NumberOfContamination.Count - NUMBER_OF_INFECTIOUS_VALUES_FOR_REPRODUCTION; i < chartsDatas.NumberOfContamination.Count; i++)
                {
                    avgRecentContamination += chartsDatas.NumberOfContamination[i];
                }

                if (currentNbOfInfectious != 0)
                {
                    result = avgRecentContamination / currentNbOfInfectious;
                }
            }
            return result;
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
                _sitesDictionnary[SiteType.WorkPlace].Add(missingSite);
                hospitals.Add(missingSite);
            }

            if (_sitesDictionnary[SiteType.Store].Count == 0)
            {
                missingSite = new Store();
                _sitesDictionnary[SiteType.Store].Add(missingSite);
                _sitesDictionnary[SiteType.WorkPlace].Add(missingSite);
                stores.Add(missingSite);
            }
                
            if (_sitesDictionnary[SiteType.Eat].Count == 0)
            {
                missingSite = new Restaurant();
                _sitesDictionnary[SiteType.Eat].Add(missingSite);
                _sitesDictionnary[SiteType.WorkPlace].Add(missingSite);
                restaurants.Add(missingSite);
            }
                
            if (_sitesDictionnary[SiteType.School].Count == 0)
            {
                missingSite = new School();
                _sitesDictionnary[SiteType.School].Add(missingSite);
                _sitesDictionnary[SiteType.WorkPlace].Add(missingSite);
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
            double nbOfBus = (int)Math.Ceiling((PERCENTAGE_OF_BUS - 100) / 100 * _nbPersons + _nbPersons);
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

                // Calcul la probabilité que l'individu soit infecté dès le départ.
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
                        { SiteType.Store, CreateTypePersonSites(SiteType.Store, NUMBER_OF_STORE_PER_PERSON, home) },
                        { SiteType.Eat, CreateTypePersonSites(SiteType.Eat, NUMBER_OF_PLACE_TO_EAT_PER_PERSON, home) },
                        { SiteType.Transport, new List<Site>{ GetVehicle() } }
                    };
                    age = rdm.NextInclusive(60, 100);
                    nbWorkDays = 0;
                }
                else if (GlobalVariables.rdm.NextBoolean(minorProbability))
                {
                    personSites = new Dictionary<SiteType, List<Site>>() {
                        { SiteType.Home, new List<Site>{home} },
                        { SiteType.Store, CreateTypePersonSites(SiteType.Store, NUMBER_OF_STORE_PER_PERSON, home) },
                        { SiteType.Eat, CreateTypePersonSites(SiteType.Eat, NUMBER_OF_PLACE_TO_EAT_PER_PERSON, home) },
                        { SiteType.Transport, new List<Site>{ _outside } },
                        { SiteType.WorkPlace, new List<Site>{_sitesDictionnary[SiteType.School][rdm.Next(0, _sitesDictionnary[SiteType.School].Count)] } },
                    };
                    age = rdm.NextInclusive(5, 24);
                    nbWorkDays = 5;
                }
                else
                {
                    personSites = new Dictionary<SiteType, List<Site>>() {
                        { SiteType.Home, new List<Site>{home} },
                        { SiteType.Store, CreateTypePersonSites(SiteType.Store, NUMBER_OF_STORE_PER_PERSON, home) },
                        { SiteType.Eat, CreateTypePersonSites(SiteType.Eat, NUMBER_OF_PLACE_TO_EAT_PER_PERSON, home) },
                        { SiteType.Transport, new List<Site>{  _outside, GetVehicle() } },
                        { SiteType.WorkPlace, new List<Site>{ FindWorkPlace() } }
                    };
                    age = rdm.NextInclusive(18, 70);
                    nbWorkDays = 5;
                }
                // Créé le planning avec les lieux créés et créé l'individu
                houses.Add(home);
                _sitesDictionnary[SiteType.Home].Add(home);
                Planning planning = new Planning(personSites, nbWorkDays);
                _population.Add(new Person(planning, hospital, age, personState));
                nbPeople--;
            }

            // Insère les maison des individus au début de la liste pour garder un ordre précis dans le GUI. 
            _sites.InsertRange(1, houses);
            // Tous les lieux sont entrés, les ids peuvent être créés.
            for (int i = 0; i < _sites.Count; i++)
            {
                _sitesIds.Add(_sites[i], i);
            }
        }

        /// <summary>
        /// Créé le nombre de lieu demandé du type demandé dans une liste.
        /// </summary>
        /// <param name="type">Type de lieux</param>
        /// <param name="quantity">Nombre de lieux à créer.</param>
        /// <returns>Liste de lieux demandé à la quantité demandée.</returns>
        private List<Site> CreateTypePersonSites(SiteType type, int quantity, Site home)
        {
            List<Site> sites = new List<Site>();
            for (int i = 0; i < quantity; i++)
            {
                sites.Add(_sitesDictionnary[type][rdm.Next(0, _sitesDictionnary[type].Count)]);
            }
            sites.Add(home);
            return sites;
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
    }
}