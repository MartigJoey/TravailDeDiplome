
/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CovidPropagation
{
    /// <summary>
    /// ID Documentation : Day_Class
    /// Jour composé de plusieurs périodes.
    /// </summary>
    public class Day
    {
        private const int FREEDAY_MORNING_TIME_FRAME_MAX = 22;
        private const int FREEDAY_MORNING_MIN = 12;

        private const int FREEDAY_NOON_TIME_FRAME_MAX = 4;
        private const int FREEDAY_NOON_MIN = 3;

        private const int FREEDAY_AFTER_NOON_TIME_FRAME_MAX = 10;
        private const int FREEDAY_AFTER_NOON_MIN = 5;

        private const int FREEDAY_EVENING_TIME_FRAME_MAX = 8;
        private const int FREEDAY_EVENING_MIN = 3;

        private const int FREEDAY_MIN_TIME_FRAME_VALUE = 3;

        private const int WORKDAY_MORNING_TIME_FRAME_MAX = 16;
        private const int WORKDAY_MORNING_MIN = 4;
        private const int WORKDAY_MORNING_TIME_FRAME_TOTAL = 22;

        private const int WORKDAY_NOON_TIME_FRAME_MAX = 4;
        private const int WORKDAY_NOON_MIN = 3;

        private const int WORKDAY_AFTER_NOON_TIME_FRAME_MAX = 10;
        private const int WORKDAY_AFTER_NOON_MIN = 4;
        private const int WORKDAY_AFTER_NOON_TIME_FRAME_TOTAL = 10;

        private const int WORKDAY_EVENING_TIME_FRAME_MAX = 8;
        private const int WORKDAY_EVENING_MIN = 3;

        private const int WORKDAY_MIN_TIME_FRAME_VALUE = 3;


        private TimeFrame[] _timeFrames;
        public TimeFrame[] TimeFrames { get => _timeFrames; }

        /// <summary>
        /// ID Documentation : Day_Creation
        /// </summary>
        public Day(Dictionary<SiteType, List<Site>> personSites, bool isWorkDay)
        {
            if (isWorkDay)
                _timeFrames = CreateWorkDay(personSites);
            else
                _timeFrames = CreateFreeDay(personSites);
        }

        /// <summary>
        /// Récupère l'activité à l'index spécifié.
        /// </summary>
        /// <param name="timeFrame">Index de la période à récupérer</param>
        /// <returns>Activité dans la période.</returns>
        public Site GetActivity(int timeFrame)
        {
            return _timeFrames[timeFrame].Activity;
        }

        /// <summary>
        /// Récupère l'activité actuelle
        /// </summary>
        /// <returns>L'activité en cours.</returns>
        public Site GetCurrentActivity()
        {
            return _timeFrames[TimeManager.CurrentTimeFrame].Activity;
        }

        /// <summary>
        /// Récupère la raison pour laquel l'individu est dans le lieu actuel.
        /// </summary>
        /// <returns>Raison de se situer sur le lieu actuel.</returns>
        public SitePersonStatus GetCurrentPersonTypeInActivity()
        {
            return _timeFrames[TimeManager.CurrentTimeFrame].PersonStatus;
        }

        /// <summary>
        /// ID Documentation : WorkDay_Creation
        /// Créé un jour de travail pour une personne avec les lieux qui lui sont fournies.
        /// </summary>
        /// <param name="personSites">Lieux avec lequel le planning sera créé</param>
        /// <returns></returns>
        private TimeFrame[] CreateWorkDay(Dictionary<SiteType, List<Site>> personSites)
        {
            int totalTimeFrame = GlobalVariables.NUMBER_OF_TIMEFRAME;
            Random rdm = GlobalVariables.rdm;
            List<TimeFrame> timeFrames;

            int morningTimeFrameMax = WORKDAY_MORNING_TIME_FRAME_MAX, morningVariation = WORKDAY_MORNING_MIN;
            int morningTimeFrameTotal = WORKDAY_MORNING_TIME_FRAME_TOTAL;
            int noonTimeFrameMax = WORKDAY_NOON_TIME_FRAME_MAX, noonMin = WORKDAY_NOON_MIN;
            int afterNoonTimeFrameMax = WORKDAY_AFTER_NOON_TIME_FRAME_MAX, afterNoonVariation = WORKDAY_AFTER_NOON_MIN;
            int afterNoonTimeFrameTotal = WORKDAY_AFTER_NOON_TIME_FRAME_TOTAL;
            int eveningTimeFrameMax = WORKDAY_EVENING_TIME_FRAME_MAX, eveningMin = WORKDAY_EVENING_MIN;
            int nightTimeFrame; // remplit ce qu'il manque

            int morningWorkTimeFrame = 0;
            int afterNoonFreeTimeTimeFrame = 0;
            int eveningActivityTimeFrame = 0;

            int morningTimeFrame = morningTimeFrameMax - rdm.NextWithMinimum(0, morningVariation, 0);
            int noonTimeFrame = rdm.Next(noonMin, noonTimeFrameMax + 1);
            int afterNoonWorkTimeFrame = afterNoonTimeFrameMax - rdm.NextWithMinimum(0, afterNoonVariation, 0);
            int eveningTimeFrame = rdm.NextWithMinimum(0, eveningTimeFrameMax, WORKDAY_MIN_TIME_FRAME_VALUE);

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

            // Choisis les lieux où l'individus va passer son temps en fonction des données reçues.
            KeyValuePair<Site, SitePersonStatus> homeSite = new KeyValuePair<Site, SitePersonStatus>(
                personSites[SiteType.Home][0],
                SitePersonStatus.Other
                );

            // Récupère le lieu de travail et la raison d'y aller
            KeyValuePair<Site, SitePersonStatus> workSite = new KeyValuePair<Site, SitePersonStatus>(
                personSites[SiteType.WorkPlace][rdm.Next(0, personSites[SiteType.WorkPlace].Count)],
                SitePersonStatus.Worker
                );

            // Manger sur le lieu de travail ou ailleurs
            KeyValuePair<Site, SitePersonStatus> noonSite;
            if (rdm.NextBoolean())
                noonSite = new KeyValuePair<Site, SitePersonStatus>(
                personSites[SiteType.Eat][rdm.Next(0, personSites[SiteType.Eat].Count)],
                SitePersonStatus.Client
                );
            else
                noonSite = workSite;

            // Récupère le lieux de l'après midi si l'individu quitte le travail plus tôt
            SiteType afterNoonSiteType = (SiteType)rdm.NextInclusive(0, (int)SiteType.Eat);
            KeyValuePair<Site, SitePersonStatus> afterNoonFreeTimeSite = new KeyValuePair<Site, SitePersonStatus>(
                personSites[afterNoonSiteType][rdm.Next(0, personSites[afterNoonSiteType].Count)],
                SitePersonStatus.Worker
                );

            // Récupère le lieux du soir si l'individu quitte le travail plus tôt
            KeyValuePair<Site, SitePersonStatus> eveningActivitySite = new KeyValuePair<Site, SitePersonStatus>(
                personSites[SiteType.Eat][rdm.Next(0, personSites[SiteType.Eat].Count)],
                SitePersonStatus.Client
                );


            // Récupère le moyen de transport et la raison de se situer dedans
            KeyValuePair<Site, SitePersonStatus> transportSite = new KeyValuePair<Site, SitePersonStatus>(
                personSites[SiteType.Transport][rdm.Next(0, personSites[SiteType.Transport].Count)],
                SitePersonStatus.Other
                );

            timeFrames = new List<TimeFrame>(totalTimeFrame);

            // Une fois que tous les lieux sont choisis, les périodes de la journée sont créés.
            CreateMorning(timeFrames, homeSite, morningTimeFrame, workSite, morningWorkTimeFrame, transportSite);
            CreateNoon(timeFrames, noonSite, noonTimeFrame, transportSite);
            CreateAfterNoon(timeFrames, workSite, afterNoonWorkTimeFrame, afterNoonFreeTimeSite, afterNoonFreeTimeTimeFrame, transportSite);
            CreateEvening(timeFrames, homeSite, eveningTimeFrame, eveningActivitySite, eveningActivityTimeFrame, transportSite);
            CreateNight(timeFrames, homeSite, nightTimeFrame, transportSite);

            return timeFrames.ToArray();
        }

        /// <summary>
        /// Créé un jour de congé pour une personne avec les lieux qui lui sont attribués
        /// </summary>
        /// <param name="personSites">Lieux avec lequel le planning sera créé</param>
        /// <returns>Tableaux contenant les timesFrames créés.</returns>
        private TimeFrame[] CreateFreeDay(Dictionary<SiteType, List<Site>> personSites)
        {
            int totalTimeFrame = GlobalVariables.NUMBER_OF_TIMEFRAME;
            Random rdm = GlobalVariables.rdm;
            List<TimeFrame> timeFrames;

            int morningTimeFrameMax = FREEDAY_MORNING_TIME_FRAME_MAX, morningMin = FREEDAY_MORNING_MIN;
            int noonTimeFrameMax = FREEDAY_NOON_TIME_FRAME_MAX, noonMin = FREEDAY_NOON_MIN;
            int afterNoonTimeFrameMax = FREEDAY_AFTER_NOON_TIME_FRAME_MAX, afterNoonMin = FREEDAY_AFTER_NOON_MIN;
            int eveningTimeFrameMax = FREEDAY_EVENING_TIME_FRAME_MAX, eveningMin = FREEDAY_EVENING_MIN;
            int nightTimeFrame; // remplit ce qu'il manque

            int morningActivityTimeFrame = 0;
            int afterNoonActivityTimeFrame = 0;
            int eveningActivityTimeFrame = 0;

            int morningTimeFrame = rdm.NextWithMinimum(morningMin, morningTimeFrameMax, FREEDAY_MIN_TIME_FRAME_VALUE);
            int noonTimeFrame = rdm.Next(noonMin, noonTimeFrameMax + 1);
            int afterNoonTimeFrame = rdm.NextWithMinimum(afterNoonMin, afterNoonTimeFrameMax, FREEDAY_MIN_TIME_FRAME_VALUE);
            int eveningTimeFrame = rdm.NextWithMinimum(0, eveningTimeFrameMax, FREEDAY_MIN_TIME_FRAME_VALUE);

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

            KeyValuePair<Site, SitePersonStatus> homeSite = new KeyValuePair<Site, SitePersonStatus>(
                personSites[SiteType.Home][rdm.Next(0, personSites[SiteType.Home].Count)],
                SitePersonStatus.Other
                );

            SiteType morningActivitySiteType = (SiteType)rdm.Next(0, (int)SiteType.Eat);
            KeyValuePair<Site, SitePersonStatus> morningActivitySite = new KeyValuePair<Site, SitePersonStatus>(
                personSites[morningActivitySiteType][rdm.Next(0, personSites[morningActivitySiteType].Count)],
                SitePersonStatus.Client
                );

            KeyValuePair<Site, SitePersonStatus> noonSite = new KeyValuePair<Site, SitePersonStatus>(
                personSites[SiteType.Eat][rdm.Next(0, personSites[SiteType.Eat].Count)],
                SitePersonStatus.Client
                );

            SiteType afterNoonActivitySiteType = (SiteType)rdm.Next(0, (int)SiteType.Eat);
            KeyValuePair<Site, SitePersonStatus> afterNoonActivitySite = new KeyValuePair<Site, SitePersonStatus>(
                personSites[afterNoonActivitySiteType][rdm.Next(0, personSites[afterNoonActivitySiteType].Count)],
                SitePersonStatus.Other
                );

            SiteType eveningActivitySiteType = (SiteType)rdm.Next(0, (int)SiteType.Eat);
            KeyValuePair<Site, SitePersonStatus> eveningActivitySite = new KeyValuePair<Site, SitePersonStatus>(
                personSites[eveningActivitySiteType][rdm.Next(0, personSites[eveningActivitySiteType].Count)],
                SitePersonStatus.Other
                );


            KeyValuePair<Site, SitePersonStatus> transportSite = new KeyValuePair<Site, SitePersonStatus>(
                personSites[SiteType.Transport][rdm.Next(0, personSites[SiteType.Transport].Count)],
                SitePersonStatus.Other
                );

            timeFrames = new List<TimeFrame>(totalTimeFrame);
            CreateMorning(timeFrames, homeSite, morningTimeFrame, morningActivitySite, morningActivityTimeFrame, transportSite);
            CreateNoon(timeFrames, noonSite, noonTimeFrame, transportSite);
            CreateAfterNoon(timeFrames, homeSite, afterNoonTimeFrame, afterNoonActivitySite, afterNoonActivityTimeFrame, transportSite);
            CreateEvening(timeFrames, homeSite, eveningTimeFrame, eveningActivitySite, eveningActivityTimeFrame, transportSite);
            CreateNight(timeFrames, homeSite, nightTimeFrame, transportSite);

            return timeFrames.ToArray();
        }

        /// <summary>
        /// Créé la matinée d'une personne dans son planning.
        /// Comprend un certain temps dans son appartement, puis une possible activité.
        /// Peut comprendre un lieu de travail à la place d'une activité.
        /// Si les lieux sont différents les uns des autres, des trajets sont ajouté entre eux modifiant les timeFrames de ceux-ci.
        /// </summary>
        /// <param name="morningTimeFrames">Durée de la matinée en timeFrames</param>
        /// <param name="home">Appartement de la personne.</param>
        /// <param name="homeTimeFrame">Durée dans l'appartement en timeFrames</param>
        /// <param name="activity">Lieu de l'activité à effectuer / travail.</param>
        /// <param name="activityTimeFrame">Durée de l'activité / travail.</param>
        /// <param name="transport">Type de transport utilisé pour se déplacer entre deux lieux.</param>
        private void CreateMorning(List<TimeFrame> morningTimeFrames, KeyValuePair<Site, SitePersonStatus> home, int homeTimeFrame, KeyValuePair<Site, SitePersonStatus> activity, int activityTimeFrame, KeyValuePair<Site, SitePersonStatus> transport)
        {
            morningTimeFrames.AddRange(Enumerable.Repeat(new TimeFrame(home.Key, home.Value), homeTimeFrame));
            if (activity.Key != home.Key && activityTimeFrame > 0)
            {
                morningTimeFrames.RemoveAt(morningTimeFrames.GetLastIndex());
                morningTimeFrames.Add(new TimeFrame(transport.Key, transport.Value));
            }
            morningTimeFrames.AddRange(Enumerable.Repeat(new TimeFrame(activity.Key, activity.Value), activityTimeFrame));
        }

        /// <summary>
        /// Créé les heures du midi d'une personne dans son plannning.
        /// Peut se situer dans divers lieu d'on le lieu où elle se situe déjà.
        /// Si le lieu est différent, des trajets sont ajouté.
        /// </summary>
        /// <param name="timeFrames">Liste des précédents lieux.</param>
        /// <param name="site">Lieu dans lequel la section de midi se situe.</param>
        /// <param name="siteTimeFrames">Durée de la section de midi en timeFrames.</param>
        /// <param name="transportSite">Type de transport utilisé pour se déplacer entre deux lieux.</param>
        private void CreateNoon(List<TimeFrame> timeFrames, KeyValuePair<Site, SitePersonStatus> site, int siteTimeFrames, KeyValuePair<Site, SitePersonStatus> transportSite)
        {
            if (timeFrames[timeFrames.GetLastIndex()].Activity != site.Key)
            {
                timeFrames.RemoveAt(timeFrames.GetLastIndex());
                timeFrames.Add(new TimeFrame(transportSite.Key, transportSite.Value));
            }
            timeFrames.AddRange(Enumerable.Repeat(new TimeFrame(site.Key, site.Value), siteTimeFrames));
        }

        /// <summary>
        /// Créé l'après-midi d'une personne dans son planning.
        /// Comprend un certain temps dans son appartement ou au travail, puis une possible activité.
        /// Si les lieux sont différents les uns des autres, des trajets sont ajouté entre eux modifiant les timeFrames de ceux-ci.
        /// </summary>
        /// <param name="timeFrames">Liste des précédents lieux.</param>
        /// <param name="site">Lieu dans lequel l'après-midi se situe.</param>
        /// <param name="siteTimeFrames">Durée de la section de l'après-midi en timeFrames.</param>
        /// <param name="activitySite">Lieu d'une possible activité.</param>
        /// <param name="activityTimeFrames">Durée de l'activité si celle-ci à lieu en timeFrames.</param>
        /// <param name="transportSite">Type de transport utilisé pour se déplacer entre deux lieux.</param>
        private void CreateAfterNoon(List<TimeFrame> timeFrames, KeyValuePair<Site, SitePersonStatus> site, int siteTimeFrames, KeyValuePair<Site, SitePersonStatus> activitySite, int activityTimeFrames, KeyValuePair<Site, SitePersonStatus> transportSite)
        {
            if (timeFrames[timeFrames.GetLastIndex()].Activity != site.Key)
            {
                timeFrames.Add(new TimeFrame(transportSite.Key, transportSite.Value));
                siteTimeFrames--;
            }
            timeFrames.AddRange(Enumerable.Repeat(new TimeFrame(site.Key, site.Value), siteTimeFrames));
            if (activitySite.Key != site.Key && activityTimeFrames > 0)
            {
                timeFrames.Add(new TimeFrame(transportSite.Key, transportSite.Value));
                activityTimeFrames--;
            }
            timeFrames.AddRange(Enumerable.Repeat(new TimeFrame(activitySite.Key, activitySite.Value), activityTimeFrames));
        }

        /// <summary>
        /// Créé la soirée d'une personne dans son planning.
        /// Comprend un certain temps dans son appartement, puis une possible activité.
        /// Si les lieux sont différents les uns des autres, des trajets sont ajouté entre eux modifiant les timeFrames de ceux-ci.
        /// </summary>
        /// <param name="timeFrames">Liste des précédents lieux.</param>
        /// <param name="site">Lieu dans lequel la soirée se situe.</param>
        /// <param name="siteTimeFrames">Durée de la section de la soirée en timeFrames.</param>
        /// <param name="activitySite">Lieu d'une possible activité.</param>
        /// <param name="activityTimeFrames">Durée de l'activité si celle-ci à lieu en timeFrames.</param>
        /// <param name="transportSite">Type de transport utilisé pour se déplacer entre deux lieux.</param>
        private void CreateEvening(List<TimeFrame> timeFrames, KeyValuePair<Site, SitePersonStatus> site, int siteTimeFrames, KeyValuePair<Site, SitePersonStatus> activitySite, int activityTimeFrames, KeyValuePair<Site, SitePersonStatus> transportSite)
        {
            if (timeFrames[timeFrames.GetLastIndex()].Activity != site.Key)
            {
                timeFrames.RemoveAt(timeFrames.GetLastIndex());
                timeFrames.Add(new TimeFrame(transportSite.Key, transportSite.Value));
            }
            timeFrames.AddRange(Enumerable.Repeat(new TimeFrame(site.Key, site.Value), siteTimeFrames));
            if (activitySite.Key != site.Key && activityTimeFrames > 0)
            {
                timeFrames.Add(new TimeFrame(transportSite.Key, transportSite.Value));
                activityTimeFrames--;
            }
            timeFrames.AddRange(Enumerable.Repeat(new TimeFrame(activitySite.Key, activitySite.Value), activityTimeFrames));
        }

        /// <summary>
        /// Créé la fin de la journée d'une personne dans son planning.
        /// Si la personne est déjà chez-elle, remplit simplement le nombre de timeFrames restant en choisissant la maison comme location.
        /// Si la personne n'est pas encore chez-elle, alors elle est déplacée.
        /// </summary>
        /// <param name="timeFrames">Liste des précédents lieux.</param>
        /// <param name="site">Lieu dans lequel la nuit se situe.</param>
        /// <param name="siteTimeFrames">Durée de la section de la soirée en timeFrames.</param>
        /// <param name="transportSite"></param>
        private void CreateNight(List<TimeFrame> timeFrames, KeyValuePair<Site, SitePersonStatus> site, int siteTimeFrames, KeyValuePair<Site, SitePersonStatus> transportSite)
        {
            if (timeFrames[timeFrames.GetLastIndex()].Activity != site.Key)
            {
                timeFrames.RemoveAt(timeFrames.GetLastIndex());
                timeFrames.Add(new TimeFrame(transportSite.Key, transportSite.Value));
            }
            timeFrames.AddRange(Enumerable.Repeat(new TimeFrame(site.Key, site.Value), siteTimeFrames));
        }
    }
}
