/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 29.04.2021
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
    public class Simulation
    {
        private const int MAX_SCHOOL_AGE = 25;
        private const int MAX_WORKING_AGE = 65;

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
            CreatePopulation(_nbPersons, retirementProbability);

            GenerateSeedWithSite(null);
        }

        public async void Iterate()
        {
            while (true)
            {
                //Debug.WriteLine(Interval);
                if (startStop)
                {
                    sp.Start();
                    TimeManager.NextPeriod();
                    population.ForEach(p => p.ChangeActivity());
                    allBuildingSites.ForEach(p => p.CalculateprobabilityOfInfection());
                    population.ForEach(p => p.ChechState());
                    sp.Stop();

                    if (sp.ElapsedMilliseconds < Interval)
                    {
                        long interval = Interval;
                        await Task.Delay((int)(Interval - sp.ElapsedMilliseconds));
                    }
                    //Debug.WriteLine("Tick");
                    sp.Reset();
                }
                await Task.Delay(100);
            }
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
                allBuildingSites.Add(new School(populationInSchool / nbOfSchool));
            }

            for (int i = 0; i < nbOfCompany; i++)
            {
                allBuildingSites.Add(new Company(populationInCompanies / nbOfCompany));
            }

            for (int i = 0; i < nbOfHospital; i++)
            {
                allBuildingSites.Add(new Hospital(population.Count / nbOfHospital));
            }

            for (int i = 0; i < nbOfSupermarket; i++)
            {
                allBuildingSites.Add(new Supermarket(population.Count / nbOfSupermarket));
            }
            
            for (int i = 0; i < nbOfHomes; i++)
            {
                allBuildingSites.Add(new Home(population.Count / nbOfHomes));
            }
        }

        private void CreateTransports()
        {
            double nbOfCar = (int)Math.Ceiling((36d - 100) / 100 * _nbPersons + _nbPersons);
            double nbOfBus = (int)Math.Ceiling((15d - 100) / 100 * _nbPersons + _nbPersons);
            double nbOfBikes = (int)Math.Ceiling((10d - 100) / 100 * _nbPersons + _nbPersons); // Augmente quanta

            for (int i = 0; i < nbOfCar; i++)
            {
               // allTransports.Add(new School(populationInSchool / nbOfSchool));
            }

        }

        private void CreatePopulation(int nbPeople, double retirementProbability)
        {
            while (nbPeople > 0)
            {
                if (GlobalVariables.rdm.NextBoolean(retirementProbability))
                {
                    nbPeople -= CreateRetired();
                }
                else
                {
                    nbPeople -= CreateFamilly();
                }
            }

                //while (nbMinor > 0)
                //{
                //    population.Add(CreateStudentPerson());
                //    nbMinor--;
                //}
                //
                //while (nbWorking > 0)
                //{
                //    population.Add(CreateElderPerson());
                //    nbWorking--;
                //}
                //
                //while (nbRetirement > 0)
                //{
                //    population.Add(CreateAdultClass());
                //    nbRetirement--;
                //}
        }

        private int CreateFamilly()
        {
            KeyValuePair<string, double>[] famillyPresetsProbability = new KeyValuePair<string, double>[] {
                new KeyValuePair<string, double>("OnePerson", 0.28d),
                new KeyValuePair<string, double>("CoupleWithChild", 0.21d),
                new KeyValuePair<string, double>("CoupleWithoutChild", 0.40d),
                new KeyValuePair<string, double>("OneParentWithChild", 0.11d)
            };

            string famillyPreset = GlobalVariables.rdm.NextProbability(famillyPresetsProbability);

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
            KeyValuePair<string, double>[] retiredPresetsProbability = new KeyValuePair<string, double>[] {
                new KeyValuePair<string, double>("OnePerson", 0.35d),
                new KeyValuePair<string, double>("Couple", 0.65d)
            };
            int nbCreated;
            string retiredPreset = GlobalVariables.rdm.NextProbability(retiredPresetsProbability);
            Dictionary<Type, Site> locations = new Dictionary<Type, Site>();

            // Switch présets
            switch (retiredPreset)
            {
                default:
                case "OnePerson":
                    // Créer personne
                    Planning planning = new Planning();
                    planning.CreateElderPlanning();
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

                    population.Add(new Person(planning, locations));
                    nbCreated = 1;
                    break;
                case "Couple":
                    // Créer deux personnes en couples
                    Planning planning1 = new Planning();
                    Planning planning2 = new Planning();
                    planning1.CreateElderPlanning(); // Link both
                    planning2.CreateElderPlanning();

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

                    population.Add(new Person(planning1, locations));
                    population.Add(new Person(planning2, locations));
                    nbCreated = 2;
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

            return nbCreated;
        }

        private string GenerateSeedWithSite(List<Site> sites)
        {
            int totalPeriods = GlobalVariables.NUMBER_OF_PERIODS;
            Random rdm = GlobalVariables.rdm;

            int morningPeriodsMax = 22, morningMin = 12;
            int noonPeriodsMax = 4, noonMin = 1;
            int afterNoonPeriodsMax = 10, afterNoonMin = 5;
            int eveningPeriodsMax = 6, eveningMin = 3;
            int nightPeriods; // remplit ce qu'il manque

            int morningPeriods = rdm.Next(morningPeriodsMax - morningMin, morningPeriodsMax + 1);
            int noonPeriods = rdm.Next(noonPeriodsMax - noonMin, noonPeriodsMax + 1);
            int afterNoonPeriods = rdm.Next(afterNoonPeriodsMax - afterNoonMin, afterNoonPeriodsMax + 1);
            int eveningPeriods = rdm.Next(eveningPeriodsMax - eveningMin, eveningPeriodsMax + 1);

            // Activities
            #region Activities

            int morningActivityPeriods = 0;
            if (morningPeriods < morningPeriodsMax)
                morningActivityPeriods = morningPeriodsMax - morningPeriods;


            if (noonPeriods < noonPeriodsMax)
                afterNoonPeriodsMax += 1;

            int afterNoonActivityPeriods = 0;
            if (afterNoonPeriods < afterNoonPeriodsMax)
                afterNoonActivityPeriods = afterNoonPeriodsMax - afterNoonPeriods;

            int eveningActivityPeriods = 0;
            if (eveningPeriods < eveningPeriodsMax)
                eveningActivityPeriods = eveningPeriodsMax - eveningPeriods;

            #endregion

            nightPeriods = (morningPeriods + morningActivityPeriods) +
                           (noonPeriods) +
                           (afterNoonPeriods + afterNoonActivityPeriods) +
                           (eveningPeriods + eveningActivityPeriods);

            nightPeriods = totalPeriods - nightPeriods;

            Debug.WriteLine((morningPeriods + " " + morningActivityPeriods) + " " + Environment.NewLine +
                           (noonPeriods) + " " + Environment.NewLine +
                           (afterNoonPeriods + " " + afterNoonActivityPeriods) + " " + Environment.NewLine +
                           (eveningPeriods + " " + eveningActivityPeriods) + " " + Environment.NewLine + nightPeriods);

            // utiliser les sites et le durée créé pour créer la seed.
            return "";
        }

        private Person CreateStudentPerson()
        {
            Person student;
            Planning planning = new Planning();
            planning.CreateStudentPlanning();
            student = new Person(planning, new Dictionary<Type, Site>());
            return student;
        }

        private Person CreateAdultClass()
        {
            Person adult;
            Planning planning = new Planning();
            planning.CreateAdultPlanning();
            adult = new Person(planning, new Dictionary<Type, Site>());
            return adult;
        }

        private Person CreateElderPerson()
        {
            Person elder;
            Planning planning = new Planning();
            planning.CreateElderPlanning();
            elder = new Person(planning, new Dictionary<Type, Site>());
            return elder;
        }

        /* private List<Period> CreateEveningWorkingClass()
         {
             int eveningTotalPeriods = 10;
             List<Period> eveningPeriods = new List<Period>();
             Random rdm = GlobalVariables.rdm;
             int nbPeriodsPerDay = GlobalVariables.NUMBER_OF_PERIODS;
             int nbHoursPerDay = GlobalVariables.NUMBER_OF_HOURS_PER_DAY;
             Type selectedActivity;
             while (eveningTotalPeriods > 0)
             {
                 int RestaurantMinPeriods = 2;
                 int SupermarketMinPeriods = 1;

                 if (eveningTotalPeriods < RestaurantMinPeriods)
                 {
                     selectedActivity = rdm.NextProbability(new KeyValuePair<Type, double>[] {
                         new KeyValuePair<Type, double>(typeof(Home), 0.90),
                         new KeyValuePair<Type, double>(typeof(Supermarket), 0.10)
                     });
                 }
                 else if (eveningTotalPeriods < SupermarketMinPeriods)
                 {
                     selectedActivity = typeof(Home);
                 }
                 else
                 {
                     selectedActivity = rdm.NextProbability(new KeyValuePair<Type, double>[] {
                         new KeyValuePair<Type, double>(typeof(Home), 0.50),
                         new KeyValuePair<Type, double>(typeof(Supermarket), 0.40),
                         new KeyValuePair<Type, double>(typeof(Restaurant), 0.10)
                     });
                 }

                 // Temps min = 0, max = full;
                 if (selectedActivity == typeof(Home))
                 {
                     int min = 5 * nbPeriodsPerDay / nbHoursPerDay;
                     int max = 8 * nbPeriodsPerDay / nbHoursPerDay;
                     eveningPeriods.AddRange(CreateSiteActivity(min, max, typeof(Restaurant)));
                 }

                 // temps min = 1, max = 3
                 if (selectedActivity == typeof(Supermarket))
                 {
                     int min = 5 * nbPeriodsPerDay / nbHoursPerDay;
                     int max = 8 * nbPeriodsPerDay / nbHoursPerDay;
                     eveningPeriods.AddRange(CreateSiteActivity(min, max, typeof(Restaurant)));
                 }

                 // Temps min = 2, max = 6
                 if (selectedActivity == typeof(Restaurant))
                 {
                     int min = 5 * nbPeriodsPerDay / nbHoursPerDay;
                     int max = 8 * nbPeriodsPerDay / nbHoursPerDay;
                     eveningPeriods.AddRange(CreateSiteActivity(min, max, typeof(Restaurant)));
                 }
                 eveningTotalPeriods--;
             }
             return eveningPeriods;
         }

         private List<Period> CreateSiteActivity(int min, int max, Type site)
         {
             Random rdm = GlobalVariables.rdm;
             List<Period> eveningPeriods = new List<Period>();

             int eveningPeriodsNb = rdm.Next(min, max);

             while (eveningPeriodsNb > 0)
             {
                 //homePeriods.Add(new Period(site));
                 eveningPeriodsNb--;
             }
             return eveningPeriods;
         }*/
    }
}
