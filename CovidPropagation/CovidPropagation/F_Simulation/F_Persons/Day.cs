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

namespace CovidPropagation
{
    /// <summary>
    /// Jour composé de plusieurs périodes.
    /// </summary>
    public class Day
    {
        private TimeFrame[] _timeFrames;
        public TimeFrame[] TimeFrames { get => _timeFrames; }

        public Day(List<KeyValuePair<Site, SitePersonStatus>> personSites, bool isWorkDay)
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
        /// Créé un jour de travail pour une personne avec les lieux qui lui sont fournies.
        /// </summary>
        /// <param name="personSites">Lieux avec lequel le planning sera créé</param>
        /// <returns></returns>
        private TimeFrame[] CreateWorkDay(List<KeyValuePair<Site, SitePersonStatus>> personSites)
        {
            int totalTimeFrame = GlobalVariables.NUMBER_OF_TIMEFRAME;
            Random rdm = GlobalVariables.rdm;
            List<TimeFrame> timeFrames;

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
            KeyValuePair<Site, SitePersonStatus> hometSite = personSites.Where(h => h.Key.Type.Contains(SiteType.Home)).OrderBy(x => rdm.Next()).First();
            KeyValuePair<Site, SitePersonStatus> workSite = personSites.Where(w => w.Value == SitePersonStatus.Worker).OrderBy(x => rdm.Next()).First();

            KeyValuePair<Site, SitePersonStatus> noonSite;
            // Manger sur le lieu de travail ou ailleurs
            if (rdm.NextBoolean())
                noonSite = personSites.Where(e => e.Key.Type.Contains(SiteType.Eat) && e.Value == SitePersonStatus.Client).OrderBy(x => rdm.Next()).First();
            else
                noonSite = workSite;

            KeyValuePair<Site, SitePersonStatus> afterNoonFreeTimeSite = personSites.Where(
                    f => f.Value == SitePersonStatus.Client ||
                    f.Value == SitePersonStatus.Other &&
                    !f.Key.Type.Contains(SiteType.Transport)
                ).OrderBy(x => rdm.Next()).First();

            KeyValuePair<Site, SitePersonStatus> eveningActivitySite = personSites.Where(
                    a => a.Value == SitePersonStatus.Client ||
                    a.Value == SitePersonStatus.Other &&
                    a.Key.Type.Contains(SiteType.Eat)
                ).OrderBy(x => rdm.Next()).First();

            KeyValuePair<Site, SitePersonStatus> transportSite = personSites.Where(t => t.Key.Type.Contains(SiteType.Transport)).OrderBy(x => rdm.Next()).First();

            timeFrames = new List<TimeFrame>();
            // Changer les siteType pour définir le type
            CreateMorning(timeFrames, hometSite, morningTimeFrame, workSite, morningWorkTimeFrame, transportSite);
            CreateNoon(timeFrames, noonSite, noonTimeFrame, transportSite);
            CreateAfterNoon(timeFrames, workSite, afterNoonWorkTimeFrame, afterNoonFreeTimeSite, afterNoonFreeTimeTimeFrame, transportSite);
            CreateEvening(timeFrames, hometSite, eveningTimeFrame, eveningActivitySite, eveningActivityTimeFrame, transportSite);
            CreateNight(timeFrames, hometSite, nightTimeFrame, transportSite);

            return timeFrames.ToArray();
        }

        /// <summary>
        /// Créé un jour de congé pour une personne avec les lieux qui lui sont attribués
        /// </summary>
        /// <param name="personSites">Lieux avec lequel le planning sera créé</param>
        /// <returns></returns>
        private TimeFrame[] CreateFreeDay(List<KeyValuePair<Site, SitePersonStatus>> personSites)
        {
            int totalTimeFrame = GlobalVariables.NUMBER_OF_TIMEFRAME;
            Random rdm = GlobalVariables.rdm;
            List<TimeFrame> timeFrames;

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

            KeyValuePair<Site, SitePersonStatus> homeSite = personSites.Where(h => h.Key.Type.Contains(SiteType.Home)).OrderBy(x => rdm.Next()).First();
            KeyValuePair<Site, SitePersonStatus> morningActivitySite = personSites.Where(h => h.Key.Type.Contains(SiteType.Hobby)).OrderBy(x => rdm.Next()).First();
            KeyValuePair<Site, SitePersonStatus> noonSite = personSites.Where(h => h.Key.Type.Contains(SiteType.Eat)).OrderBy(x => rdm.Next()).First();
            KeyValuePair<Site, SitePersonStatus> afterNoonActivitySite = personSites.Where(h => h.Key.Type.Contains(SiteType.Hobby)).OrderBy(x => rdm.Next()).First();
            KeyValuePair<Site, SitePersonStatus> eveningActivitySite = personSites.Where(h => h.Key.Type.Contains(SiteType.Eat)).OrderBy(x => rdm.Next()).First();
            KeyValuePair<Site, SitePersonStatus> transportSite = personSites.Where(h => h.Key.Type.Contains(SiteType.Transport)).OrderBy(x => rdm.Next()).First();

            timeFrames = new List<TimeFrame>();
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
