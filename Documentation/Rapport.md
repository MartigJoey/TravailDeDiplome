# 1. `Table des matières`
- [1. `Table des matières`](#1-table-des-matières)
- [2. `Résumé`](#2-résumé)
- [3. `Abstract`](#3-abstract)
- [4. `Introduction`](#4-introduction)
  - [4.1. `WPF`](#41-wpf)
    - [4.1.1. `Simulation`](#411-simulation)
    - [4.1.2. `UI`](#412-ui)
  - [4.2. `XML`](#42-xml)
  - [4.3. `Graphiques`](#43-graphiques)
  - [4.4. `Unity`](#44-unity)
- [5. `Schéma de fonctionnement`](#5-schéma-de-fonctionnement)
- [6. `Maquettes`](#6-maquettes)
  - [6.1. `UI`](#61-ui)
    - [6.1.1. `Page Simulation`](#611-page-simulation)
    - [6.1.2. `Page Paramètres graphiques`](#612-page-paramètres-graphiques)
    - [6.1.3. `Page Paramètres`](#613-page-paramètres)
    - [6.1.4. `Page Informations`](#614-page-informations)
  - [6.2. `Interface Graphique`](#62-interface-graphique)
- [7. `Organisation`](#7-organisation)
  - [7.1. `Planification`](#71-planification)
  - [7.2. `Tâches`](#72-tâches)
  - [7.3. `Versionning - Backup`](#73-versionning---backup)
- [8. `Technologies utilisées`](#8-technologies-utilisées)
  - [8.1. `C#`](#81-c)
  - [8.2. `Microsoft Visual studio`](#82-microsoft-visual-studio)
  - [8.3. `WPF`](#83-wpf)
  - [8.4. `Unity`](#84-unity)
  - [8.5. `XML`](#85-xml)
  - [8.6. `LiveCharts`](#86-livecharts)
  - [8.7. `JSON`](#87-json)
- [9. `Cahier des charges`](#9-cahier-des-charges)
  - [9.1. `Titre`](#91-titre)
  - [9.2. `Fonctionnalités`](#92-fonctionnalités)
  - [9.3. `Matériel et logiciels`](#93-matériel-et-logiciels)
  - [9.4. `Prérequis`](#94-prérequis)
  - [9.5. `Descriptif complet du projet`](#95-descriptif-complet-du-projet)
    - [9.5.1. `Méthodologie`](#951-méthodologie)
    - [9.5.2. `Description de l’application`](#952-description-de-lapplication)
      - [9.5.2.1. `Graphique`](#9521-graphique)
      - [9.5.2.2. `Interface graphique`](#9522-interface-graphique)
      - [9.5.2.3. `propagation`](#9523-propagation)
      - [9.5.2.4. `Population`](#9524-population)
        - [9.5.2.4.1. `Temporalité`](#95241-temporalité)
        - [9.5.2.4.2. `Individus`](#95242-individus)
        - [9.5.2.4.3. `Hopitaux / écoles / entreprise`](#95243-hopitaux--écoles--entreprise)
  - [9.6. `Protocols de tests`](#96-protocols-de-tests)
  - [9.7. `Persona`](#97-persona)
    - [9.7.1. `Utilisateur expérimenté`](#971-utilisateur-expérimenté)
    - [9.7.2. `Utilisateur inexpérimenté`](#972-utilisateur-inexpérimenté)
  - [9.8. `User stories`](#98-user-stories)
    - [9.8.1. `Ashley`](#981-ashley)
    - [9.8.2. `Kanan`](#982-kanan)
  - [9.9. `Diagramme d'activité`](#99-diagramme-dactivité)
  - [9.10. `Planning`](#910-planning)
  - [9.11. `Diagramme de classe initial`](#911-diagramme-de-classe-initial)
  - [9.12. `Interactions`](#912-interactions)
    - [9.12.1. `Menu principal`](#9121-menu-principal)
    - [9.12.2. `Population`](#9122-population)
    - [9.12.3. `Virus`](#9123-virus)
    - [9.12.4. `Affichage`](#9124-affichage)
    - [9.12.5. `Simulation`](#9125-simulation)
  - [9.13. `Livrables`](#913-livrables)
- [10. `Analyse interface graphique`](#10-analyse-interface-graphique)
  - [10.1. `Comparaison technologies`](#101-comparaison-technologies)
    - [10.1.1. `WinForm (Windows Forms)`](#1011-winform-windows-forms)
    - [10.1.2. `WPF (Windows Presentation Foundation)`](#1012-wpf-windows-presentation-foundation)
    - [10.1.3. `Unity`](#1013-unity)
      - [10.1.3.1. `Communication`](#10131-communication)
        - [10.1.3.1.1. `Unity Controller`](#101311-unity-controller)
        - [10.1.3.1.2. `PipeLines`](#101312-pipelines)
      - [10.1.3.2. `Intégration`](#10132-intégration)
  - [10.2. `Choix de la solution`](#102-choix-de-la-solution)
- [11. `Problèmes rencontrés`](#11-problèmes-rencontrés)
  - [11.1. `Pipeline`](#111-pipeline)
  - [11.2. `WPF UI`](#112-wpf-ui)
- [12. `Environnement`](#12-environnement)
- [13. `Architecture`](#13-architecture)
  - [13.1. `Arborescence`](#131-arborescence)
  - [13.2. `Structure`](#132-structure)
- [14. `Analyse fonctionnelle`](#14-analyse-fonctionnelle)
- [15. `Analyse organique`](#15-analyse-organique)
- [16. `Planning`](#16-planning)
  - [16.1. `Prévisionnel`](#161-prévisionnel)
  - [16.2. `Effectif`](#162-effectif)
- [17. `Bilan personnel`](#17-bilan-personnel)
- [18. `Conclusion`](#18-conclusion)
- [19. `Table des figures`](#19-table-des-figures)
- [20. `Bibliographie`](#20-bibliographie)
- [21. `Annexes`](#21-annexes)
- [22. `Livrables`](#22-livrables)

# 2. `Résumé`
Covid propagation est une application permettant de visualiser l'évolution du covid au sain d'un environnement peuplé d'individus uniques. La visualisation se fait à l'aide de graphiques (Colonne, courbe, circulaire) et de données tel que le nombre d'infecté actuellement ou le nombre de rétablissement ainsi qu'une interface graphique permettant la visualisation des individus, des lieux, des véhicules et de leurs déplacements.

L'utilisateur a une certaine liberté dans les paramètres de la simulation comme pour la création de la population en choisissant leur nombre et l'age moyen.

Des mesures contre le virus peuvent être prisent, limitant ou stoppant sa propagation.

# 3. `Abstract`

# 4. `Introduction`
Dans le cadre du cursus technicien, nous somme amené à réaliser un travail de diplôme qui dure du 19 avril aux 11 juin. Durant cette période, plusieurs évaluations intermédiaires sont prévuent, la première se situant 10 jours après le début du travail de diplôme. La seconde évaluation est prévu le 17 mai, la troisième le 31 mai et la dernière est le rendu final le 11 juin et dure 9 jours contrairement aux autres sprint qui durent 10 jours. 

Il est nécessaire de réaliser un poster pour ce travail ainsi que de remplir un journal de bord comprenant nos activités et nos réflexions.

Le but de mon projet est de simuler une propagation du covid dans une simulation dite individu-centré. Ce qui signifie que des individus sont simulé et agissent selont leur planning. S'ils sont infectés c'est en cas de contact avec une autre personne infectée. Les données utilisées sont des données officiels et sont maintenuent à jour aussi souvent que possible.

## 4.1. `WPF`
Le programme WPF est le coeur de l'application, il réunit toutes les sections du projet et les gères.
### 4.1.1. `Simulation`
La simulation génère tous les objets nécessaires au fonctionnement de celle-ci. Ses paramètres peuvent être modifié depuis L'UI. Ses paramètres concernant le virus sont écrit dans un fichier XML. La simulation gère aussi la temporalité permettant la propagation et les déplacements.

### 4.1.2. `UI`
L'interface utilisateur est gérée par les grilles WPF permettant un affichage responsive. Elle permet à l'utilisateur de modifier les paramètres de la simulation ainsi que les paramètres d'affichage des graphiques. 

## 4.2. `XML`
Les paramètres jugé fixes du virus sont stocké dans un fichier XML.

## 4.3. `Graphiques`
Les graphiques sont créé par la librairie liveCharts qui permet l'affichage de nombreux type de graphiques ainsi qu'un grand controle sur ceux-ci. Les données sont mise à jour en temps réel et des animations intégrée à la librairie.

## 4.4. `Unity`
Le programme Unity s'occupe de gérer l'interface graphique qui comprend les batiments, véhicules et individus. L'interface est animée en fonction de la temporalité de l'application WPF. La simulation et l'interface graphique avance donc ensemble. La communication s'effectue à travers un pipeline nommé. Les données de la simulation sont envoyé par celui-ci. Le programme Unity est integré directement dans le projet WPF.

# 5. `Schéma de fonctionnement`
![Diagramme de fonctionnement](Medias/rapport/DiagrammeDeFonctionnement.png)
<center><p style="font-size: 11px">Figure 1: Diagramme de fonctionnement</p></center>

# 6. `Maquettes`
## 6.1. `UI`
### 6.1.1. `Page Simulation`
![Maquette page simulation](Medias/Rapport/MaquetteSimulationPage.png)
<center><p style="font-size: 11px">Figure 2: Maquette page de simulation</p></center>

La page simulation permet la visualisation de la simulation via les graphiques, l'interface graphiques ou des données brutes.

### 6.1.2. `Page Paramètres graphiques`
![Maquette page paramètres graphiques 1](Medias/Rapport/MaquetteGraphicSettings1.png)
<center><p style="font-size: 11px">Figure 3: Maquette page de paramètres graphiques 1</p></center>
Cette page permet à l'utilisateur de modifier l'interface graphique qui s'affichera dans la page simulation.

![Maquette page paramètres graphiques 2](Medias/Rapport/MaquetteGraphicSettings2.png)
<center><p style="font-size: 11px">Figure 4: Maquette page de paramètres graphiques 2</p></center>
Cette section permet de modifier en détail les paramètres d'un graphique.

### 6.1.3. `Page Paramètres`
![Maquette page paramètres](Medias/Rapport/MaquettePageParametre.png)
<center><p style="font-size: 11px">Figure 5: Maquette page de paramètres</p></center>
Cette page permet à l'utilisateur de modifier les paramètres de la simulation.

### 6.1.4. `Page Informations`
![Maquette page informations](Medias/Rapport/MaquettePageInformations.png)
<center><p style="font-size: 11px">Figure 6: Maquette page d'informations</p></center>
Cette page sert d'aide à l'utilisateur sur le fonctionnement de l'application et contient aussi des informations sur sa logique et ses sources.

## 6.2. `Interface Graphique`
![Maquette interface graphique](Medias/Rapport/MaquetteInterfaceGraphique.png)
<center><p style="font-size: 11px">Figure 7: Maquette interaface graphique</p></center>
L'interface graphique permet de visualiser ce qui se passe dans la simulation. Les batiments, les véhicules et les individus sont affiché ainsi que leur déplacements et leur status.

# 7. `Organisation`
## 7.1. `Planification`
Pour la planification du travail de diplôme, j'ai décidé d'utiliser excel qui permet de réaliser un planning simple et très compréhensible. Le planning me servant de fil conducteur et de moyen d'organiser l'ordre d'execution des tâches que j'ai créé. 
## 7.2. `Tâches`
Le traçage des tâches s'effectue sur github en suivant le modèle de scrum. Les tâches à effectuer sont dans une section "To Do" les tâches qui sont en cours, sont dans la section "In progress" et finalement les tâches terminées sont dans la section "Done".

Les sprints sont tous séparés ayant des tâches différentes.

![Tâches](Medias/Rapport/Taches.png)
<center><p style="font-size: 11px">Figure 8: Gestion des tâches</p></center>

## 7.3. `Versionning - Backup`
Le versionning est fait à l'aide de github. Au moins deux sauvegarde sont faite chaque jour. Une à midi et une en fin de journée. En cas de perte de données, je ne perd qu'une demi-journée dans le pire des cas. 

Ayant eu des problèmes avec git par le passé. ( Corruptions de fichiers - conflits ) J'ai décidé de faire une sauvegarde supplémentaire sur un disque dur externe. La fréquence de sauvegarde étant plus faible mais suffisante étant donné qu'il s'agit d'une sauvegarde de secours.

# 8. `Technologies utilisées`
## 8.1. `C#`
C# est un language de programmation orienté objet développé dans les années 2000 par Microsoft. Sa première version a été adoptée comme standard internationnal en 2002 par Ecma.
Il est régulièrement mis à jour, des versions majeures sont publiées tous les 2 à 3 ans environ.
La dernière version de C# est la version 8.0 et c’est avec celle-ci que j’ai développé l’application.
Son environnement de développement Visual Studio permet de créer des applications windows
facilement.

## 8.2. `Microsoft Visual studio`
Microsoft Visual Studio est une suite de logiciels disponible sur windows et mac. La dernière version qui est la version utilisée dans la réalisation de ce projet est la version 2019.

Il permet de générer des services web XML, des applications web ASP .NET, des applications Visual basic, Visual C++, Visual C#. C#, C++ et basic utilisent tous les même IDE, ce qui permet de partager certaines ressources.

## 8.3. `WPF`
Windows Presentation Foundation (WPF) ou nom de code Avalon est une spécification graphique de .NET 3.0. Il utilise le XAML qui le raproche d'une page HTML avec un système de balise. Il est apparu en 2006.

WPF comparé à WinForms permet par exemple l'affichage d'une interface responsive et l'utilisation du GPU pour certaines fonctionnalités.

## 8.4. `Unity`
Unity est un moteur de jeu développé par Unity Technologies. Il est majoritairement utilisé par des petits studio et des indépendant pour la création de jeux. Il est compatible avec le C# et le javascript qui permet de réaliser les scripts. Il permet de développer des jeux compatibles avec Windows, Mac OS X, iOS, Android, TV OS, PlayStation 3, PlayStation Vita, PlayStation 4, Xbox 360, Xbox One, Xbox One X, Windows Phone 8, Windows 10 Mobile , PlayStation Mobile, Tizen, Oculus Rift, Wii U, Nintendo 3DS, Nintendo Switch, WebGL.

## 8.5. `XML`
XML qui est un acronyme pour Extensible Markup Language. C'est un language de balises et fait parti du sous-enseble du strandard Generalized Markup Language (SGML). Il a été créé en 1999.

Le bute premier du XMl étant de permettre au SGML d'être utilisé sur le web de la même manière que l'html.
Dans mon cas, il  permettra de stocker certaines données du programme.

## 8.6. `LiveCharts`
LiveCharts est une librairie C# permettant de créer des graphiques. Il permet d'inclure une grande quantité de graphiques à des projets, de lier les données au code. Lorsque les données changes, les graphiques s'adaptent automatiquement et sont animés. Les graphiques sont personnalisables et interactable. Il est même possible d'importer des cartes composées de régions en tant que graphique.

En plus d'ajouter énormément d'éléments graphiques et animations, LiveCharts est très performants et peut par exemple afficher des graphiques contenant plus de 100'000 points tout en restant fluide.

## 8.7. `JSON`
JavaScript Object Notation (JSON) est un format de données dérivé de la notation des objets javascript. Il permet d'afficher la structure d'une information comme par exemple un objet C# ainsi que ses données. C'est le format de données qui me permet de communiquer avec le programme Unity depuis le programme WPF.

# 9. `Cahier des charges`
## 9.1. `Titre`
Covid propagation
## 9.2. `Fonctionnalités`
- Simulation
  - Population
    - Mesures
    - Hôpitaux
    - individus
      - Patient à risque
      - Âge
      - Décès dû au virus
      - Famille
      - Cercle d'amis
      - "Vie" *`Calendrier`*
  - Virus
    - propagation
    - effets sur les individus
      - De “Aucun”
      - À “Grave”
  - Hôpitaux
    - Places limitée
  - Mesure de sécuritées
    - Port du masque
    - Quarantaine
    - Confinement global
    - Distanciation
- Graphiques
  - Informations sur la population
    - Décès
    - Rétablissements
    - Infecté
    - Sains
  - Informations sur le virus
    - Dangerosité

## 9.3. `Matériel et logiciels`
- Pc techniciens
- Visual studio 2019
- Une connexion internet
- Github

## 9.4. `Prérequis`
- C#
- Visual studio 2019

## 9.5. `Descriptif complet du projet`
### 9.5.1. `Méthodologie`
Scrum

### 9.5.2. `Description de l’application`
Simuler un grand nombre de personnes possédant toutes des variables différentes (âge, résistance immunitaire,
etc...), y introduire le virus et observer sa propagation. Il est possible d’affecter des mesures de sécurité, tel que le port du masque ou la distanciation pour observer la possible réduction de la propagation.
L'affichage permet de voir en temps réel la propagation du virus et permet de visualiser chaque individu distinctement au besoin. Des graphiques sont aussi présents pour avoir une idée en chiffres de ce que signifie
l'affichage.

#### 9.5.2.1. `Graphique`
Les données des graphiques sont choisies par l'utilisateur et donc personnalisable. Plusieurs graphiques peuvent être affichés en même temps. Leur position est définie par l'utilisateur au sein de la page de l'application.

L'interface graphique est fournie par [LiveChart](https://lvcharts.net/App/examples/wpf/start). Les données sont directement fournies par l'application ainsi que les échelles de grandeurs qui sont ajustées automatiquement.
Les graphiques à courbes et en forme camembert sont disponibles.
![Exemple de graphiques](Medias/Rapport/Graph.png)

#### 9.5.2.2. `Interface graphique`
En plus des graphiques, une interface graphique affichant les individus ainsi que leur lieur de travail, habitation et déplacement est disponible. Elle permet d'avoir une visualisation plus naturelle de la situation. Elle est très simple car simuler une ville est une tâche trop complexe et longue pour être ajoutée au projet. Il s'agit donc d'une aide visuel simple de la simulation. Il n'y a donc pas de routes ou autres éléments complexe similaires.
Voici deux exemples d'interface graphique :
![Interface graphique](Medias/Rapport/ExemplesInterfaceGraphique.png)

#### 9.5.2.3. `propagation`
La propagation se fait à l'aide de calcul et de différentes variables. 1000 m<sup>2</sup> contenant 10 individus à l'intérieur aura de faibles chances de transmettre le virus. Le même nombre de personnes dans un espace clos de 10 m<sup>2</sup> aura des résultats totalement différents.
 
La température est prise est compte ainsi que les mesures telles que le masque. Le masque réduit les chances de transmettre le virus. La température, elle fait varier la durée de vie du virus à l'extérieur d'un hôte.
La complexité de ce type de calcul étant d'une difficulté largement supérieur aux compétences acquises en tant que technicien, je me base sur cette fiche Excel réalisée par des professionnels. Elle est très bien documentée et sourcée. 

[Fiche excel](https://docs.google.com/spreadsheets/d/1ZWG4LslRBUjMC00Rsi65TKmfVJyzVUf2)

#### 9.5.2.4. `Population`
La population est constituée d'objets C# généré partiellement, aléatoirement en fonction des paramètres de la simulation. Ils informent la simulation en cas de changement d'état (sain, infecté, etc...). Des itérations sont faites dans la simulation pour calculer si un individu est infecté ou non durant le temps écoulé. Il a un planning simple à suivre dans sa journée qui peut être constituée de par exemple :
- Être dans son habitation
- Prendre le bus
- Travailler
- Prendre le bus
- Faire les courses dans un supermarché
- Prendre le bus
- Et finalement rentrer chez sois

Ce planning est différent en fonction des individus même si vaguement le même. Durant sa journée, il croisera d'autres individus et à chaque itération, il aura des chances d'être infecté si des personnes aux alentours le sont. En fonction du lieu, il rencontrera des personnes différentes, parfois les mêmes comme dans son travail où ses collègues sont fixes. Dans le bus, des variations seront possibles. Son cercle d'amis ainsi que sa famille, lorsqu'il se trouve dans son habitation, seront les individus risquant de le contaminer.

##### 9.5.2.4.1. `Temporalité`
Le quotidien des individus est défini par la simulation lors de leur création. Elles peuvent évoluer avec l'âge des individus. 

Une itération est équivalente à ~30min dans la simulation. À chaque itération, chaque individu calcul ses chances d'attraper le virus en fonction de son environnement et des mesures prises. Elle permet aussi à un individu d'évoluer dans son quotidien en passant d'une tâche à une autre par exemple. Leur permettant aussi de changer de lieu et tous les événements liés à l'agenda des individus ainsi que la propagation du virus. La "durée" de la simulation est définie par l'utilisateur et peut donc durer plusieurs jours.

##### 9.5.2.4.2. `Individus`

Les individus possèdent différents paramètres qui vont modifier leur quotidien ainsi que leur résistance au virus. La valeur la plus essentielle est l'âge de ces personnes. L'âge permet de contribuer à la modification de la résistance au virus. Il modifie aussi le quotidien en définissant si la personne va travailler, va à l'école, est libre de faire ce qu'il souhaite ou rien si trop jeune. L'âge évolue avec le temps de la simulation.

Chaque individu a un entourage qui peut le contaminer. Il possède un cercle d'amis avec lequel il peut y avoir des contacts à domicile, et avec lequel il y aura des contacts en extérieur.
Il a aussi une famille avec qui les contacts se font majoritairement à domicile même s'il  peut y avoir des déplacements groupés. Par exemple déposer des enfants à l'école, aller au restaurant en famille.
Finalement, il a des collègues/camarades qui sont des contacts qui se trouve dans les écoles ou lieu de travail et qui sont ne définit pas ceux-ci.

Les moyens de transport des individus sont choisis par la simulation en fonction des paramètres de celle-ci. Un individu possédant une voiture aura beaucoup moins de risque de propager le virus qu'en prenant les transports publics. Il est cependant possible que d'autres personnes du cercle familial ou du cercle d'amis utilisent le même véhicule. De ce fait, il n'est pas forcement 100% sécurisé.
Les transports publics eux ont des risques élevés, car beaucoup de monde se situe dans le même véhicule de taille moyenne. En plus de cela, les individus sont en contact avec des étrangers qui peuvent varier en fonction des jours augmentant encore plus les chances de contagion.

La résistance au virus des individus défini si la personne a des symptômes en cas d'infection, si elle est asymptomatique, ou si elle a besoin de soins. Ce paramètre est défini par pourcentage. De 100% à 90% de résistance, l'individu est asymptomatique. De 90% à 50% de résistance, l'individu a des symptômes tels que la toux. De 50% à 10% de résistance, la personne est hospitalisée. Et finalement, à moins de 10%, l'individu est hospitalisé et risque la mort.
  - Plus ce paramètre est haut moins les effets du virus sont présents
  - 90-100 => asymptomatiques
  - 90-50 => symptômes normaux
  - 50< => hospitalisations
  - 10< => décès 

Chaque individu créé commence avec une valeur entre 80 et 100. Sachant qu'environ 5% de ces individus ont plus de 90 de résistance. Des maladies peuvent entrer en compte et baisser la résistance naturelle. Plus l'âge est élevé, plus l'individu sera impacté par un grand nombre de maladies et celles-ci seront plus dangereuses.

Les maladies sont inspirées de maladie réelle impactant l'effet du covid. Cependant, dans la simulation, elle n'affecte que la résistance au virus. Ces maladies apparaissent de façons aléatoires et plus fréquemment sur les individus dont l'âge est élevé. Elles ne se propagent pas. Elles sont en partie assignées au départ par la simulation puis apparaissent avec le temps. Elles réduisent la résistance au covid de 1% à 20% en fonction de la maladie et de l'âge de la personne.

##### 9.5.2.4.3. `Hopitaux / écoles / entreprise`
Ces différents lieux fonctionnent de façon similaire. Ils ont tous des individus en leur seins qui peuvent se transmettre le virus. Ils ont des tailles différentes en fonction du nombre de personnes pouvant être à l'intérieur. 

Les hôpitaux fonctionnent légérement différement. Ils ont des patients ainsi que des membres du staff de l'hopital. Il y a donc des différences de mesures et quantités. Les patients sont là de manière temporaire en fonction du nombre de personnes attrapant le covid.

Les écoles ont une situation similaire en aillant des élèves ainsi que des profs qui ont des mesures et quantités différentes.

Les entreprise elles fonctionnent en groupe d'individus, similaire aux classes des écoles mais sans personnel ayant des mesures différentes des autres.

## 9.6. `Protocols de tests`
Ce projet étant en c#, je vais utiliser les tests unitaires intégrés dans visual studio.

Les tests unitaires ne garantissant pas qu'il n'y ait aucun bug dans l'application, je vais créer des scénarios que je testerais avant et après chaque implémentation de fonctionnalités. Ces scénarios auront pour but de couvrir un maximum de possibilités pour éviter l'apparition de bug dû à une modification du code ou l'ajout d'une fonctionnalité. Ils permettent aussi de trouver d'éventuels des problèmes d'ergonomie en me plongeant à la place d'un utilisateur.

## 9.7. `Persona`
### 9.7.1. `Utilisateur expérimenté`
![Perona experimenté](Medias/Rapport/Persona_Ashley.png)
<p style="text-align: center">Figure 1: Persona experimenté</p>

### 9.7.2. `Utilisateur inexpérimenté`
![Perona inexpérimenté](Medias/Rapport/Persona_Kanan.png)
<p style="text-align: center">Figure 2: Persona inexpérimenté</p>

## 9.8. `User stories`
### 9.8.1. `Ashley`
**En tant que** Ashley<br>
**Je veux** comparer différentes situations avec différentes personnes en prenant des mesures identiques<br>
**Afin de** pouvoir observer les différences et déterminer quelles mesures est efficaces dans quelle situation.<br>

**scénarios**
Je crée sans soucis une situation à l'aide de l'application. Pour ce faire, j'entre différents paramètres, tel que le nombre de personnes, les mesures prisent pour limiter la transmission ainsi que d'autres paramètres. <br>
J'observe la simulation et prends note des résultats.<br> 
Une fois terminée, j'en lance une autre avec certains paramètres différents et prends note des résultats.<br>
Je compare les résultats avec la simulation précédente et effectue ma conclusion.

### 9.8.2. `Kanan`
**En tant que** Kanan<br>
**Je veux** vérifier l'efficacité de différentes mesures prisent pour éviter la propagation du covid<br>
**Afin de** afin de me donner une idée concrète et visuelle de l'efficacité de ses mesures.<br>

**scénarios**
Je lance l'application et cherche à créer une simulation. Une fois trouvé, je peux voir les mesures qui apparaissent clairement, d'autres paramètres sont disponibles mais je n'y touche pas.<br>
Une fois la simulation lancée, je vois un message m'indiquant que celle-ci commence.<br>
Des aides sont disponibles me permettant de comprendre les données qui sont affichées. <br>
Après avoir terminé cette simulation, j'en lance une autre en désactivant les mesures. <br>
Je relance la simulation et observe la différence entre les deux simulations. <br>

## 9.9. `Diagramme d'activité`
![Diagramme d'activité](Medias/Rapport/DiagrammeDactivite.png)
<p style="text-align: center">Figure 3: Diagramme d'activité</p>

## 9.10. `Planning`
https://docs.google.com/spreadsheets/d/1tSpIbcDVvGnzMhEN71UDwPOxEy0oapQSSbxzjqXt3RA/edit?usp=sharing

## 9.11. `Diagramme de classe initial`
![Diagramme de classe initial](Medias/Rapport/InitialClassDiagram.png)
<p style="text-align: center">Figure 4: Diagrame de classe initial</p>

## 9.12. `Interactions`
### 9.12.1. `Menu principal`
- Affiche un preview de l'affichage de la simulation
- Btn Paramètres
  - Population
    - Remplace l'affichage actuel se situant à droite pour afficher les paramètres de la population
  - Virus
    - Remplace l'affichage actuel se situant à droite pour afficher les paramètres du virus
  - Affichage
    - Remplace l'affichage actuel se situant à droite pour afficher les paramètres de l'affichage
- Btn lancer la simulation
  - Change l'affichage de la totalité de l'application, affiche une barre de chargement indiquant l'état de création de la simulation.

### 9.12.2. `Population`
Affiche une page avec les paramètres suivant :
- Écoles / lieux de travail
  - Différentes selon l'âge
  - Zone de transmission
- Familles / Cercles d'amis
  - Transmission
- Moyenne d'âge de la population
  - Permet de modifier la moyenne d'âge de la population de 1 à ~100
  - Permet de délimiter une limite d'âge maximal et minimal
  - Il est possible de le laisser par défaut
- Nombre d'individus
  - Le nombre d'individus simulé dans une population
  - La limite n'est pas définie par le programme
  - L'utilisateur connaît les limites de sa machine
- Mesures
  - Permet de sélectionner plusieurs mesures
  - Les mesures ont un pourcentage d'efficacité
  - Permet de réduire les chances de propagation du virus
    - Affecte différemment le virus en fonction de la mesure
    - Pourrait totalement contrer un virus
  - Peut être modifier par l'utilisateur jusqu'à un niveau de 100% de protection
  - Valeur par défaut défini par des études sur le sujet
  - Appliquer uniquement sur certaines parties de la population
    - Infectés
    - Sains
    - À risques
- Cercle social
  - Ami
  - Famille
  - Collègues
  - ...
  - Transmissions accrues
  - Rencontres inclussent dans le planning journalier des individus
- Hôpitaux
  - Il y a plusieurs hôpitaux avec les options :
    - Copier
    - Coller
    - Appliquer sur tout
  - Permet de modifier le nombre d'hôpitaux
  - Permet de modifier le nombre de places
  - Stabilise les individus y étant admis
    - Réduis leurs chances de décès
  - Nécessite du personnel qui peut être infecté pour fonctionner
    - Mesures du personnel : 
      - Permet de sélectionner plusieurs mesures
      - Les mesures ont un pourcentage d'efficacité
      - Permet de réduire les chances de propagation du virus
        - Affecte différemment le virus en fonction de la mesure
        - Pourrait totalement contrer un virus
      - Peut être modifier par l'utilisateur jusqu'à un niveau de 100% de protection
- Btn annuler
  - Annule les modifications faites à l'hôpital
  - Réaffiche les données précédemment affichées
- Btn sauvegarder
  - Sauvegarde les paramètres choisis par l'utilisateur
### 9.12.3. `Virus`
Affiche une page avec les paramètres suivant :
- Effet sur le corps
  - Permet de modifier le pourcentage de propagation en fonction du symptôme (toux)
  - Les effets mortels nécessitant une hospitalisation 
- Moyens de transmissions
  - Sont impacté par les symptômes (en incrémentant l'efficacité)
  - Sont impacté par les mesures (en décrémentant l'efficacité)
- Durée
  - Permet de définir la durée durant laquelle le virus prend effet
- Asymptomatique
  - Permet de définir si oui ou non il y a des asymptomatiques
  - Permet de définir le pourcentage d'asymptomatiques

### 9.12.4. `Affichage`
Affiche une page avec les paramètres suivant :
- Graphiques
  - Permet de sélectionner différents styles de graphiques à afficher
    - Permet de sélectionner une donnée au choix en X et en Y
    - Un exemple du graphique avec les données est affiché à côté de la barre de sélection
  - Plusieurs graphiques possibles à sélectionner
- Affichage d'une "carte" permettant une visualisation plus simple

### 9.12.5. `Simulation`
Affiche une page :
- Affichage d'une barre de chargement lors de la génération de la simulation
  - Évolue en fonction du nombre d'individus créé
- Affiche les graphiques sélectionnés
  - Onglets permettant de sélectionner quel graphique affiché
  - Possibilité d'afficher jusqu'à 4 graphiques sur le même onglet
- S'actualise toutes les secondes (environ)

## 9.13. `Livrables`
- Mind Map
- Planning
- Rapport de projet
- Manuel utilisateur
- Journal de travail ou LogBook
- Résumé / Abstract

# 10. `Analyse interface graphique`
Cette analyse concerne l'interface graphique et le choix de la technologie à utiliser pour réaliser celle-ci.

## 10.1. `Comparaison technologies`
### 10.1.1. `WinForm (Windows Forms)`
Lors du CFC ainsi que de l'apprentissage technicien, nous avons toujours utilisé cet interface pour réaliser l'entièreté de nos projets. Je connais donc bien cet environnement contrairement au WPF. En plus de cela, l'interface graphique réalisée dans le poc est en WinForm. Me permettant donc de simplement importer le projet déjà existant.

Cependant, WinForm ainsi que l'interface graphique déjà existante apportent de gros problèmes tel que les timers. Lorsqu'il y a une charge CPU trop lourde, les timers perdent leur rythmes et n'arrivent plus à suivre. Le résultat de cette surcharge est que plus rien ne fait de sense. Les animations n'ont plus le temps de s'effectué rendant les individus immobile ou presque.

### 10.1.2. `WPF (Windows Presentation Foundation)`

WPF est plus récent que WinForms et a donc cerains avantage non négligeable en comparaison. Il est beaucoup plus complet en terme d'estéthique et donc d'UI que WinForms. En plus de cela, il est possible de créer des objets en 2D ou 3D. Ces objets contrairement à WinForms sont gérés par le GPU plutôt qu'être entièrement basé sur le CPU. Cet différence à elle-seule fait pencher la balance pour WPF.

La liaison entre la vue et les données est aussi plus efficaces ce qui est très important dans mon cas.

Cependant, une application WPF ne peut pas être lancée sur un mac ou sur linux. C'est un gros désavantage mais dans le cadre de se travail, il me semble négligeable.

<div style="page-break-after: always;"></div>
Le possible problème de timer bien que probablement réduit du au fait que la charge du CPU est allégé par la carte graphique, risque d'être toujours présent.

Il faut aussi noter que je n'ai aucune expérience en WPF et vait donc devoir m'y habituer durant un certain temps avant d'être efficace à 100%.

### 10.1.3. `Unity`
Unity est un moteur de jeu en 2D et 3D. Il est possible de l'intégrer directement à une application WPF. Ça me semble être le meilleur choix si l'on prend en compte les problèmes de timer des deux autres technologies. Unity possède de façon native des méthodes qui sont appelée à chaque frame permettant le bon déroulement de la simulation.

En plus de cela, j'ai beaucoup d'expérience avec ce logiciel, ayant réalisé mon TPI avec celui-ci. Je peux donc affirmer qu'il est beaucoup plus simple de réalisé l'interface graphique avec Unity.

Cependant un autre problème est présent. La liaison des données. Il m'est impossible, sans le tester, de savoir si ce modèle de fonctionnement est compatible avec mon projet. Je sais qu'il est possible de transférer des informations de WPF à unity cependant, je ne sais pas si la fréquence d'envoie est suffisante ou même si la quantité de données envoyées que je souhaite atteindre est possible.

#### 10.1.3.1. `Communication`
Pour communiquer entre WPF et Unity j'ai essayé plusieurs méthodes fonctionnant différement et surtout de complexité différente.
##### 10.1.3.1.1. `Unity Controller`
Mon premier essai fut avec Unity Controller qui permet de créer un server qui communique entre une application C# et Unity.

Pour l'installer il faut d'ajouter le paquet nuget "Unity Controller" au projet ainsi qu'un using "UnityController". Sont implémentation est la plus simple des solutions testées sachant que sont implémentation ne prend que quelque lignes au total.

Le code dans un script unity ne comprend que deux lignes. La première étant le démarage du serveur.
```C#
void Start()
{
    UnityCommands.StartServer("008");
}
```

La deuxième s'updatant à chaque image, permet de recevoir la commande et de l'appliquer.
```C#
void Update()
{
    UnityCommands.ReceiveMessage();
}
```

Maintenant, dans le projet windows. Dans l'initialisation de la form, il faut démarrer le server en localhost.
```C#
public MainWindow()
{
    InitializeComponent();
    UnityCommands.StartClient("localhost", "008");
}
```

La dernière ligne située dans un évènement click d'un bouton permet de modifier l'élèment texte du GameObject "GameObjectText" en lui ajoutant la valeur "Texte".
```C#
private void Button_Click(object sender, RoutedEventArgs e)
{
    UnityCommands.UpdateText("GameObjectText", "Texte");
}
```

Cette implémentation de la communication est extrêmement simple à mettre en place cependant, les possibilités sont très limitées. Les seules actions possibles sont le fait de changer le texte d'un GameObject, sa couleur, son image, etc. Il est impossible d'envoyer un message de code à code puis de l'interpreter. Cette façon de faire ne peut donc pas servir à la réalisation de mon projet qui demande un traitement des données.

##### 10.1.3.1.2. `PipeLines`
Contrairement à UnityController, les pipelines laissent plus de liberté mais leur complexité est bien supérieur. J'ai rencontré divers problèmes en implémentant cette fonctionnalité.

Dans mon cas, la communication se fait à sense unique, WPF donnant les informations à l'interface graphique se trouvant sur Unity. Il font donc commencer par créer un serveur du côté WPF.

`Écriture`<!-- TITRE-->

L'écriture se situe dans le projet WPF.
Cette méthode permet de créer le serveur, le démarrer le serveur et d'établir une connexion avec le client qui est Unity. La méthode ServerThread sera appelée lors de l'appelle de la méthode sever.Start().
```C#
private void ConnectToUnity()
{
    Thread server;
    Debug.WriteLine("Waiting for client connect...\n");
    server = new Thread(ServerThread);
    server.Start();
}
```

Lors du démarrage du Thread, le pipeline est créé et le serveur attend que le client se connecte. Une fois qu'il est connecté, un objet StreamString est créé permettant l'écriture de message pouvant être transferé via le pipeline.
```C#
private void ServerThread(object data)
{
    NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe", 
                                          PipeDirection.InOut, numThreads);

    pipeServer.WaitForConnection();

    Debug.WriteLine("Client connected.");
    try
    {
        Debug.WriteLine("Creating streamString...");
        ss = new StreamString(pipeServer);

        Debug.WriteLine("You can now write.");
    }
    catch (IOException e)
    {
        Debug.WriteLine("ERROR: {0}", e.Message);
    }
}
```

Le constructeur de l'objet StreamString récupère le pipeline créé et le transform en Stream qui est à sont tour transformé en BinaryWriter qui permettra l'envoie des données. Créé un objet UnicodeEncoding permettant la conversion de string en bytes pour le transfer.
```C#
public StreamString(Stream stream)
{
    this.stream = new BinaryWriter(stream);
    streamEncoding = new UnicodeEncoding();
}
```

WriteString est la méthode appellée à chaque fois que des données doivent être envoyées. Elle converti le message qui lui est fournit en byte et envoie celui-ci dans le pipeline.
```C#
public async void WriteString(string outString)
{
    await Task.Run(() => {
        byte[] outBuffer = streamEncoding.GetBytes(outString);
        int len = outBuffer.Length;

        List<byte> dataToSend = new List<byte>();
        dataToSend.Add((byte)(len >> 8));
        dataToSend.Add((byte)(len >> 0));
        dataToSend.AddRange(outBuffer.ToList());
        stream.Write(dataToSend.ToArray(), 0, dataToSend.Count);
        stream.Flush();
    });           
}
```
`Lecture` <!-- TITRE-->

L'ouverture de la connexion avec le server s'effectue dans la méthde "Start" d'unity qui s'effectue au démarrage du projet. Puis appelle la méthode ConnectToServer(). Si la connection à échouée, un nouvel essai sera effectué à chaque frame du projet jusqu'à que celle-ci soit effectuée.
```C#
void Start()
{
    Debug.Log("Pipe Opening Process Started");
    pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.In,   PipeOptions.Asynchronous);
  
    Debug.Log("Connecting to server...\n");
    ConnectToServer();
}

void Update()
{
    if (!pipeClient.IsConnected)
    {
        ConnectToServer();
    }
}
```

ConnectToServer() essai donc de ce connecter, si la connexion est effectuée, un objet SreamString est créé et la lecture du flux commence.
```C#
private void ConnectToServer()
{
    pipeClient.Connect();
    if (pipeClient.IsConnected)
    {
        ss = new StreamString(pipeClient);
        Thread.Sleep(250);
        ReadPipeData();
    }
}
```

ReadPipeData() est une méthode récursive et asynchrone. Elle permet de lire le résultat reçu par le pipeline. Elle attend la réception d'un message. Une fois qu'elle en reçoit un grâce à ReadStringAsync(), elle le lis et finit par s'appeler elle-même et recommence le cycle.
```C#
private async void ReadPipeData()
{
    string result = await ss.ReadStringAsync();
    ChangingText.GetComponent<Text>().text = result;
    ReadPipeData();
}
```

La lecture du résultat s'effectue exactement comme l'écriture mais dans le sens inverse. La longueur du message est récupérée et utilisée pour le lire dans son entièreté. Une fois le message reçu, le résultat est converti en string et retourné à ReadPipData() qui pourra effectuer son cycle.

```C#
public async Task<string> ReadStringAsync()
{
    return await Task.Run(() =>
    {
        int len;
        len = stream.ReadByte() << 8;
        len += stream.ReadByte();
        byte[] inBuffer = new byte[len];
        stream.Read(inBuffer, 0, len);
        return streamEncoding.GetString(inBuffer);
    });
}
```

#### 10.1.3.2. `Intégration`
L'intégration permet d'avoir un rectangle au sein de la page WPF qui sera constitué d'une application .exe. Dans ce cas, il s'agit d'unity. Ça ne permet pas de commander le contenu de la fenêtre mais uniquement sa taille, position et quand démarrer le .exe.

Cette méthode permet de charger et démarrer le projet Unity qui a été buildée au préalable. UnityGrid étant une grille crée dans la vue du code WPF.
```XML
<Grid x:Name="unityGrid" Width="454" Height="319" VerticalAlignment="Top" HorizontalAlignment="Right" Margin="0,10,327,0"></Grid>
```
Cette grille est ensuite transformée en unityHandle qui permet donner au programme la grid ou il va devoir s'afficher. Le process récpuère l'emplacement du programme à lancer. Les arguments permettent de définir où le programme doit se lancer, sans les arguments, le programme se lance dans une fenêtre indépendante. Ensuite, le process est lancé ce qui démarre le programme. EnumChildWindows (user32.dll) permet de lier le programme lancé à la fenêtre, permettant la modification de sa taille en fonction de la taille du programme WPF.
```C#
private Process process;
private void LoadUnityExe()
{
    HwndSource source = PresentationSource.FromVisual(unityGrid) as HwndSource;
    IntPtr unityHandle = source.Handle;
    
    //Start embedded Unity Application
    process = new Process();
    process.StartInfo.FileName = @".\UnityBuild\testWPF_Unity.exe";
    process.StartInfo.Arguments = "-parentHWND " + unityHandle.ToInt32() + " " + 
                                  Environment.CommandLine;
    process.Start();
    
    if (process.WaitForInputIdle())
    {
        EnumChildWindows(unityHandle, WindowEnum, IntPtr.Zero);
    }
}
```

## 10.2. `Choix de la solution`
Mon attention se porte premièrement sur Unity qui me semble être la solution avec le meilleur rendu et permet de contourner certains problèmes présent dans les deux autres options. Le premier test que j'ai effectué ne permet pas de transmettre des données complexes, uniquement des strings ou images mais pas de list c# ou autre éléments que je pourrais utiliser. </br>
Le second test que j'ai effectué avec les pipelines permet de transférer une grande quantité de données (Int32) en string. Il est donc possible de transferer des objets en json d'un projet à l'autre.</br>
C'est cette deuxième option que j'ai donc choisis pour réaliser l'interface graphique. Permettant donc d'utiliser Unity qui est beaucoups plus simple à utiliser pour la réalisation de ce genre de fonctionnalité.


# 11. `Problèmes rencontrés`
## 11.1. `Pipeline`
Durant l'implémentation des pipelines, j'ai rencontré divers problèmes, le premier étant que la structure original des pipelines utilise une communication synchrone. Lors de l'attente de données, le programme unity s'arretait completement jusqu'à la réception de la donnée attendue. Une fois reçues, une exécutait une frame puis attendait à nouveau des données. Le rproblèmes était similaire dans le code WPF qui, après avoir envoyé des données, attendait qu'Unity les ais réceptionnées pour continuer.

 Pour palier à ce problème, j'ai opté pour l'implémentation de l'asynchrone dans la réception et dans l'envoie des données. Concernant l'envoie, j'ai rencontré un léger problème qui m'empêchait d'accéder à une méthode en asynchrone car j'envoyais le contenu d'un textbox appartenant donc au thread principal. Ce problème à été reglé avec l'utilisationd de Dispatcher.Invoke. Ce problème de thread m'a malgré tout pris un certain temps à reglé du au fait que WPF, Unity et WinForms utilisent tous une façon d'invoke différente rendant les recherche plus compliquée.
```C#
await Task.Run(() =>
{
    Dispatcher.Invoke((Action)(() =>
    {
        ss.WriteString(tbxValue.Text);
    }));
});
```

 Le gros poisson concerne la réception des données. Comme étant un code bloquant, j'ai cherché différentes façon d'appeler mon code de manière constante. Le placer dans la méthode update(appelée chaque frame) ne fonctionnant tout simplement pas, j'ai opté pour la méthode "InvokeRepeating" qui permet de donner un interval dans lequel une méthode sera executée.

 InvokeRepeating couplé avec de l'asynchrone m'a permis d'éviter le programme de s'arrêter à chaque attente de données tout en étant capable d'en recevoir. Cependant, les données reçues n'étaient pas fidèlent aux données envoyées.
 
 Par exemple:<br>
 la première réception de données est fidèle à celles envoyées. Lors de la deuxième réception, le message est tronqué et certaines lettres du début de la transmission sont manquantes. La troisième réception est encore plus corrompue, recevant donc un message en caractère Chinois. Après cette réception il était corrant de ne recevoir des données vides.

 ![Fidélité des données](Medias/Rapport/DataFidelity.png)

 Pour régler ce problème, j'ai pensé à remplacer InvokeRepeating par un méthode asynchrone récursive. Cette méthode est appelée une première fois au démarrage du script puis s'appelle une fois qu'elle a récptionné des données. Permettant de recevoir les données correctes et sans bloquer le code.
```C#
    private async void ReadPipeData()
    {
        string result = await ss.ReadStringAsync();
        ChangingText.GetComponent<Text>().text = result;
        ReadPipeData();
    }
```

## 11.2. `WPF UI`
N'ayant encore jamais travaillé avec WPF, la structure du projet m'a déconcerté au départ mais j'ai rapidement pu prendre la main. Cependant, pour réaliser une apparence spécial, j'ai du modifier certains outils mis à disposition par WPF. Les boutons étant très similaires à WinForms ne m'ont pas posé de problèmes. 

Ce n'est qu'après avoir eu envie de modifier l'affichage d'un slider et après des recherches, que je me suis rendu compte qu'il était impossible de modifier le slider existant pour satisfaire au paramètres que j'avais choisis. Pour le modifier, il m'a fallut créer un template du slider qui revient à récupérer le code XAML du slider et le modifier à la main. Il m'a fallut un certain temps pour comprendre chaque composants ainsi que leurs paramètres. Même si ça permet de modifier dans le moindre détail l'outil, j'ai été étonné qu'il n'existe par de paramètres facilement modifiable comme le background des boutons par exemple. 

Après avoir eu du mal à modifier le slider, j'ai pu modifier les autres outils (RadioBoutons, Checkbox) avec aisance.

# 12. `Environnement`
L'environnement de travail est composé d'un pc technicien, 3 écrans, clavier, souris et d'un SSD amovible avec Windows 10 pro version 10.0.19042 Build 19042. Le code est réalisé à l'aide de visual studio 2019 versions 16.9.2. La documentation et le logbook sont réalisés à l'aide de visual studio code et des extensions Markdown All in One et Mardown PDF.

Le projet WPF utilise .core 3.1 qui est la version lts.
La version d'Unity est la 2020.3.4f1 qui est aussi la version lts.
# 13. `Architecture`
## 13.1. `Arborescence`
```
├── CovidPropagation
│   ├── .vs
│   ├── CovidPropagationGraphicInterface
│   └── CovidPropagationGraphicInterface.sln
├── Documentation
│   ├── Medias
│   │   ├── LogBook
│   │   └── Rapport
│   ├── LogBook.md
│   └── Rapport.md
└── POC
    ├── TestUnity_WPF
    └── testWPF_Unity.sln
```
## 13.2. `Structure`

# 14. `Analyse fonctionnelle`
# 15. `Analyse organique`

# 16. `Planning`
## 16.1. `Prévisionnel`
Dans le cadre de ce projet, je vais commencer par réflechir à la structure général de celui-ci et de ces interactions entre les différentes section (Simulation - Graphique - etc.) ainsi que toujours trouver le meilleurs moyen d'optimiser le code et la structure pour permettre la simulation d'un plus grand nombre d'individus.

Les timers de Visual studio étant très aléatoire dès lors que le programme nécessite une trop grand charge de travail, je vais essayer de trouver une alternative ou de corriger ce problème en modifiant le timer.
Une fois la structure réfléchis et le problème de timer reglé, je vais commencer par créer l'interface de l'application où viendront s'ajouter tous les autres composants.
Je vais ensuite commencer à créer la population et vérifier que tout fonctionne correctement. Pour la population, j'aurais tout de même besoin d'une esquisse des batiments.

Après la population, et pour le second sprint, je pourrait commencer à générer la propagation ainsi que les batiments et leurs différents paramètres pour compléter la simulation.

Pour le troisième sprint, je vais m'attaquer à la partie graphique en commençant par les graphiques et la librairie Live Charts. Et je finirais par adapter l'interface graphique déjà existante au projet en y apportant des modifications majeurs.

Le dernier sprint est consacré entièrement aux finitions du projet ainsi qu'à l'optimisation et si le temps est suffisant, aux améliorations prévus dans le cahier des charges. Les deux derniers jours étant consacré entièrement à la documentation.

![Planning prévisionnel Sprint 1](Medias/Rapport/PlanningPrevisionnelSprint1.png)
![Planning prévisionnel Sprint 2](Medias/Rapport/PlanningPrevisionnelSprint2.png)
![Planning prévisionnel Sprint 3](Medias/Rapport/PlanningPrevisionnelSprint3.png)
![Planning prévisionnel Sprint 4](Medias/Rapport/PlanningPrevisionnelSprint4.png)

## 16.2. `Effectif`

# 17. `Bilan personnel`

# 18. `Conclusion`

# 19. `Table des figures`
- [Figure 1: Diagramme de fonctionnement](#5-schéma-de-fonctionnement)
- [Figure 2: Maquette page de simulation](#611-page-simulation)
- [Figure 3: Maquette page de paramètres graphiques 1](#612-page-paramètres-graphiques)
- [Figure 4: Maquette page de paramètres graphiques 2](#612-page-paramètres-graphiques)
- [Figure 5: Maquette page de paramètres](#613-page-paramètres)
- [Figure 6: Maquette page d'informations](#614-page-informations)
- [Figure 7: Maquette interaface graphique](#62-interface-graphique)
- [Figure 8: Gestion des tâches](#72-tâches)

# 20. `Bibliographie`

19.04.2021
  - Utilisés dans la compairson entre les différentes technologies de l'interface graphique
    - [c-sharpcorner - Sandeep Mishra - WPF vs WinForms 1](https://www.c-sharpcorner.com/article/wpf-vs-winforms/#:~:text=The%20abbreviation%20W.P.F%20simply%20refers,to%20develop%20Windows%20desktop%20applications.)
    - [wpf-tutorial - WPF vs WinForms 2](https://www.wpf-tutorial.com/about-wpf/wpf-vs-winforms/)
    - [educba - Priya Pedamkar - WPF vs WinForms 3](https://www.educba.com/winforms-vs-wpf/)
    - [stackoverflow - Litisqe Kumar - WPF vs WinForms 4](https://stackoverflow.com/questions/31154338/windows-forms-vs-wpf)
  - Utilisés dans le programme de test WPF et unity
    - [stackoverflow - Programmer - Intégration d'unity en WPF](https://stackoverflow.com/questions/44059182/embed-unity3d-app-inside-wpf-application)
    - [youtube - Anousha - Communication](https://www.youtube.com/watch?v=rz6MNZMyza4)
    - [Packet NuGet sur Unity](https://github.com/GlitchEnzo/NuGetForUnity/releases)

20.04.2021
  - Utilisés dans la création des pipelines
    - [MSDN - Auteurs disponibles sur la page - Création des pipelines nommés](https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-use-named-pipes-for-network-interprocess-communication)
    - [Forum Unity - WylieFoxxx - Modification du code msdn pour fonctionner sur Unity](https://answers.unity.com/questions/483123/how-do-i-get-named-pipes-to-work-in-unity.html)
- 21.04.2021
  - Utilisé dans la création des documentation des pipelines
    - [Stackoverflow - usr - Modification du code MSDN](https://stackoverflow.com/questions/49172352/c-sharp-explanation-of-stream-string-example)

# 21. `Annexes`
- Projet C#
- Images
  - Diagramme de classe
  - Planning prévisionnel
  - Planning effectif
- Journal de bord

# 22. `Livrables`
- Documentation
- Logbook
- Programme C#
- Poster