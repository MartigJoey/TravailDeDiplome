   M S B A M L   `   `   `  ����[  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms  5      K GCovidPropagation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:CovidPropagationCovidPropagation 5      %   CovidPropagation.PageSimulation   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5	      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5
      Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:CovidPropagation     5        N� Title$  PageSimulation��5      .��5      �  � ColumnDefinitions 5   
   �� 5      $	��25* �6   $��25��6+   �� 5      $	��25* �6   $��25��6+   �� 5      $	��25* �6   $��25��6+   �� 5      $	��25* �6   $��25��6+    � RowDefinitions 5   
   �� 5      $��5* �6   $��25��6(   $��25��67   �� 5      $��5* �6   $��25��6(   $��25��67   �� 5      $��5* �6   $��25��6(   $��25��67   �� 5      $��5* �6   .+�5    
   �� $��0��6   $��0��5!         	labelText#��[  5"      .��63   Vitesse d'itération :6J   �� 5#   
   -   5         ��Name$ intervalSlider��5$        HorizontalSlider#��[ 5#      $��0��5%      $��1��6%   $��2��62    � Minimum$ 0Q�5&       � Maximum$
 1000Q�6!    �� IsSelectionRangeEnabled$
 True��5'      5)      �� 5,   
   $��2��6   $��0��5-      #��[  5.      .��63   Date:6:   �� 51   
   -   5         ��Name$ lblDate��53      $��2��51      $��1��52      #��[  54      65   �� 56   
   -   5         ��Name$ btnStart��57        smallButton#��[ 56   %   $��0��58      $��2��6%   $��	Démarrer��59      �� 5;   
   -   5        $ btnBreak��5<      #��[ 5;   %   $��1��5=      $��2��6%   $��Pause��5>      $
��auto��5?      �� 5A   
   -   5        $ btnReset��5B      #��[ 5A   %   $��2��5C      $��2��6%   $��Réinitialiser��5D      $
��auto��5E      �� 5G   
   -   5        $ btnOpenRawDatas��5H      #��[ 5G   2   $��3��5I      $��2��6%   $��Données��5J      $
��auto��5K      ${�False��5L      �� 5N   
   -   5        	 ��Name$	 slvScroller��5N      $
��Auto=�5O      $
��Auto=�5P      $��0��5Q      $��3��5R      $��2��5S      $��4��5T      .��5U      � -   5        
 �Name$
 
grdContent��5U      $
��True��5V      $��Stretch=�5W      $��Stretch=�5X       � ShowGridLines$
 True��5Y      5Z      5[      5\      5]      