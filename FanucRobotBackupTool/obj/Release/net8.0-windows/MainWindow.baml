   M S B A M L   `   `   `  ���� O  KFanucRobotBackupTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:FanucRobotBackupToolFanucRobotBackupTool  %    FanucRobotBackupTool.MainWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     4local"clr-namespace:FanucRobotBackupTool       � Title$  Fanuc Robot Backup Tool��$	��450��$	��800��.��� ���	#FFF8F8F8 � ColumnDefinitions �� $	��220 ��� $��* �.+�]� $��0�����#F8F8F8.+���  �� Orientation$ 
Horizontal=�$��5q�$	��Top=�.+��� -    ��Name$ 	AddButton��$	��Add��$��60��$��0,0,5,0q��� -   $ RemoveButton��$��Remove��$��60��$��0,0,5,0q��� -    ��Name$ LogFilesCheckBox��$��	Log Files��$��center=�$��0,0,5,0q� d� 	IsChecked �� *
LogFiles+ �� Mode$ TwoWay=�R� -   $��5,0,5,5q����� *	Devices+���� .R��� ���� *Name+����  �� 
TargetType" Q�.���  �� Property � ��	 �� Value$	 Normal"���  � ��
	 ���� ��  � ��
	 �   ���  � ��	 �� 
 �� 
TargetType"
 Q�.R���  ��Name$ Bd��
������  �� Padding$	 4,2q� J� SnapsToDevicePixels$
 True��$��1q�
������ ._��� $��Header��$��Stretch=�$��Center=� �� Triggers P�  P� Property �@Q�
IsSelected P� Value$
 True��. ���  �� 
TargetName$ Bd�� � ��	 �	#FFBFCDDB�� $ Bd�� � ��
	 �   ��� $ Bd�� � ��
	 �   ���  � ��$
	 Bold"�P�  �@J�IsMouseOver$
 True��. ��� $ Bd�� � ��
	 ��ح��� $ Bd�� � ��
	 �   �� $��1��$��0,0,5,5q� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto � �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��$��0��$��Center=�$��Right=�$��0,0,5,5q�$��15��$��Device Name:���� -    ��Name$ DeviceNameTextBox��$��0��$��1��$��Center=�$��0,5,5,5q����� *SelectedDevice.Name+ �� UpdateSourceTrigger$ PropertyChanged=��� $��1��$��0��$��Center=�$��Right=�$��0,0,5,5q�$��16��$��IP Address:���� -   $ IpAddressTextBox��$��1��$��1��$��Center=�$��0,5,5,5q����� *SelectedDevice.IpAddress+$ PropertyChanged=��� $��2��$��0��$��Center=�$��Right=�$��0,0,5,5q�$��16��$��
Directory:���� -   $ DirectoryTextBox��$��2��$��1��$��Center=�$��0,6,5,6q� �� 
IsReadOnly$
 True������ *SelectedDevice.FilePath+$ PropertyChanged=��� -   $ BrowseButton��$��2��$��2��$��Browse��$��0,5,0,5q��� -	   $ 
LogTextBox��$��3��$��0��$��3��$��0,0,0,0q�$
 True�� �� TextWrapping$
 Wrap=� �� AcceptsReturn$
 True��$��Stretch=�" �� VerticalScrollBarVisibility$
 Auto=����	#FF293A56
��������� -
    ��Name$ ProgressLabel��$��4��$��0��$��2��$
��Left=�$��Center=�$��0,0,0,0q�$��16��$	��175��$�� ��$
��Bold"�$y�Hidden=�� -    �Name$ ProgressBar��$��4��$��1��$��Center=�$��Center=�$��20��$	��200��$	��200�� � Minimum$ 0Q� � Maximum$	 100Q�$��0,0,0,0q�$y�Hidden=�$��2���� -   $ 	RunButton��$��4��$��2��$	��Run��$��60��$��25��$��0,5,5,0q��� -   $ CancelButton��$��4��$��2��$��Cancel��$��60��$��25��$��0,5,5,0q�$y�Hidden=�