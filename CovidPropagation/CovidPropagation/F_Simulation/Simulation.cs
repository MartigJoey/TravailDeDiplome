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

        public event MyEventHandler OnTickSP;

        private const int MAX_SCHOOL_AGE = 25;
        private const int MAX_WORKING_AGE = 65;

        private Random rdm = GlobalVariables.rdm;
        private int _averageAge;
        private int _nbInfected;
        private int _nbPersons;
        List<Site> allBuildingSites;
        List<Site> allTransports;
        List<Person> population;
        Stopwatch sp;
        bool startStop;
        int interval;

        public int Interval { get => interval; set => interval = value; }

        public Simulation(int avgAge, int nbInfected, int nbPersons)
        {
            _averageAge = avgAge;
            _nbInfected = nbInfected;
            _nbPersons = nbPersons;
            allBuildingSites = new List<Site>();
            allTransports = new List<Site>();
            population = new List<Person>();
            sp = new Stopwatch();
            population = new List<Person>();

            startStop = true;

            // Récupérer depuis les paramètres
            double minorProbability = 0.22d;
            double retirementProbability = 0.14d;
            double workingProbability = 1 - minorProbability - retirementProbability;

            int nbMinor = (int)Math.Round(minorProbability * _nbPersons);
            int nbRetirement = (int)Math.Round(retirementProbability * _nbPersons);
            int nbWorking = (int)Math.Round(workingProbability * _nbPersons);

            CreateBuildings(nbMinor, nbWorking, nbRetirement);
            CreateTransports();
            CreatePopulation(_nbPersons, _nbInfected, retirementProbability, minorProbability);

        }

        public async void Iterate()
        {
            while (true)
            {
                //Debug.WriteLine(Interval);
                if (startStop)
                {
                    sp.Start();
                    TimeManager.NextTimeFrame();
                    if (OnTickSP != null)
                    {
                        OnTickSP(10, this);
                    }
                    population.ForEach(p => p.ChangeActivity()); // Tester traitement en parallel
                    allBuildingSites.ForEach(p => p.CalculateprobabilityOfInfection());
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
                   $"Probabilité d'infection : {(double)allBuildingSites.Sum(b => b.ProbabilityOfInfection)} {Environment.NewLine}" +

                   $"Quanta concentre        : {(double)allBuildingSites.Sum(b => b.AvgQuantaConcentration)} {Environment.NewLine}" +
                   $"inhal mask eff          : {(double)allBuildingSites.Sum(b => b.InhalationMaskEfficiency)} {Environment.NewLine}" +
                   $"Fraction persons w mask : {(double)allBuildingSites.Sum(b => b.FractionPersonsWithMask)} {Environment.NewLine}" +

                   $"Quanta inhalé par person: {(double)allBuildingSites.Sum(b => b.QuantaInhaledPerPerson)} {Environment.NewLine}" +
                   $"Re                      : {allBuildingSites.Sum(b => b.VirusAraisingCases)} {Environment.NewLine}" +
                   $"Temps                   : {TimeManager.CurrentDayString} {TimeManager.CurrentHour}";
        }

        public void Start()
        {
            startStop = true;
        }

        public void Stop()
        {
            startStop = false;
        }

        private void CreateBuildings(int populationInSchool, int populationInCompanies, int populationInRetirement)
        {
            int nbOfSchool = (int)Math.Ceiling((0.03d - 100) / 100 * _nbPersons + _nbPersons);
            int nbOfCompany = (int)Math.Ceiling((6.74d - 100) / 100 * _nbPersons + _nbPersons);
            int nbOfHospital = (int)Math.Ceiling((0.01d - 100) / 100 * _nbPersons + _nbPersons);
            int nbOfSupermarket = (int)Math.Ceiling((0.03d - 100) / 100 * _nbPersons + _nbPersons);
            int nbOfHomes = (int)Math.Ceiling((50d - 100) / 100 * _nbPersons + _nbPersons);

            allBuildingSites.Add(new Outside());
            allBuildingSites.Add(new Store());
            allBuildingSites.Add(new Restaurant());

            for (int i = 0; i < nbOfSchool; i++)
            {
                allBuildingSites.Add(new School());
            }

            for (int i = 0; i < nbOfCompany; i++)
            {
                allBuildingSites.Add(new Company());
            }

            for (int i = 0; i < nbOfHospital; i++)
            {
                allBuildingSites.Add(new Hospital());
            }

            for (int i = 0; i < nbOfSupermarket; i++)
            {
                allBuildingSites.Add(new Supermarket());
            }
            
            for (int i = 0; i < nbOfHomes; i++)
            {
                allBuildingSites.Add(new Home());
            }
        }

        private void CreateTransports()
        {
            double nbOfCar = (int)Math.Ceiling((36d - 100) / 100 * _nbPersons + _nbPersons); //  Modifier les % de chances d'utiliser une voiture dans la création de population
            double nbOfBus = (int)Math.Ceiling((15d - 100) / 100 * _nbPersons + _nbPersons);
            double nbOfBikes = (int)Math.Ceiling((10d - 100) / 100 * _nbPersons + _nbPersons); // Augmente quanta

            for (int i = 0; i < nbOfCar; i++)
            {
               // allTransports.Add(new School(populationInSchool / nbOfSchool));
            }
        }

        private void CreatePopulation(int nbPeople, int nbInfected, double retirementProbability, double minorProbability)
        {
            while (nbPeople > 0)
            {
                nbInfected--;
                PersonState personState;
                if (nbInfected > 0)
                    personState = PersonState.Infectious;
                else
                    personState = PersonState.Healthy;

                if (GlobalVariables.rdm.NextBoolean(retirementProbability))
                {
                    CreateElder(personState);
                    nbPeople--;
                    //nbPeople -= CreateRetired();
                }else if (GlobalVariables.rdm.NextBoolean(minorProbability))
                {
                    CreateStudent(personState);
                    nbPeople--;
                    //nbPeople -= CreateRetired();
                }
                else
                {
                    CreateAdult(personState);
                    nbPeople--;
                    //nbPeople -= CreateFamilly();
                }
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

                    locations.Add(typeof(Outside), allBuildingSites.Where(b => typeof(Outside) == b.GetType()).First());
                    locations.Add(typeof(Company), allBuildingSites.Where(b => typeof(Company) == b.GetType()).First());
                    locations.Add(typeof(Hospital), allBuildingSites.Where(b => typeof(Hospital) == b.GetType()).First());
                    locations.Add(typeof(Restaurant), allBuildingSites.Where(b => typeof(Restaurant) == b.GetType()).First());
                    locations.Add(typeof(School), allBuildingSites.Where(b => typeof(School) == b.GetType()).First());
                    locations.Add(typeof(Store), allBuildingSites.Where(b => typeof(Store) == b.GetType()).First());
                    locations.Add(typeof(Supermarket), allBuildingSites.Where(b => typeof(Supermarket) == b.GetType()).First());

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

        private void CreateAdult(PersonState personState)
        {
            List<KeyValuePair<Site, SitePersonStatus>> personSitesFree = new List<KeyValuePair<Site, SitePersonStatus>>() {
                        new KeyValuePair<Site, SitePersonStatus>(new Home(), SitePersonStatus.Other),
                        new KeyValuePair<Site, SitePersonStatus>(allBuildingSites.Where(b => typeof(Store) == b.GetType()).OrderBy(x => rdm.Next()).First(), SitePersonStatus.Client),
                        new KeyValuePair<Site, SitePersonStatus>(allBuildingSites.Where(b => typeof(Restaurant) == b.GetType()).OrderBy(x => rdm.Next()).First(), SitePersonStatus.Client),
                        new KeyValuePair<Site, SitePersonStatus>(allBuildingSites.Where(b => b.Type.Contains(SiteType.WorkPlace)).OrderBy(x => rdm.Next()).First(), SitePersonStatus.Worker),
                        new KeyValuePair<Site, SitePersonStatus>(GetVehicle(), SitePersonStatus.Other)
                    };
            Planning planning = new Planning(personSitesFree, 5);

            population.Add(new Person(planning, 30, personState)); // Changer pour age random
        }

        private void CreateStudent(PersonState personState)
        {
            List<KeyValuePair<Site, SitePersonStatus>> personSitesFree = new List<KeyValuePair<Site, SitePersonStatus>>() {
                        new KeyValuePair<Site, SitePersonStatus>(new Home(), SitePersonStatus.Other),
                        new KeyValuePair<Site, SitePersonStatus>(allBuildingSites.Where(b => typeof(Store) == b.GetType()).OrderBy(x => rdm.Next()).First(), SitePersonStatus.Client),
                        new KeyValuePair<Site, SitePersonStatus>(allBuildingSites.Where(b => typeof(Restaurant) == b.GetType()).OrderBy(x => rdm.Next()).First(), SitePersonStatus.Client),
                        new KeyValuePair<Site, SitePersonStatus>(allBuildingSites.Where(b => typeof(Outside) == b.GetType()).First(), SitePersonStatus.Other),
                        new KeyValuePair<Site, SitePersonStatus>(allBuildingSites.Where(b => typeof(School) == b.GetType()).OrderBy(x => rdm.Next()).First(), SitePersonStatus.Worker)
                    };
            Planning planning = new Planning(personSitesFree, 5);
            population.Add(new Person(planning, 15, personState));
        }

        private void CreateElder(PersonState personState)
        {
            List<KeyValuePair<Site, SitePersonStatus>> personSitesFree = new List<KeyValuePair<Site, SitePersonStatus>>() {
                        new KeyValuePair<Site, SitePersonStatus>(new Home(), SitePersonStatus.Other),
                        new KeyValuePair<Site, SitePersonStatus>(allBuildingSites.Where(b => typeof(Store) == b.GetType()).OrderBy(x => rdm.Next()).First(), SitePersonStatus.Client),
                        new KeyValuePair<Site, SitePersonStatus>(allBuildingSites.Where(b => typeof(Restaurant) == b.GetType()).OrderBy(x => rdm.Next()).First(), SitePersonStatus.Client),
                        new KeyValuePair<Site, SitePersonStatus>(GetVehicle(), SitePersonStatus.Other)
                    };
            Planning planning = new Planning(personSitesFree, 0);
            population.Add(new Person(planning, 70, personState));
        }


        private Site GetVehicle()
        {
            KeyValuePair<object, double>[] transportsProbability = new KeyValuePair<object, double>[] {
                new KeyValuePair<object, double>(new Car(), 0.36),
                new KeyValuePair<object, double>(allBuildingSites.Where(b => typeof(Outside) == b.GetType()).First(), 0.37),
                new KeyValuePair<object, double>(new Bike(), 0.27), // Normalement 0.11 avec les bus
                //new KeyValuePair<object, double>(allBuildingSites.Where(b => typeof(Bus) == b.GetType()).OrderBy(x => rdm.Next()).First(), 0.15),
            };
            return (Site)rdm.NextProbability(transportsProbability);
        }
    }
}
