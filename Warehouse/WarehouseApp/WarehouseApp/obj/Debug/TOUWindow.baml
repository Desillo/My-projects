   M S B A M L   `   `   `  ����G  CWarehouseApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:WarehouseAppWarehouseApp  5   	       WarehouseApp.TOUWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:WarehouseApp     5   	     � Title$  	TOUWindow��5   	   $	��300��6   $	��300��6(   .��5	      � $��5q�6    � RowDefinitions 5
   
   �� 5      $��50 �6   �� 5      $
��230* �6   �� 5      $��41 �6   �� 5      $��41 �6    � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $��* �6   .+�5   
   �� $.��(Поиск по наименованию��6   $��0,0,0,0q�62   .��5      �� -   5        $
��txt1��5      5      �� 5   
   -   5        $��	BtnSearch��5   Q   $��1��6   $��0��6!   $��5q�6.   .��6b   
Поиск6i   &   System.Windows.Controls.DataGrid  5   
   -   5        $��	dataGrid1��5      $��0,5,0,0q�6$      AutoGenerateColumns .  $��2��6Q   $��1��6e      CanUserAddRows .     Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Наименование��6%      
IsReadOnly .    Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 6f   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 6;   �� *name+6f   5      �� 5   
   -   5        $��spanel1��5      $��2��6$   $��0��61   $��5q�6A   
 �� Orientation$
 
Horizontal=�6L   6g   �� 5   
   $��Center=�6   $
 
Horizontal=�62   $��3��6K   $��2��6X   .+�5      �� -   5        $��5q�5      .��68   Добавить6B   �� 5       -   5        $��	BtnUpdate��5    ;   $��5q�6   .��6L   Изменить6V   �� 5!      -   5        $��	BtnDelete��5!   ;   $��5q�6   .��6L   Удалить6U   �� 5"      -   5        $��	BtnSelect��5"   8   $��5q�6   .��6I   Выбрать6R   5#      5$      5%      