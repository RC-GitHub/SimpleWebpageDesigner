   M S B A M L   `   `   `  ���� >  :SWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:SWD.ComponentsSWD.Components  $    SWD.Components.ComponentWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:SWD.Components     $	��850��$
��1400��  � Title$  ComponentWindow��$	��850��$
��1400�� � 	Resources  �� 
TargetType �� Property �� Value   Heading �� 
StartPoint �� EndPoint � Offset � Color  	BoolToVis(   "SWD.Components.CapitalizeConverter  CapitalizeConverter  LowercaseConverter	   ToLower��%  '��       '�� '     '�� S     '�� �     &	  �     '�� �    &	 �    &	 �    &	 �    �� " ��.���  � ��$ Center=��� " ��.���  � �� �� *Font+�� " ��.���  � �� �� *Font+��  � �� �� *UnderlaySecondary+�� " ��.���  � �� �� *Font+�� " ��.���  � �� �� *
HeaderFont+��  � ��$ Normal"���  � ��$ 0,0,0,0q���  � ��$ Century Gothic)���  � ��$ 22(���  � ��$ DemiBold"���  � �� �� *
HeaderFont+��  � ��$ Stretch=���  � ��$ Center=���  � ��$ Center=���  � ��$ Center=���  � �� �� *ButtonSecondary+��  � ��$ 2q���  � �� �� $	 0,0&�$	 0,1&�.��� $ 0Q� �� *DecorationPrimary+� $ 1Q� �� *DecorationSecondary+�� " ��.���  � ��$ 60����  � ��$ 5q���  � ��$ 28����  � �� �� *ButtonSecondary+��  � ��$ 2����  � ��$ Center=���  � ��$ Stretch=���     	 . ���� $ 0.5,1&�$ 0.5,0&�.���  �� *BackgroundGradEnd+� $ 1Q� �� *BackgroundGradStart+.��� 
 � ColumnDefinitions
 �� $
��1.5* ��� $
��3.5* � � RowDefinitions �� $	��250 ��� $��* �.+��� $��2��$��2��y��� *BackgroundImageOn+ �� 	Converter# [ ���� *BackgroundImageAlignX+���� *BackgroundImageAlignY+ J� Opacity �� *BackgroundImageOpacity+���� *BackgroundImageStretch+���� *BackgroundImageFullPath+�� $��2��$��2��$��Stretch=�$��Stretch=�y��� *BackgroundImageOn+# [ ���� *BackgroundImageOverlay+���� " ���� $��0��$��0��$��15q�$��Center=����� *UnderlayPrimary+.+�� $��
15,20,15,0q�.+��� $��Name:��$
��Bold"�$
��Left=�$��90���� -   $��ComponentNameBox��$��Right=�$	��200������ *MyComponent.Name+ �� RelativeSource ��  �� AncestorType$ WindowL�� $��15,12.5,15,7.5q� �� $��* ��� $��* ��� $��* �
 �� $��* ��� $
��Auto ��� $��30 ��� $
��Auto ��� $
��Auto �.+��� $��Colors:��$
��Bold"�$��0���� $��Normal��$
��Bold"�$��0��$��1��$
��10,0q��� $��Selected��$
��Bold"�$��0��$��3��$
��10,0q��� $��Background:��$��1��$��0��$��Center=�$��0,0,0,0q��� -    ��Name$ lblBackground��$��1��$��1��$��Center=�$��Stretch=�$��Center=�$��5q�.��X�� -    ��Name$ rectBackground��$��1��$��1������ *MyComponent.BackgroundColor+ �� $ WindowL�� -    �Name$ popBackground��$��Bottom=� � 	StaysOpen .  � AllowsTransparency . ��.   � PlacementTarget ��  �� ElementName$ rectBackground��.����  �� Padding$ 5q� �� CornerRadius$ 4������ *BackgroundGradStart+._��� -    ��Name$ slidBackground�� � Minimum$ 0Q� � Maximum$	 255Q�$	��120���� $	��→��$��1��$��2��$��Center=�$��Center=��� -   $ lblSelectedBackground��$��1��$��3��$��Center=�$��Stretch=�$��Center=�$��5q�.��X�� -   $ rectSelectedBackground��$��1��$��3������ *%#MyComponent.SelectedBackgroundColor+ �� $ WindowL�� -   $ popSelectedBackground��$��Bottom=� .  . ��.   �� $ rectSelectedBackground��.���� $ 5q�$ 4������ *BackgroundGradStart+._��� -	   $ slidSelectedBackground��$ 0Q�$	 255Q�$	��120���� $��Border:��$��2��$��0��$��Center=�$��0,0,0,0q��� -
   $ 	lblBorder��$��2��$��1��$��Center=�$��Stretch=�$��Center=�$��5q�.��X�� -   $ 
rectBorder��$��2��$��1������ *MyComponent.BorderColor+ �� $ WindowL�� -   $ 	popBorder��$��Bottom=� .  . ��.   �� $ 
rectBorder��.���� $ 5q�$ 4������ *BackgroundGradStart+._��� -   $ 
slidBorder��$ 0Q�$	 255Q�$	��120���� $	��→��$��2��$��2��$��Center=�$��Center=��� -   $ lblSelectedBorder��$��2��$��3��$��Center=�$��Stretch=�$��Center=�$��5q�.��X�� -   $ rectSelectedBorder��$��2��$��3������ *!MyComponent.SelectedBorderColor+ �� $ WindowL�� -   $ popSelectedBorder��$��Bottom=� .  . ��.   �� $ rectSelectedBorder��.���� $ 5q�$ 4������ *BackgroundGradStart+._��� -   $ slidSelectedBorder��$ 0Q�$	 255Q�$	��120���� -    ��Name$ btnSave��$��Center=�$��Stretch=�$��
15,0,15,15q�$��0q�.��Save� $��0��$��1��$��
5,10,10,20q�$��Stretch=�$��Stretch=�
 �� $
��3.5* ��� $��2* � �� $
��2.5* ��� $
��3.5* �.+�� -   $ popSize��$��Bottom=� .  .  �� $ 
SizeHeader��.���� $ 5q�$ 4��$��1q����� *HeaderPrimary+���� *HeaderSecondary+._��� $���zValues of 0 represent default size. If you want to make a component not visible, change its opacity in the styles section.��$	��400��$
��Wrap=����� *
HeaderFont+�� $��0��$��0��$��2��$
��10,0q�$��0,0,10,0q����� -    ��Name$ 
SizeHeader��$��16(��� $
�Size�� �� $�*��
���  ��.��� 
 �� $��* ��� $��0.65* ��� $��0.55* ��� $��* ��� $��0.65* ��� $��0.55* � �� $��* ��� $��* ��� $��* �.+��� $��Width:��$
��Bold"�$��0��$��0��$��Center=�$��	10,0,10,0q��� -   $��WidthBox��$��1��$��0��$��
10,10,0,10q����� *MyComponent.CompStyle.Width+ �� $ WindowL��� -   $��WidthComboBox��$��2��$��0��$��Stretch=�$��Center=�$��Stretch=�$��
0,10,10,10q� �� SelectedValuePath$ Content�� �� SelectedValue �� *!MyComponent.CompStyle.WidthUnit+ �� $ WindowL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $��Height:��$
��Bold"�$��3��$��0��$��Center=�$��	10,0,10,0q��� -   $��	HeightBox��$��4��$��0��$��
10,10,0,10q����� *MyComponent.CompStyle.Height+ �� $ WindowL��� -   $��HeightComboBox��$��5��$��0��$��Stretch=�$��Center=�$��Stretch=�$��
0,10,10,10q�$ Content�� �� *" MyComponent.CompStyle.HeightUnit+ �� $ WindowL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $��
Min Width:��$
��Bold"�$��0��$��1��$��Center=�$��	10,0,10,0q��� -   $��MinWidthBox��$��1��$��1��$��
10,10,0,10q����� * MyComponent.CompStyle.MinWidth+ �� $ WindowL��� -   $��MinWidthComboBox��$��2��$��1��$��Stretch=�$��Center=�$��Stretch=�$��
0,10,10,10q�$ Content�� �� *$"MyComponent.CompStyle.MinWidthUnit+ �� $ WindowL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $��Min Height:��$
��Bold"�$��3��$��1��$��Center=�$��	10,0,10,0q��� -   $��MinHeightBox��$��4��$��1��$��
10,10,0,10q����� *!MyComponent.CompStyle.MinHeight+ �� $ WindowL��� -   $��MinHeightComboBox��$��5��$��1��$��Stretch=�$��Center=�$��Stretch=�$��
0,10,10,10q�$ Content�� �� *%#MyComponent.CompStyle.MinHeightUnit+ �� $ WindowL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $��
Max Width:��$
��Bold"�$��0��$��2��$��Center=�$��	10,0,10,0q��� -   $��MaxWidthBox��$��1��$��2��$��
10,10,0,10q����� * MyComponent.CompStyle.MaxWidth+ �� $ WindowL��� -   $��MaxWidthComboBox��$��2��$��2��$��Stretch=�$��Center=�$��Stretch=�$��
0,10,10,10q�$ Content�� �� *$"MyComponent.CompStyle.MaxWidthUnit+ �� $ WindowL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $��Max Height:��$
��Bold"�$��3��$��2��$��Center=�$��	10,0,10,0q��� -   $��MaxHeightBox��$��4��$��2��$��
10,10,0,10q����� *!MyComponent.CompStyle.MaxHeight+ �� $ WindowL��� -    $��MaxHeightComboBox��$��5��$��2��$��Stretch=�$��Center=�$��Stretch=�$��
0,10,10,10q�$ Content�� �� *%#MyComponent.CompStyle.MaxHeightUnit+ �� $ WindowL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $��	Alignment��$��1��$��1��$
��10,0q�$��	10,0,10,0q�.��� 
 �� $��* ��� $��* � �� $��* ��� $��* �.+��� $��H Align:��$
��Bold"�$��0��$��0��$��Center=�$��	10,10,5,5q��� -!   $��VerticalAlignComboBox��$��1��$��0��$��2��$��Center=�$��
10,10,10,5q�$ Content�� �� *MyComponent.CompStyle.Justify+ �� $ WindowL�.@��� $��Center���� $��Start���� $	��End���� $��V Align:��$
��Bold"�$��0��$��1��$��Center=�$��10,0,5,5q��� -"   $��HorizontalAlignComboBox��$��1��$��1��$��Center=�$��
10,5,10,10q�$ Content�� �� *" MyComponent.CompStyle.AlignItems+ �� $ WindowL�.@��� $��Center���� $��Start���� $	��End���� $��Component Type��$��
10,0,10,10q�$��0��$��1��.���� -#   $��ComponentTypeComboBox��$��Center=�$��15,10q�$ Content�� �� *MyComponent.Type+ �� $ WindowL�.@��� $
��text���� $��button���� $��image���� $
��code���� $��1��$��0��$��10q�$
��Auto=�.���� $
��0,10q�.+��� $��Padding / Margin��$��10,30,10,10q�.��� $��
10,10,10,5q� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �
 �� $��* ��� $��* ��� $��* ��� $��* �.+��� $��Padding:��$
��Bold"�$��0��$��0��$��2��$��	0,2.5,0,5q�$��Center=��� -$   $��PaddingTypeComboBox��$��0��$��3��$��Stretch=�$��Center=�$��0,0,0,5q�$ Content�� �� *#!MyComponent.CompStyle.PaddingUnit+ �� $ WindowL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $
��Left��$��1��$��0��$��Center=��� -%   $��PaddingLeftBox��$��2��$��0��$��2q����� *#!MyComponent.CompStyle.PaddingLeft+ �� $ WindowL��� $	��Top��$��1��$��1��$��Center=��� -&   $��PaddingTopBox��$��2��$��1��$��2q����� *" MyComponent.CompStyle.PaddingTop+ �� $ WindowL��� $��Right��$��1��$��2��$��Center=��� -'   $��PaddingRightBox��$��2��$��2��$��2q����� *$"MyComponent.CompStyle.PaddingRight+ �� $ WindowL��� $��Bottom��$��1��$��3��$��Center=��� -(   $��PaddingBottomBox��$��2��$��3��$��2q����� *%#MyComponent.CompStyle.PaddingBottom+ �� $ WindowL��� $��Margin:��$
��Bold"�$��3��$��0��$��0,15,0,5q�$��Center=��� -)   $��MarginTypeComboBox��$��3��$��3��$��Stretch=�$��Center=�$��0,15,0,5q�$ Content�� �� *" MyComponent.CompStyle.MarginUnit+ �� $ WindowL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $
��Left��$��4��$��0��$��Center=��� -*   $��MarginLeftBox��$��5��$��0��$��2q����� *" MyComponent.CompStyle.MarginLeft+ �� $ WindowL��� $	��Top��$��4��$��1��$��Center=��� -+   $��MarginTopBox��$��5��$��1��$��2q����� *!MyComponent.CompStyle.MarginTop+ �� $ WindowL��� $��Right��$��4��$��2��$��Center=��� -,   $��MarginRightBox��$��5��$��2��$��2q����� *#!MyComponent.CompStyle.MarginRight+ �� $ WindowL��� $��Bottom��$��4��$��3��$��Center=��� --   $��MarginBottomBox��$��5��$��3��$��2q����� *$"MyComponent.CompStyle.MarginBottom+ �� $ WindowL��� $��Border:��$
��Bold"�$��6��$��0��$��3��$��0,15,0,5q��� $��
Thickness:��$��7��$��0��$��2��$��0,5,0,5q�$��Center=��� -.   $��BorderThicknessBox��$��7��$��2��$��1��$��0,0,5,0q�$��25������ *'%MyComponent.CompStyle.BorderThickness+ �� $ WindowL��� -/   $��BorderThicknessComboBox��$��7��$��3��$��Stretch=�$��Center=�$��0,5,0,5q�$ Content�� �� *+)MyComponent.CompStyle.BorderThicknessUnit+ �� $ WindowL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $��Radius:��$��8��$��0��$��2��$��0,5,0,5q�$��Center=��� -0   $��BorderRadiusBox��$��8��$��2��$��1��$��0,0,5,0q�$��25������ *$"MyComponent.CompStyle.BorderRadius+ �� $ WindowL��� -1   $��BorderRadiusComboBox��$��8��$��3��$��Stretch=�$��Center=�$��0,5,0,5q�$ Content�� �� *(&MyComponent.CompStyle.BorderRadiusUnit+ �� $ WindowL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $��Color:��$��9��$��0��$��2��$��0,5,0,15q�$��Center=��� -2   $ lblBorderColor��$��9��$��2��$��2��$��Center=�$��Stretch=�$��Center=�$��0,-5,0,0q�.��X�� -3   $ rectBorderColor��$��9��$��2��$��2��$
��Auto��$��Stretch=�$��0,0,0,5q����� *#!MyComponent.CompStyle.BorderColor+ �� $ WindowL�� -4   $ popBorderColor��$��Bottom=� .  . ��.   �� $ rectBorderColor��.���� $ 5q�$ 4������ *BackgroundGradStart+._��� -5   $ slidBorderColor��$ 0Q�$	 255Q�$	��120���� $!��Background Gradient / Image��$��
10,0,10,10q�.���� $��10,10,10,15q�.+��� $��Gradient Type:��$
��Bold"�$��0,0,0,5q�� $��0,0,0,10q�
 �� $��* ��� $��* �.+��� $��Type:��$��Center=�$��0,0,10,0q��� -6   $��GradientTypeComboBox��$��1��$��Stretch=�$��0,0,0,5q�$ Content�� �� *$"MyComponent.CompStyle.GradientType+ �� $ WindowL�.@��� $��Linear���� $��Radial���� $��Gradient Stops:��$
��Bold"�$��0,0,0,5q�� $��0,0,0,10q� �� $
��Auto ��� $
��Auto �
 �� $��2.25* ��� $��* ��� $��1.25* �.+��� $��Start:��$��0��$��0��$��Center=�$��0,0,0,5q��� -7   $ lblGradientStart��$��0��$��1��$��Center=�$��Stretch=�$��Center=�$��0,-7,0,0q�.��X�� -8   $ rectGradientStart��$��0��$��1��$
��Auto��$��0,0,0,5q����� *%#MyComponent.CompStyle.GradientStart+ �� $ WindowL�� -9   $ popGradientStart��$��Bottom=� .  . ��.   �� $ rectGradientStart��.���� $ 5q�$ 4������ *BackgroundGradStart+._��� -:   $ slidGradientStart��$ 0Q�$	 255Q�$	��120���� $��0��$��2��  �� Orientation$  
Horizontal=�$��Right=�$��0,0,0,5q�.+��� -;   $��Color1Percent��$��50��$��5q�$��Right=����� *,*MyComponent.CompStyle.GradientStartPercent+ �� $ WindowL��� $��%��$��Center=��� $
��End:��$��1��$��0��$��Center=�$��0q��� -<   $ lblGradientEnd��$��1��$��1��$��Center=�$��Stretch=�$��Center=�$��0,0,0,0q�.��X�� -=   $ rectGradientEnd��$��1��$��1��$
��Auto��$��0q����� *#!MyComponent.CompStyle.GradientEnd+ �� $ WindowL�� ->   $ popGradientEnd��$��Bottom=� .  . ��.   �� $ rectGradientEnd��.���� $ 5q�$ 4������ *BackgroundGradStart+._��� -?   $ slidGradientEnd��$ 0Q�$	 255Q�$	��120���� $��1��$��2��$  
Horizontal=�$��Right=�$��0q�.+��� -@   $��Color2Percent��$��50��$��5q�$��Right=����� **(MyComponent.CompStyle.GradientEndPercent+ �� $ WindowL��� $��%��$��Center=��� $��0,5,0,15q����� *ButtonSecondary+�� -A   $ ��UseBackgroundImageCheckBox��$��12(�$��Use Background Image��$��0,0,0,10q�! d� 	IsChecked! �� **(MyComponent.CompStyle.UseBackgroundImage+ �� $ WindowL��� $��Background Image:��$
��Bold"�$��0,0,0,5q�� $��0,0,0,10q�
 �� $��* ��� $
��Auto �.+��� -B   $��tbBackgroundImage��$��30��" �� 
IsReadOnly" . $��0������ *'%MyComponent.CompStyle.BackgroundImage+ �� $ WindowL��� -C   $ btnBackgroundImage��$��5,0,0,0q�$��30��$��2q�$��1��.���� $��../Icons/edit.png��� $��0,0,0,0q� �� $
��Auto ��� $
��Auto �
 �� $��* ��� $��* �.+��� $��
Alignment:��$��0��$��0��$��Center=�$��0,0,10,5q�$��25���� -D   # ��Name$# ImageAlignmentComboBox��$��0��$��1��$��0,0,0,5q�$��Stretch=�$ Content�� �� *0.MyComponent.CompStyle.BackgroundImageAlignment+ �� $ WindowL�.@��� $��Center���� $
��Left���� $��Right���� $��Stretch���� $��Stretch:��$��1��$��0��$��Center=�$��0,0,10,0q��� -E   $��ImageStretchComboBox��$ �� SelectedIndex$$ 3��$��1��$��1��$��Stretch=�$ Content�� �� *.,MyComponent.CompStyle.BackgroundImageStretch+ �� $ WindowL�.@��� $
��None���� $
��Fill���� $��Uniform���� $��UniformToFill���� $��Visual��$��1��$��
10,0,10,10q�.��� $��0,0,0,7q�
 �� $��* ��� $��* � �� $��* ��� $��* ��� $��* ��� $��* �.+��� $��Opacity:��$
��Bold"�$��0��$��0��$��Center=�$��
10,10,5,10q��� -F   $��OpacitySlider��$ 0Q�$ 1Q�% � SmallChange$
% 0.01Q�& � LargeChange$	& 0.1Q�' �� TickFrequency$	' 0.1Q�( �� IsSnapToTickEnabled( .  $��1��$��Center=�$��0,5,10,5q�) � Value) �� *MyComponent.CompStyle.Opacity+ �� $ WindowL��� $��	Overflow:��$
��Bold"�$��0��$��1��$��Center=�$��	10,5,10,5q��� -G   $��OverflowComboBox��$��1��$��1��$��0,7,10,7q�$ Content�� �� * MyComponent.CompStyle.Overflow+ �� $ WindowL�.@��� $��Visible���� $��Hidden���� $��Scroll���� $
��Auto���� $��Z-Index:��$
��Bold"�$��0��$��2��$��Center=�$��
10,10,5,10q��� -H   $��	ZIndexBox��$��1��$��2��$��Stretch=�$��0,7,10,7q����� *MyComponent.CompStyle.ZIndex+ �� $ WindowL��� $��Box Shadow:��$
��Bold"�$��0��$��3��$��Center=�$��
10,10,5,10q��� -I   $��BoxShadowBox��$��1��$��3��$��0,7,10,7q�* � ToolTip$1* +Format: h-offset v-offset blur-radius color������ *!MyComponent.CompStyle.BoxShadow+ �� $ WindowL��� #��[  $	��Top=�$
��10,0q�$��1��$��0��$��Styles�� � -J   +  �Name$+ 	frameHost��$��1��$��1��$��Bottom=�$y�Visible=�$��20q�,  � NavigationUIVisibility$, Hidden=����� $ 0.5,1&�$ 0.5,0&�.���  �� *BackgroundGradStart+� $ 1Q� �� *BackgroundGradEnd+