   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  > :SWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:SWDSWD    SWD.ContentWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6sys&clr-namespace:System;assembly=mscorlib     #localclr-namespace:SWD     -     �Name$  	wdContent�� � Title$ ContentWindow��$	��950��$
��1450�� � WindowState$ 	Maximized=� � WindowStyle$ SingleBorderWindow=� � 	Resources  �� 
TargetType �� Property �� Value   Heading �� 
StartPoint	 �� EndPoint
 � Offset � Color  	BoolToVis��%�  '��       '�� L     &	  s     &	 �    �� " ��.���  � �� �� *
HeaderFont+��  � ��$ Normal"��� " ��.���  � ��$ Center=��� " ��.���  � �� �� *
HeaderFont+��  � ��$ Normal"���  � ��$ 0,0,0,0q���  � ��$ Century Gothic)���  � ��$ 22(���  � ��$ DemiBold"���  � �� �� *
HeaderFont+��  � ��$ Stretch=���  � ��$ Center=���  � ��$ Center=���  � ��$ Center=���  � �� �� *ButtonSecondary+��  � ��$ 2q���  � �� �� $	 0,0&�$		 0,1&�.��� $
 0Q� �� *DecorationPrimary+� $
 1Q� �� *DecorationSecondary+�� ���� $	 0.5,1&�$ 0.5,0&�.���  �� *BackgroundGradStart+� $
 1Q� �� *BackgroundGradEnd+.��� -    �Name$ gdContentAndBar��.+��� y��� *BackgroundImageOn+ �� 	Converter# [ ���� *BackgroundImageAlignX+���� *BackgroundImageAlignY+ J� Opacity �� *BackgroundImageOpacity+���� *BackgroundImageStretch+���� *BackgroundImageFullPath+�� $��Stretch=�$��Stretch=�y��� *BackgroundImageOn+# [ ���� *BackgroundImageOverlay+�  � RowDefinitions �� $��35 ��� $��* �.+�`� $	��Top=�.�c� .@��� -    ��Name$ btnNewContext��$��0,0,10,0q� � ContextMenu �� -    ��Name$ newContextMenu��.@�m� -    m�Name$ miNewProject��$��New project�� � ToolTip$  Creates a new SWD Project.���� m� -   $ 	miNewFile��$��New file��$8 2Creates a new file inside the current SWD Project.��m� -   $ miNewFolder��$��
New folder��$: 4Creates a new folder inside the current SWD project.��.����  �� Orientation$ 
Horizontal=�.+��� -    ��Name$ imgNew��$��20��$��20��$��0,0,5,0q��� $��Center=�New�� -	   $ btnOpenContext��$��0,0,10,0q� �� -
   $ openContextMenu��.@�m� -   $ miOpenProject��$��Open project��$6 0Opens a new window with a different SWD Project.���� m� -   $ 
miOpenFile��$��Add file��$< 6Adds an existing JSON file to the current SWD Project.��m� -   $ miOpenFolder��$��Add a folder��$9 3Adds an existing folder to the current SWD Project.��.���� $ 
Horizontal=�.+��� -   $ 	imgImport��$��20��$��20��$��0,0,5,0q��� $��Center=�Import�� -   $ btnTheme��$ Change a theme.��$��0,0,10,0q� �� -   $ editContextMenu��.@�m� -   $ miEditInitial��$��Edit initial settings��$$ Opens initial settings editor.���� m� -   $ miEditTheme��$��
Edit theme��$ Opens theme editor.��.���� $ 
Horizontal=�.+��� -   $ imgEdit��$��20��$��20��$��0,0,5,0q��� $��Center=�Edit�� -   $ btnSave��$ Save current file.��$��0,0,10,0q�.���� $ 
Horizontal=�.+��� -   $ imgSave��$��20��$��20��$��0,0,5,0q��� $��Center=�Save�� -   $ btnClose��$ Close current project.��$��0,0,10,0q�.���� $ 
Horizontal=�.+��� -   $ imgClose��$��20��$��20��$��0,0,5,0q��� $��Center=�Close�� $
��Auto���� -   $ btnBuild��$# Convert project to a webpage!��$��0,0,10,0q�$��Right=�.���� $ 
Horizontal=�.+��� -   $ imgBuild��$��20��$��20��$��0,0,5,0q��� $��Center=�Build�� -   $ 	btnReveal��$# Open current file in browser!��$��0,0,10,0q�$��Right=�.���� $ 
Horizontal=�.+��� -   $ 	imgReveal��$��20��$��20��$��0,0,5,0q��� $��Center=�Reveal� -   $ 	gdContent��$��20q�$��1�� � ColumnDefinitions �� $��4* ��� $	��12* � �� $��* ��� $��4* ��� $��5* ��� $��6* �.+��� $��0��$��0��$��3��$��4q�$��20, 45, 20, 20q� �� Padding$ 10, 15, 10, 5q�$��Stretch=�$��Stretch=����� *ButtonSecondary+���� *UnderlayPrimary+._�� -   $ spFileBrowserPanel�� �� $��50 ��� $��50 ��� $��55 ��� $��15 ��� $��1.25* ��� $��3.25* ��� $��15 ��� $��65 � �� $
��3.5* ��� $��4* �.+��� $��0��$��Center=�$��0,0,0,10q����� *Font+.��	File name�� -    ��Name$ tbFileNameChange��$��0��$��1��$��0,0,0,10q�$��5,0,0,0q�$��Stretch=�$��Stretch=�$��Center=��� $��1��$��Center=�$��0,0,0,10q����� *Font+.��Parent name�� -   $ tbFolderNameChange��$��1��$��1��$��0,0,0,10q�$��5,0,0,0q�$��Stretch=�$��Stretch=�$��Center=�� $��0��$��2��$��2��$��Stretch=�$��Stretch=� �� $��* ��� $��* �.+��� -    $ btnMoveFile��$��0��$��Stretch=�$��Center=�$��Stretch=�$��0,0,5,10q�.��	Move file�� -!   $ btnMoveFolder��$��1��$��Stretch=�$��Center=�$��Stretch=�$��5,0,0,10q�.��Move folder�� $��3��$��2������ *ButtonSecondary+�� -"    ��Name$ lblFS��$��4��$��0��$��2��$��Folders inside: -��$��Stretch=�$��Center=�$��Center=�$��2q�$��0,0,0,0q����� *
HeaderFont+���� *HeaderSecondary+���� *HeaderPrimary+� $��5��$��2��$	��5,0q� �� $��5* ��� $��* ��� $��* � �� $��* ��� $��* ��� $��1.25* �.+��� -#    ��Name$
 lbFS��$��0��$��2��$��3��$��0,0,5,5q��� -$   $ btnInsideFolderAdd��$��2��$��20(�$��35��$��35��$��	ExtraBold"�$��0,0,0,0q�.���� $��+��$��0,-4,0,0q��� -%   $ btnInsideFolderDelete��$��2��$��1��$��20(�$��35��$��35��$��	ExtraBold"�$��0,0,0,0q�.���� $��-��$��0,-4,0,0q��� -&   $ btnFolderRefresh��$��2��$��2��$��20(�$��35��$��35��$��	ExtraBold"�$��0,0,0,0q�.���� -'   $ imgFolderRefresh��$��4��$��2��$��Right=�$	��Top=�$��Icons/refresh.png��$��20��$��20���� $��6��$��2������ *ButtonSecondary+� $��7��$��2��$��5,10,0,5q� �� $��* ��� $��* �.+��� -(   $ btnFileSave��$��Stretch=�$	��Top=�$��45��$��0,0,5,5q�.��
Save names�� -)   $ btnFileDelete��$��1��$��Stretch=�$	��Top=�$��45��$��5,0,0,5q�.��Delete file�� -*   $ lblControlTitle��#��[  $��File and folder control��$��	15,0,15,0q��� $��1��$��0��$��4��$��5,20,5,5q����� $	 0,0&�$		 0,1&�.��� $	
 0.5Q� �� *UnderlayPrimary+� $
 1Q� �� *UnderlaySecondary+�� -+   $ lblGridTitle��#��[  $��0��$��1��$��1��$��
index.html��� -,   $ gdColRow��$��1��$��1��$
��20,0q�$��Center=�$	��175�� �� $��1* ��� $��1* ��� $
��1.5* �.+�� --   $ gdCol��$��0��$��0��$��0,0,20,0q� �� $��* ��� $��* � �� $
��1.5* ��� $��* ��� $��1.25* ����� $	 0,0&�$		 0,1&�.��� $
 0Q� �� *DecorationPrimary+� $
 1Q� �� *DecorationSecondary+.+��� -.   $ lblCols��$��0��$��0��$��2��$��Columns��$��Stretch=�$��Center=�$��Center=�$��2q�$��10q����� *
HeaderFont+���� *HeaderSecondary+���� *HeaderPrimary+�� -/   $ lblColsAmount��$��0��$��1��$��Amount:��$
��Left=�$��Center=�$��10,0,0,0q��� $ 
Horizontal=�$��1��$��1��$��Right=�$��Center=�$��0,0,10,0q�.+��� -0   $ btn_Col_Increase��$��12(�$	��▲��$��30��$��30��$��0q��� -1   $ tbColAmount��$��60��$��30��$��1�� �� TextAlignment$ Center=�$��Center=�� -2    �Name$ popCol��$	��Top=���.   � PlacementTarget ��  �� ElementName$ tbColAmount��.���� -3     ��Name$  tbColPop��
�������! �� Padding$! 5q�*(Insert value from 1 to 12 and hit enter.�� -4   $ btn_Col_Decrease��$��12(�$	��▼��$��30��$��30��$��0q��� -5   $ lblColsModify��$��0��$��2��$��
Modify at:��$
��Left=�$��Center=�$��10,0,0,0q��� $ 
Horizontal=�$��1��$��2��$��Right=�$��Center=�$��0,0,10,0q�.+��� -6   $ btn_Col_InsertAt��$��20(�$��30��$��30��$��	ExtraBold"�$��0q�.���� $��+��$��0,-4,0,0q��� -7   $ tbColModify��$��60��$��30��$��1��$ Center=�� -8   $ popColModify��$��Bottom=���.   �� $ tbColModify��.���� -9   $  tbPopColModify��
�������$! 5q�Insert value from 1 to 12.�� -:   $ btn_Col_DeleteAt��$��20(�$��30��$��30��$��	ExtraBold"�$��0q�.���� $��-��$��0,-4,0,0q�� -;   $ gdRows��$��1��$��	10,0,10,0q� �� $��* ��� $��* � �� $
��1.5* ��� $��* ��� $��1.25* ����� $	 0,0&�$		 0,1&�.��� $
 0Q� �� *DecorationPrimary+� $
 1Q� �� *DecorationSecondary+.+��� -<   $ lblRows��$
��Rows��$��0��$��0��$��2��$��Stretch=�$��Center=�$��Center=�$��2q�$��10q����� *
HeaderFont+���� *HeaderSecondary+���� *HeaderPrimary+�� -=   $ lblRowAmount��$��Amount:��$
��Left=�$��Center=�$��0��$��1��$��10,0,0,0q��� $ 
Horizontal=�$��1��$��1��$��2��$��Center=�$��Right=�$
��Auto��$��0,0,10,0q�.+��� $ 
Horizontal=�$��Right=�.+��� ->   $ btn_Row_Increase��$��12(�$	��▲��$��30��$��30��$��Right=�$��0q��� -?   $ tbRowAmount��$��60��$��30��$��1��$ Center=�$��Right=�� -@   $ popRow��$	��Top=���.   �� $ tbRowAmount��.���� -A   $  tbRowPop��
�������$! 5q�'%Insert a numeric value and hit enter.�� -B   $ btn_Row_Decrease��$��12(�$	��▼��$��30��$��30��$��Right=�$��0q��� -C   $ lblRowsModify��$��
Modify at:��$��2��$��0��$��Center=�$
��Left=�$��10,0,0,0q��� $ 
Horizontal=�$��2��$��2��$��2��$��Right=�$��Center=�$��0,0,10,0q�.+��� -D   $ btn_Row_InsertAt��$��20(�$��30��$��30��$��	ExtraBold"�$��0q�.���� $��+��$��0,-4,0,0q��� -E   $ tbRowModify��$��60��$��30��$��1��$ Center=�� -F   $ popRowModify��$��Bottom=���.   �� $ tbRowModify��.���� -G   $  tbPopRowModify��
�������$! 5q�Insert a numeric value.�� -H   $ btn_Row_DeleteAt��$��20(�$��30��$��30��$��	ExtraBold"�$��0q�.���� $��-��$��0,-4,0,0q�� -I   $ gdComp��$��0��$��2��$��20,0,0,0q� �� $��2* ��� $��* ��� $��2* ��� $��* � �� $
��1.5* ��� $��* ��� $��1.25* ����� $	 0,0&�$		 0,1&�.��� $
 0Q� �� *DecorationPrimary+� $
 1Q� �� *DecorationSecondary+.+��� -J   $ lblComps��$��0��$��0��$��4��$��Component: -��$��Stretch=�$��Center=�$��Center=�$��2q�$��10,10,52,10q����� *
HeaderFont+���� *HeaderSecondary+���� *HeaderPrimary+�� -K   $ btnCompEdit��$��0��$��0��$��4��$��Right=�$��Center=�$��30��$��30��$
��11,0q�.���� -L   $ imgCompEdit��$��Icons/edit.png���� -M   $ lblCompWidth��$��0��$��1��$��Width:��$
��Left=�$��Center=�$��10,0,0,0q��� $ 
Horizontal=�$��1��$��1��$��Right=�$��Center=�$��0,0,10,0q�.+��� -N   $ tbCompWidth��$��50��$��30��$��-��$ Center=�� -O   $ popCompWidth��$	��Top=���.   �� $ tbCompWidth��.���� -P   $  tbPopCompWidth��
�������$! 5q�'%Insert a numeric value and hit enter.�� -Q   $ lblCompHeight��$��2��$��1��$��Height:��$
��Left=�$��Center=�$��10,0,0,0q��� $ 
Horizontal=�$��3��$��1��$��Right=�$��Center=�$��0,0,10,0q�.+��� -R   $ tbCompHeight��$��50��$��30��$��-��$ Center=�� -S   $ popCompHeight��$��Bottom=���.   �� $ tbCompHeight��.���� -T   $  tbPopCompHeight��
�������$! 5q�'%Insert a numeric value and hit enter.�� -U   $ 
lblCompCol��$��0��$��2��$��Start column:��$
��Left=�$��Center=�$��10,0,0,0q��� $ 
Horizontal=�$��1��$��2��$��Right=�$��Center=�$��0,0,10,0q�.+��� -V   $ 	tbCompCol��$��50��$��30��$��-��$ Center=�� -W   $ 
popCompCol��$��Bottom=���.   �� $ 	tbCompCol��.���� -X   $  tbPopCompCol��
�������$! 5q�*(Insert value from 1 to 12 and hit enter.�� -Y   $ 
lblCompRow��$��2��$��2��$��
Start row:��$
��Left=�$��Center=�$��10,0,0,0q��� $ 
Horizontal=�$��3��$��2��$��Right=�$��Center=�$��0,0,10,0q�.+��� -Z   $ 	tbCompRow��$��50��$��30��$��-��$ Center=�� -[   $ 
popCompRow��$	��Top=���.   �� $ 	tbCompRow��.���� -\   $  tbPopCompRow��
�������$! 5q�'%Insert a numeric value and hit enter.&   System.Windows.Controls.DataGrid  -]   "  Name$" 	dgContent��#   RowHeaderWidth$# 30Q�$��2��$��2��$��2��$��20q�$   HeadersVisibility$	$ All=�%   CanUserAddRows% .  &   CanUserSortColumns& .  '   AutoGenerateColumns' .  $��Stretch=�$��Stretch=�$
��Auto��$
��Auto��(   SelectionUnit$
( Cell=�$��4q�)   EnableRowVirtualization) . !*   EnableColumnVirtualization* . + %� IsVirtualizing+ . , %� VirtualizationMode$, 	Recycling=�-   HorizontalGridLinesBrush- �� *HeaderSecondary+.   VerticalGridLinesBrush. �� *HeaderSecondary+���� *DecorationSecondary+���� *ButtonSecondary+/   Columns/ 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn0   Header0 �� *Title+1   CellTemplate1 �� .R��� $��Uniform=����� *	Content+ �� -^   $ cellContextMenu��.@�m� -_   $ miEdit��$
��Edit��$y�	Collapsed=�m� -`   $ miDelete��$��Delete��$y�	Collapsed=��� -a   2 ��Name$2 cmActionSeparator��$y�	Collapsed=�m� -b   $ miText��$��Text (simple)��m� -c   $ miButton��$��Button��m� -d   $ miImage��$��Image��m� -e   $ miCode��$
��Code��3   	CellStyle3 �� *  $System.Windows.Controls.DataGridCell"  4 �� 	Resources4 ��%K   '��       �� " ��.���  � ��$ Center=���  � ��$ Center=�5 �� Triggers5 P� 6 P� Property6 �@ 
IsSelected7 P� Value7 . . ���  � �� �� *SelectedCellPrimary+��  � �� �� *SelectedCellSecondary+��  � �� �� *SelectedCellSecondary+.���  � �� �� *CellSecondary+��  � �� �� *Color+��  � ��$ 10q���  � ��$ Stretch=���  � ��$ Stretch=���  � ��$ Stretch=���  � ��$ Stretch=�8   RowStyle8 �� )  #System.Windows.Controls.DataGridRow"  .���  � ��
9 X� NaN# Z9 :   RowHeaderStyle: �� :  4System.Windows.Controls.Primitives.DataGridRowHeader"  ; �� BasedOn#; [B .���  � �� �� $	 0,0&�$		 1,1&�.��� $	
 0.0Q� �� *DecorationPrimary+� $	
 1.0Q� �� *DecorationSecondary+��  � ��# � ����  � �� �� *Title+��  � ��$ Stretch=���  � ��$ Stretch=���  � �� �� < �� 
TargetType"<  .R��� #��z��#��z������ $	 0,0&�$		 1,1&�.��� $	
 0.0Q� �� *DataContext.DecorationPrimary+= �� RelativeSource= �� > �� AncestorType$> WindowL�� $	
 1.0Q� �� *!DataContext.DecorationSecondary+= �� $> WindowL�._��� $��DemiBold"�#��z��$��Center=�$��Center=����� *DataContext.HeaderFont+= �� $> WindowL�?   ColumnHeaderStyle? �� =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  .���  � ��$ Center=���  � ��$ Center=���  � ��$ 30����  � �� �� "<  .R��� #��z��#��z������ $	 0,0&�$		 1,1&�.��� $	
 0.0Q� �� *DataContext.DecorationPrimary+= �� $> WindowL�� $	
 1.0Q� �� *!DataContext.DecorationSecondary+= �� $> WindowL�._��� $��DemiBold"�#��z��$��Center=�$��Center=���  � �� �� $	 0,0&�$		 1,1&�.��� $	
 0.0Q� �� *DecorationPrimary+� $	
 1.0Q� �� *DecorationSecondary+� -f   $ Overlay�����	#AAFFFFFF$y�	Collapsed=�.+��� $��
Loading...��$��Center=�$��Center=�$��20(�  -g   $" dgPages��$��3��$��20,35,20,20q�% .  $��Stretch=�$��Stretch=�$
( Cell=�$��4q����� *ButtonSecondary+���� *UnderlayPrimary+: �� "  #; [B .���  � �� �� $	 0,0&�$		 1,1&�.��� $	
 0.0Q� �� *DecorationPrimary+� $	
 1.0Q� �� *DecorationSecondary+��  � ��# � ����  � ��$ Stretch=���  � ��$ Stretch=���  � ��$ Center=���  � ��$ Center=���  � �� �� "<  .R��� #��z��$��1q����� $	 0,0&�$		 1,1&�.��� $	
 0.0Q� �� *DataContext.DecorationPrimary+= �� $> WindowL�� $	
 1.0Q� �� *!DataContext.DecorationSecondary+= �� $> WindowL�._�� $��Center=�$��Center=�.+��� $��30��$��30��$��Center=�$��Center=�#��z��#��z��? �� "  .���  � ��$
 Left=���  � ��$ Center=���  � ��$ 30����  � ��$ 5,0,5,0q���  � ��$ DemiBold"���  � ��$ 1q���  � �� �� *DataContext.HeaderSecondary+= �� $> WindowL���  � �� �� $	 0,0&�$		 1,1&�.��� $	
 0.0Q� �� *DataContext.DecorationPrimary+= �� $> WindowL�� $	
 1.0Q� �� *!DataContext.DecorationSecondary+= �� $> WindowL��� -h   $ lblFilesTitle��$��3��$	��Top=�$��Files��#��[  $��	15,0,15,0q��� -i   $ 
imgRefresh��$��3��$��Right=�$	��Top=�$��Icons/refresh.png��$��20��$��20��$��
15,10,25,0q�