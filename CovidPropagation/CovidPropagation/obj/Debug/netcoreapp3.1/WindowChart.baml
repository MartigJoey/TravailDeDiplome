   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5   	   K GCovidPropagation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:CovidPropagationCovidPropagation 5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   "   CovidPropagation.WindowChart   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	   0localclr-namespace:CovidPropagation     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   :xctk)http://schemas.xceed.com/wpf/xaml/toolkit     5	   	     � Title$  WindowGraph��5   	   $	��450��6   $	��800��6*   ���#1a1a1a � WindowStyle$
 None=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � AllowsTransparency$
 True��5   	    � 
ResizeMode$ CanResizeWithGrip=�5   	    � 	Resources 5         
ChartsType S� 
MethodName S� 
ObjectTypeU	 QSystem.Runtime, Version=4.2.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a 	 System.Enum S� MethodParameters	 M� TypeName  
ChartsAxis  ChartsDatas  ChartsDisplayInterval��%�  &	        &	 m     &	 �     &	 Z    S� 5   
   $ 	GetValues��60   "  5       5      M� 5      $	 local:UIType��6   5      S� 5   
   $ 	GetValues��60   "  5       5      M� 5      $	 local:ChartsAxisData��6   5      S� 5   
   $ 	GetValues��61   "  5       5       M� 5!      $	 local:ChartsDisplayData��6   5#      S� 5$   
   $ 	GetValues��6;   "  5%       5&      M� 5'      $!	 local:ChartsDisplayInterval��6   5)      5*      .��5+      � -   5        
 �Name$
 
grdContent��5+      $��Stretch=�5,      $��Stretch=�5-       � ColumnDefinitions 5.   
   �� 5/      $	��25* �6   $��60��6+   �� 50      $	��25* �6   $��60��6+   �� 51      $	��25* �6   $��60��6+   �� 52      $	��25* �6   $��60��6+    � RowDefinitions 54   
   �� 55      $��5* �6   $��25��6(   $��25��67   �� 56      $��5* �6   $��30��6(   �� 57      $��5* �6   $��30��6(   �� 58      $��5* �6   $��30��6(   �� 59      $��5* �6   $��30��6(   �� 5:      $��5* �6   $��30��6(   �� 5;      $��5* �6   $��30��6(   �� 5<      $��5* �6   $��30��6(   �� 5=      $��5* �6   $��30��6(   �� 5>      $��5* �6   $��30��6(   �� 5?      $��5* �6   $��30��6(   �� 5@      $	��37* �6   $��50��6)   �� 5A      $��8* �6   $��25��6(   $��25��67   .+�5C   
   �� $��4��6     TitleBar#��[ 6C   ��6/   �� 6C   A� 5E   
   -   5         A�Name$ ugrGraph��5E      $��2��5F      $��2��5G      $��1��5H      $��8��5I      �� 5K   
   $��1��6   $��0��5L        foreGroundLight#��[ 5M      .��5N      Type de graphique :6&   �� 5O   
   -   5         ��Name$ cbxGraphType��5O       �� SelectedIndex$ 0��5P      $��1��5R      $��1��5S      $��0,2,0,2q�5U      ��5T      ��  �� Source# [  5U      �� 5X   
   $��2��6   $��0��5Y      #��[ 5Z      .��5[      Valeur sur l'axe X :6'   �� 5\   
   -   5        $ 	cbxValueX��5\      $ 0��5]      
 � Tag$ X��5_      $��2��5`      $��1��5a      $��0,2,0,2q�5c      ��5b      �� # [ 5c      �� 5f   
   $��3��6   $��0��5g      #��[ 5h      .��5i      Valeur sur l'axe Y :6'   �� 5j   
   -   5        $ 	cbxValueY��5j      $ 1��5k      $ Y��5m      $��3��5n      $��1��5o      $��0,2,0,2q�5q      ��5p      �� # [ 5q      �� 5t   
   $��4��6   $��0��5u      #��[ 5v      .��5w      Quantité de données :6(   �� 5x   
   -   5        $ cbxQuantityOfCurves��5x      $��4��5z      $��1��5{      $��0,2,0,2q�5|      �� 5   
   $��5��6   $��0��5�      #��[ 5�      .��5�      
Données :6   �� 5�   
   -   5         ��Name$ btnSave��5�      $
��Save��5�        smallButton#��[ 5�      $��12��5�      $��2��5�      �� 5�   
   -   5        $ 	btnCancel��5�      $��cancel��5�      #��[ 5�      $��12��5�      $��3��5�      5�      5�      