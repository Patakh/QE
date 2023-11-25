using QE.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows;
using QE.Function.PrintTalon;
namespace QE.Models.Main.ButtonAction;

public class ButtonAction : Button
{
    public ButtonAction(string buttonName, SService sServices,string Ip)
    {  
        Content = buttonName;
        HorizontalAlignment = HorizontalAlignment.Center;
        VerticalAlignment = VerticalAlignment.Top;
        Height = 75;
        Width = 200;
        Margin = new Thickness(0, 18, 32, 0);
        Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
        BorderBrush = new SolidColorBrush(Color.FromRgb(55, 55, 55));
        FontFamily = new FontFamily("Area");
        FontSize = 25;
        Foreground = new SolidColorBrush(Color.FromRgb(135, 98, 27));
        DropShadowEffect shadowEffect = new DropShadowEffect();
        shadowEffect.Color = Color.FromRgb(22, 22, 22);
        shadowEffect.Direction = 315;
        shadowEffect.ShadowDepth = 3;
        Effect = shadowEffect;
        ControlTemplate myControlTemplate = new ControlTemplate(typeof(Button));
        FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
        border.Name = "border";
        border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
        border.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
        border.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
        border.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
        FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
        contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
        contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
        border.AppendChild(contentPresenter);
        myControlTemplate.VisualTree = border;
        Template = myControlTemplate;
        Click += (s, e) =>
        {
             Talon.PrintTalon(s, e, sServices,Ip);
        }; 
    }
}

