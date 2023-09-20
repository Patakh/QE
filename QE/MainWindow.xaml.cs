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


            // Офис
            HeaderTextBlockOfice.FontFamily = new FontFamily("Area");
            HeaderTextBlockOfice.FontSize = 30;
            HeaderTextBlockOfice.Foreground = new SolidColorBrush(Color.FromRgb(44, 54, 75));
            HeaderTextBlockOfice.Text = eqContext.SOffices.First(l => l.Id == eqContext.SOfficeTerminals.First(g => g.IpAddress == IpOffise).SOfficeId).OfficeName;
            #endregion

            #region Кнопки на главной 
            eqContext.SOfficeTerminalButtons.Where(s => s.SOfficeTerminal.IpAddress == IpOffise).OrderBy(o => o.ButtonType).ToList().ForEach(b =>
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
                     btnMenu.HorizontalAlignment = HorizontalAlignment.Center;
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

                     //все кнопки этого меню
                     var SOfficeTerminalButton = eqContext.SOfficeTerminalButtons.Where(q => q.SOfficeTerminalId == b.SOfficeTerminalId && q.ParentId == b.ParentId && q.ButtonType != 1);

                     //Заголовок меню
                     TextBlock textBlockMenu = new TextBlock();
                     textBlockMenu.FontFamily = new FontFamily("Area");
                     textBlockMenu.FontSize = 60;
                     textBlockMenu.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
                     textBlockMenu.Text = eqContext.SOfficeTerminalButtons.First(t => t.ParentId == SOfficeTerminalButton.First().ParentId && t.ButtonType == 1).ButtonName;

                     WrapPanel warpPanelHeadMenu = new WrapPanel();
                     warpPanelHeadMenu.Orientation = Orientation.Horizontal;
                     warpPanelHeadMenu.VerticalAlignment = VerticalAlignment.Center;
                     warpPanelHeadMenu.Visibility = Visibility.Collapsed;
                     warpPanelHeadMenu.Margin = new Thickness(25,0,0,0);
                     warpPanelHeadMenu.Children.Add(textBlockMenu);
                     Buttnos.Children.Add(warpPanelHeadMenu);

                     //создаем кнопки меню
                     List<SService> sServices = new List<SService>();
                     WrapPanel wrapPanel = new WrapPanel();
                     wrapPanel.Orientation = Orientation.Horizontal;
                     wrapPanel.Visibility = Visibility.Collapsed;
                     wrapPanel.MaxWidth = 800; 
                     SOfficeTerminalButton.ToList().ForEach(button =>
                     {
                         int Btn_idx = 1;
                         SService sServices = eqContext.SServices.First(f => f.Id == button.SServiceId);
                         Button btn = new Button();
                         btn.Name = "button" + Btn_idx;
                         btn.Content = button.ButtonName;
                         btn.HorizontalAlignment = HorizontalAlignment.Center;
                         btn.VerticalAlignment = VerticalAlignment.Center;
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
                         };
                         wrapPanel.Children.Add(btn);
                     });
                     Buttnos.Children.Add(wrapPanel);

                     btnMenu.Click += (s, e) =>
                     {
                         StackClose.Visibility = Visibility.Visible;
                         wrapPanel.Visibility = Visibility.Visible;
                         warpPanelHeadMenu.Visibility = Visibility.Visible;
                         Buttnos.Visibility = Visibility.Visible;
                         Menu_Buttnos.Visibility = Visibility.Collapsed;
                     };

                     this.Menu_Buttnos.Children.Add(btnMenu);

                 }

                 else
               if (b.ParentId == 0)
                 {
                     SService sServices = eqContext.SServices.First(f => f.Id == b.SServiceId);
                     Button btn = new Button();
                     btn.Name = "button" + Btn_idx;
                     btn.Content = b.ButtonName;
                     btn.HorizontalAlignment = HorizontalAlignment.Center;
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

                     StackPanel stackPanelBtn = new StackPanel();
                     stackPanelBtn.Orientation = Orientation.Vertical;
                     stackPanelBtn.HorizontalAlignment = HorizontalAlignment.Center;
                     stackPanelBtn.Children.Add(btn);
                     Menu_Buttnos.Children.Add(stackPanelBtn);

                 }
             });
            #endregion

            #region Режим работы 
            TextBlock textSchedulesHead = new TextBlock();
            textSchedulesHead.FontFamily = new FontFamily("Area");
            textSchedulesHead.FontSize = 30;
            textSchedulesHead.HorizontalAlignment = HorizontalAlignment.Center;
            textSchedulesHead.Margin = new Thickness(0, 0, 0, 20);
            textSchedulesHead.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
            textSchedulesHead.FontWeight = FontWeights.Bold;
            textSchedulesHead.Text = "Режим работы";
            Schedules.Children.Add(textSchedulesHead);

            eqContext.SOfficeSchedules.Where(k => k.SOfficeId == eqContext.SOffices.First(l => l.Id == eqContext.SOfficeTerminals.First(g => g.IpAddress == IpOffise).SOfficeId).Id).ToList().ForEach(r =>
               {
                   TextBlock textBlockDayWeek = new TextBlock();
                   textBlockDayWeek.FontFamily = new FontFamily("Area");
                   textBlockDayWeek.FontSize = 25;
                   textBlockDayWeek.HorizontalAlignment = HorizontalAlignment.Left;
                   textBlockDayWeek.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
                   textBlockDayWeek.Text = eqContext.SDayWeeks.First(l => l.Id == r.SDayWeekId).DayName;
                   textBlockDayWeek.Width = 250;

                   TextBlock textBlockTime = new TextBlock();
                   textBlockTime.FontFamily = new FontFamily("Area");
                   textBlockTime.FontSize = 25;
                   textBlockTime.HorizontalAlignment = HorizontalAlignment.Right;
                   textBlockTime.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
                   textBlockTime.Text = r.StartTime + " - " + r.StopTime;


                   StackPanel stackPanelSchedules = new StackPanel();
                   stackPanelSchedules.Orientation = Orientation.Horizontal;
                   stackPanelSchedules.HorizontalAlignment = HorizontalAlignment.Center;
                   stackPanelSchedules.Children.Add(textBlockDayWeek);
                   stackPanelSchedules.Children.Add(textBlockTime);

                   Schedules.Children.Add(stackPanelSchedules);
               });
            #endregion

            #region Кнопка "Предварительная запись" 

            TextBlock textBlockPre_registration = new TextBlock();
            textBlockPre_registration.FontFamily = new FontFamily("Area");
            textBlockPre_registration.FontSize = 60;
            textBlockPre_registration.HorizontalAlignment = HorizontalAlignment.Center;
            textBlockPre_registration.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
            textBlockPre_registration.Text = "Введите номер дела";

            TextBox textBoxPre_registration = new TextBox();
            textBoxPre_registration.FontSize = 35;
            textBoxPre_registration.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 100));
            textBoxPre_registration.Margin = new Thickness(0, 50, 0, 20);
            textBoxPre_registration.TextWrapping = TextWrapping.Wrap;
            textBoxPre_registration.Focus();

            StackPanel stackPanelHeadPre_registration = new StackPanel();
            stackPanelHeadPre_registration.Orientation = Orientation.Vertical;
            stackPanelHeadPre_registration.VerticalAlignment = VerticalAlignment.Top;
            stackPanelHeadPre_registration.Children.Add(textBlockPre_registration);
            stackPanelHeadPre_registration.Children.Add(textBoxPre_registration);

            WrapPanel wrapPanelPre_registration = new WrapPanel();
            wrapPanelPre_registration.Orientation = Orientation.Vertical;
            wrapPanelPre_registration.VerticalAlignment = VerticalAlignment.Top;
            wrapPanelPre_registration.Visibility = Visibility.Collapsed;
            wrapPanelPre_registration.HorizontalAlignment = HorizontalAlignment.Center;
            wrapPanelPre_registration.Name = "Pre_registration";
            wrapPanelPre_registration.Children.Add(stackPanelHeadPre_registration);

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
                                    textBoxPre_registration.Text = textBoxPre_registration.Text.Length == 0 ? "" : textBoxPre_registration.Text.Substring(0, textBoxPre_registration.Text.Length - 1);
                                    break;
                                case "Пробел":
                                    textBoxPre_registration.Text += " ";
                                    break;
                                case "Очистить":
                                    textBoxPre_registration.Text = "";
                                    break;
                                case "Ввод":
                                    textBoxPre_registration.Text = "";
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
                                    textBoxPre_registration.Text += upperCase ? button.Content.ToString().ToLower() : button.Content.ToString().ToUpper();
                                    break;
                            }
                            textBoxPre_registration.CaretIndex = textBoxPre_registration.Text.Length;
                            textBoxPre_registration.Focus();
                        };
                    }
                }
            }
            wrapPanelPre_registration.Children.Add(stackPanelKeyboard);
            BodyWindow.Children.Add(wrapPanelPre_registration);
            this.Button_Click_Pre_registration.Click += (s, e) =>
            {
                Button_Click_Pre_registration.Background = new SolidColorBrush(Color.FromRgb(240, 250, 220));
                foreach (WrapPanel obj in BodyWindow.Children)
                {
                    obj.Visibility = Visibility.Collapsed;
                    if (obj.Name == "Pre_registration")
                    {
                        obj.Visibility = Visibility.Visible;
                    }
                }
                StackClose.Visibility = Visibility.Visible;
            };
            #endregion

            #region Кнопка "Льготная категория граждан" 
            TextBlock textBlockPreferentialСategoryСitizens = new TextBlock();
            textBlockPreferentialСategoryСitizens.FontFamily = new FontFamily("Area");
            textBlockPreferentialСategoryСitizens.FontSize = 60;
            textBlockPreferentialСategoryСitizens.HorizontalAlignment = HorizontalAlignment.Center;
            textBlockPreferentialСategoryСitizens.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
            textBlockPreferentialСategoryСitizens.Text = "Введите номер дела";

            TextBox textBoxPreferentialСategoryСitizens = new TextBox();
            textBoxPreferentialСategoryСitizens.FontSize = 35;
            textBoxPreferentialСategoryСitizens.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 100));
            textBoxPreferentialСategoryСitizens.Margin = new Thickness(0, 50, 0, 20);
            textBoxPreferentialСategoryСitizens.TextWrapping = TextWrapping.Wrap;
            textBoxPreferentialСategoryСitizens.Focus();

            StackPanel stackPanelHeadPreferentialСategoryСitizens = new StackPanel();
            stackPanelHeadPreferentialСategoryСitizens.Orientation = Orientation.Vertical;
            stackPanelHeadPreferentialСategoryСitizens.VerticalAlignment = VerticalAlignment.Top;
            stackPanelHeadPreferentialСategoryСitizens.Children.Add(textBlockPreferentialСategoryСitizens);
            stackPanelHeadPreferentialСategoryСitizens.Children.Add(textBoxPreferentialСategoryСitizens);

            WrapPanel wrapPanelPreferentialСategoryСitizens = new WrapPanel();
            wrapPanelPreferentialСategoryСitizens.Orientation = Orientation.Vertical;
            wrapPanelPreferentialСategoryСitizens.VerticalAlignment = VerticalAlignment.Top;
            wrapPanelPreferentialСategoryСitizens.Visibility = Visibility.Collapsed;
            wrapPanelPreferentialСategoryСitizens.HorizontalAlignment = HorizontalAlignment.Center;
            wrapPanelPreferentialСategoryСitizens.Name = "PreferentialСategoryСitizens";
            wrapPanelPreferentialСategoryСitizens.Children.Add(stackPanelHeadPreferentialСategoryСitizens);

            //клавиатура
            StackPanel stackPanelKeyboardPreferentialСategoryСitizens = new StackPanel();
            stackPanelKeyboardPreferentialСategoryСitizens.Children.Add((StackPanel)this.FindResource("KeyboardNumber")); 
            foreach (StackPanel item in stackPanelKeyboardPreferentialСategoryСitizens.Children)
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
                               

                                case "Удалить":
                                    textBoxPreferentialСategoryСitizens.Text = textBoxPreferentialСategoryСitizens.Text.Length == 0 ? "" : textBoxPreferentialСategoryСitizens.Text.Substring(0, textBoxPreferentialСategoryСitizens.Text.Length - 1);
                                    break;
                               
                                case "Ввод":
                                    textBoxPreferentialСategoryСitizens.Text = "";
                                    break;
                                default:
                                    textBoxPreferentialСategoryСitizens.Text += upperCase ? button.Content.ToString().ToLower() : button.Content.ToString().ToUpper();
                                    break;
                            }
                            textBoxPreferentialСategoryСitizens.CaretIndex = textBoxPreferentialСategoryСitizens.Text.Length;
                            textBoxPreferentialСategoryСitizens.Focus();
                        };
                    }
                }
            }
            
            wrapPanelPreferentialСategoryСitizens.Children.Add(stackPanelKeyboardPreferentialСategoryСitizens);
            BodyWindow.Children.Add(wrapPanelPreferentialСategoryСitizens);
            
            this.Button_Click_PreferentialСategoryСitizens.Click += (s, e) =>
            {
                Button_Click_PreferentialСategoryСitizens.Background = new SolidColorBrush(Color.FromRgb(240, 250, 220));
                foreach (WrapPanel obj in BodyWindow.Children)
                {
                    obj.Visibility = Visibility.Collapsed;
                    if (obj.Name == "PreferentialСategoryСitizens")
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
                foreach (WrapPanel obj in Buttnos.Children)
                {
                    obj.Visibility = Visibility.Collapsed;
                }
                foreach (WrapPanel obj in BodyWindow.Children)
                {
                    if (obj.Name == "Menu_Buttnos" || obj.Name == "Schedules")
                    {
                        obj.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        obj.Visibility = Visibility.Collapsed;
                    }
                }
                StackClose.Visibility = Visibility.Hidden;
                Button_Click_Pre_registration.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)); ;
            };
            #endregion
        }

        #region Поставка на очередь
        private async Task Click_Button(object sender, RoutedEventArgs e, SService sService)
        {
            EqContext eqContext = new EqContext();
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            string IpOffise = localIPs.Where(w => w.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(w => w.ToString()).First();

            FastReport.Report report = new FastReport.Report();
            var path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))).Replace("\\bin", "") + "\\FastReport\\Operator.frx";
            report.Load(path);
            var LastTicketNumber = await eqContext.DTickets.Where(s => s.SOfficeTerminal.IpAddress == IpOffise && s.SServiceId == sService.Id && s.DateRegistration == DateOnly.FromDateTime(DateTime.Now)).OrderByDescending(o => o.TicketNumber).Select(s => s.TicketNumber).FirstOrDefaultAsync();

            DTicket dTicket_New = new DTicket();
            dTicket_New.SOfficeId = eqContext.SOfficeTerminals.First(s => s.IpAddress == IpOffise).SOfficeId;
            dTicket_New.SOfficeTerminalId = eqContext.SOfficeTerminals.First(s => s.IpAddress == IpOffise).Id;
            dTicket_New.SServiceId = sService.Id;
            dTicket_New.ServicePrefix = sService.ServicePrefix;
            //dTicket.SPriorityId = 1; 
            dTicket_New.TicketNumber = LastTicketNumber + 1;
            dTicket_New.TicketNumberFull = sService.ServicePrefix + (LastTicketNumber + 1);
            //dTicket.DTicketPrerecordId = 1;
            dTicket_New.SStatusId = 1;
            //dTicket.SEmployeeId = 1;
            //dTicket.SOfficeWindowId = 1;
            dTicket_New.DateRegistration = DateOnly.FromDateTime(DateTime.Now);
            dTicket_New.TimeRegistration = TimeOnly.FromDateTime(DateTime.Now);

            DTicketStatus dTicketStatus = new DTicketStatus
            {
                // DTicketId = eqContext.DTickets.First(s => s.SOfficeTerminal.IpAddress == IpOffise && s.DateRegistration == dTicket_New.DateRegistration && s.TimeRegistration == dTicket_New.TimeRegistration).Id,
                SStatusId = 1
            };

            dTicket_New.DTicketStatuses.Add(dTicketStatus);

            await eqContext.DTickets.AddAsync(dTicket_New);
            await eqContext.SaveChangesAsync();

            report.SetParameterValue("Operation", sService.ServiceName);
            report.SetParameterValue("Number", dTicket_New.TicketNumberFull);
            report.SetParameterValue("Time", dTicket_New.TimeRegistration);
            report.SetParameterValue("TotalQueue", eqContext.DTickets.Where(s => s.SOfficeTerminal.IpAddress == IpOffise && s.DateRegistration == DateOnly.FromDateTime(DateTime.Now)).Count());
            report.SetParameterValue("BeforeCount", LastTicketNumber);
            report.SetParameterValue("MFC", eqContext.SOffices.First(s => s.Id == 1).OfficeName);
            report.Prepare();
            report.PrintSettings.ShowDialog = false;
            report.PrintSettings.PrintOnSheetRawPaperSize = 0;
            report.Print();
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
            TextBlock textBlockD = new TextBlock();
            textBlockD.FontFamily = new FontFamily("Area");
            textBlockD.FontSize = 30;
            textBlockD.Foreground = new SolidColorBrush(Color.FromRgb(137, 116, 81));
            textBlockD.Text = now.ToString("D") + "         " + now.ToString("HH:mm:ss");

            TextBlock textBlockdddd = new TextBlock();
            textBlockdddd.FontFamily = new FontFamily("Area");
            textBlockdddd.FontSize = 30;
            textBlockdddd.HorizontalAlignment = HorizontalAlignment.Right;
            textBlockdddd.Foreground = new SolidColorBrush(Color.FromRgb(137, 116, 81));
            textBlockdddd.Text = now.ToString("dddd");
            HeaderTextBlock.Children.Clear();

            HeaderTextBlock.Children.Add(textBlockD);
            HeaderTextBlock.Children.Add(textBlockdddd);

        }
    }
}
