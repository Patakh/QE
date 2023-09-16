using Accessibility;
using FastReport.DevComponents.DotNetBar;
using FastReport.DevComponents.UI.ContentManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Win32;
using Microsoft.Windows.Themes;
using QE.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.ComponentModel;
using FastReport.AdvMatrix;
using System.Windows.Media.Effects;
using System.Windows.Input;
using FastReport.DataVisualization.Charting;
using System.Threading;
using System.Windows.Controls.Primitives;

namespace QE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //настройки главного окна
            this.Icon = new BitmapImage(new System.Uri(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))) + "/img/icon-eq.png", System.UriKind.Absolute));
            Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            AllowDrop = false;
            AllowsTransparency = false;
            WindowStyle = WindowStyle.None;
            PreviewKeyDown += HandleKeyPress;
            /*GridMain.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new System.Uri(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))) + "/img/icon-eq.png", System.UriKind.Absolute))
            };*/

            //подключение к базе
            EqContext eqContext = new EqContext();
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            string IpOffise = "";

            foreach (IPAddress address in localIPs)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IpOffise = address.ToString();
                }
            }

            Title = eqContext.SOfficeTerminals.First(s => s.IpAddress == IpOffise).TerminalName;
            int Btn_idx = 1;

            //Заголовок
            StackPanel stackPanelHeader = new StackPanel();
            stackPanelHeader.Margin = new Thickness(0, 18, 0, 0);
            TextBlock textBlock = new TextBlock();
            textBlock.FontFamily = new FontFamily("Area");
            textBlock.FontSize = 60;
            textBlock.Foreground = new SolidColorBrush(Color.FromRgb(237, 216, 181));
            textBlock.Text = eqContext.SOffices.First(l => l.Id == eqContext.SOfficeTerminals.First(g => g.IpAddress == IpOffise).SOfficeId).OfficeName;

            stackPanelHeader.Children.Add(textBlock);
            GridMain.Children.Add(stackPanelHeader); 
            eqContext.SOfficeTerminalButtons.Where(s => s.SOfficeTerminal.IpAddress == IpOffise).ToList().ForEach(b =>
             {
                  

                 if (b.ButtonType == 1) // 1 - Меню. 2 - Кнопка
                 {
                     //создаем кнопку перехода на меню
                     Button btnMenu = new Button();
                     DropShadowEffect shadowEffect = new DropShadowEffect();
                     shadowEffect.Color = Colors.White;
                     shadowEffect.ShadowDepth = 3;
                     btnMenu.Effect = shadowEffect;
                     btnMenu.Name = "button" + Btn_idx;
                     btnMenu.Content = b.ButtonName;
                     btnMenu.HorizontalAlignment = HorizontalAlignment.Left;
                     btnMenu.VerticalAlignment = VerticalAlignment.Top;
                     btnMenu.Height = 75;
                     btnMenu.Width = 200;
                     btnMenu.Margin = new Thickness(0, 18, 32, 0);
                     btnMenu.Background = new SolidColorBrush(Colors.DarkRed);
                     btnMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
                     btnMenu.FontFamily = new FontFamily("Area");
                     btnMenu.FontSize = 25;
                     btnMenu.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                     btnMenu.TabIndex = 999;

                     ControlTemplate myControlTemplate = new ControlTemplate(typeof(Button));
                     FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
                     border.Name = "border";
                     border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
                     border.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
                     border.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
                     border.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
                     FrameworkElementFactory contentPresenterMenu = new FrameworkElementFactory(typeof(ContentPresenter));
                     contentPresenterMenu.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                     contentPresenterMenu.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                     border.AppendChild(contentPresenterMenu);
                     myControlTemplate.VisualTree = border;
                     btnMenu.Template = myControlTemplate;

                     //находим все кнопки этого меню
                     var SOfficeTerminalButton = eqContext.SOfficeTerminalButtons.Where(q => q.SOfficeTerminalId == b.SOfficeTerminalId && q.ParentId == b.ParentId && q.ButtonType != 1);

                     List<SService> sServices = new List<SService>();

                     // Создание нового окна
                     Window newWindow = new Window
                     {
                         Title = eqContext.SOfficeTerminalButtons.First(t => t.ParentId == SOfficeTerminalButton.First().ParentId && t.ButtonType == 1).ButtonName,
                         WindowStartupLocation = WindowStartupLocation.CenterScreen,
                         Background = new SolidColorBrush(Color.FromRgb(173, 213, 222)),
                         AllowDrop = false,
                         AllowsTransparency = false,
                     };

                     StackPanel stackPanel = new StackPanel();
                     stackPanel.Margin = new Thickness(32, 100, 0, 0);
                     stackPanel.Orientation = Orientation.Horizontal;

                     // Отображение нового окна внутри основного окна
                     newWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                     //Заголовок меню
                     StackPanel stackPanelHeaderMenu = new StackPanel();
                     stackPanelHeaderMenu.Margin = new Thickness(64, 18, 0, 0);

                     TextBlock textBlockMenu = new TextBlock();
                     textBlockMenu.FontFamily = new FontFamily("Area");
                     textBlockMenu.FontSize = 60;
                     textBlockMenu.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
                     textBlockMenu.Text = eqContext.SOfficeTerminalButtons.First(t => t.ParentId == SOfficeTerminalButton.First().ParentId && t.ButtonType == 1).ButtonName;
                     stackPanelHeaderMenu.Children.Add(textBlockMenu);

                     //создаем кнопки меню
                     SOfficeTerminalButton.ToList().ForEach(button =>
                     {
                         int Btn_idx = 1;
                         SService sServices = eqContext.SServices.First(f => f.Id == button.SServiceId);
                         Button btn = new Button();
                         btn.Name = "button" + Btn_idx;
                         btn.Content = button.ButtonName;
                         btn.HorizontalAlignment = HorizontalAlignment.Left;
                         btn.VerticalAlignment = VerticalAlignment.Top;
                         btn.Height = 75;
                         btn.Width = 200;
                         btn.Margin = new Thickness(32, 18, 0, 0);
                         btn.Background = new SolidColorBrush(Color.FromRgb(1, 200, 1));
                         btn.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
                         btn.FontFamily = new FontFamily("Area");
                         btn.FontSize = 20;
                         btn.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                         DropShadowEffect btnshadowEffect = new DropShadowEffect();
                         btnshadowEffect.Color = Colors.AliceBlue;
                         btnshadowEffect.Direction = 50;
                         btnshadowEffect.ShadowDepth = 2;
                         btn.Effect = btnshadowEffect;

                         ControlTemplate myControlTemplate = new ControlTemplate(typeof(Button));
                         FrameworkElementFactory btnBorder = new FrameworkElementFactory(typeof(Border));
                         btnBorder.Name = "border";
                         btnBorder.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
                         btnBorder.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
                         btnBorder.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
                         btnBorder.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
                         FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
                         contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                         contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                         btnBorder.AppendChild(contentPresenter);
                         myControlTemplate.VisualTree = btnBorder;
                         btn.Template = myControlTemplate;
                         btn.Click += (s, e) =>
                         {
                          
                             Click_Button(s, e, sServices);
                             this.Show(); 
                         };
                         stackPanel.Children.Add(btn);

                     });
                     newWindow.WindowStyle = WindowStyle.None;
                     newWindow.WindowState = WindowState.Maximized;

                     //кнопка назад
                     StackPanel stackPanelClose = new StackPanel();
                     stackPanelClose.Orientation = Orientation.Horizontal;
                     stackPanelClose.VerticalAlignment = VerticalAlignment.Bottom;
                     Button btnClose = new Button();
                     btnClose.Content = "< Назад";
                     btnClose.HorizontalAlignment = HorizontalAlignment.Left;
                     btnClose.VerticalAlignment = VerticalAlignment.Top;
                     btnClose.Height = 50;
                     btnClose.Width = 200;
                     btnClose.Margin = new Thickness(64, 32, 32, 32);
                     btnClose.Background = new SolidColorBrush(Color.FromRgb(255, 200, 1));
                     btnClose.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
                     btnClose.FontFamily = new FontFamily("Area");
                     btnClose.FontSize = 20;
                     btnClose.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                     DropShadowEffect btnCloseShadowEffect = new DropShadowEffect();
                     btnCloseShadowEffect.Color = Colors.AliceBlue;
                     btnCloseShadowEffect.Direction = 50;
                     btnCloseShadowEffect.ShadowDepth = 2;
                     btnClose.Effect = btnCloseShadowEffect;
                     ControlTemplate btnmyControlCloseTemplate = new ControlTemplate(typeof(Button));
                     FrameworkElementFactory btnCloseBorder = new FrameworkElementFactory(typeof(Border));
                     btnCloseBorder.Name = "border";
                     btnCloseBorder.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
                     btnCloseBorder.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
                     btnCloseBorder.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
                     btnCloseBorder.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
                     FrameworkElementFactory btnCloseContentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
                     btnCloseContentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                     btnCloseContentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                     btnCloseBorder.AppendChild(btnCloseContentPresenter);
                     btnmyControlCloseTemplate.VisualTree = btnCloseBorder;

                     //обрабочик кнопки назад
                     btnClose.Click += (s, e) =>
                    {
                       newWindow.Hide();
                       this.Show();
                    };

                     System.Windows.Controls.Grid grid = new System.Windows.Controls.Grid();
                     stackPanelClose.Children.Add(btnClose); 
                     //вставляем все на свои места
                     grid.Children.Add(stackPanelHeaderMenu); //заголовок / название менню
                     grid.Children.Add(stackPanel);           //содержание / кнопки
                     grid.Children.Add(stackPanelClose);      // подвал / кнопка назад
                     newWindow.Content = grid;

                     //обрабочик кнопки меню
                     btnMenu.Click += (s, e) =>
                     {
                        //показываем результат
                        newWindow.Show();
                        this.Hide();

                     };

                     menu.Children.Add(btnMenu);
                 }

                 else
               if (b.ParentId == 0)
                 {
                     SService sServices = eqContext.SServices.First(f => f.Id == b.SServiceId);
                     Button btn = new Button();
                     btn.Name = "button" + Btn_idx;
                     btn.Content = b.ButtonName;
                     btn.HorizontalAlignment = HorizontalAlignment.Left;
                     btn.VerticalAlignment = VerticalAlignment.Top;
                     btn.Height = 75;
                     btn.Width = 200;
                     btn.Margin = new Thickness(0, 18, 32, 0);
                     btn.Background = new SolidColorBrush(Color.FromRgb(135, 98, 27));
                     btn.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
                     btn.FontFamily = new FontFamily("Area");
                     btn.FontSize = 25;
                     btn.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                     DropShadowEffect shadowEffect = new DropShadowEffect();
                     shadowEffect.Color = Color.FromRgb(180, 224, 201);
                     shadowEffect.Direction = 315;
                     shadowEffect.ShadowDepth = 3;
                     btn.Effect = shadowEffect;

                     btn.Click += (s, e) =>
                     { 
                         Click_Button(s, e, sServices); 
                     };

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
                     btn.Template = myControlTemplate;
                     buttons.Children.Add(btn);
                 }
             });
        }
          
        private async void Click_Button(object sender, RoutedEventArgs e, SService sService)
        { 
            EqContext eqContext = new EqContext();
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            string IpOffise = "";
            foreach (IPAddress address in localIPs)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IpOffise = address.ToString();
                }
            }
            FastReport.Report report = new FastReport.Report();
            var path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))) + "\\FastReport\\Operator.frx";
            report.Load(path);
            if (eqContext.DTickets.Where(s => s.SOfficeTerminal.IpAddress == IpOffise).OrderBy(d => d.TimeRegistration).Any())
            { 
                DTicket dTicket_Last = eqContext.DTickets.Where(s => s.SOfficeTerminal.IpAddress == IpOffise).OrderBy(d => d.TimeRegistration).OrderBy(o=>o.DateRegistration).Last();
                DTicket dTicket_New = new DTicket();
                dTicket_New.SOfficeId = eqContext.SOfficeTerminals.First(s => s.IpAddress == IpOffise).SOfficeId;
                dTicket_New.SOfficeTerminalId = eqContext.SOfficeTerminals.First(s => s.IpAddress == IpOffise).Id;
                dTicket_New.SServiceId = sService.Id;
                dTicket_New.ServicePrefix = sService.ServicePrefix;
                //dTicket.SPriorityId = 1;
                dTicket_New.ServicePriority = 1;
                dTicket_New.TicketNumber = dTicket_Last.TicketNumber + 1;
                dTicket_New.TicketNumberFull = dTicket_Last.ServicePrefix + (dTicket_Last.TicketNumber + 1);
                //dTicket.DTicketPrerecordId = 1;
                dTicket_New.SStatusId = 1;
                //dTicket.SEmployeeId = 1;
                //dTicket.SOfficeWindowId = 1;
                dTicket_New.DateRegistration = DateOnly.FromDateTime(DateTime.Now);
                dTicket_New.TimeRegistration = TimeOnly.FromDateTime(DateTime.Now);

                eqContext.DTickets.Add(dTicket_New);
                eqContext.SaveChanges(); 
                report.SetParameterValue("Operation", sService.ServiceName);
                report.SetParameterValue("Number", dTicket_New.TicketNumberFull);
                report.SetParameterValue("Time", dTicket_New.TimeRegistration);
                report.SetParameterValue("TotalQueue", eqContext.DTickets.Count());
                report.SetParameterValue("BeforeCount", eqContext.DTickets.Where(s => s.ServicePrefix == sService.ServicePrefix).Count());
                report.SetParameterValue("MFC", eqContext.SOffices.First(s => s.Id == 1).OfficeName);
                report.Prepare();
                report.PrintSettings.ShowDialog = false;
                report.Print(); 
            }
            else
            { 
                DTicket dTicket = new DTicket();
                dTicket.SOfficeId = eqContext.SOfficeTerminals.First(s => s.IpAddress == IpOffise).SOfficeId;
                dTicket.SOfficeTerminalId = eqContext.SOfficeTerminals.First(s => s.IpAddress == IpOffise).Id;
                dTicket.SServiceId = 1;
                dTicket.ServicePrefix = "A";
                //dTicket.SPriorityId = 1;
                dTicket.ServicePriority = 1;
                dTicket.TicketNumber = 1;
                dTicket.TicketNumberFull = "A1";
                //dTicket.DTicketPrerecordId = 1;
                dTicket.SStatusId = 1;
                //dTicket.SEmployeeId = 1;
                //dTicket.SOfficeWindowId = 1;
                dTicket.DateRegistration = DateOnly.FromDateTime(DateTime.Now);
                dTicket.TimeRegistration = TimeOnly.FromDateTime(DateTime.Now);
                eqContext.DTickets.Add(dTicket);
                eqContext.SaveChanges();

                report.SetParameterValue("Operation", sService.ServiceName);
                report.SetParameterValue("Number", sService.ServicePrefix + 1);
                report.SetParameterValue("Time", dTicket.TimeRegistration);
                report.SetParameterValue("TotalQueue", "1");
                report.SetParameterValue("BeforeCount", "1");
                report.SetParameterValue("MFC", eqContext.SOffices.First(s => s.Id == eqContext.SOfficeTerminals.First(k => k.IpAddress == IpOffise).Id).OfficeName);
                report.Prepare();

                report.PrintSettings.ShowDialog = false;
                report.PrintSettings.PrintOnSheetRawPaperSize = 89; 
                  report.Print();
            } 
        }

        //закритие приложения
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == System.Windows.Input.Key.Escape)
            {
                // Закрываем приложение
                Application.Current.Shutdown();
            }
        } 
    }
}
