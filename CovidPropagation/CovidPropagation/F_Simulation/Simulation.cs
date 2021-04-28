/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 27.04.2021
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



        }

        private void CreatePopulation()
        {
            int _personAge = 0;
            Site _dayActivity;
            if (_personAge < 18)
            {
                _dayActivity = new School(5); // A modifier par le fait de récupérer dans une liste d'école disponibles.
            }
            else if (_personAge < 65)
            {
                _dayActivity = new Company(5);
            }
            else
            {
                _dayActivity = new Company(5);
            }
        }
    }
}
