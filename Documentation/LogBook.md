# LogBook
| Auteur | Date de début | Projet  | Description | Communication |
| ------------- |:-------------:| :-----|---|---|
| Joey Martig| 19.04.2021 | Travail de diplome Covid propagation | Applications permettant de voir l'évolution d'un virus dans un environnement peuplé d'individus. | Joey.mrtg@eduge.ch Joey.mrtg@gmail.com |

# 19.04.2021 - 08h05/17h00
- Rendu appréciation du travail de stage.
- Démarrage du travail de diplôme et présentation moodle.
- Documentation
  - Intégration CDC
  - Introduction
  - Résumé
  - Planning prévisionnel
- Comparaison WPF et winforms
  - [Référence 1](https://www.c-sharpcorner.com/article/wpf-vs-winforms/#:~:text=The%20abbreviation%20W.P.F%20simply%20refers,to%20develop%20Windows%20desktop%20applications.)
    - Peu de différence autre que la structure
    - WPF semble plus récent et puissant
  - [Référence 2](https://www.wpf-tutorial.com/about-wpf/wpf-vs-winforms/)
    - Interface GUI plus performante (très intéressant)
    - Plus récent (Plus de librairies mises à jour)
  - [Référence 3](https://www.educba.com/winforms-vs-wpf/)
    - WPF est plus rapide
    - Aussi plus complexe
  - [Référence 4](https://stackoverflow.com/questions/31154338/windows-forms-vs-wpf)
    - Les deux se valent
    - WPF est meilleur pour les UI
    - WPF n'est pas disponible pour linux et mac
    - Le data binding est meilleur en WPF ainsi que le design
  - Conclusion
    - L'apprentissage de la structure WPF va prendre du temps, mais semble en valoir la peine.
    - L'UI sera plus belle et simple à réaliser en WPF
    - Le plus gros avantage semble être BEAUCOUP plus efficace pour l'affichage de l'interface graphique me permettant d'utiliser le GPU à pleine puissance contrairement au winform qui ne l'utilise pas. Sachant que les pc sont équipés de gtx 1060, le changement en WPF semble être le meilleur choix.
    - WPF semble donc être le meilleur choix.
  - Une autre alternative à l'utilisation de l'interface graphique de WPF est Unity qui peut être intégré et communiquer avec le projet. [Intégration d'Unity en WPF](https://stackoverflow.com/questions/44059182/embed-unity3d-app-inside-wpf-application). [Communication](https://www.youtube.com/watch?v=rz6MNZMyza4). [Génération dynamique](https://www.youtube.com/watch?v=8eTWq27h4sY)
  - Unity semble être une bonne idée cependant, dans mon cas, en prenant en compte le nombre d'appelle, utiliser Unity et faire communiquer les deux projets me semblent trop compliqué. Peut-être que rester sur WPF est plus sûr.
- Documentation
  - Analyse interface graphique
    - Comparaison WPF - WinForms - Unity
    - Choix de technologie
- Tentative de communication réussie à l'aide de ce [tutoriel](https://www.youtube.com/watch?v=rz6MNZMyza4).
  - Inutilisable pour ce projet, car la communication est trop restreinte.
  - La communication se fait uniquement avec des string ou des images, mais pas d'objets c#.
- Tentative d'intégration d'Unity dans un projet WPF
  - Réussi
  - Problème de resize
    - Au lancement prend toute la fenêtre
    - Le resize et positionnement fonctionnent
    - À la fermeture du programme, Unity ne s'arrêtait pas et utilisait 15% du processeur à chaque ouverture.

# 20.04.2021 - 08h05/17h00
- Rétrospective 19.04.2021
- Recherche des méthodes dans la dll user32.dll
- Recherche de différents moyens de communication entre WPF et Unity
  - Essai d'envois de données de WPF à Unity avec un pipeline anonyme
  - Essai d'envois de données de WPF à Unity avec un pipeline nommé
    - Réussi entre Unity et WPF, mais inutilisable actuellement
    - Il faut stopper le programme pour lire le message
    - Peut-être un problème lié au fait que les deux programmes essaient de lire au même moment.

- Problèmes d'intelliSense sur Unity
  - la connexion entre le script vs 2019 et Unity n'est pas établie.
  - Mise à jour de vs 2019 et installation des paquets Unity
  - Redémarrage du pc
  - Modifications de paramètre Unity et vs sans succès
  - Recherche de solutions concernant vs 2019 et non 2015
  - Pas suffisant. Ne fonctionne toujours pas.
  - [Le point e. à reglé le problème](https://blog.terresquall.com/2020/11/fixing-visual-studios-intellisense-autocomplete-in-unity/)
- Problème avec Unity qui supprime mes objets à chaque fermeture du projet
  - La scène avait été supprimée et ne s'était pas rajoutée automatiquement
- Communication fonctionnelle, mais le projet Unity freeze après chaque message.
  - Tentative d'utilisation de l'asynchrone pour pallier au problème
    - Ce problème est réglé cependant un problème de décalage fait que WPF parle chinois.
    - ![Communication entre WPF et Unity qui résulte en chinoi](Medias/LogBook/Chinoi.png)
    - Essai de lecture de byte pour obtenir le résultat. sans succès
    - Refactoring du code pour fonctionner en async.
    - Le code est toujours imparfait et décalé
      - "Test communication" => "Test communication"
      - "Test communication" => "est communication"
      - "Test communication" => Signes chinois
  - Communication en string fonctionnelle à 100% en utilisant une méthode async récursive appelée au démarrage du projet Unity.
    - Méthode appelée chaque seconde ayant des résultats très aléatoires.
    - ![Code de base](Medias/LogBook/CodeReadPipeData1.png)
    - Méthode appelée une fois au démarrage puis à chaque fois qu'elle termine l'affichage de données reçues.
    - ![Nouvel exemple de code](Medias/LogBook/CodeReadPipeData2.png)
  - Idée pour l'implémentation en Unity : Envoyer des objets en JSON via le pipeline.
  - Test d'envoi d'un objet de WPF à Unity en JSON
    - ![Objets envoyés](Medias/LogBook/ObjectsSent.png)
    - ![Valeurs reçues](Medias/LogBook/ObjectReveived.png)

# 21.04.2021 - 08h05/17h00
- Documentation
  - Complétion de la comparaison de technologie d'interface graphique des pipelines.
  - Problèmes rencontrés - Pipelines
  - Intégration d'Unity à WPF
- Modification de l'envoi de données pour utiliser un BinaryWriter à la place d'une Stream
  - Augmente la limite de données transmissible d'un Uin16 à int32.
- Modification de la réception des données pour utiliser un BinaryReader à la place d'un stream.
- Test d'intégration + communication réussie
- Début de la création de l'UI
  - Recherche d'un thème pour l'application
    - [Thème prometteur](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)
    - Test du thème
    - Le thème fonctionne et possède énormément de possibilités
  - Test de différents composants du thème
  - Boutons
  - TabControl
    - Changement d'icônes
    - Changements de la couleur
    - Tentative de changer le background pour un background non prédéfini.
    - Intégration d'une autre page xaml depuis le TabControl
  - Création d'une page, pour la simulation, les paramètres graphiques et les paramètres de la simulation
  - Abandon du thème trouvé dû à son manque de responsivité malgré sa grande variété de contenu.

# 22.04.2021 - 08h05/17h00
- UI
  - Ajouts d'icône de boutons [Source](https://materialdesignicons.com/)
  - Création de l'interface principale
    - Menu
    - Sections
    - Responsivité
  - Utilisation des columns et rows
  - Impossible de changer la couleur de la barre de titre du projet
    - Désaffichage de la barre
    - Création de ma propre barre
    - Différence entre les deux versions
      - ![Barre de titre Windows](Medias/LogBook/BeforeTitleBar.png)
      - ![Barre de titre personnalisée](Medias/LogBook/AfterTitleBar.png)
  - Modification des méthodes de la barre de titre pour être utilisable sur toutes les pages
  - Modification de la structure des boutons pour intégrer directement l'image dans leur balise et non appeler une image. La version précédente empêchait plusieurs boutons d'avoir la même image. Résultat en l'affichage de premier bouton sans image.
  - Création de différentes pages de contenus
  - Navigation entre les pages
  - Modification de l'affichage d'un slider
    - Difficulté à trouver les bons paramètres
    - Binding de la couleur à la valeur du slider
    - Création d'un template pour le slider en récupérant la version de base de  [WPF](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/controls/slider-styles-and-templates?view=netframeworkdesktop-4.8)
    - Suppression de la track-bar
    - Modification du RepeatButton
      - Création de deux versions se situant à gauche et droit du thumb et ayant des couleurs différentes
    - Comparaison du slider de Windows et du slider personnalisé à l'aide d'un template
      - ![slider comparaison](Medias/Logbook/Slider.png)
  - Modification de l'affichage des radioBoutons en récupérant la version de base de [WPF](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/controls/radiobutton-styles-and-templates?redirectedfrom=MSDN&view=netframeworkdesktop-4.8)
    - Nouveau design 
    - ![slider comparaison](Medias/Logbook/RadioButton.png)
  - Modification des couleurs pour qu'elles soient contenues dans des balisent facilement modifiable.
  - Modification de l'affichage des checkbox en récupérant la version de base de [WPF](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/controls/checkbox-styles-and-templates?view=netframeworkdesktop-4.8)
    - Nouveau design 
    - ![slider comparaison](Medias/Logbook/CheckBox.png)
- Modification de la fermeture des fenêtres pour cacher les fenêtres secondaires et fermer le programme en cas de fermeture de la fenêtre principale.

# 23.04.2021 - 08h05/16h10
- Documentation
  - Organisation
  - Maquettes
  - Introduction
    - Technologies utilisées
  - Schéma de fonctionnement
  - Description des technologies utilisées
- Réflexion sur le fonctionnement des threads pour l'application.
  - Parallel
- Installation LiveCharts
  - Installation packet LiveCharts
  - Installation packet LiveCharts.Wpf
  - Installation packet LiveCharts.Geared
- Graphiques
  - Bug à chaque changement de taille, régénérer la solution est nécessaire.
    - Uniquement dans le concepteur et non au lancement du programme
    - Nettoyer la solution permet de tout de même voir l'affichage de la fenêtre
  - Intégration de graphiques
    - Courbe
    - HeatMap
    - Colonne
    - Ligne
    - Bulle
    - Circulaire (Camembert)
    - Area Stack
    - ![Graphiques intégrés à l'application WPF](Medias/LogBook/Graphiques.png)

# 26.04.2021 08h05 / 16h10
- Début de la création de la simulation
- Récupération du code du stage et suppression des éléments graphiques
- Commentaires
- Création d'une classe "Site" regroupant tous les lieux et véhicules
- Relecture du cahier des charges
- Relecture du fichier Excel contenant la transmission par aérosol [2021_COVID-19_Aerosol_Transmissionn_Estimator](https://docs.google.com/spreadsheets/d/1x_QFiFPbqLtZTjuoVoCyPQdu7onu6c370NNlPZ3TfTk/edit#gid=1679887415)
- Création du code des lieux en fonction du fichier Excel
  - Taille du bâtiment, longueur - largeur - hauteur - air - volume
  - Paramètres de l'air et de déposition
  - Utilisation du type double pour le calcul du taux d'infection
  - Suppression de certains paramètres non utilisés
- Réflexion sur la structure du code et l'interaction entre les individus et les lieux
  - Résistance au virus
    - Maladies
    - Âge
    - ![Évolution individus et accessibilité des données](Medias/LogBook/MaladiesEtAge.png)
  - Création des plannings
    - Pattern
    - (Gérer les cercles sociaux)
    - Création en fonction de l'âge
    - ![Création d'un planning](Medias/Logbook/CreationPlanning.png)
  - Mesures & Symptômes
    - resistance
    - comportement
    - Augmentent certains paramètres
    - ![Évolution individus et accessibilité des données](Medias/LogBook/EvolutionIndividu.png)
  - Les activités ne contiennent que des types d'objets permettant de définir ce que l'individu fait. Le planning contient les lieux et les indique à l'individu.
- Création des plannings en fonction de la figure concernant leur création.
- Nombre d'école et autres bâtiments en fonction du nombre de personnes. Genève servant d'exemple.
  - Formules  
    - 100 + Abs((y2 - y1) / y1)*100 = %
    - (% - 100)/100 * y1 + y1 = y2
  - ~0.033%  --> 165 écoles pour 500'000 personnes 100 - ((165 - 500'000) / 500'000)*100
  - ~6,74%  --> 33'700 entreprises pour 231'000 emplois
  - ~0.0016%  --> 8 hôpitaux, 2 cliniques, 30 lieux de soins
  - ~0,0272%  --> ~136 supermarchés
  - ~50% - 100%    --> ~250'000 appartements
- pareil, mais pour l'âge de la population
  - ~22% 0-19  ans
  - ~63% 20-64 ans
  - ~10% 65-79 ans
  - ~5%  80+   ans
  - [Source](https://www.ge.ch/statistique/graphiques/affichage.asp?filtreGraph=11_02&dom=1)
- Pareil pour les transports
  - ~36%  --> 218'000 voitures (Chauffeur ou passagers)
  - ~15%  --> 452 + 117 tpg
  - ~10%  --> ~ 200'000 vélos
  - [Source](https://www.letemps.ch/suisse/nombre-voitures-menage-diminue-geneve#:~:text=Au%20d%C3%A9but%20de%20l'ann%C3%A9e,augmentation%20de%206%2C5%25)
- Réflexion sur la création des transports et bâtiments et sur la source de certaines données.
  - On a une population de 100 personnes
    - 22% 0-19  ans
    - 63% 20-64 ans
    - 15% 65+ ans
  - Il faut donc au minimum 
    - Une école
    - Un supermarché
    - Une maison par personne (pour le moment les < 19 ans vivent seuls)
  - On crée les bâtiments en fonction des besoins cités
  - Pour les véhicules
    - On crée 36 voitures
    - et des bus (à voir plus tard pour la quantité)
  - Ensuite, on crée les 100 individus
    - En fonction du pourcentage d'âge
    - On leur attribue 
      - Une maison vide
      - Une école
      - Un moyen de transport (vide si voiture)
    - On créé leurs résistances

# 27.04.2021 08h05 / 16h30
- Les véhicules dans la simulation agissent exactement comme les bâtiments.
  - Pas de déplacements
  - La voiture étant la seule d'"unique" à une personne.
    - La différence réside dans l'assignation et non dans le fonctionnement.
- Suppression du parent "Vehicle" et "Building"
- Création d'un objet parent "Site" représentant tous les lieux, dont les véhicules et l'extérieur.
- Création de variables constantes globales pour la création des sites
- Réflexion sur le fonctionnement des individus
  - Marche à suivre.
  - Utiliser du async pour récupérer le taux de contamination ? (Attendre que tout le monde soit bien entré)
- Création des individus
  - Maladies
  - Résistance au virus
  - Durée de l'immunité
  - Durée de l'infection par le virus
- Création des maladies
  - Puissance
    - En fonction de l'âge
  - Durée
- Recherche de source pour gérer mathématiquement les probabilités qu'une personne attrape des maladies en fonction de son âge.
  - Meilleure compréhension du sujet, mais rien de concluant.
- Modification des namespace qui avaient automatiquement créé sous namespace en créant des dossiers dans le programme WPF
- Modification du site pour prendre en compte les personnes qui se trouvent dedans.
  - Cette modification impacte aussi à quel moment le calcul des probabilités est effectué.
  - Si une personne entre ou sort. Les probabilités sont recalculées.
  - Sinon on récupère la dernière valeur sans recalculer
  - Dois recalculer si une personne a été infectée. (en prenant en compte le temps d'incubation du virus, ce n'est pas probablement pas nécessaire.)
- Données récupérées concernant le temps d'incubation et d'infection du covid
  - [Durée du covid](https://www.cdc.gov/coronavirus/2019-ncov/if-you-are-sick/end-home-isolation.html#:~:text=You%20can%20be%20around%20others%20after%3A,of%20fever%2Dreducing%20medications%20and)
  - [Incubation et durée](https://patient.info/news-and-features/coronavirus-how-quickly-do-covid-19-symptoms-develop-and-how-long-do-they-last)
- Suppression des cercles familiaux et d'amis du planning
  - Raison :
    - Pas par manque de temps, mais par logique due au fonctionnement de la simulation
    - Les plannings gèrent entièrement la simulation, les lieux., les horaires, les contacts.
    - Au final, la famille sont les personnes habitant au même endroit. Les amis, les personnes qui vont aux mêmes bars.
    - La création du planning s'occupera donc de gérer les contacts proches
  - Cette modification pourrait être revue plus tard en fonction de l'évolution du fonctionnement de la simulation et des interactions entre les objets.
- Commentaires et création d'en-têtes dans le code
- Recherche de conversion de probabilité d'années en mois
  - Rien trouvé
  - Uniquement dans l'autre sens
- Est-il justifié d'avoir des symptômes de virus ?
  - La résistance au virus étant simplifié et l'accent étant majoritairement mis sur la propagation, elle-il nécessaire d'intégré des symptômes qui dans 90% des cas ne changent en rien le taux de contamination (excepté la toux donc).
  - Simplifier en utilisant simplement le taux de transmission ?
    - Pas viable, car asymptomatiques
  - Les symptômes pourraient être utilisés pour détecter le virus dans la simulation pour les quarantaines et autres.
  - Garder les symptômes, mais avec pour la majorité, uniquement un indice de détection
- Création de la classe Virus, Transmission et Symptom
- Création du fichier XML comportant les données du Covid
- Lecture du fichier depuis le code et récupération des données.
- Fonctionnement imaginé du virus et de ses composants
  - ![Virus transmissions and symptoms](Medias/LogBook/VirusTransmissionsAndSymptoms.png)
  - Informations importante concernant la respiration et affectant donc la toux [Source](https://docs.google.com/spreadsheets/d/1x_QFiFPbqLtZTjuoVoCyPQdu7onu6c370NNlPZ3TfTk/edit#gid=519189277)
    - Resting – Oral breathing = 2.0 quanta/h
    - Resting – Speaking = 9.4 quanta/h
    - Resting – Loudly speaking = 60.5 quanta/h
    - Standing – Oral breathing = 2.3 quanta/h
    - Standing – Speaking = 11.4 quanta/h
    - Standing – Loudly speaking = 65.1 quanta/h
    - Light exercise – Oral breathing = 5.6 quanta/h
    - Light exercise – Speaking = 26.3 quanta/h
    - Light exercise – Loudly speaking = 170 quanta/h
    - Heavy exercise – Oral breathing = 13.5 quanta/h
    - Heavy exercise – Speaking = 63.1 quanta/h
    - Heavy exercise – Loudly speaking = 408 quanta/h
- Modification de la transmission par aérosol sur les sites pour prendre en compte les paramètres individuels de chaque individu présent sur les lieux.

# 28.04.2021 07h50 / 17h00
- Refactoring
  - Certain point logiques à revoir
    - Le lieu calculait le risque de transmission aérosol alors que c'est au type de transmission du virus de le faire. Ou un mix des deux.
- Penser à modifier les calculs récupérés sur le fichier Excel pour être plus en accord avec le fonctionnement de la simulation.
- Revoir le pattern visistor pour éventuellement l'implémenter pour le calcul de risque de la transmission
- Transmission effectuée
  - Revoir la structure du code pour certaines améliorations nécessaires
- Ajout de données du covid dans le fichier Excel
- Refactorisation de la lecture du fichier XML
  - Échec
  - La structure de code essayée ne correspond pas aux besoins de la simulation
    - Tentative de conversion d'une boucle itérant dans le XML à un système de query. Cependant, les query ne permettent pas de récupérer des valeurs précisent en groupes et de les lire individuellement.
- Penser à remplacer le XML par du JSON.
  - Permettant de générer le virus de façon dynamique en fonction du JSON.
- Création du diagramme de classe
- Discussion avec M. Mathieu sur les threads de l'application
  - Création de 2 thread, un gérant la simulation, l'autre la GUI.
  - À la place d'un timer, vérifier le temps entre deux "tick" et attendre si le traitement est trop rapide.
  - ![Résultat rencontre](Medias/LogBook/AppFonctionnement.jpg)
- Création du "timer" dans la simulation
- lecture du code TrainSimulatorApp
  - Fonctionnement des Threads
  - Boucle
- Intégration de la simulation au programme WPF
  - Start
  - Stop
  - Changement de l'interval en fonction du slider
- Recherche sur le binding de données en WPF
  - Trop complexe pour le moment. À éventuellement ajouter plus tard.
- Création des bâtiments
  - Reprise de réel
- Perte du câble du disque dur externe --> pas de sauvegarde dessus
- Données concernant le nombre d'étudiants à Genève pour calculer le ratio école / élèves.
  - Total = 103'008 étudiants en 2020
  - [Sources](https://drive.google.com/drive/folders/16TU3wTAwMIrx5XxhLVHWT5VI7Rbx-zqy?usp=sharing)
- Rectification du compteur de bâtiment nécessaire à la population
  - Formules
    - (% - 100) / 100 * y1 + y1 = y2
    - 100 - ((y2 - y1) / y1) * 100 = %

# 29.04.2021 08h05 / 17h00
- Début de la création des plannings
  - Modification de la création des bâtiments
  - Ajouts des restaurants
  - Ajouts de l'extérieur
- Quantité de personne retraitée à Genève = ~14%
  - [Source](https://www.ge.ch/statistique/tel/publications/2016/donnees_generales/annuaire/dg_as_2016.pdf)
- Création de la structure du planning d'un individu qui travaille
  - Jour de travail :
```
  Home 5h-8h
  trajet (5h30 - 8h30)
  company (Arrivé 6h - 9h  Départ (11h-12h))
  trajet | company (11h30-12h30)
  Resto  | Company (Arrivé 12h - 13h  Départ (12h30-13h30))
  trajet | company (13h-14h)
  company (Arrivé 13h30-14h30  Départ (16h-19h))
  trajet (16h30 - 19h30)
  Supermarché 30m-1h30 | maison 30min-Fin | restaurant 1h-3h | Bar (17h-19h)
                      \ /                \ /                 | Bar
  Supermarché          | maison           | restaurant       | Bar (17h30-19h)
                      \                  \ /                 | Bar
                       | maison           | restaurant       | Bar (19h - 20h)
   Outisde             | maison                              | Bar (20h - minuit)
   maison
   ```
- Ajouts de méthode d'extensions pour le calcul de random booléen
  - Méthode choisissant un choix aléatoire entre plusieurs choix à disposition selon un poids
- ~~Création d'une méthode créant la soirée d'une personne de manière dynamique~~
  - ~~Manque de connaissance pour la réaliser de manière propre et fonctionnel~~
- Création de différents patterns préfaits choisis de manière aléatoire.
  - Simplifie la version précédente
  - Suffisamment aléatoire
- Utiliser une string en temps que seed pour générer des plannings ?
  -  +Meilleur contrôle sur les plannings
  -  +Simple
  -  +Possibilité d'ajouter une génération aléatoire plus tard
  -  -Prégénéré
  -  -durée des périodes (ne devraient pas changer)
- Création de la string
- Modification de l'emplacement des méthodes de création de planning.
  - CreateDays simulation --> Day
  - CreatePlanning simulation --> Planning
- Génération du planning en fonction de la seed
- Création de seeds pour étudiants, adultes et retraités
- Première compilation de la simulation (Alpha) réussie après quelques bugs mineurs
- Suppression des paramètres "Pression", "Temperature" et "Co2" dans les lieux, car pas encore utilisé par [](https://docs.google.com/spreadsheets/d/1x_QFiFPbqLtZTjuoVoCyPQdu7onu6c370NNlPZ3TfTk/edit#gid=519189277).

# 30.04.2021 08h05 / 17h00
- Documentation
  - ~~Revoir la structure pour fonctionner avec le système de sprint.~~
  - Réduction de la quantité de code affiché dans la documentation
  - Modification de l'agencement
  - Correction de l'orthographe
- Discussion avec M. Mathieu
  - Contenu du rapport
    - Pas de code
    - Pseudo-codes OK
    - Utiliser un "ID" sur le pseudo-code qui se retrouve dans les commentaires du code correspondant. Permets de retrouver rapidement.
  - Contenu du LogBook
  - Rendu Sprint 1
    - Uniquement le rapport
- Modification du diagramme de classe
- Avancement de l'interface graphique simulation
  - Ajout d'une barre de scroll
  - Ajouts d'éléments à l'emplacement des futurs éléments graphiques
  - Création d'une grille par le code
  - Création d'une méthode d'extension permettant de récupérer le dernier index d'une liste.
  - Ajouter / retirer colonnes et lignes responsives

# 03.05.2021 08h05 / 17h00
- Répartition des Site dans les plannings
  - Réflexion sur l'attribution des sites pour la famille et amis.
    - Attribution au préalable ?
    - ~~Attribution à chaque itération ?~~
    - ~~Mixe ?~~
  - ![Différence création avec/sans cercle sociaux](Medias/Logbook/CercleSociaux.png)
  - Création de différents présets
  - [Source des données de répartition des ménages](https://www.ge.ch/statistique/tel/publications/2014/analyses/communications/an-cs-2014-48.pdf)
    - Graphique page 4
    - Graphique page 7
      - Original :
        - ![Proprotions ménages original 2011](Medias/LogBook/MenageProportions.png)
      - Version simplifiée utilisée dans la simulation :
        - Suppression des "autres types de ménages" et "sans indications"
        - Séparation en 3 sections 
          - Jeunes
          - Adultes
          - Retraités
        - Moyenne de chaque type de ménages par section d'âge
        - ![Proprotions ménages modifiée 2011](Medias/LogBook/MenageProportions2.png)
        - Cette version simplifiée permet de grandement améliorer le code sans dénaturer les données
- Modification de la variable d'extensions NextProbability() pour prendre en compte des strings
  - Vérification des résultats
    - Attendus:
    ```
    OnePerson          28
    CoupleWithChild    21
    CoupleWithoutChild 40
    OneParentWithChild 11
    ```
    - Résultats pour 100 essais :
    ```
    OnePerson          27.85
    CoupleWithChild    20.57
    CoupleWithoutChild 40.52
    OneParentWithChild 11.05
    ```
- L'attribution des lieux ne se fait pas dans le planning mais lors de la première itération ou lors de la création de la personne(dictionnaire --> si existe pas entre, sinon utiliser existant.)
- Chaque personne reçoit un lieu de chaque soit random (Company, restaurant, bar, etc.) soit prédéfini à la création (maison, véhicule, etc.)
- Insertion d'éléments dans la grille de l'UI de la simulation.
  - Recherche d'une case vide
  - Ajout d'un élèment dans la case
  - Suppression de l'élèment d'une case
  - Déplacement d'un élèment
  - Bug une case se bloque.
    - à la création, la mauvaise case se bloque (inversion x et y)
    - Lors de la suppression la mauvaise case se libérait (inversion x et y)
  - Changement de taille + / -
    - Ne supporte par encore le déplacement
- Documentation
  - Difficultés rencontrées
    - Planning

# 04.05.2021 08h05 / 16h10
- Interface utilisateur
  - Support du déplacement des objets de tailles supérieur à 1
  - Modification de la taille en x et y (+ et -)
  - Création d'une méthode permettant de vérifier si des cellules sont utilisées dans un certain périmètre
    - pareil mais pour changer l'état de ces cellules
  - Problème, pour le moment, un élément prend en compte sa propre position et se bloque lors de déplacements.
    - Reglé en libérant l'espace utilisé lors du MouseDown event. L'espace est réassigné si le bouton n'est pas déplacé.
- Discussion avec M. Mathieu
  - Propagation (Rapport)
    - Qui 
    - quoi 
    - pourquoi 
    - expliquer comment 
    - justifier utilisation 
  - Éviter graphiques 3d
    - Moins claires
    - Perspective trompeuse
  - Faire attention à l'utilisation des graphiques
    - Toujours partir de 0
    - Si pourcentages alors de 0 à 100
    - Sinon de 0 à ~valeur max
- Ajout de boutons dans l'UI de chaque élément
  - Création d'une grille dans les cellules remplies.
    - Grille contenant les boutons permettants la modification / suppression de la cellule
  - Modification du code pour fonctionner avec le fait qu'il y a plusieurs grilles
  - Ajout d'un bouton permettant de modifier les données du graphique
    - Bouton ouvrant une nouvelle fenêtre avec les paramètre du graphique
  - Boutons créés avec différents style
    - Bug après quelque secondes d'exécutions : ''{DependencyProperty.UnsetValue}' n'est pas une valeur valide pour la propriété 'Background'.'
    - L'utilisation du style créé pour ces boutons à plusieurs reprise créé cette erreur.
    - Erreur réglée, il s'agissait d'un code couleur mal inscrit dans App.xaml.  
      - ```HTML
        <Setter Property="Background" Value="{StaticResource 9b9b9b}"/>  
        ↓
        <Setter Property="Background" Value="{StaticResource normalGray}"/> 
        ```
- Ajout d'une limite de taille des éléments en fonction de la taille de la grille
- ⚠ Penser à changer les noms de variables qui contiennent "période" ⚠
- Structure du planning
  - Assignation des lieux --> Sélection d'un planning compatible
  - Création d'une méthode créant des horaires aléatoires suivant des paramètres donné (Durée matin, durée midi, durée après-midi, etc.)

# 05.05.2021 08h05 / 17h00
- Ajouts d'icônes pour les boutons des éléments graphiques
- Création des planning
  - Transformation des horaires générés en seed
  - Modification des sites pour comprendre un type (Hobby, maison, autres)
  - Utilisation des types dans l'assignations des sites dans le planning
- Modification fonctionnement planning
  - La simulation créé la seed
  - Le planing traduit la seed
  - L'individus sait ou aller en fonction du type d'endroit où il doit aller qui lui est fournit par le planning
  - ~~Utiliser les valeurs de l'enum ou du type de site dans la seed ?~~
    - ~~+Création simplifée~~
    - ~~+Moins de problèmes au démarrage~~
    - ~~-Plus de calculs à chaque itération~~
    - ~~-Moins de contrôle~~
- Ajout des moyens de transport lorsque la personne change de lieux dans la création des plannings
- Suppression de la seed pour directement créer les périodes
- 1ère version du planning fonctionnelle (Jours de congé) ! (Usine à gaz à refactoriser)
- Modification de toutes les variables classes contenant "period" en "TimeFrame"
- Jours de travail du planning fonctionnel. Je doit modifier l'assignation des lieux pour avoir un meilleur contrôle et définir ou la personne va et pourquoi.
  - À établir dans les activités
- ~~Modifier SiteType ou~~ ajouter un nouveau paramètre qui permet de définir pourquoi la personne est là.
  - Différencie le client d'une personne qui travail

# 06.05.2021 08h15 / 17h00
- Ajout de la raison d'un individu d'aller dans un lieu dans la création du planning
  - utilisation d'un mixe de SitePersonStatus et SiteType pour déterminer les lieux où va la personne. 
    ```
    public enum SiteType
    {
        Home,
        Hobby,
        WorkPlace,
        Store,
        Transport,
        Hospital,
        Eat
    }
    ```
    ```
    public enum SitePersonStatus
    {
        Client,
        Worker,
        Other
    }
    ```
- Bug la voiture est utilisée 4 fois d'affilée sans aller nul part
  - ~~Rectification, la voiture faisant partie des hobby, c'est le cas ici et donc pas un bug.~~
  - Apparaît tout de même beaucoup trop souvent pour ne pas modifier l'alogrithm.
  - 
- [Informations concernant les repas à midi dans les entreprises](https://newsroom.heart.org/news/survey-more-than-half-of-u-s-employees-who-typically-eat-lunch-during-work-hours-struggle-to-make-it-healthy)
- Mise à jour de la création de jours libre en prenant en compte les modifications dans la génération de jour de travail
  - Il n'est pas nécessaire d'utiliser SitePersonStatus dans le choix de lieu car il est toujours client contrairement à un jour de la semaine.
- Déplacement du code de la classe Simulation à Day
- Suppression du code de génération des seeds
- Source de données permettant l'approximation des personnes travaillant le samedi et le dimanche [Source](https://www.regionsjob.com/actualites/horaires-organisation-temps-travail-horaires-atypiques.html#:~:text=Selon%20une%20%C3%A9tude%20de%20l,pour%20se%20terminer%20%C3%A0%2018h%20!)
  - Création des plannings des individus en comprenant que la majorité travail en semaine mais que certains travaillent aussi le week-end
- Attributions de lieux aléatoires créé au préalable.
  - Choix du véhicule en fonction de probabilités
  - Bus pas encore implémenté mais ajoutés
  - Ajouts de la classe vélos
- Ajout de commentaire dans les méthodes d'extension et modifications de certaines
  - Modification de Nextprobability() qui à la place d'avoir plusieurs versions acceptant différent types de paramètres, accepte uniquement le type object.
- Ajout de commentaire dans la classe Day
- Modification de SiteType pour inclure "WorkPlace"
- Modification des valeurs en utilisant la source précédement récupérée [Source](https://www.letemps.ch/suisse/nombre-voitures-menage-diminue-geneve#:~:text=De%20mani%C3%A8re%20g%C3%A9n%C3%A9rale,mod%C3%A9r%C3%A9e%20(%2B%201%20point).)
- Ajout de la fonctionnalité "Copy link to text" dans google chrome pour simplifier l'utilisation de sources.
- Modification de certaines sources pour surligner directement le paragraphe utilisé.
- Création d'un évènement permettant de récupérer les données à chaque itération de la simulation automatiquement.
- Lancement de la simulation 1ère version.
  - Oubli d'ajout des infectés
  - La moyenne des quantas commence à 0 et augmente peut à petit jusqu'à s'arrêter à ~5.6 qui est la valeur par défaut.
    - Les individus n'ignisialisaient pas leur "quantaExhalationRate" au démarrage mais uniquement lorsqu'ils changaient de lieux.
- Création d'un event load dans la page simulation qui permet de récupérer 
- Ajout d'infecté dès le départ
  - Ils désparaissent à la première itération
  - Les durée récupérée dans le fichier XML étaient set à 0
  - Ils ne sont pas pris en compte dans les calcules.
    - Ils l'étaient mais comme leur état etait celui d'infecté qui signifie que le virus ne s'est pas encore dévelopé.
    - Changement de l'état "infected" à "infectious" pour aller plus rapidement vers la phase de contagion.

# 07.06.2021 07h45 / 17h00
- Modification du code de propagation qui a des problèmes de format de données. certains doubles retournant NaN du à des dividions par 0.
  - Le nombre de reproduction du virus était mélangé aux probabilités d'être infecté créant un valeur non-utilisable.
  - Le calculs nombre de personnes susceptibles d'attraper le virus ne calcul par correctement et donne des résultats négatifs.
  - ~~Différence de résultat en calculant les mêmes valeurs dans la calculatrice/code et le fichier excel~~
    - Différence de format % et probabilités
  - Problème de résultats dans les probabilités d'être infectés.
    - le problème réside dans la quantité de quantas qui n'est pas correcte.
    - Nécessité d'ajouter de nombreux calculs concernant :
      - Les émissions de Co2
      - moyenne de concentration de quanta
      - Les quantas inhalé par personnes
    - émissions de Co2 fonctionnelles
    - Le calcul de quanta moyen n'est pas bon
      - La durée était calculée en minute alors qu'elle doit être calculée en heure (0 -> 0.5 -> 1) ~~(1 -> 1h30 -> 2h00)~~
      - Le calcul fonctionne en modifiant le volume du lieux et la somme des mesures pour qu'ils soient identiquent au excel.
    - En utilisant le même BreathingRate du fichier excel, le résultat de quantaInhaledPerPerson est identique.
    - Modification de calculs déjà existant pour utiliser les nouvelles valeurs calculées en temps réel.
    - Probabilité d'infection fonctionnelle.
- Besoin de refactoriser le code des calculs
  - Actuellement illisible
- Refactorisation du code dans la classe Site
- Discussion avec M. Mathieu sur l'état actuel du projet
  - Pas besoin de trop complexifier les planning en faisant par exemple déposer la fille par le père en voiture
    - Gain de temps et complexité des plannings.
- ~~La méthode NextDouble ne permet pas de générer des randoms avec une différence de taille suffisante pour permettre à des individus d'être infectés~~
- Test sur 2 semaines dans la simulation
  - 100 personnes et 9 infectés au départ
  - 3 on été contaminé.
- Modification du code pour empêcher le résultat NaN avec la concentration de quanta.
- Le volume de certains lieux est de 0 empêchant le calcul de concentration de quanta dans un lieu.
  - Il s'agissait d'un ancien code qui créait les lieux avec le nombre maximum de personnes sur le lieu comme premier paramètre (Qui est maintenant un paramètre de taille)
- Test Sur ~2 semaines
  - 1000 personnes et 100 infectés au départs
  - 650 personnes infectées maximum
  - une moyenne de 500 cas. 
- Testé avec 100'000 personnes et le résultat est fonctionnel mais extrêmement lent.
  - 95% d'infectés maximum
  - Moyenne de 80%