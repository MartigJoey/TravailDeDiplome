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
            CreateBuildings();
            CreateTransports();
            CreatePopulation();
        }

        public async void Iterate()
        {
            while (true)
            {
                Debug.WriteLine(Interval);
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
                    Debug.WriteLine("Tick");
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

        private void CreateBuildings()
        {
            int populationInSchool = population.Where(p => p.Age < GlobalVariables.MAX_SCHOOL_AGE).Count();
            int populationInCompanies = population.Where(p => p.Age > GlobalVariables.MAX_SCHOOL_AGE).Count();

            int nbOfSchool = (int)Math.Ceiling((0.03d - 100) / 100 * _nbPersons + _nbPersons);
            int nbOfCompany = (int)Math.Ceiling((6.74d - 100) / 100 * _nbPersons + _nbPersons);
            int nbOfHospital = (int)Math.Ceiling((0.01d - 100) / 100 * _nbPersons + _nbPersons);
            int nbOfSupermarket = (int)Math.Ceiling((0.03d - 100) / 100 * _nbPersons + _nbPersons);
            int nbOfHomes = (int)Math.Ceiling((50d - 100) / 100 * _nbPersons + _nbPersons);

            allBuildingSites.Add(new Outside(_nbPersons));
            
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

        private void CreatePopulation()
        {
            // Récupérer depuis les paramètres
            double minorProbability = 0.22d;
            double retirementProbability = 0.14d;
            double workingProbability = 1 - minorProbability - retirementProbability;

            int nbMinor = (int)Math.Round(minorProbability * _nbPersons);
            int nbRetirement = (int)Math.Round(retirementProbability * _nbPersons);
            int nbWorking = (int)Math.Round(workingProbability * _nbPersons);

            while (nbMinor > 0)
            {
                population.Add(CreateStudentPerson());
                nbMinor--;
            }

            while (nbRetirement > 0)
            {
                population.Add(CreateAdultClass());
                nbRetirement--;
            }

            while (nbWorking > 0)
            {
                population.Add(CreateElderPerson());
                nbWorking--;
            }
        }

        private Person CreateStudentPerson()
        {
            Person student;
            Planning planning = new Planning();
            planning.CreateStudentPlanning();
            student = new Person(planning);
            return student;
        }

        private Person CreateAdultClass()
        {
            Person adult;
            Planning planning = new Planning();
            planning.CreateAdultPlanning();
            adult = new Person(planning);
            return adult;
        }

        private Person CreateElderPerson()
        {
            Person elder;
            Planning planning = new Planning();
            planning.CreateElderPlanning();
            elder = new Person(planning);
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
