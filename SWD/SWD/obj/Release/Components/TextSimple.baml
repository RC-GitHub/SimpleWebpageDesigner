   M S B A M L   `   `   `  ���� >  :SWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:SWD.ComponentsSWD.Components      SWD.Components.TextSimple   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .localclr-namespace:SWD.Components       N� Title$  
TextSimple�� � 	Resources    ImageButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� RelativeSource �� Path	 �� Triggers
 P� Property P� Value �� 
TargetName  ImageMarginButtonStyle  ImageToggleButtonStyle   ImageToggleMarginButtonStyle��%  &	        &	 �     &	 �    &	 j    �� " ��.���  � �� �� " ��.R��� $ imageBorder��
������ ._��� $��0q�$��25��$��25������  �� *TemplatedParent+$	 Tag�	 P� 
 �@J�IsMouseOver . . ��� $ imageBorder�� � �� �	#20000000�� " ��.���  � �� �� " ��.R��� $ imageBorder��
������ ._��� $��0q�$��29��$��29������  �� *TemplatedParent+$	 Tag�	 P� 
 �@J�IsMouseOver . . ��� $ imageBorder�� � �� �	#20000000�� " d�.���  � �� �� " d�.R��� $ 
baseBorder��$��2q�#��z��
������ ._��� $��0q�$��25��$��25������  �� *TemplatedParent+$	 Tag�	 P� 
 �@J�IsMouseOver . . ��� $ 
baseBorder�� � �� �	#20000000P� 
 �@d�	IsChecked$
 TrueY�. ���  � ��
 �   �P� 
 �@d�	IsChecked$ FalseY�. ���  � ��
 ���� �� " d�.���  � �� �� " d�.R��� $ marginBorder��$��2q�#��z��
������ ._��� $��0,1,0,0q�$��25��$��25������  �� *TemplatedParent+$	 Tag�	 P� 
 �@J�IsMouseOver . . ��� $ marginBorder�� � �� �	#20000000P� 
 �@d�	IsChecked$
 TrueY�. ���  � ��
 �   �P� 
 �@d�	IsChecked$ FalseY�. ���  � ��
 ���� .��]� $��10q�.+�c� $	��Top=����#EEE$��40��.@���  �� Orientation$ 
Horizontal=�$	��5,0q�.+��� -    ��Name$ 	togBtnCut��#��[  
 � Tag$ ../Icons/cut.png��$��25��$��25��$��0,0,5,0q��� -   $ 
togBtnCopy��#��[  $ ../Icons/copy.png��$��25��$��25��$��0,0,5,0q��� -   $ togBtnPaste��#��[ $ ../Icons/paste.png��$��28��$��28��$��0,0,0,0q��� �� $ 
Horizontal=�$	��5,0q�.+��� -   $ 
togBtnUndo��#��[  $ ../Icons/undo.png��$��25��$��25��$��0,0,5,0q��� -   $ 
togBtnRedo��#��[  $ ../Icons/redo.png��$��25��$��25��$��0,0,0,0q��� �� $ 
Horizontal=�$	��5,0q�.+�d� -    d�Name$ 
togBtnBold��#��[ $ ../Icons/bold.png��$��30��$��30��$��0,0,5,0q�d� -   $ togBtnItalic��#��[ $ ../Icons/italic.png��$��30��$��30��$��0,0,5,0q�d� -   $ togBtnUnderline��#��[ $ ../Icons/underline.png��$��30��$��30��$��0,0,5,0q��� -	   #��[  $ ../Icons/font-color.png��$��25��$��25��$��0,0,5,0q��� �� $ 
Horizontal=�$	��5,0q�.+�d� -
   $ 
togBtnLeft��#��[ $ ../Icons/left.png��$��30��$��30��$��0,0,5,0q�d� -   $ togBtnCenter��#��[ $ ../Icons/center.png��$��30��$��30��$��0,0,5,0q�d� -   $ togBtnRight��#��[ $ ../Icons/right.png��$��30��$��30��$��0,0,5,0q�d� -   $ togBtnJustify��#��[ $ ../Icons/justify.png��$��30��$��30��$��0,0,0,0q��� �� $ 
Horizontal=�$	��5,0q�.+�d� -   $ 	togBtnTop��#��[ $ ../Icons/top.png��$��30��$��30��$��0,0,5,0q�d� -   $ togBtnMiddle��#��[ $ ../Icons/middle.png��$��30��$��30��$��0,0,5,0q�d� -   $ togBtnBottom��#��[ $ ../Icons/bottom.png��$��30��$��30��$��0,0,0,0q��� �� $ 
Horizontal=�$	��5,0q�.+��� -   #��[  $  ../Icons/font-increase.png��$��25��$��25��$��0,0,5,0q��� -   #��[  $  ../Icons/font-decrease.png��$��25��$��25��$��0,0,0,0q��� -    ��Name$ 	txtEditor�� �� AcceptsReturn .  �� 
AcceptsTab . " �� VerticalScrollBarVisibility$
 Auto=�$ �� HorizontalScrollBarVisibility$
 Auto=�$��Stretch=�$��Stretch=� �� TextWrapping$
 Wrap=�$��5q�$��5q����� *	FontStyle+ �� TextDecorations �� *
FontLine+���� *
FontFamily+���� *
FontWeight+���� *
FontSize+���� *Text+ �� Mode$ TwoWay=����� *ForegroundColor+���� *TextHorizontal+���� *TextVertical+