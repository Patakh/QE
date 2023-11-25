using QE.Context;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.Windows.Media;
namespace QE.Models.Main.Menu;

/// <summary>
/// Красная кнопка меню
/// </summary>
public class ButtonMenu : Button
{
    public ButtonMenu(SOfficeTerminalButton button)
    {
        DropShadowEffect shadowEffect = new DropShadowEffect();
        shadowEffect.Color = Colors.White;
        shadowEffect.ShadowDepth = 3;
        Effect = shadowEffect;
        Content = button.ButtonName;
        HorizontalAlignment = HorizontalAlignment.Center;
        VerticalAlignment = VerticalAlignment.Top;
        Height = 75;
        Width = 200;
        Margin = new Thickness(0, 18, 32, 0);
        Background = new SolidColorBrush(Colors.DarkRed);
        BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
        FontFamily = new FontFamily("Area");
        FontSize = 25;
        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        TabIndex = 999;
        ControlTemplate myControlTemplate = new ControlTemplate(typeof(Button));
        FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
        border.Name = "border";
        border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
        border.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
        border.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
        border.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
        FrameworkElementFactory contentPresenterMenu = new FrameworkElementFactory(typeof(ContentPresenter));
        contentPresenterMenu.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
        contentPresenterMenu.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
        border.AppendChild(contentPresenterMenu);
        myControlTemplate.VisualTree = border;
        Template = myControlTemplate;
    }
}
