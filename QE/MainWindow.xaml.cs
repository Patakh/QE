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
using System.ComponentModel;
using FastReport.AdvMatrix;
using System.Windows.Media.Effects;
using System.Windows.Input;
using FastReport.DataVisualization.Charting;
using System.Threading;
using System.Windows.Controls.Primitives;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Controls.Button;
using Window = System.Windows.Window;
using TextBox = System.Windows.Controls.TextBox;
using System.Windows.Threading;

namespace QE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        public MainWindow()
        {

            InitializeComponent();

            #region Прочие настройки
            this.Icon = new BitmapImage(new System.Uri(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))).Replace("\\bin", "") + "/img/icon-eq.png", System.UriKind.Absolute));
            Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            AllowDrop = false;
            AllowsTransparency = false;
            WindowStyle = WindowStyle.None;
            PreviewKeyDown += HandleKeyPress;
            ImageFooter.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new System.Uri(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))).Replace("\\bin", "") + "/img/footer_img.png", System.UriKind.Absolute))
            };

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
             
            // Создаем таймер для обновления даты и времени каждую секунду
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            // Установка начальных значений даты и времени
            UpdateDateTime();

            HeaderTextBlock.FontFamily = new FontFamily("Area");
            HeaderTextBlock.FontSize = 40;
            HeaderTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(237, 216, 181));
            #endregion

            #region Кнопки на главной 
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
                     StackPanel stackPanelHeadMenu = new StackPanel();
                     stackPanelHeadMenu.Orientation = Orientation.Vertical;
                     stackPanelHeadMenu.VerticalAlignment = VerticalAlignment.Top;

                     TextBlock textBlockMenu = new TextBlock();
                     textBlockMenu.FontFamily = new FontFamily("Area");
                     textBlockMenu.FontSize = 60;
                     textBlockMenu.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
                     textBlockMenu.Text = eqContext.SOfficeTerminalButtons.First(t => t.ParentId == SOfficeTerminalButton.First().ParentId && t.ButtonType == 1).ButtonName;
                     stackPanelHeadMenu.Children.Add(textBlockMenu);

                     List<SService> sServices = new List<SService>();
                     StackPanel stackPanel = new StackPanel();
                     stackPanel.Orientation = Orientation.Vertical;
                     stackPanel.Visibility = Visibility.Collapsed;
                     stackPanel.Children.Add(stackPanelHeadMenu);
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
                         btn.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                         btn.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                         btn.FontFamily = new FontFamily("Area");
                         btn.FontSize = 20;
                         btn.Foreground = new SolidColorBrush(Color.FromRgb(135, 98, 27));
                         DropShadowEffect btnShadowEffect = new DropShadowEffect();
                         btnShadowEffect.Color = Color.FromRgb(22, 22, 22);
                         btnShadowEffect.Direction = 50;
                         btnShadowEffect.ShadowDepth = 2;
                         btn.Effect = btnShadowEffect;

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

                     BodyWindow.Children.Add(stackPanel);

                     btnMenu.Click += (s, e) =>
                     {
                         StackClose.Visibility = Visibility.Visible;
                         stackPanel.Visibility = Visibility.Visible;
                         Menu.Visibility = Visibility.Collapsed;
                         Buttons.Visibility = Visibility.Collapsed;
                     };

                     this.Menu.Children.Add(btnMenu);

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
                     btn.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                     btn.BorderBrush = new SolidColorBrush(Color.FromRgb(55, 55, 55));
                     btn.FontFamily = new FontFamily("Area");
                     btn.FontSize = 25;
                     btn.Foreground = new SolidColorBrush(Color.FromRgb(135, 98, 27));
                     DropShadowEffect shadowEffect = new DropShadowEffect();
                     shadowEffect.Color = Color.FromRgb(22, 22, 22);
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
                     Buttons.Children.Add(btn);
                 }
             });
            #endregion

            #region блок "Оценить качество обслуживания" 

            TextBlock textBlockEstimate = new TextBlock();
            textBlockEstimate.FontFamily = new FontFamily("Area");
            textBlockEstimate.FontSize = 60;
            textBlockEstimate.HorizontalAlignment = HorizontalAlignment.Center;
            textBlockEstimate.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
            textBlockEstimate.Text = "Введите номер дела";

            TextBox textBox = new TextBox();
            textBox.FontSize = 35;
            textBox.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 100));
            textBox.Margin = new Thickness(0, 50, 0, 20);
            textBox.TextWrapping = TextWrapping.Wrap;
            textBox.Focus();

            StackPanel stackPanelHeadEstimate = new StackPanel();
            stackPanelHeadEstimate.Orientation = Orientation.Vertical;
            stackPanelHeadEstimate.VerticalAlignment = VerticalAlignment.Top;
            stackPanelHeadEstimate.Children.Add(textBlockEstimate);
            stackPanelHeadEstimate.Children.Add(textBox);

            StackPanel stackPanelEstimate = new StackPanel();
            stackPanelEstimate.Orientation = Orientation.Vertical;
            stackPanelEstimate.VerticalAlignment = VerticalAlignment.Top;
            stackPanelEstimate.Visibility = Visibility.Collapsed;
            stackPanelEstimate.HorizontalAlignment = HorizontalAlignment.Center;
            stackPanelEstimate.Name = "Estimate";
            stackPanelEstimate.Children.Add(stackPanelHeadEstimate);

            //клавиатура
            StackPanel stackPanelKeyboard = new StackPanel();
            stackPanelKeyboard.Children.Add((StackPanel)this.FindResource("Keyboard"));
             
            bool upperCase = true;
            foreach (StackPanel item in stackPanelKeyboard.Children)
            {
                foreach (StackPanel stackPanel in item.Children)
                {
                    foreach (Button button in stackPanel.Children)
                    {
                        button.Background = new SolidColorBrush(Colors.Blue);
                        button.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        button.Click += (s, e) =>
                        {
                            Button buttonClick = (Button)s;
                            switch (buttonClick.Content.ToString())
                            {
                                case "EN":
                                    button.Content = "RU";
                                    //Keyboard_RU
                                    item.Children[1].Visibility = Visibility.Visible;
                                    item.Children[2].Visibility = Visibility.Visible;
                                    item.Children[3].Visibility = Visibility.Visible;

                                    //Keyboard_EN
                                    item.Children[4].Visibility = Visibility.Collapsed;
                                    item.Children[5].Visibility = Visibility.Collapsed;
                                    item.Children[6].Visibility = Visibility.Collapsed;
                                    break;

                                case "RU":
                                    button.Content = "EN";
                                    //Keyboard_RU
                                    item.Children[1].Visibility = Visibility.Collapsed;
                                    item.Children[2].Visibility = Visibility.Collapsed;
                                    item.Children[3].Visibility = Visibility.Collapsed;

                                    //Keyboard_EN
                                    item.Children[4].Visibility = Visibility.Visible;
                                    item.Children[5].Visibility = Visibility.Visible;
                                    item.Children[6].Visibility = Visibility.Visible;
                                    break;

                                case "Удалить":
                                    textBox.Text = textBox.Text.Length == 0 ? "" : textBox.Text.Substring(0, textBox.Text.Length - 1);
                                    break;
                                case "Пробел":
                                    textBox.Text += " ";
                                    break;
                                case "Очистить":
                                    textBox.Text = "";
                                    break;
                                case "Ввод":
                                    textBox.Text = "";
                                    break;
                                case "Регистр":
                                    upperCase = !upperCase;
                                    if (!upperCase)
                                    {
                                        button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                        button.Foreground = new SolidColorBrush(Colors.Blue);
                                    }
                                    else
                                    {
                                        button.Background = new SolidColorBrush(Colors.Blue);
                                        button.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                    }
                                    break;
                                default:
                                    textBox.Text += upperCase ? button.Content.ToString().ToLower() : button.Content.ToString().ToUpper();
                                    break;
                            }
                            textBox.CaretIndex = textBox.Text.Length;
                            textBox.Focus();
                        };
                    }
                }
            }
            stackPanelEstimate.Children.Add(stackPanelKeyboard);
            BodyWindow.Children.Add(stackPanelEstimate);
            this.Button_Click_Estimate.Click += (s, e) =>
            {
                Button_Click_Estimate.Background = new SolidColorBrush(Color.FromRgb(240, 250, 220));
                foreach (StackPanel obj in BodyWindow.Children)
                {
                    obj.Visibility = Visibility.Collapsed;
                    if (obj.Name == "Estimate")
                    {
                        obj.Visibility = Visibility.Visible;
                    }
                }
                StackClose.Visibility = Visibility.Visible;
            };
            #endregion

            #region Кнопка Домой

            CloseButton.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new System.Uri(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))).Replace("\\bin", "") + "/img/home_red_icon.jpg", System.UriKind.Absolute))
            };
            this.CloseButton.Click += (s, e) =>
            {
                foreach (StackPanel obj in BodyWindow.Children)
                {
                    if (obj.Name == "Menu" || obj.Name == "Buttons")
                    {
                        obj.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        obj.Visibility = Visibility.Collapsed;
                    }
                }
                StackClose.Visibility = Visibility.Hidden;
                Button_Click_Estimate.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)); ;
            };
            #endregion 

        }

        #region Поставка на очередь
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
            var path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))).Replace("\\bin", "") + "\\FastReport\\Operator.frx";
            report.Load(path);
            if (eqContext.DTickets.Where(s => s.SOfficeTerminal.IpAddress == IpOffise).OrderBy(d => d.TimeRegistration).Any())
            {
                DTicket dTicket_Last = eqContext.DTickets.Where(s => s.SOfficeTerminal.IpAddress == IpOffise).OrderBy(d => d.TimeRegistration).OrderBy(o => o.DateRegistration).Last();
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
        #endregion

        #region закритие приложения
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == System.Windows.Input.Key.Escape)
            {
                // Закрываем приложение
                Application.Current.Shutdown();
            }
        }
        #endregion

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Обновляем дату и время при каждом срабатывании таймера
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            // Обновляем значения даты и времени
            DateTime now = DateTime.Now;
            HeaderTextBlock.Text = now.ToString("dddd")+" " + now.ToString("D") +" "+ now.ToString("HH:mm:ss"); 
        }
    }
}
