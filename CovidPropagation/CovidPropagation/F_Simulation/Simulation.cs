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

            GenerateWeekDay();
            //GenerateFreeDay();
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

        private string GenerateWeekDay()
        {
            int totalTimeFrame = GlobalVariables.NUMBER_OF_TIMEFRAME;
            Random rdm = GlobalVariables.rdm;

            List<Site> personSites = new List<Site>() { new Home(), new Store(), new Restaurant(), new Car(), new Company() };

            int morningTimeFrameMax = 16, morningVariation = 4;
            int morningTimeFrameTotal = 22;
            int noonTimeFrameMax = 4, noonMin = 3;
            int afterNoonTimeFrameMax = 10, afterNoonVariation = 4;
            int afterNoonTimeFrameTotal = 10;
            int eveningTimeFrameMax = 8, eveningMin = 3;
            int nightTimeFrame; // remplit ce qu'il manque

            int morningWorkTimeFrame = 0;
            int afterNoonFreeTimeTimeFrame = 0;
            int eveningActivityTimeFrame = 0;

            int morningTimeFrame = morningTimeFrameMax - rdm.NextWithMinimum(0, morningVariation, 0);
            int noonTimeFrame = rdm.Next(noonMin, noonTimeFrameMax + 1);
            int afterNoonWorkTimeFrame = afterNoonTimeFrameMax - rdm.NextWithMinimum(0, afterNoonVariation, 0);
            int eveningTimeFrame = rdm.NextWithMinimum(eveningMin, eveningTimeFrameMax, 3);

            // Activities
            #region Activities

            if (morningTimeFrame < morningTimeFrameTotal)
                morningWorkTimeFrame = morningTimeFrameTotal - morningTimeFrame;

            if (noonTimeFrame < noonTimeFrameMax)
                afterNoonTimeFrameMax += 1;

            if (afterNoonWorkTimeFrame < afterNoonTimeFrameTotal)
                afterNoonFreeTimeTimeFrame = afterNoonTimeFrameTotal - afterNoonWorkTimeFrame;

            if (eveningTimeFrame < eveningTimeFrameMax)
                eveningActivityTimeFrame = eveningTimeFrameMax - eveningTimeFrame;
            #endregion

            nightTimeFrame = (morningTimeFrame + morningWorkTimeFrame) +
                           (noonTimeFrame) +
                           (afterNoonWorkTimeFrame + afterNoonFreeTimeTimeFrame) +
                           (eveningTimeFrame + eveningActivityTimeFrame);

            nightTimeFrame = totalTimeFrame - nightTimeFrame;

            Site hometSite = personSites.Where(h => h.Type.Contains(SiteType.Home)).OrderBy(x => rdm.Next()).First();
            Site morningWorkSite = personSites.Where(h => h.Type.Contains(SiteType.Work)).OrderBy(x => rdm.Next()).First();
            Site noonSite = personSites.Where(h => h.Type.Contains(SiteType.Eat)).OrderBy(x => rdm.Next()).First();
            Site afterNoonWorkSite = personSites.Where(h => h.Type.Contains(SiteType.Work)).OrderBy(x => rdm.Next()).First();
            Site afterNoonFreeTimeSite = personSites.Where(h => h.Type.Contains(SiteType.Hobby)).OrderBy(x => rdm.Next()).First();
            Site eveningActivitySite = personSites.Where(h => h.Type.Contains(SiteType.Eat)).OrderBy(x => rdm.Next()).First();
            Site transportSite = personSites.Where(h => h.Type.Contains(SiteType.Transport)).OrderBy(x => rdm.Next()).First();

            List<TimeFrame> timeFrames = new List<TimeFrame>();
            // Changer les siteType pour définir le type
            CreateMorning(timeFrames, hometSite, morningTimeFrame, SitePersonStatus.Other, morningWorkSite, morningWorkTimeFrame, SitePersonStatus.Worker, transportSite);
            CreateNoon(timeFrames, noonSite, noonTimeFrame, SitePersonStatus.Client, transportSite);
            CreateAfterNoon(timeFrames, afterNoonWorkSite, afterNoonWorkTimeFrame, SitePersonStatus.Other, afterNoonFreeTimeSite, afterNoonFreeTimeTimeFrame, SitePersonStatus.Worker, transportSite);
            CreateEvening(timeFrames, hometSite, eveningTimeFrame, SitePersonStatus.Other, eveningActivitySite, eveningActivityTimeFrame, SitePersonStatus.Client, transportSite);
            CreateNight(timeFrames, hometSite, nightTimeFrame, transportSite);

            foreach (var item in timeFrames)
            {
                Debug.WriteLine(item.Activity);
            }
            return "";
        }

        private string GenerateFreeDay()
        {
            int totalTimeFrame = GlobalVariables.NUMBER_OF_TIMEFRAME;
            Random rdm = GlobalVariables.rdm;

            List<Site> personSites = new List<Site>() { new Home(), new Store(), new Restaurant(), new Car()};

            int morningTimeFrameMax = 22, morningMin = 12;
            int noonTimeFrameMax = 4, noonMin = 3;
            int afterNoonTimeFrameMax = 10, afterNoonMin = 5;
            int eveningTimeFrameMax = 8, eveningMin = 3;
            int nightTimeFrame; // remplit ce qu'il manque

            int morningActivityTimeFrame = 0;
            int afterNoonActivityTimeFrame = 0;
            int eveningActivityTimeFrame = 0;

            int morningTimeFrame = rdm.NextWithMinimum(morningMin, morningTimeFrameMax, 3);
            int noonTimeFrame = rdm.Next(noonMin, noonTimeFrameMax + 1);
            int afterNoonTimeFrame = rdm.NextWithMinimum(afterNoonMin, afterNoonTimeFrameMax, 3);
            int eveningTimeFrame = rdm.NextWithMinimum(eveningMin, eveningTimeFrameMax, 3);

            // Activities
            #region Activities

            if (morningTimeFrame < morningTimeFrameMax)
                morningActivityTimeFrame = morningTimeFrameMax - morningTimeFrame;

            if (noonTimeFrame < noonTimeFrameMax)
                afterNoonTimeFrameMax += 1;

            if (afterNoonTimeFrame < afterNoonTimeFrameMax)
                afterNoonActivityTimeFrame = afterNoonTimeFrameMax - afterNoonTimeFrame;

            if (eveningTimeFrame < eveningTimeFrameMax)
                eveningActivityTimeFrame = eveningTimeFrameMax - eveningTimeFrame;
            #endregion

            nightTimeFrame = (morningTimeFrame + morningActivityTimeFrame) +
                           (noonTimeFrame) +
                           (afterNoonTimeFrame + afterNoonActivityTimeFrame) +
                           (eveningTimeFrame + eveningActivityTimeFrame);

            nightTimeFrame = totalTimeFrame - nightTimeFrame;

            Debug.WriteLine(eveningTimeFrame);

            // établir différement
            Site homeSite = personSites.Where(h => h.Type.Contains(SiteType.Home)).OrderBy(x => rdm.Next()).First();
            Site morningActivitySite = personSites.Where(h => h.Type.Contains(SiteType.Hobby)).OrderBy(x => rdm.Next()).First();
            Site noonSite = personSites.Where(h => h.Type.Contains(SiteType.Eat)).OrderBy(x => rdm.Next()).First();
            Site afterNoonActivitySite = personSites.Where(h => h.Type.Contains(SiteType.Hobby)).OrderBy(x => rdm.Next()).First();
            Site eveningActivitySite = personSites.Where(h => h.Type.Contains(SiteType.Eat)).OrderBy(x => rdm.Next()).First();
            Site transportSite = personSites.Where(h => h.Type.Contains(SiteType.Transport)).OrderBy(x => rdm.Next()).First();

            // Morning
            List<TimeFrame> timeFrames = new List<TimeFrame>();
            CreateMorning(timeFrames, homeSite, morningTimeFrame, SitePersonStatus.Other, morningActivitySite, morningActivityTimeFrame, SitePersonStatus.Client, transportSite);
            CreateNoon(timeFrames, noonSite, noonTimeFrame, SitePersonStatus.Client, transportSite);
            CreateAfterNoon(timeFrames, homeSite, afterNoonTimeFrame, SitePersonStatus.Other, afterNoonActivitySite, afterNoonActivityTimeFrame, SitePersonStatus.Client, transportSite);
            CreateEvening(timeFrames, homeSite, eveningTimeFrame, SitePersonStatus.Other, eveningActivitySite, eveningActivityTimeFrame, SitePersonStatus.Client, transportSite);
            CreateNight(timeFrames, homeSite, nightTimeFrame, transportSite);

            foreach (var item in timeFrames)
            {
                Debug.WriteLine(item.Activity);
            }
            return "";
        }

        private void CreateMorning(List<TimeFrame> morningTimeFrames, Site defaultSite, int defaultTimeFrame, SitePersonStatus defaultType, Site activitySite, int activityTimeFrame, SitePersonStatus activityType ,Site transportSite)
        {
            morningTimeFrames.AddRange(Enumerable.Repeat(new TimeFrame(defaultSite, defaultType), defaultTimeFrame));
            if (activitySite != defaultSite && activityTimeFrame > 0)
            {
                morningTimeFrames.RemoveAt(morningTimeFrames.GetLastIndex());
                morningTimeFrames.Add(new TimeFrame(transportSite, SitePersonStatus.Client));
            }
            morningTimeFrames.AddRange(Enumerable.Repeat(new TimeFrame(activitySite, activityType), activityTimeFrame));
        }

        private void CreateNoon(List<TimeFrame> timeFrames, Site site, int siteTimeFrames, SitePersonStatus siteType, Site transportSite)
        {
            if (timeFrames[timeFrames.GetLastIndex()].Activity != site)
            {
                timeFrames.RemoveAt(timeFrames.GetLastIndex());
                timeFrames.Add(new TimeFrame(transportSite, SitePersonStatus.Client));
            }
            timeFrames.AddRange(Enumerable.Repeat(new TimeFrame(site, siteType), siteTimeFrames));
        }

        private void CreateAfterNoon(List<TimeFrame> timeFrames, Site site, int siteTimeFrames, SitePersonStatus siteType, Site activitySite, int activityTimeFrames, SitePersonStatus activityType, Site transportSite)
        {
            if (timeFrames[timeFrames.GetLastIndex()].Activity != site)
            {
                timeFrames.Add(new TimeFrame(transportSite, SitePersonStatus.Client));
                siteTimeFrames--;
            }
            timeFrames.AddRange(Enumerable.Repeat(new TimeFrame(site, siteType), siteTimeFrames));
            if (activitySite != site && activityTimeFrames > 0)
            {
                timeFrames.Add(new TimeFrame(transportSite, SitePersonStatus.Client));
                activityTimeFrames--;
            }
            timeFrames.AddRange(Enumerable.Repeat(new TimeFrame(activitySite, activityType), activityTimeFrames));
        }

        private void CreateEvening(List<TimeFrame> timeFrames, Site site, int siteTimesFrames, SitePersonStatus siteType, Site activitySite, int activityTimeFrames, SitePersonStatus activityType, Site transportSite)
        {
            if (timeFrames[timeFrames.GetLastIndex()].Activity != site)
            {
                timeFrames.RemoveAt(timeFrames.GetLastIndex());
                timeFrames.Add(new TimeFrame(transportSite, SitePersonStatus.Client));
            }
            timeFrames.AddRange(Enumerable.Repeat(new TimeFrame(site, siteType), siteTimesFrames));
            if (activitySite != site && activityTimeFrames > 0)
            {
                timeFrames.Add(new TimeFrame(transportSite, SitePersonStatus.Client));
                activityTimeFrames--;
            }
            timeFrames.AddRange(Enumerable.Repeat(new TimeFrame(activitySite, activityType), activityTimeFrames));
        }

        private void CreateNight(List<TimeFrame> timeFrames, Site site, int siteTimeFrames, Site transportSite)
        {
            if (timeFrames[timeFrames.GetLastIndex()].Activity != site)
            {
                timeFrames.RemoveAt(timeFrames.GetLastIndex());
                timeFrames.Add(new TimeFrame(transportSite, SitePersonStatus.Client));
            }
            timeFrames.AddRange(Enumerable.Repeat(new TimeFrame(site, SitePersonStatus.Other), siteTimeFrames));
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
    }
}
