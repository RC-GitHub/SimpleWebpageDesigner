   M S B A M L   `   `   `  ���� >  :SWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:SWD.ComponentsSWD.Components  !    SWD.Components.ButtonSimple   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .localclr-namespace:SWD.Components       N� Title$  ButtonSimple�� � 	Resources  �� 
TargetType �� Property �� Value��%�   '��       '�� F     '�� n     �� " ��.���  � �� �� *Font+��  � ��$ Center=��� " ��.���  � ��$ Stretch=��� " ��.���  � ��$ Stretch=� N� 
Background ��  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color �� *BackgroundGradEnd+� 	 � Offset$	 1Q� �� *BackgroundGradStart+.���� $��Center=�.+��� $
��15,0q����� *ButtonSecondary+� $��10q�
 � ColumnDefinitions
 �� $��3* ��� $��2* � � RowDefinitions �� $��* �.+�� $��	15,0,15,0q�$��Center=�
 �� $
��Auto ��� $��* ��� $��75 � �� $
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
��Auto ��� $
��Auto �.+��� $��0��$��Width:��$��5q��� $��0��$��1��$��5,5,0,5q����� *ComponentContent.ButtonWidth+ �� RelativeSource ��  �� AncestorType$
 PageL��� $��0��$��2��$��5q�$��Center=� �� SelectedValuePath$ Content�� �� SelectedValue ��  �� Path$&  ComponentContent.ButtonWidthUnit� �� $
 PageL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $��1��$��Height:��$��5q��� $��1��$��1��$��5,5,0,5q����� *ComponentContent.ButtonHeight+ �� $
 PageL��� $��1��$��2��$��5q�$��Center=�$ Content�� �� $' !ComponentContent.ButtonHeightUnit� �� $
 PageL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $��2��$��
Font Size:��$��5q��� $��2��$��1��$��5,5,0,5q����� *!ComponentContent.ButtonFontSize+ �� $
 PageL��� $��2��$��2��$��5q�$��Center=�$ Content�� �� $) #ComponentContent.ButtonFontSizeUnit� �� $
 PageL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $��3��$��Font Color:��$��5q��� -    ��Name$ tbFontColor��$��3��$��1��$��5,5,0,5q�{�. ���� *" ComponentContent.ButtonFontColor+ �� $
 PageL� �� Mode$ TwoWay=��� -    ��Name$ btnFontColor��$��3��$��2��$��Stretch=�$��2q�$��30��$��5q�{�. .���� $��../Icons/edit.png���� $��4��$��Background Color:��$��5q��� -   $ tbBackgroundColor��$��4��$��1��$��5,5,0,5q�{�. ���� *(&ComponentContent.ButtonBackgroundColor+ �� $
 PageL�$ TwoWay=��� -   $ btnBackgroundColor��$��4��$��2��$��Stretch=�$��2q�$��30��$��5q�{�. .���� $��../Icons/edit.png���� $��5��$��Border:��$��5q��� $��5��$��1��$��5,5,0,5q����� *ComponentContent.ButtonBorder+ �� $
 PageL�$ TwoWay=��� $��5��$��2��$��5q�$��Center=�$ Content�� �� $' !ComponentContent.ButtonBorderUnit� �� $
 PageL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $��6��$��Border Color:��$��5q��� -   $ tbBorderColor��$��6��$��1��$��5,5,0,5q�{�. ���� *$"ComponentContent.ButtonBorderColor+ �� $
 PageL�$ TwoWay=��� -   $ btnBorderColor��$��6��$��2��$��Stretch=�$��2q�$��30��$��5q�{�. .���� $��../Icons/edit.png���� $��7��$��Border Radius:��$��5q��� $��7��$��1��$��5,5,0,5q����� *%#ComponentContent.ButtonBorderRadius+ �� $
 PageL��� $��7��$��2��$��5q�$��Center=�$ Content�� �� $- 'ComponentContent.ButtonBorderRadiusUnit� �� $
 PageL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $��8��$��Padding:��$��5q��� $��8��$��1��$��5,5,0,5q����� * ComponentContent.ButtonPadding+ �� $
 PageL��� $��8��$��2��$��5q�$��Center=�$ Content�� �� $( "ComponentContent.ButtonPaddingUnit� �� $
 PageL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $��9��$��Margin:��$��5q��� $��9��$��1��$��5,5,0,5q����� *ComponentContent.ButtonMargin+ �� $
 PageL��� $��9��$��2��$��5q�$��Center=�$ Content�� �� $' !ComponentContent.ButtonMarginUnit� �� $
 PageL�.@��� $��px���� $��em���� $	��rem���� $��%���� $��vw���� $��vh���� $��1��$��Center=�$��20q�.+��� $��Imperfect Preview:��$
��Bold"�$��0,0,0,10q��� $��Center=�$��Center=�$��0,0,0,10q�$	��225��$��50������ *(&ComponentContent.ButtonBackgroundColor+ �� $
 PageL����� *$"ComponentContent.ButtonBorderColor+ �� $
 PageL����� *ComponentContent.ButtonBorder+ �� $
 PageL� �� CornerRadius �� *%#ComponentContent.ButtonBorderRadius+ �� $
 PageL� �� Padding �� * ComponentContent.ButtonPadding+ �� $
 PageL�._��� $��Center=�$��Center=����� *ComponentContent.ButtonText+ �� $
 PageL����� *!ComponentContent.ButtonFontSize+ �� $
 PageL����� *" ComponentContent.ButtonFontColor+ �� $
 PageL��� $��Text:��$
��Bold"�$��0,0,0,5q��� $��5,5,5,10q����� *ComponentContent.ButtonText+ �� $
 PageL��� $��Link:��$
��Bold"�$��0,0,0,5q��� $��5,5,5,10q����� *ComponentContent.ButtonLink+ �� $
 PageL��� $��Function Name:��$
��Bold"�$��0,0,0,5q��� $��5,5,5,10q����� *ComponentContent.ButtonFunc+ �� $
 PageL��� $��Predefined Color Style:��$
��Bold"�$��0,0,0,5q��� -    ��Name$ cbPredefinedStyle��$��Center=�$ Content��$��5,5,5,10q� �� $" ComponentContent.ButtonStyle� �� $
 PageL�.@��� $��Custom���� $��Header-Body���� $��Header-Grid���� $��Header-Footer���� $��Body-Header���� $��	Body-Grid���� $��Body-Footer���� $��Grid-Header���� $��	Grid-Body���� $��Grid-Footer���� $��Footer-Header���� $��Footer-Body���� $��Footer-Grid���� $
��15,0q����� *ButtonSecondary+