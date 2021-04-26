using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation.Simulation
{
    public class Simulation
    {
        private int _averageAge;
        private int _nbInfected;
        private int _nbPersons;
        Random rdm;
        List<Site>[] allSites;
        List<Vehicle>[] allTransports;
        List<Person> population;
        public Simulation(int avgAge, int nbInfected, int nbPersons)
        {
            _averageAge = avgAge;
            _nbInfected = nbInfected;
            _nbPersons = nbPersons;
            rdm = new Random();
            allSites = new List<Site>[] { 
                new List<Site>(),
                new List<Site>(),
                new List<Site>(),
                new List<Site>(),
                new List<Site>()
            };

            allTransports = new List<Vehicle>[] {
                new List<Vehicle>(),
                new List<Vehicle>(),
                new List<Vehicle>(),
                new List<Vehicle>(),
            };
            population = new List<Person>();

        }

        private void CreateBuildings()
        {
            double nbOfSchool = 0.03d * 100 / _nbPersons;
            double nbOfCompany = 6.74d * 100 / _nbPersons;
            double nbOfHomes = 0.01d * 100 / _nbPersons;
            double nbOfSupermarket = 0.03d * 100 / _nbPersons;
            double nbOfHospital = 50d * 100 / _nbPersons;

            int[] nbOfSites = new int[] {
                (int)Math.Ceiling(nbOfSchool),
                (int)Math.Ceiling(nbOfCompany),
                (int)Math.Ceiling(nbOfHomes),
                (int)Math.Ceiling(nbOfSupermarket),
                (int)Math.Ceiling(nbOfHospital)
            };

            Type[] typeOfSite = new Type[] {
                typeof(School),
                typeof(Company),
                typeof(Home),
                typeof(Supermarket),
                typeof(Hospital),
            };

            for (int indexSiteType = 0; indexSiteType < allSites.Length; indexSiteType++)
            {
                for (int i = 0; i < nbOfSites[indexSiteType]; i++)
                {
                    allSites[indexSiteType].Add(new School(1));
                }
            }

        }

        private void CreateTransports()
        {
            double nbOfCar = 0.03d * 100 / _nbPersons;
            double nbOfBus = 6.74d * 100 / _nbPersons;
            double nbOfBikes = 0.01d * 100 / _nbPersons;
            double nbOfWalking = 0.03d * 100 / _nbPersons;

            int[] nbOfTransports = new int[] {
                (int)Math.Ceiling(nbOfCar),
                (int)Math.Ceiling(nbOfBus),
                (int)Math.Ceiling(nbOfBikes),
                (int)Math.Ceiling(nbOfWalking)
            };

            for (int indexTransportType = 0; indexTransportType < allSites.Length; indexTransportType++)
            for (int i = 0; i < nbOfTransports[indexTransportType]; i++)
            {
                // Create Transports

            }
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
