   M S B A M L   `   `   `  ���� >  :SWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:SWD.ComponentsSWD.Components       SWD.Components.ImageSimple   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .localclr-namespace:SWD.Components       N� Title$  Image�� N� 
Background ��  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color �� *BackgroundGradEnd+�  � Offset$ 1Q� �� *BackgroundGradStart+.��� $��10q� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* �.+�� $��0��$��0,0,0,10q� � ColumnDefinitions �� $��0.85* ��� $
��1.5* ��� $
��1.5* �.+��� -   $��Open Dialog��$��0��$��0,0,10,0q�$��40��$��Stretch=��� -    ��Name$ PathInputBox��$��1��$��0,0,10,0q�$��30��{�.  � $��2��$��Center=����� *UnderlayPrimary+ �� $��* ��� $��* ��� $��* ��� $��* �.+��� $��Width:��$��0��$��5q�$��Center=��� -   $ WidthBox��$��30��$��1��$��5q�$��Center=����� 	 �� Path$!	 ComponentContent.ImageWidth�
 �� RelativeSource
 ��  �� AncestorType$
 PageL��� $��Height:��$��5q�$��Center=�$��2���� -   $ 	HeightBox��$��30��$��5q�$��3��$��Center=����� $"	 ComponentContent.ImageHeight�
 �� $
 PageL�� $��1��$��0,0,0,10q����� *UnderlayPrimary+ �� $
��1.5* ��� $��2* ��� $
��1.5* ��� $��2* ��� $
��Auto ��� $
��1.5* ��� $��2* ��� $
��1.5* ��� $��2* �.+��� $��H Align:��$��5q�$��Center=�$��0���� -    ��Name$ 	HAlignBox��$��1��$	��5,0q�$��30��$��Center=� �� SelectedValuePath$ Content�� �� SelectedValue �� $"	 ComponentContent.ImageHAlign�
 �� $
 PageL�.@��� $
��Left���� $��Center���� $��Right���� $��Stretch���� $��V Align:��$��5q�$��Center=�$��2���� -   $ 	VAlignBox��$��3��$	��5,0q�$��30��$ Content�� �� $"	 ComponentContent.ImageVAlign�
 �� $
 PageL�.@��� $	��Top���� $��Center���� $��Bottom���� $��Stretch���� $��4��$��5q� � LayoutTransform ��  �� Angle$ 90Q��� $��Stretch:��$��5q�$��Center=�$��5���� -   $ StretchModeBox��$��6��$	��5,0q�$��30��$ Content�� �� $#	 ComponentContent.ImageStretch�
 �� $
 PageL�.@��� $��Uniform���� $��UniformToFill���� $
��Fill���� $
��None���� $��Filter:��$��5q�$��Center=�$��7���� -   $ 	FilterBox��$��8��$	��5,0q�$��30��$ Content�� �� $"	 ComponentContent.ImageFilter�
 �� $
 PageL�.@��� $
��None���� $
��Blur���� $��Shadow���� $��	Grayscale���� $��2��
�������$��1q����� *UnderlayPrimary+._�� .+��� -	    ��Name$ NoImage��$��Center=�$��Center=�$��90(�
No image�� -
    ��Name$ ImageViewer������ $!	 ComponentContent.ImageWidth�
 �� $
 PageL����� $"	 ComponentContent.ImageHeight�
 �� $
 PageL����� $#	 ComponentContent.ImageStretch�
 �� $
 PageL����� $"	 ComponentContent.ImageHAlign�
 �� $
 PageL����� $"	 ComponentContent.ImageVAlign�
 �� $
 PageL��� -   $��Apply Settings��$��Right=�$��Bottom=�$��10q�$
��10,2q�$	��200��$��45��