   M S B A M L   `   `   `  ����K  GCovidPropagation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:CovidPropagationCovidPropagation  5   	   !    CovidPropagation.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:CovidPropagation     5   	   LmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes     5	   	     � Title$  Covid propagation��5   	   $	��555��6#   $	��987��60      MaterialDesignBody#���   5
   	   $��Regular"�5   	   $��13(�5   	   &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     windowBackground#���  5   	     MaterialDesignFont#���  5   	    � WindowStyle$
 None=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � AllowsTransparency$
 True��5   	    � 
ResizeMode$ CanResizeWithGrip=�5   	   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��13* �6   $	��150��6+   $��50��6:   �� 5      $	��45* �6   �� 5      $	��42* �6    � RowDefinitions 5   
   �� 5      $��5* �6   $��25��6(   $��25��67   �� 5      $	��24* �6   $	��150��6)   $��50��69   �� 5      $	��24* �6   $	��150��6)   $��50��69   �� 5      $	��24* �6   $	��150��6)   $��50��69   �� 5       $	��24* �6   �� 5!      $��3* �6   $��13��6(   $��13��67   .+�5%   
   �� -   5        $��
windowName��5%   ?   $
{�True��6   $�� Covid propagation��6%   6?   �� 5&   
   $��3��6     TitleBar#��[ 6d   ��6/   �� 	 �� ElementName$	 
windowName��
 �� Path$

 Text�6d   �� 5)   
     normalGreen#��[ 6   $��1��6>   $��5��6K   �� 5*   
   -   5         ��Name$ btnSimulation��5*       
 button#��[ 6E     play#��[ 6e   $��0��6�   $��1��6�   �� 5+   
   -   5        $ btnGraphicSettings��5+      #��[ 6O    	 tuneV#��[ 6o   $��0��6�   $��2��6�   �� 5,   
   -   5        $ btnSettings��5,      #��[ 6A     settings#��[ 6a   $��0��6�   $��3��6�    � 5/   
   -   5          �Name$ MainContent��5/   S   $��1��6   $��2��6!   $��1��65   $��4��6B   $��Stretch=�6h   $��Center=�50      $��Stretch=�63   $��Center=�51        � Source$ PageSimulation.xaml?�65   52      53      