using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using QE.Context;

namespace QE.Models.Main.Menu;

public class HeaderMenu : WrapPanel
{
    public HeaderMenu(string menuName)
    {
        TextBlock textBlockMenu = new TextBlock();
        textBlockMenu.FontFamily = new FontFamily("Area");
        textBlockMenu.FontSize = 60;
        textBlockMenu.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
        textBlockMenu.Text = menuName;
        textBlockMenu.TextWrapping = TextWrapping.Wrap;

        Orientation = Orientation.Horizontal;
        VerticalAlignment = VerticalAlignment.Center;
        Visibility = Visibility.Collapsed;
        Margin = new Thickness(25, 0, 0, 0);
        Children.Add(textBlockMenu);
    }
}

