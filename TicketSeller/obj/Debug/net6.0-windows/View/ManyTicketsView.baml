   M S B A M L   `   `   `  ????G  CTicketSeller, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:TicketSeller.ViewModelTicketSeller.ViewModel  5      ?$clr-namespace:TicketSeller.ConverterTicketSeller.Converter  5      '    !TicketSeller.View.ManyTicketsView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=4.6.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      4	viewModel$clr-namespace:TicketSeller.ViewModel  5      4	converter$clr-namespace:TicketSeller.Converter  5        N? Title$  ManyTicketsView??5       ? 	Resources 5      <   6TicketSeller.Converter.BooleanToVisibilityConverterInv #  BooleanToVisibilityConverterInv??%   &	          5   
   64   5      .??5      ?  ? RowDefinitions 5   
   ?? 5      $
??Auto ?5      ?? 5      $??* ?5      .+?5   
   ?? $??0??6    ?? Orientation$ 
Horizontal=?6"   .+?5      ?? $??Добавить??6   $??	10,10,0,0q?6M   ??6(   ?? *NewTicketCommand+y?5      ?? *IsUserAdmin+   BooleanToVisibilityConverter ?? 	Converter# [ ?? 5      $??
Отчёт??6   $??	10,10,0,0q?6G   ??6%   ?? *ReportCommand+y?5      ?? *IsUserAdmin+# [ ?? 5      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Тэг билета??6   .  (MaterialDesignThemes.Wpf.TextFieldAssist   HasClearButton$
 True??6C    ?? 
IsEditable$
 True??5      $??18(?6k   $??	10,10,0,0q?5      ??5   )   ?? *Tags+ ?? SelectedItem 6F   ?? *SelectedTag+5      5      ?? 5   
   -   5        	 ??Name$	 
GlobalList??5      $??1??6'   $
??0,10q?64   ??6B   ?? *	Tickets+??5       ?? 5!      .R?5"      ? 5#      ??5%      ?? 5&      .R?5'      #  MaterialDesignThemes.Wpf.Card  $	??200??6*   .??5(      ?  5)      ?? 5*   "   $
??Auto ?5+   %   ?? 5,   "   $??* ?5-   %   .+?5/      ?? $
??5, 5q?6)   .+?50   "   ?? $??18(?51   %   $
??Bold"?52   %   $
??Wrap=?53   %   $??Center=?54   %   ??55   %   ?? *
TicketName+?? 56   "   $??18(?57   %   $
??Wrap=?58   %   $??Center=?59   %   ??5:   %   ?? *Tag+?? 5;   "   $??18(?5<   %   $
??Wrap=?5=   %   $??Center=?5>   %   ??5?   %   ?? *DateOfConcert+?? 5@   "   $??18(?5A   %   $
??Wrap=?5B   %   $??Center=?5C   %   ??5D   %   ?? *ConcertPlace+5E      ?? 5F      $??1??6)   $
??5, 0q?66   .+?5G   "   ?? $
??Wrap=?6,   $??Продано??6@   y?5H   ,   ?? *IsSold+# [ ?? 5I   "   $
??Wrap=?6,   $"??Забронированно??6@   y?5J   ,   ?? *
IsBocked+# [ ?? 5K   "   $	??0,5q?6)   $??Купить??5L   )   ??5M   )   ?? 
 ?? ElementName$
 
GlobalList?? ?? Path$ DataContext.BuyCommand?y?5N   )   ?? *IsSold+# [   ?? ConverterParameter$ Inverted????5O   )   ?? *Id+?? 5P   "   $	??0,5q?6)   $ ??Забронировать??5Q   )   ??5R   )   ?? $
 
GlobalList??$ DataContext.BockedCommand?y?5S   )   ?? *
IsBocked+# [  $ Inverted????5T   )   ?? *Id+?? 5U   "   $	??0,5q?6)   $ ??Редактировать??5V   )   ??5W   )   ?? $
 
GlobalList??$ DataContext.EditCommand?y?5X   )   ?? $
 
GlobalList??$ DataContext.IsUserAdmin?# [ ??5Y   )   ?? *Id+?? 5Z   "   $	??0,5q?6)   $??Удалить??66   ??5[   )   ?? $
 
GlobalList??$ DataContext.DeleteCommand?y?5\   )   ?? $
 
GlobalList??$ DataContext.IsUserAdmin?# [ ??5]   )   ?? *Id+5^      5_      5`      5a      5c      5d      5e      