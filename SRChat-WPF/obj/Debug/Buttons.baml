   M S B A M L   `   `   `  ����E  ASRChat-WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SRChat_WPF
SRChat_WPF  5          SRChat_WPF.Buttons   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      *localclr-namespace:SRChat_WPF     5      -   5          >�Name$  PMwindow��5      .��5      � $	��Top=�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   � $��3��6   $��0,10,0,0q�6!   .+�5      �� 5      5      �� 5   
   -   5         ��Name$ PrivateMessage��5      $
��Left=�6*   $��29��6E   $��18,19,0,-48q�6Q   $��4��6f    �� TextWrapping$
 Wrap=�6s   $�� ��6�   $	��Top=�6�   $	��496��6�    �� 	IsEnabled . 6�   �� 5   
   -   5         ��Name$ PrivateSend��5   -   $��Right=�6      MaterialDesignRaisedButton#��[  6[   $��0,16,25,-48q�6�   $
��Send��6�   $��80��6�   $��4��6�   6�   �� 5   
   -   5        $ PrivateChat��5      $
��Left=�6@   $	��220��6[   $��20,76,0,-296q�6h   $��4��6~   $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��595��6�    �� 
IsReadOnly . " �� VerticalScrollBarVisibility$
 Auto=�6�   �� 5   
   $
��Left=�6   $��24��6-   $��18,301,0,-325q�69   $��4��6P   $
 Wrap=�6]   $	��Top=�5   5   $	��496��6M   {�.  ��5   q   ��  �� Path$ MyValue�	 �� RelativeSource	 �� 
 �� AncestorType"
 �5   Y   �� 5    
   $
��Left=�6   $��24��6-   $��535,301,0,-325q�69   $��4��6Q   $
 Wrap=�6^   $
��Info��6r   $	��Top=�6~   $��80��6�   {�.  6�   5"      5#      