   M S B A M L   `   `   `  ����  InformaticaIndustrial5    /Microsoft.Samples.Kinect.DepthBasics.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Detector de escalera��5   	   $	��735��6&   $
��1334��63    � WindowStyle$ 
ToolWindow=�6p    � WindowStartupLocation$ CenterScreen=�6�    � Topmost .  � 	Resources 5         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  SnapshotButton	 �� 
TargetType
 �� Orientation ��Name �� TextAlignment ��Name �� Triggers P� Property P� Value �� 
TargetName  SquareCheckBox �Name��%w
  &	        &	 #     &	 F     '�� i     &	 �     7  7  &	 �    7  7  �� 5   
   $ 	#ff6e6e6e��62   �� 5   
   $ 	#ff52318f��64   �� 5	   
   $ 	#ff00BCF2��62   �� 5
   
   " ��6   .�5      ��  �@J�SnapsToDevicePixels . 64   5      �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      "	 ��6&   .R�5      � .+�5      �� $
 
Horizontal=�6)   
������ .+�5   "   �� $ SnapText��6,   #��z��6>   $
 Left=�6_   $��Center=�6u   8��  $��15(�6�   � 5   "   $��9,0,0,0q�6'   .+�5   &   �� $ 
SnapNormal��6,   $��Images\SnapNormal.png��6@   $
��None=�6_   $��Center=�6n   �� 5   &   $ 	SnapHover��6,   $��Images\SnapHover.png��6?   $
��None=�6]   $��Center=�6l   $y�	Collapsed=�6�   5   #   5      5       5      P� 5       �@J�IsMouseOver . . �5   "   �� $ 
SnapNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5   "   $ 	SnapHover��6O    � y�$ Visible=�6?   6O   �� 5   "   8  $ SnapText��6h    � ��5      5!      5#      5$      �� 5%   
   " ��6   .�5&      ��  � �� 5'      �� 5(      "	 ��6&   .R�5)      � .+�5*      �� $
 
Horizontal=�6)   
������ .+�5+   "   � $ SquareCheckBoxChecked��6'   .+�5,   &   �� $ CheckedNormal��6,   $��Images\CheckedNormal.png��6C   $
��None=�6e   $��Center=�6t   �� 5-   &   $ CheckedHover��6,   $��Images\CheckedHover.png��6B   $
��None=�6c   $��Center=�6r   $y�	Collapsed=�6�   5.   #   � 5/   "   $ SquareCheckBoxUnchecked��6'   $y�	Collapsed=�6H   .+�50   &   �� $ UncheckedNormal��6,   $ ��Images\UncheckedNormal.png��6E   $
��None=�6i   $��Center=�6x   �� 51   &   $ UncheckedHover��6,   $��Images\UncheckedHover.png��6D   $
��None=�6g   $��Center=�6v   $y�	Collapsed=�6�   52   #   �� 53   "   $ SquareCheckBoxText��6,   #��z��6H   $
 Left=�6i   $��Center=�6   8��  $��15(�6�   $��9,0,0,0q�6�   54      55       56      P� 57       �@d�	IsChecked$ falseY�6;   . �58   "   �� $ SquareCheckBoxChecked��6Q    � y�$ 	Collapsed=�6?   6Q   �� 59   "   $ SquareCheckBoxUnchecked��6O    � y�$ Visible=�6?   6O   5:      P� 5;       �@J�IsMouseOver . . �5<   "   �� $ CheckedNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5=   "   $ UncheckedNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5>   "   $ CheckedHover��6O    � y�$ Visible=�6?   6O   �� 5?   "   $ UncheckedHover��6O    � y�$ Visible=�6?   6O   �� 5@   "   8  $ SquareCheckBoxText��6h    � ��5A      5C      5E      5F      5G      .��5I      � -   5        $��
layoutGrid��5I      $��	10 0 10 0q�6    � RowDefinitions 5J   
   �� 5K      $
��Auto �6   �� 5L      $��* �6   �� 5M      $
��Auto �6   �� 5N      $
��Auto �6   .+�5P   
   ]� $��0��6   $��0 0 0 20q�6!   .+�5Q      �� $
��Left=�6   $��Images\Logo.png��6*   $
��Fill=�6C   $��32��6R   $��81��6^   $��0 10 0 5q�6i   �� 5R      $��Right=�6   $��0 0 -1 0q�6/   $��Bottom=�6A   #��[  6\   $��Segoe UI)�6�   $��18(�6�   Depth Basics6�   �� 5S      $��1��6   $��Images\Status.png��6$   $
��None=�6?   $��Center=�6N   $��0 0 0 5q�6k   5T      (� 5U   
   $��1��6    (� Stretch$ Uniform=�6   $��Center=�61   �� 5V   
   -   5        $��
screenShot��5V   �   $��2��6   #��[ 6   $��
Screenshot��6F   $
��Auto��6[   $��Right=�6i   $��Center=�6�   $��0,10,924,10q�6�   6�   �� 5W   
   -   5        $��	statusBar��5W   ?   $��3��6   $��Stretch=�6!   $��Bottom=�6P   
�������#��[  6~   .@�5X      �� $��0 0 0 10q�6   5Y      �� 5Z   
   -   5         ��Name$ filtroCanny��5Z      $��Canny��6&   $
��Left=�66   $��
808,10,0,0q�6Q   $��2��6e   $��Center=�6r   $	��102��6�   $��32��6�   �� 5[   
   -   5        $ Video��5[      $��8,24,658,55q�6   $��1��64   �� 5\   
   -   5        $ Imagen��5\      $��658,24,10,55q�6    $��1��66   �� 5]   
   -   5        $ filtroSobel��5]      $��Sobel��6&   $
��Left=�66   $��
975,10,0,0q�6Q   $��2��6e   $��Center=�6r   $	��102��6�   $��32��6�   �� 5^   
   -	   5        $ detectorCuadrilateros��5^      $��Detectar Cuadrilateros��60   $
��Left=�6Q   $��1143,15,0,5q�6l   $��2��6�   $��Center=�6�   $	��133��6�   $��32��6�   �� 5_   
   -
   5        $ Panel_de_control��5_      $��Panel de control��6+   $
��Left=�6F   $��
33,10,0,10q�6a   $��2��6u   $��Center=�6�   $	��102��6�   $��32��6�   �� 5`   
   -   5        $ iniciarProcesamiento��5`      $��Iniciar procesamiento��6/   $
��Left=�6O   $��
444,10,0,0q�6j   $��2��6~   $	��Top=�6�   $	��130��6�   $��32��6�   $y�Hidden=�6�   �� 5a   
   -   5         ��Name$ comboBoxEfectos��5a      $
��Left=�6,   $��
636,10,0,0q�6G   $��2��6[   $��Center=�6h   $	��120��6�   $��32��6�   .@�5b      �� $��BITONAL��6   �� 5c      $��GAMMA��6   �� 5d      $��GRISES��6   �� 5e      $��	CONTRASTE��6   5f      �� 5g   
   -   5         ��Name$ profundidad��5g      $��Profundidad��6(   $
��Left=�6>   $��
164,19,0,0q�6Y   $��2��6m   $	��Top=�6z   $��19��6�   $��90��6�   6�   5h      5i      