   M S B A M L   `   `   `  ????  
MonitorMPS    MonitorMPS.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     ? Title$O  IДисплей модуля преобразования сигналов??5   	   $	??495??68   $	??792??6E   $	??320??6Q   $	??480??6a    ? 
ResizeMode$ NoResize=?6p    ? Icon$ RunControl.ico??6?   .??5      ?  ? RowDefinitions 5   
   ?? 5      $
??308* ?6   ?? 5      $	??87* ?6   .+?5
   
   ?? $??Stretch=?6   $
??Auto??63   $??Stretch=?6A   $
??Auto??6]   $??2??6j   .@?5      ?? -   5         ??Name$
 Main??5      $??Измерение??6$   .??5      ? ???	#FFE5E5E5$??0,0,0,0q?6.    ? ColumnDefinitions 5      ?? 5      .+?5      ?? -   5         ??Name$ Open_Btn??5      $
??Left=?6/   $??64??6J   $??2,0,0,2q?6U   $??64??6}   $??Bottom=?6?    ? ToolTip$ Открыть порт??6?   .??5      ?? -   5         ??Name$ OpenBtn_Image??5       $??48??67   $??48??6C   $??Images/Connect.png??6N   5      ?? 5      -   5        $ OutDebug_Btn??5      $??Bottom=?63   $
??Left=?6N   $??64??6i   $??64??6t   $??74,0,0,2q?6?   {?.  $@ :Прочитать содержимое регистров??6?   .??5      ?? -   5        $ OutDebug_Image??5       $??48??68   $??48??6D   $??Images/help_about.png??6O   5      ?? 5      -   5        	 ??Name$	 
Report_RTB??5   "   $??
0,0,446,82q?66   $??Arial)?6J   "
 ?? VerticalScrollBarVisibility$

 Auto=?6^    ?? 
LineHeight$ 1??6?   $
??auto??6?   $
??auto??6?   . ?5      /? 5      ?? 5      -   5        $ 	Start_Btn??5      $??Bottom=?60   $
??Left=?6K   $??64??6f   $??64??6q   $??	146,0,0,2q?6}   {?.  $0 *Запуск цикла измерений??6?   .??5      ?? -   5        $ StartBtn_Image??5       $??48??68   $??48??6D   $??Images/runcontrol.png??6O   5      ?? 5      $'??!Частота измерений??6   $
??Left=?68   $	??Top=?6S   $??26??6k   $	??117??6w   $??
330,10,0,0q?6?   ?? 5      $-??'Количество измерений??6   $	??Top=?6;   $??26??6S   $??330,114,302,0q?6_   ?? 5      $)??#Количество каналов??6   $
??Left=?69   $	??Top=?6T   $??26??6l   $	??133??6x   $??
330,62,0,0q?6?   ?? 5      -	   5         ??Name$ MeasFrequency_CB??5      $
??Left=?69   $	??Top=?6T   $	??120??6l   $??22??6x   $??
337,36,0,0q?6?   ?? 5       -
   5        $ FrequencySet_Btn??5       $
??Left=?67   $??
479,36,0,0q?6R   $	??Top=?6f   $??42??6~   $??22??6?   {?.  .??5!      ?? $??16??6    $??16??6,   $??Images/button_ok.png??67   5"      ?? 5#      -   5        $ Channels_CB??5#      $
??Left=?64   $	??Top=?6O   $	??120??6g   $??22??6s   $??
337,88,0,0q?6   ?? 5$      -   5        $ ChannelsSet_Btn??5$      $
??Left=?66   $??
479,88,0,0q?6Q   $	??Top=?6e   $??42??6}   $??22??6?   {?.  .??5%      ?? $??16??6    $??16??6,   $??Images/button_ok.png??67   5&      ?? 5'      -   5        $ 	Points_CB??5'      $
??Left=?62   $	??Top=?6M   $	??120??6e   $??337,140,0,0q?6q   $??22??6?   ?? 5(      -   5        $ PointsSet_Btn??5(      $
??Left=?64   $??479,140,0,0q?6O   $	??Top=?6d   $??42??6|   $??22??6?   {?.  .??5)      ?? $??16??6    $??16??6,   $??Images/button_ok.png??67   5*      ?? 5+      -   5         ??Name$ Frequency_L??5+      $
??НД??61   $
??Left=?6>   $??
479,10,0,0q?6Y   $	??Top=?6m   ?? 5,      -   5        $ 
Channels_L??5,      $
??НД??60   $
??Left=?6=   $??
479,62,0,0q?6X   $	??Top=?6l   ?? 5-      -   5        $ Points_L??5-      $
??НД??6.   $
??Left=?6;   $??479,114,0,0q?6V   $	??Top=?6k   ?? 5.      -   5         ??Name$ Pin_Convst_Ck??5.      $??CONVST??66   $
??Left=?6G   $??572,324,0,0q?6b   $	??Top=?6w   {?.  6?   ?? 5/      -   5        $ 	Pin_Rd_Ck??5/      $??Rd??62   $
??Left=?6?   $??572,343,0,0q?6Z   $	??Top=?6o   {?.  6?   ?? 50      -   5        $ 
Pin_CS1_Ck??50      $	??CS1??63   $
??Left=?6A   $??572,364,0,0q?6\   $	??Top=?6q   {?.  6?   ?? 51      -   5        $ 
Pin_CS2_Ck??51      $	??CS2??63   $
??Left=?6A   $??572,387,0,0q?6\   $	??Top=?6q   {?.  6?   ?? 52      -   5        $ Pin_Reset_Ck??52      $??Reset??65   $
??Left=?6E   $??572,408,0,0q?6`   $	??Top=?6u   {?.  6?   ?? 53      -   5        $ Pin_Busy1_Ck??53      $??Busy_1??65   $
??Left=?6F   $??668,324,0,0q?6a   $	??Top=?6v   {?.  6?   ?? 54      -   5        $ Pin_Busy2_Ck??54      $??Busy2??65   $
??Left=?6E   $??668,345,0,0q?6`   $	??Top=?6u   {?.  6?   ?? 55      -   5        $ Pin_DB0_1_Ck??55      $	??DB0??65   $
??Left=?6C   $??
572,20,0,0q?6^   $	??Top=?6r   {?.  6?   ?? 56      -   5        $ Pin_DB1_1_Ck??56      $	??DB1??65   $
??Left=?6C   $??
572,41,0,0q?6^   $	??Top=?6r   {?.  6?   ?? 57      -   5        $ Pin_DB2_1_Ck??57      $	??DB2??65   $
??Left=?6C   $??
572,63,0,0q?6^   $	??Top=?6r   {?.  6?   ?? 58      -   5        $ Pin_DB3_1_Ck??58      $	??DB3??65   $
??Left=?6C   $??
572,84,0,0q?6^   $	??Top=?6r   {?.  6?   ?? 59      -   5        $ Pin_DB4_1_Ck??59      $	??DB4??65   $
??Left=?6C   $??572,105,0,0q?6^   $	??Top=?6s   {?.  6?   ?? 5:      -   5        $ Pin_DB5_1_Ck??5:      $	??DB5??65   $
??Left=?6C   $??572,126,0,0q?6^   $	??Top=?6s   {?.  6?   ?? 5;      -   5        $ Pin_DB6_1_Ck??5;      $	??DB6??65   $
??Left=?6C   $??572,148,0,0q?6^   $	??Top=?6s   {?.  6?   ?? 5<      -    5        $ Pin_DB7_1_Ck??5<      $	??DB7??65   $
??Left=?6C   $??572,169,0,0q?6^   $	??Top=?6s   {?.  6?   ?? 5=      -!   5        $ Pin_DB8_1_Ck??5=      $	??DB8??65   $
??Left=?6C   $??572,190,0,0q?6^   $	??Top=?6s   {?.  6?   ?? 5>      -"   5        $ Pin_DB9_1_Ck??5>      $	??DB9??65   $
??Left=?6C   $??572,212,0,0q?6^   $	??Top=?6s   {?.  6?   ?? 5?      -#   5        $ Pin_DB10_1_Ck??5?      $
??DB10??66   $
??Left=?6E   $??572,233,0,0q?6`   $	??Top=?6u   {?.  6?   ?? 5@      -$   5        $ Pin_DB11_1_Ck??5@      $
??DB11??66   $
??Left=?6E   $??572,254,0,0q?6`   $	??Top=?6u   {?.  6?   ?? 5A      -%   5        $ Pin_DB12_1_Ck??5A      $
??DB12??66   $
??Left=?6E   $??572,275,0,0q?6`   $	??Top=?6u   {?.  6?   ?? 5B      -&   5        $ Pin_DB13_1_Ck??5B      $
??DB13??66   $
??Left=?6E   $??572,297,0,0q?6`   $	??Top=?6u   {?.  6?   ?? 5C      -'   5        $ Pin_DB0_2_Ck??5C      $	??DB0??65   $
??Left=?6C   $??
668,20,0,0q?6^   $	??Top=?6r   {?.  6?   ?? 5D      -(   5        $ Pin_DB1_2_Ck??5D      $	??DB1??65   $
??Left=?6C   $??
668,41,0,0q?6^   $	??Top=?6r   {?.  6?   ?? 5E      -)   5        $ Pin_DB2_2_Ck??5E      $	??DB2??65   $
??Left=?6C   $??
668,63,0,0q?6^   $	??Top=?6r   {?.  6?   ?? 5F      -*   5        $ Pin_DB3_2_Ck??5F      $	??DB3??65   $
??Left=?6C   $??
668,84,0,0q?6^   $	??Top=?6r   {?.  6?   ?? 5G      -+   5        $ Pin_DB4_2_Ck??5G      $	??DB4??65   $
??Left=?6C   $??668,105,0,0q?6^   $	??Top=?6s   {?.  6?   ?? 5H      -,   5        $ Pin_DB5_2_Ck??5H      $	??DB5??65   $
??Left=?6C   $??668,126,0,0q?6^   $	??Top=?6s   {?.  6?   ?? 5I      --   5        $ Pin_DB6_2_Ck??5I      $	??DB6??65   $
??Left=?6C   $??668,148,0,0q?6^   $	??Top=?6s   {?.  6?   ?? 5J      -.   5        $ Pin_DB7_2_Ck??5J      $	??DB7??65   $
??Left=?6C   $??668,169,0,0q?6^   $	??Top=?6s   {?.  6?   ?? 5K      -/   5        $ Pin_DB8_2_Ck??5K      $	??DB8??65   $
??Left=?6C   $??668,190,0,0q?6^   $	??Top=?6s   {?.  6?   ?? 5L      -0   5        $ Pin_DB9_2_Ck??5L      $	??DB9??65   $
??Left=?6C   $??668,212,0,0q?6^   $	??Top=?6s   {?.  6?   ?? 5M      -1   5        $ Pin_DB10_2_Ck??5M      $
??DB10??66   $
??Left=?6E   $??668,233,0,0q?6`   $	??Top=?6u   {?.  6?   ?? 5N      -2   5        $ Pin_DB11_2_Ck??5N      $
??DB11??66   $
??Left=?6E   $??668,254,0,0q?6`   $	??Top=?6u   {?.  6?   ?? 5O      -3   5        $ Pin_DB12_2_Ck??5O      $
??DB12??66   $
??Left=?6E   $??668,275,0,0q?6`   $	??Top=?6u   {?.  6?   ?? 5P      -4   5        $ Pin_DB13_2_Ck??5P      $
??DB13??66   $
??Left=?6E   $??668,297,0,0q?6`   $	??Top=?6u   {?.  6?   ?? 5Q      -5   5        $ GetPins_Btn??5Q      $??Bottom=?62   $
??Left=?6M   $??	351,0,0,2q?6h   $??64??6{   {?.  $??64??6?   .??5R      ?? $??48??6    $??Images/button_ok.png??6,   $
??Fill=?6J   $??48??6Y   5S      ?? 5T      -6   5        $ ReadResults_Ck??5T      $)??#Запись результатов??67   $
??Left=?6T   $??
338,0,0,89q?6o   $??Bottom=?6?    d? 	IsChecked$
 TrueY?6?   6?   ?? 5U      -7   5        $ EveryTooth_CB??5U      $
??Left=?66   $	??Top=?6Q   $	??120??6i   $??337,192,0,0q?6u   $??22??6?   ?? 5V      -8   5        $ EveryTooth_Btn??5V      $
??Left=?65   $??479,192,0,0q?6P   $	??Top=?6e   $??42??6}   $??22??6?   {?.  .??5W      ?? $??16??6    $??16??6,   $??Images/button_ok.png??67   5X      ?? 5Y      $!??Каждый 1/X зубец??6   $	??Top=?67   $??26??6O   $??330,166,302,0q?6[   ?? 5Z      -9   5        $ EveryTooth_L??5Z      $
??НД??62   $
??Left=?6?   $??479,166,0,0q?6Z   $	??Top=?6o   5[      5\      ?? 5]      -:   5        $ Settings??5]      $??Настройки??6(   .??5^      ? ???	#FFE5E5E5$??0,0,0,0q?6.    5_      ?? 5`      $??0* ?6+   ?? 5a      $	??66* ?6+   ?? 5b      $	??73* ?6+   ?? 5c      $	??132 ?6+   ?? 5d      $	??76* ?6+    5f      ?? 5g      $	??10* ?6(   ?? 5h      $	??38* ?6(   ?? 5i      $	??38* ?6(   ?? 5j      $	??38* ?6(   ?? 5k      $	??39* ?6(   ?? 5l      $	??38* ?6(   ?? 5m      $	??38* ?6(   ?? 5n      $
??130* ?6(   .+?5p      ?? $??Порт??6   $
??Left=?6+   $	??Top=?6F    J? RenderTransformOrigin$ 0,0.154&?6^   $??25??6~   $??40??6?   $??1??6?   $??1??6?   ?? 5q      $#??Скорость обмена??6   $
??Left=?66   $??2??6Q   $	??Top=?6^   $??26??6v   $	??107??6?   $??1??6?   ?? 5r      -;   5        $ PortName_CB??5r      $??2??64   $
??Left=?6D   $	??Top=?6_   $	??120??6w   $??1??6?   $??22??6?   ?? 5s      -<   5        $ Baudrate_CB??5s      $??2??64   $
??Left=?6D   $??2??6_   $	??Top=?6l   $	??120??6?   $??22??6?   ?? 5t      $!??Количество бит??6   $??1??65   $
??Left=?6E   $??3??6`   $	??Top=?6m   $??26??6?   $??96??6?   ?? 5u      -=   5        $ DataBits_CB??5u      $??2??64   $
??Left=?6D   $??3??6_   $	??Top=?6l   $	??120??6?   $??22??6?   ?? 5v      $*??$Количество стоп-бит??6   $??1??6:   $
??Left=?6J   $??4??6e   $	??Top=?6r   $??26??6?   $	??126??6?   ?? 5w      ->   5        $ StopBits_CB??5w      $??2??64   $
??Left=?6D   $??4??6_   $	??Top=?6l   $	??120??6?   $??22??6?   ?? 5x      $??Паритет??6   $??1??6.   $
??Left=?6>   $??5??6Y   $	??Top=?6f   $??26??6~   $??55??6?   ?? 5y      -?   5        $ 	Parity_CB??5y      $??2??62   $
??Left=?6B   $??5??6]   $	??Top=?6j   $	??120??6?   $??22??6?   ?? 5z      -@   5        $ SaveSettings_Btn??5z      $
??Left=?67   $??	45,40,0,0q?6R   $	??Top=?6e   $??75??6}   $ -0.627,-0.591&?6?   $??72??6?   $??7??6?   $??4??6?   .??5{      ?? $??62??6    $??65??6,   $??Images/button_ok.png??67   5|      ?? 5}      $??Порт??6   $
??Left=?6+   $	??Top=?6F   $ 0,0.154&?6^   $??25??6~   $??40??6?   $??3??6?   $??1??6?   ?? 5~      $#??Скорость обмена??6   $
??Left=?66   $??2??6Q   $	??Top=?6^   $??26??6v   $	??107??6?   $??3??6?   ?? 5      -A   5        $ PortName_2_CB??5      $??4??66   $
??Left=?6F   $	??120??6a   $??1??6m   $??0,0,0,16q?6z   ?? 5?      -B   5        $ Baudrate_2_CB??5?      $??4??66   $
??Left=?6F   $??2??6a   $	??Top=?6n   $	??120??6?   $??22??6?   ?? 5?      $!??Количество бит??6   $??3??65   $
??Left=?6E   $??3??6`   $	??Top=?6m   $??26??6?   $??96??6?   ?? 5?      -C   5        $ DataBits_2_CB??5?      $??4??66   $
??Left=?6F   $??3??6a   $	??Top=?6n   $	??120??6?   $??22??6?   ?? 5?      $*??$Количество стоп-бит??6   $??3??6:   $
??Left=?6J   $??4??6e   $	??Top=?6r   $??26??6?   $	??126??6?   ?? 5?      -D   5        $ StopBits_2_CB??5?      $??4??66   $
??Left=?6F   $??4??6a   $	??Top=?6n   $	??120??6?   $??22??6?   ?? 5?      $??Паритет??6   $??3??6.   $
??Left=?6>   $??5??6Y   $	??Top=?6f   $??26??6~   $??55??6?   ?? 5?      -E   5        $ Parity_2_CB??5?      $??4??64   $
??Left=?6D   $??5??6_   $	??Top=?6l   $	??120??6?   $??22??6?   ?? 5?      -F   5         ??Name$ Directory_TB??5?      $??2??64   $
??Left=?6D   $??23??6_   $??6??6k    ?? TextWrapping$
 Wrap=?6x   $	??Top=?6?   $	??326??6?   $??3??6?   ?? 5?      $'??!Каталог программы??6   $??1??68   $
??Left=?6H   $??0,1,0,0q?6c   $??6??6t   $	??Top=?6?   $ -8.692,-1.846&?6?   ?? 5?      -G   5        $ GetDirectory_Btn??5?      $	??...??67   $??4??6E   $
??Left=?6U   $??72,0,0,0q?6p   $??6??6?   $	??Top=?6?   $??48??6?   $??22??6?   6?   5?      5?      5?      5?      5?      