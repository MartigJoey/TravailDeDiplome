# LogBook
| Auteur | Date de début | Projet  | Description | Communication |
| ------------- |:-------------:| :-----|---|---|
| Joey Martig| 19.04.2021 | Travail de diplome Covid propagation | Applications permettant de voir l'évolution d'un virus dans un environnement peuplé d'individus. | Joey.mrtg@eduge.ch Joey.mrtg@gmail.com |
# 19.03.2021 - 08h05/17h
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
    - Plus récent (Plus de librairies mise à jour)
  - [Référence 3](https://www.educba.com/winforms-vs-wpf/)
    - WPF est plus rapide
    - Aussi plus complexe
  - [Référence 4](https://stackoverflow.com/questions/31154338/windows-forms-vs-wpf)
    - Les deux se valent
    - WPF est meilleur pour les UI
    - WPF n'est pas disponible pour linux et mac
    - Le data binding est meilleur en WPF ainsi que le design
  - Conclusion
    - L'apprentissage de la structure WPF va prendre du temps mais semble en valoir la peine.
    - L'UI sera plus belle et simple à réaliser en WPF
    - Le plus gros avantage semble être BEAUCOUP plus efficace pour l'affichage de l'interface graphique me permettant d'utiliser le GPU à pleine puissance contrairement au winform qui ne l'utilise pas. S'achant que les pc sont équipé de gtx 1060, le changement en wpf semble être le meilleur choix.
    - WPF semble donc être le meilleur choix.
  - Une autre alternative à l'utilisation de l'interface graphique de WPF est unity qui peut être intégré et communiquer avec le projet. [Intégration d'unity en WPF](https://stackoverflow.com/questions/44059182/embed-unity3d-app-inside-wpf-application). [Communication](https://www.youtube.com/watch?v=rz6MNZMyza4). [Génération dynamique](https://www.youtube.com/watch?v=8eTWq27h4sY)
  - Unity semble être une bonne idée cependant, dans mon cas, en prenant en compte le nombre d'appelles, utiliser unity et faire communiquer les deux projets me semblent trop compliqué. Peut-être que rester sur WPF est plus sûr.
- Documentation
  - Analyse interface graphique
    - Comparaison WPF - WinForms - Unity
    - Choix de technologie
- Tentative de communication réussi à l'aide de ce [tutoriel](https://www.youtube.com/watch?v=rz6MNZMyza4).
  - inutilisable pour ce projet car la communication est trop restrainte.
  - La communication se fait uniquement avec des string ou des images mais pas d'objets c#.
- Tentative d'intégration d'unity dans un projet WPF
  - Réussi
  - Problème de resize
    - Au lancement prend toute la fenêtre
    - Le resize et positionnement fonctionnent
    - À la fermeture du programme, unity ne s'arrêtait pas et utilisait 15% du processeur à chaque ouverture.