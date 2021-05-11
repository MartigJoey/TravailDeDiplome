/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 06.05.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CovidPropagation
{
    //First we have to define a delegate that acts as a signature for the
    //function that is ultimately called when the event is triggered.
    //You will notice that the second parameter is of MyEventArgs type.
    //This object will contain information about the triggered event.
    public delegate void MyEventHandler(object source, Simulation e);

    //This is a class which describes the event to the class that recieves it.
    //An EventArgs class must always derive from System.EventArgs.
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

        private const double PROBABILITY_OF_USING_A_CAR = 0.36d;
        private const double PROBABILITY_OF_USING_A_BIKE = 0.37d;
        private const double PROBABILITY_OF_USING_A_BUS = 0.15d;
        private const double PROBABILITY_OF_WALKING = 0.27d;// Normalement 0.11 avec les bus

        private const int MAX_SCHOOL_AGE = 25;
        private const int MAX_WORKING_AGE = 65;

        public event MyEventHandler OnTickSP;

        private Random rdm = new Random();
        private int _averageAge;
        private double _probabilityOfInfected;
        private int _nbPersons;
        List<Site> buildingSites;
        List<Site> companies;
        List<Site> stores;
        List<Site> restaurants;
        List<Site> schools;
        List<Site> hospitals;
        List<Site> supermarkets;
        List<Site> homes;
        List<Site> allTransports;
        List<Person> population;
        Stopwatch sp;
        bool startStop;
        int interval;

        public int Interval { get => interval; set => interval = value; }

        public Simulation(int avgAge, double nbInfected, int nbPersons)
        {
            _averageAge = avgAge;
            _probabilityOfInfected = nbInfected;
            _nbPersons = nbPersons;
            buildingSites = new List<Site>();
            companies = new List<Site>();
            stores = new List<Site>();
            restaurants = new List<Site>();
            schools = new List<Site>();
            hospitals = new List<Site>();
            supermarkets = new List<Site>();
            homes = new List<Site>();
            allTransports = new List<Site>();
            population = new List<Person>();
            sp = new Stopwatch();

            startStop = true;

            // Récupérer depuis les paramètres
            double minorProbability = DEFAULT_PROBABILITY_OF_BEING_MINOR;
            double retirementProbability = DEFAULT_PROBABILITY_OF_BEING_RETIRED;
            double workingProbability = 1 - minorProbability - retirementProbability;

            int nbMinor = (int)Math.Round(minorProbability * _nbPersons);
            int nbRetirement = (int)Math.Round(retirementProbability * _nbPersons);
            int nbWorking = (int)Math.Round(workingProbability * _nbPersons);

            Stopwatch speedTest = new Stopwatch();
            speedTest.Start();
            CreateBuildings();
            speedTest.Stop();
            Debug.WriteLine("Building" + speedTest.ElapsedMilliseconds + "  Count" + buildingSites.Count);
            speedTest.Restart();
            CreateTransports();
            speedTest.Stop();
            Debug.WriteLine("Transport" + speedTest.ElapsedMilliseconds);
            speedTest.Restart();
            CreatePopulation(_nbPersons, retirementProbability, minorProbability);
            speedTest.Stop();           
            Debug.WriteLine("Population" + speedTest.ElapsedMilliseconds + "  Count" + population.Count);
            //buildingSites.ForEach(b => b.SetMaskMeasure(true, true));
        }

        /// <summary>
        /// Itère la simulation. 
        /// Créé un "timer" à l'aide des stopwatch pour une meilleur précision.
        /// Ordonne à la population de changer d'activité,
        /// aux bâtiment de calculer les chances d'attraper le virus dans ceux-ci, 
        /// à la population de calculer si elle a été infectée.
        /// </summary>
        public async void Iterate()
        {
            while (true)
            {
                if (startStop)
                {
                    sp.Start();
                    TimeManager.NextTimeFrame();
                    if (OnTickSP != null)
                    {
                        OnTickSP(10, this);
                    }

                    population.ForEach(p => p.ChangeActivity());
                    buildingSites.ForEach(p => p.CalculateprobabilityOfInfection());
                    population.ForEach(p => p.ChechState());

                    sp.Stop();

                    if (sp.ElapsedMilliseconds < Interval)
                    {
                        long interval = Interval;
                        await Task.Delay((int)(Interval - sp.ElapsedMilliseconds));
                    }
                    sp.Reset();
                }
                await Task.Delay(100);
            }
        }

        /// <summary>
        /// Récupère les données lors d'un évènement qui est déclanché à chaque itération de la simulation.
        /// </summary>
        /// <returns>Données actuelles de la simulation.</returns>
        public string GetData()
        {
            //foreach (var item in allBuildingSites)
            //{
            //    if (item.NbPersons > 0)
            //    {
            //        Debug.WriteLine("Probability of infection " + item.ProbabilityOfInfection);
            //        Debug.WriteLine("Probability of being infective " + item.ProbabilityOfBeingInfective);
            //        Debug.WriteLine("Probability of one infection " + item.ProbabilityOfOneInfection);
            //        Debug.WriteLine("Number of persons " + item.NbPersons);
            //    }
            //}

            return $"Nombre de personne      : {population.Count} {Environment.NewLine}" +
                   $"Average age             : {(double)population.Average(p => p.Age)} {Environment.NewLine}" +
                   $"Infecté(s)              : {(double)population.Where(p => (int)p.CurrentState >= (int)PersonState.Infected).Count()} {Environment.NewLine}" +
                   $"Moyenne quanta exhalé   : {(double)population.Average(p => p.QuantaExhalationRate)} {Environment.NewLine}" +
                   $"Probabilité d'infection : {(double)buildingSites.Sum(b => b.ProbabilityOfInfection)} {Environment.NewLine}" +

                   $"Quanta concentre        : {(double)buildingSites.Sum(b => b.AvgQuantaConcentration)} {Environment.NewLine}" +
                   $"inhal mask eff          : {(double)buildingSites.Sum(b => b.InhalationMaskEfficiency)} {Environment.NewLine}" +
                   $"Fraction persons w mask : {(double)buildingSites.Sum(b => b.FractionPersonsWithMask)} {Environment.NewLine}" +

                   $"Quanta inhalé par person: {(double)buildingSites.Sum(b => b.QuantaInhaledPerPerson)} {Environment.NewLine}" +
                   $"Re                      : {buildingSites.Sum(b => b.VirusAraisingCases)} {Environment.NewLine}" +
                   $"Wearmasks               : {buildingSites.Sum(b => b.FractionPersonsWithMask) / buildingSites.Where(b => b.GetType() != typeof(Home)).Count()}" +
                   $"Temps                   : {TimeManager.CurrentDayString} {TimeManager.CurrentHour}";
        }

        /// <summary>
        /// Démarre/Redémarre la simulation.
        /// </summary>
        public void Start()
        {
            startStop = true;
        }

        /// <summary>
        /// Arrête/Pause la simulation.
        /// </summary>
        public void Stop()
        {
            startStop = false;
        }

        /// <summary>
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
            buildingSites.Add(new Outside());

            while(nbBuildings > 0){
                Type result = (Type)rdm.NextProbability(companyType);
                Site newBuilding;
                List<Site> buildingTypeListe;
                if (result == typeof(Company))
                {
                    newBuilding = new Company();
                    buildingTypeListe = companies;
                }
                else if (result == typeof(Store))
                {
                    newBuilding = new Store();
                    buildingTypeListe = stores;
                }
                else if (result == typeof(Restaurant))
                {
                    newBuilding = new Restaurant();
                    buildingTypeListe = restaurants;
                }
                else if (result == typeof(School))
                {
                    newBuilding = new School();
                    buildingTypeListe = schools;
                }
                else if (result == typeof(Hospital))
                {
                    newBuilding = new Hospital();
                    buildingTypeListe = hospitals;
                }
                else
                {
                    newBuilding = new Supermarket();
                    buildingTypeListe = supermarkets;
                }

                buildingSites.Add(newBuilding);
                buildingTypeListe.Add(newBuilding);
                nbBuildings--;
            }


            //if (!buildingSites.Any(b => b.GetType() == (Type)buildingType.Key))
            //    buildingSites.Add((Site)Activator.CreateInstance((Type)buildingType.Key));
            if (!buildingSites.Any(b => b.GetType() == typeof(School)))
            {
                School school = new School();
                schools.Add(school);
                buildingSites.Add(school);
            }

            if (!buildingSites.Any(b => b.GetType() == typeof(Hospital)))
            {
                Hospital hospital = new Hospital();
                hospitals.Add(hospital);
                buildingSites.Add(hospital);
            }

            if (!buildingSites.Any(b => b.GetType() == typeof(Supermarket)))
            {
                Supermarket supermarket = new Supermarket();
                supermarkets.Add(supermarket);
                buildingSites.Add(supermarket);
            }

            if (!buildingSites.Any(b => b.GetType() == typeof(Store)))
            {
                Store store = new Store();
                stores.Add(store);
                buildingSites.Add(store);
            }

            if (!buildingSites.Any(b => b.GetType() == typeof(Restaurant)))
            {
                Restaurant restaurant = new Restaurant();
                restaurants.Add(restaurant);
                buildingSites.Add(restaurant);
            }
        }

        /// <summary>
        /// créé les transports publiques de la simulation.
        /// </summary>
        private void CreateTransports()
        {
            double nbOfCar = (int)Math.Ceiling((36d - 100) / 100 * _nbPersons + _nbPersons); //  Modifier les % de chances d'utiliser une voiture dans la création de population
            double nbOfBus = (int)Math.Ceiling((15d - 100) / 100 * _nbPersons + _nbPersons);
            double nbOfBikes = (int)Math.Ceiling((10d - 100) / 100 * _nbPersons + _nbPersons); // Augmente quanta

            //for (int i = 0; i < nbOfCar; i++)
            //{
            //   // allTransports.Add(new School(populationInSchool / nbOfSchool));
            //}
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
            while(nbPeople > 0) 
            { 
                int age;
                int nbWorkDays;
                Home home = new Home();
                List<KeyValuePair<Site, SitePersonStatus>> personSitesFree;
                PersonState personState;

                if (rdm.NextBoolean(_probabilityOfInfected))
                    personState = PersonState.Infectious;
                else
                    personState = PersonState.Healthy;

                if (GlobalVariables.rdm.NextBoolean(retirementProbability))
                {
                    personSitesFree = new List<KeyValuePair<Site, SitePersonStatus>>() {
                        new KeyValuePair<Site, SitePersonStatus>(home, SitePersonStatus.Other),
                        new KeyValuePair<Site, SitePersonStatus>(stores[rdm.Next(0, stores.Count)], SitePersonStatus.Client),
                        new KeyValuePair<Site, SitePersonStatus>(restaurants[rdm.Next(0, restaurants.Count)], SitePersonStatus.Client),
                        new KeyValuePair<Site, SitePersonStatus>(GetVehicle(), SitePersonStatus.Other)
                    };
                    age = 70; // Changer pour age random
                    nbWorkDays = 0;
                }
                else if (GlobalVariables.rdm.NextBoolean(minorProbability))
                {
                    personSitesFree = new List<KeyValuePair<Site, SitePersonStatus>>() {
                        new KeyValuePair<Site, SitePersonStatus>(home, SitePersonStatus.Other),
                        new KeyValuePair<Site, SitePersonStatus>(stores[rdm.Next(0, stores.Count)], SitePersonStatus.Client),
                        new KeyValuePair<Site, SitePersonStatus>(restaurants[rdm.Next(0, restaurants.Count)], SitePersonStatus.Client),
                        new KeyValuePair<Site, SitePersonStatus>(buildingSites.Where(b => typeof(Outside) == b.GetType()).First(), SitePersonStatus.Other),
                        new KeyValuePair<Site, SitePersonStatus>(schools[rdm.Next(0, schools.Count)], SitePersonStatus.Worker)
                    };
                    age = 15; // Changer pour age random
                    nbWorkDays = 5;
                }
                else
                {
                    personSitesFree = new List<KeyValuePair<Site, SitePersonStatus>>() {
                        new KeyValuePair<Site, SitePersonStatus>(home, SitePersonStatus.Other),
                        new KeyValuePair<Site, SitePersonStatus>(stores[rdm.Next(0, stores.Count)], SitePersonStatus.Client),
                        new KeyValuePair<Site, SitePersonStatus>(restaurants[rdm.Next(0, restaurants.Count)], SitePersonStatus.Client),
                        new KeyValuePair<Site, SitePersonStatus>(buildingSites.Where(b => b.Type.Contains(SiteType.WorkPlace)).OrderBy(x => rdm.Next()).First(), SitePersonStatus.Worker),
                        new KeyValuePair<Site, SitePersonStatus>(GetVehicle(), SitePersonStatus.Other)
                    };
                    age = 30; // Changer pour age random
                    nbWorkDays = 5;
                }

                homes.Add(home);
                buildingSites.Add(home);
                Planning planning = new Planning(personSitesFree, nbWorkDays);
                population.Add(new Person(planning, age, personState));
                nbPeople--;
            }
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

                    locations.Add(typeof(Outside), buildingSites.Where(b => typeof(Outside) == b.GetType()).First());
                    locations.Add(typeof(Company), buildingSites.Where(b => typeof(Company) == b.GetType()).First());
                    locations.Add(typeof(Hospital), buildingSites.Where(b => typeof(Hospital) == b.GetType()).First());
                    locations.Add(typeof(Restaurant), buildingSites.Where(b => typeof(Restaurant) == b.GetType()).First());
                    locations.Add(typeof(School), buildingSites.Where(b => typeof(School) == b.GetType()).First());
                    locations.Add(typeof(Store), buildingSites.Where(b => typeof(Store) == b.GetType()).First());
                    locations.Add(typeof(Supermarket), buildingSites.Where(b => typeof(Supermarket) == b.GetType()).First());

                    population.Add(new Person(planning));
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
            // Nouveau
            // choisir lieux
            // Choisir une seed ICI pour la famille en fonction des lieux
            // créer tous les planning de la famille

            // ONE PERSON
            // Simulation
                // Créer 1 personne
                // Créer véhicule ou pas
                // Créer maison
                // Choisir lieux de loisirs
            // Planning
                // Sélectionne un planning compatible avec les paramètres reçus
                // Plus tard générer le planning automatiquement
                // Stocker les lieux
            // Personne

            // COUPLE
            // Simulation
                // Créer 2 personnes
                // Créer véhicule ou pas | liée
                // Créer maison | liée
                // Choisir lieux de loisirs | liée ou pas
            // Planning
                // Sélectionne un planning compatible avec les paramètres reçus
                // Plus tard générer le planning automatiquement
                // Stocker les lieux
            // Personne

            return nbCreated =  1;
        }

        /// <summary>
        /// Récupère un véhicule qu'un individu utilisera.
        /// </summary>
        /// <returns></returns>
        private Site GetVehicle()
        {
            KeyValuePair<object, double>[] transportsProbability = new KeyValuePair<object, double>[] {
                new KeyValuePair<object, double>(new Car(), PROBABILITY_OF_USING_A_CAR),
                new KeyValuePair<object, double>(buildingSites.Where(b => typeof(Outside) == b.GetType()).First(), PROBABILITY_OF_WALKING),
                new KeyValuePair<object, double>(new Bike(), PROBABILITY_OF_USING_A_BIKE), 
                //new KeyValuePair<object, double>(allBuildingSites.Where(b => typeof(Bus) == b.GetType()).OrderBy(x => rdm.Next()).First(), PROBABILITY_OF_USING_A_BUS),
            };
            return (Site)rdm.NextProbability(transportsProbability);
        }
    }
}
