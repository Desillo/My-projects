   M S B A M L   `   `   `  ����G  CWarehouseApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:WarehouseAppWarehouseApp  5   	   "    WarehouseApp.ProviderWindow1   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:WarehouseApp     5   	     � Title$  ProviderWindow1��5   	   $	��400��6!   $	��400��6.   .��5	      �� .@�5
   
   �� $��Список��6   .��5      � $��5q�6    � RowDefinitions 5      �� 5      $
��230* �6$   �� 5      $��30 �6$   �� 5      $��41 �6$    � ColumnDefinitions 5      �� 5      �� 5      .+�5      &   System.Windows.Controls.DataGrid  -   5        $��	dataGrid1��5      $��5q�6,      AutoGenerateColumns .  $��0��6S      CanUserAddRows .  $��2��5   X      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Имя��6-      
IsReadOnly .    Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 6e   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 6:   �� *name+6e     5      $ 
Адрес��6-    . $	 100 6i   	 6<   �� *adress+6i     5      $ Контакты��6-    . $	 100 6n   	 6?   �� *
contacts+6n   5      �� 5      -   5        $��spanel1��5      $��1��6,   $��1��69   $��5q�6I   
 �� Orientation$
 
Horizontal=�6T   6o   �� 5      $��Center=�6   $
 
Horizontal=�6:   $��2��6S   $��2��6`   .+�5       �� -   5        $��5q�5       .��6@   Добавить6J   �� 5!      -   5        $��	BtnUpdate��5!   C   $��5q�6   .��6T   Изменить6^   �� 5"      -   5        $��	BtnDelete��5"   C   $��5q�6   .��6T   Удалить6]   �� 5#      -   5        $��	BtnSelect��5#   @   $��5q�6   .��6Q   Выбрать6Z   5$      5%      5&      �� 5'   
   $��
Поиск��6   .��5(      �� $��Center=�6   $��Center=�66   $	��200��6Q   .+�5)      �� $��Имя��6   $��0��6(   .��5*      �� -   5        $��txtName��5*      5+      �� 5,      $��
Адрес��6   $��1��6*   .��5-      �� -   5        $��	txtAdress��5-      5.      �� 5/      $��Контакты��6   $��0��6-   $��1��6=   .��50      �� -	   5        $��txtContacts��50      51      �� 52      -
   5        $��	BtnSearch��52   ]   $��1��6   $��1��6)   $��20,10q�66   $��50��6n   .��6}   
Поиск6�   53      54      55      56      