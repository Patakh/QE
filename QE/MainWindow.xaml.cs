using Microsoft.EntityFrameworkCore;
using QE.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;
using Button = System.Windows.Controls.Button;
using Window = System.Windows.Window;
using TextBox = System.Windows.Controls.TextBox;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Npgsql;
using System.Configuration;
using FastReport.AdvMatrix;
using NpgsqlTypes;
using System.Data;
using Function;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using static System.Net.Mime.MediaTypeNames;
using ExCSS;
using Microsoft.IdentityModel.Tokens;

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

            this.Icon = new BitmapImage(new System.Uri(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))) + "/img/icon-eq.png"));

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

            // Создаем таймер для обновления даты и времени каждую секунду
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            // Установка начальных значений даты и времени
            UpdateDateTime();

            if (eqContext.SOfficeTerminals.Where(s => s.IpAddress == IpOffise).Any())
            {
                Title = eqContext.SOfficeTerminals.First(s => s.IpAddress == IpOffise).TerminalName;
                int Btn_idx = 1;

                // Офис
                HeaderTextBlockOfice.FontFamily = new FontFamily("Area");
                HeaderTextBlockOfice.FontSize = 30;
                HeaderTextBlockOfice.Foreground = new SolidColorBrush(Color.FromRgb(44, 54, 75));
                HeaderTextBlockOfice.Text = eqContext.SOffices.First(l => l.Id == eqContext.SOfficeTerminals.First(g => g.IpAddress == IpOffise).SOfficeId).OfficeName;

                long officeId = eqContext.SOffices.First(l => l.Id == eqContext.SOfficeTerminals.First(g => g.IpAddress == IpOffise).SOfficeId).Id;
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
                         textBlockMenu.Text = b.ButtonName;
                         textBlockMenu.TextWrapping = TextWrapping.Wrap;

                         WrapPanel warpPanelHeadMenu = new WrapPanel();
                         warpPanelHeadMenu.Orientation = Orientation.Horizontal;
                         warpPanelHeadMenu.VerticalAlignment = VerticalAlignment.Center;
                         warpPanelHeadMenu.Visibility = Visibility.Collapsed;
                         warpPanelHeadMenu.Margin = new Thickness(25, 0, 0, 0);
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

                //Первый предок
                WrapPanel wrapPanelPreRegistrationMain = new WrapPanel();
                wrapPanelPreRegistrationMain.Name = "PreRegistration";
                wrapPanelPreRegistrationMain.Orientation = Orientation.Vertical;
                wrapPanelPreRegistrationMain.Visibility = Visibility.Collapsed;

                //нажатие на кнопку "Предварительная запись"

                this.Button_Click_PreRegistration.Click += (s, e) =>
                 {

                     if (wrapPanelPreRegistrationMain.Children.Count > 0) wrapPanelPreRegistrationMain.Children.Clear();

                     //Блок 1 этапа
                     WrapPanel wrapPanelPreRegistrationStage1 = new WrapPanel();
                     wrapPanelPreRegistrationStage1.HorizontalAlignment = HorizontalAlignment.Center;
                     wrapPanelPreRegistrationStage1.Name = "PreRegistrationStage1";

                     //Блок 2 этапа
                     WrapPanel wrapPanelPreRegistrationStage2 = new WrapPanel();
                     wrapPanelPreRegistrationStage2.HorizontalAlignment = HorizontalAlignment.Center;
                     wrapPanelPreRegistrationStage2.Name = "PreRegistrationStage2";

                     //Блок 3 этапа
                     WrapPanel wrapPanelPreRegistrationStage3 = new WrapPanel();
                     wrapPanelPreRegistrationStage3.HorizontalAlignment = HorizontalAlignment.Center;
                     wrapPanelPreRegistrationStage3.Name = "PreRegistrationStage3";

                     //Блок 4 этапа
                     WrapPanel wrapPanelPreRegistrationStage4 = new WrapPanel();
                     wrapPanelPreRegistrationStage4.HorizontalAlignment = HorizontalAlignment.Center;
                     wrapPanelPreRegistrationStage4.VerticalAlignment = VerticalAlignment.Bottom;
                     wrapPanelPreRegistrationStage4.Name = "PreRegistrationStage4";

                     WrapPanel wrapPanelStage1Menu = new WrapPanel();
                     wrapPanelStage1Menu.Name = "PreRegistrationStage1Menu";
                     WrapPanel wrapPanelStage1Buttons = new WrapPanel();
                     wrapPanelStage1Buttons.Name = "PreRegistrationStage1Buttons";

                     wrapPanelPreRegistrationStage1.Visibility = Visibility.Visible;
                     wrapPanelStage1Menu.Visibility = Visibility.Visible;
                     wrapPanelStage1Buttons.Visibility = Visibility.Collapsed;

                     #region Кнопка далее и назад

                     Button btnBack = new Button();
                     DropShadowEffect shadowEffectBack = new DropShadowEffect();
                     shadowEffectBack.Color = Colors.White;
                     shadowEffectBack.ShadowDepth = 3;
                     btnBack.Effect = shadowEffectBack;
                     btnBack.Name = "Back";
                     btnBack.Content = "Назад";
                     btnBack.HorizontalAlignment = HorizontalAlignment.Left;
                     btnBack.VerticalAlignment = VerticalAlignment.Bottom;
                     btnBack.Height = 75;
                     btnBack.Width = 200;
                     btnBack.Background = new SolidColorBrush(Colors.DimGray);
                     btnBack.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
                     btnBack.FontFamily = new FontFamily("Area");
                     btnBack.FontSize = 25;
                     btnBack.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                     btnBack.TabIndex = 999;
                     btnBack.Visibility = Visibility.Hidden;
                     ControlTemplate myControlTemplateBack = new ControlTemplate(typeof(Button));
                     FrameworkElementFactory borderBack = new FrameworkElementFactory(typeof(Border));
                     borderBack.Name = "border";
                     borderBack.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
                     borderBack.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
                     borderBack.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
                     borderBack.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
                     FrameworkElementFactory contentPresenterBack = new FrameworkElementFactory(typeof(ContentPresenter));
                     contentPresenterBack.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                     contentPresenterBack.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                     borderBack.AppendChild(contentPresenterBack);
                     myControlTemplateBack.VisualTree = borderBack;
                     btnBack.Template = myControlTemplateBack;

                     Button btnNextStage = new Button();
                     DropShadowEffect shadowEffectNextStage = new DropShadowEffect();
                     shadowEffectNextStage.Color = Colors.White;
                     shadowEffectNextStage.ShadowDepth = 3;
                     btnNextStage.Effect = shadowEffectNextStage;
                     btnNextStage.Name = "NextStage";
                     btnNextStage.Content = "Далее";
                     btnNextStage.HorizontalAlignment = HorizontalAlignment.Right;
                     btnNextStage.VerticalAlignment = VerticalAlignment.Bottom;
                     btnNextStage.Height = 75;
                     btnNextStage.Width = 200;
                     btnNextStage.Background = new SolidColorBrush(Colors.DarkGreen);
                     btnNextStage.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
                     btnNextStage.FontFamily = new FontFamily("Area");
                     btnNextStage.FontSize = 25;
                     btnNextStage.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                     btnNextStage.TabIndex = 999;
                     btnNextStage.Visibility = Visibility.Hidden;
                     ControlTemplate myControlTemplateNextStage = new ControlTemplate(typeof(Button));
                     FrameworkElementFactory borderNextStage = new FrameworkElementFactory(typeof(Border));
                     borderNextStage.Name = "border";
                     borderNextStage.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
                     borderNextStage.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
                     borderNextStage.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
                     borderNextStage.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
                     FrameworkElementFactory contentPresenterNextStage = new FrameworkElementFactory(typeof(ContentPresenter));
                     contentPresenterNextStage.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                     contentPresenterNextStage.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                     borderNextStage.AppendChild(contentPresenterNextStage);
                     myControlTemplateNextStage.VisualTree = borderNextStage;
                     btnNextStage.Template = myControlTemplateNextStage;

                     #endregion

                     TextBlock textBlockPreRegistration = new TextBlock();
                     textBlockPreRegistration.Text = "Предварительная запись";
                     textBlockPreRegistration.Visibility = Visibility.Visible;
                     textBlockPreRegistration.HorizontalAlignment = HorizontalAlignment.Center;
                     textBlockPreRegistration.FontFamily = new FontFamily("Area");
                     textBlockPreRegistration.FontSize = 40;
                     textBlockPreRegistration.Margin = new Thickness(0, 0, 0, 100);
                     textBlockPreRegistration.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));

                     wrapPanelPreRegistrationMain.Children.Add(textBlockPreRegistration);

                     // меню и кнопки  Предварительная запись
                     eqContext.SOfficeTerminalButtons.Where(s => s.SOfficeTerminal.IpAddress == IpOffise).OrderBy(o => o.ButtonType).ToList().ForEach(b =>
                     {
                         if (b.ButtonType == 1) // 1 - Меню. 2 - Кнопка
                         {
                             #region создаем кнопку перехода на меню 
                             Button btnMenu = new Button();
                             DropShadowEffect shadowEffectMenu = new DropShadowEffect();
                             shadowEffectMenu.Color = Colors.White;
                             shadowEffectMenu.ShadowDepth = 3;
                             btnMenu.Effect = shadowEffectMenu;
                             btnMenu.Name = "menu";
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
                             ControlTemplate myControlTemplateMenu = new ControlTemplate(typeof(Button));
                             FrameworkElementFactory borderMenu = new FrameworkElementFactory(typeof(Border));
                             borderMenu.Name = "border";
                             borderMenu.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
                             borderMenu.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
                             borderMenu.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
                             borderMenu.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
                             FrameworkElementFactory contentPresenterMenu = new FrameworkElementFactory(typeof(ContentPresenter));
                             contentPresenterMenu.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                             contentPresenterMenu.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                             borderMenu.AppendChild(contentPresenterMenu);
                             myControlTemplateMenu.VisualTree = borderMenu;
                             btnMenu.Template = myControlTemplateMenu;
                             #endregion

                             //все кнопки этого меню
                             var SOfficeTerminalButton = eqContext.SOfficeTerminalButtons.Where(q => q.SOfficeTerminalId == b.SOfficeTerminalId && q.ParentId == b.ParentId && q.ButtonType != 1);

                             //создаем кнопки меню
                             List<SService> sServices = new List<SService>();
                             WrapPanel wrapPanelButtons = new WrapPanel();
                             wrapPanelButtons.Orientation = Orientation.Horizontal;
                             wrapPanelButtons.HorizontalAlignment = HorizontalAlignment.Center;
                             wrapPanelButtons.Visibility = Visibility.Collapsed;
                             wrapPanelButtons.MaxWidth = 800;
                             SOfficeTerminalButton.ToList().ForEach(button =>
                             {
                                 Button btnStage1 = new Button();
                                 btnStage1.Name = "button";
                                 btnStage1.Content = button.ButtonName;
                                 btnStage1.HorizontalAlignment = HorizontalAlignment.Center;
                                 btnStage1.VerticalAlignment = VerticalAlignment.Top;
                                 btnStage1.Height = 75;
                                 btnStage1.Width = 200;
                                 btnStage1.Margin = new Thickness(0, 18, 32, 0);
                                 btnStage1.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                                 btnStage1.BorderBrush = new SolidColorBrush(Color.FromRgb(55, 55, 55));
                                 btnStage1.FontFamily = new FontFamily("Area");
                                 btnStage1.FontSize = 25;
                                 btnStage1.Foreground = new SolidColorBrush(Color.FromRgb(135, 98, 27));
                                 DropShadowEffect btnShadowEffectStage1 = new DropShadowEffect();
                                 btnShadowEffectStage1.Color = Color.FromRgb(22, 22, 22);
                                 btnShadowEffectStage1.Direction = 50;
                                 btnShadowEffectStage1.ShadowDepth = 2;
                                 btnStage1.Effect = btnShadowEffectStage1;
                                 ControlTemplate myControlTemplateStage1 = new ControlTemplate(typeof(Button));
                                 FrameworkElementFactory borderStage1 = new FrameworkElementFactory(typeof(Border));
                                 borderStage1.Name = "border";
                                 borderStage1.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
                                 borderStage1.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
                                 borderStage1.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
                                 borderStage1.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
                                 FrameworkElementFactory contentPresenterStage1 = new FrameworkElementFactory(typeof(ContentPresenter));
                                 contentPresenterStage1.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                                 contentPresenterStage1.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                                 borderStage1.AppendChild(contentPresenterStage1);
                                 myControlTemplateStage1.VisualTree = borderStage1;
                                 btnStage1.Template = myControlTemplateStage1;
                                 btnStage1.Click += (s, e) =>
                                 {
                                     foreach (Button button in wrapPanelButtons.Children)
                                     {
                                         button.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                                     };

                                     btnStage1.Background = new SolidColorBrush(Color.FromRgb(100, 250, 255));

                                     btnNextStage.Visibility = Visibility.Visible;

                                     //переход на 2 этап
                                     btnNextStage.Click += (s, e) =>
                                     {
                                         btnNextStage.Visibility = Visibility.Hidden;
                                         btnBack.Visibility = Visibility.Hidden;

                                         wrapPanelPreRegistrationStage1.Visibility = Visibility.Collapsed;
                                         wrapPanelPreRegistrationStage2.Visibility = Visibility.Visible;
                                         wrapPanelPreRegistrationStage2.Children.Clear();

                                         // Кнопки с датами записи

                                         foreach (var ter in Prerecord.GetPrerecordData(button.SServiceId.Value, DateOnly.FromDateTime(DateTime.Now)).DistinctBy(x => x.Date).ToList())
                                         {
                                             Button btnDate = new Button();
                                             btnDate.Content = ter.Date.ToString("d") + "\n" + ter.DayName;
                                             btnDate.HorizontalAlignment = HorizontalAlignment.Center;
                                             btnDate.VerticalAlignment = VerticalAlignment.Center;
                                             btnDate.Height = 75;
                                             btnDate.Width = 200;
                                             btnDate.Margin = new Thickness(32, 18, 0, 0);
                                             btnDate.Background = new SolidColorBrush(Color.FromRgb(81, 96, 151));
                                             btnDate.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                                             btnDate.FontFamily = new FontFamily("Area");
                                             btnDate.FontSize = 20;
                                             btnDate.Foreground = new SolidColorBrush(Color.FromRgb(252, 252, 240));
                                             btnDate.Effect = btnShadowEffectStage1;
                                             btnDate.Template = myControlTemplateStage1;

                                             btnDate.Click += (s, e) =>
                                             {
                                                 //горение выбраанной кнопки
                                                 foreach (Button button in wrapPanelPreRegistrationStage2.Children) button.Background = new SolidColorBrush(Color.FromRgb(81, 96, 151));
                                                 btnDate.Background = new SolidColorBrush(Color.FromRgb(100, 250, 255));

                                                 btnNextStage.Visibility = Visibility.Visible;

                                                 //переход на 3 этап
                                                 btnNextStage.Click += (s, e) =>
                                                 {
                                                     wrapPanelPreRegistrationStage2.Visibility = Visibility.Collapsed;
                                                     wrapPanelPreRegistrationStage3.Children.Clear();
                                                     wrapPanelPreRegistrationStage3.Visibility = Visibility.Visible;
                                                     // Кнопки с временем записи
                                                     foreach (var ter in Prerecord.GetPrerecordData(button.SServiceId.Value, DateOnly.FromDateTime(DateTime.Now)).DistinctBy(x => x.StopTimePrerecord).ToList())
                                                     {
                                                         Button btnTime = new Button();
                                                         btnTime.Content = ter.StartTimePrerecord.ToString("hh\\:mm") + " - " + ter.StopTimePrerecord.ToString("hh\\:mm");
                                                         btnTime.HorizontalAlignment = HorizontalAlignment.Center;
                                                         btnTime.VerticalAlignment = VerticalAlignment.Center;
                                                         btnTime.Height = 75;
                                                         btnTime.Width = 200;
                                                         btnTime.Margin = new Thickness(32, 18, 0, 0);
                                                         btnTime.Background = new SolidColorBrush(Color.FromRgb(81, 96, 151));
                                                         btnTime.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                                                         btnTime.FontFamily = new FontFamily("Area");
                                                         btnTime.FontSize = 20;
                                                         btnTime.Foreground = new SolidColorBrush(Color.FromRgb(252, 252, 240));
                                                         btnTime.Effect = btnShadowEffectStage1;
                                                         btnTime.Template = myControlTemplateStage1;

                                                         //переход на 4 этап
                                                         btnTime.Click += (s, e) =>
                                                         {

                                                             foreach (Button button in wrapPanelPreRegistrationStage3.Children)
                                                             {
                                                                 button.Background = new SolidColorBrush(Color.FromRgb(81, 96, 151));
                                                             };
                                                             btnTime.Background = new SolidColorBrush(Color.FromRgb(100, 250, 255));
                                                             btnNextStage.Visibility = Visibility.Visible;

                                                             //переход на 4 этап
                                                             btnNextStage.Click += (s, e) =>
                                                             {

                                                                 textBlockPreRegistration.Margin = new Thickness(0, 0, 0, 50);
                                                                 wrapPanelPreRegistrationStage4.Orientation = Orientation.Vertical;
                                                                 wrapPanelPreRegistrationStage3.Visibility = Visibility.Collapsed;

                                                                 //поля фио и телефон
                                                                 StackPanel stackPanelForm = new StackPanel();
                                                                 stackPanelForm.HorizontalAlignment = HorizontalAlignment.Center;

                                                                 TextBox textBoxFio = new TextBox();
                                                                 textBoxFio.FontSize = 25;
                                                                 textBoxFio.FontFamily = new FontFamily("Area");
                                                                 textBoxFio.Padding = new Thickness(5, 8, 5, 8);
                                                                 textBoxFio.Height = 45;
                                                                 textBoxFio.Width = 600;
                                                                 textBoxFio.Focus();
                                                                 Label labelFio = new Label();
                                                                 labelFio.FontFamily = new FontFamily("Area");
                                                                 labelFio.FontSize = 20;
                                                                 labelFio.Content = "ФИО: ";

                                                                 TextBox textBoxPhone = new TextBox();
                                                                 textBoxPhone.FontFamily = new FontFamily("Area");
                                                                 textBoxPhone.Padding = new Thickness(5, 8, 5, 8);
                                                                 textBoxPhone.FontSize = 25;
                                                                 textBoxPhone.Width = 600;
                                                                 textBoxPhone.Height = 45;
                                                                 textBoxPhone.Text = "+7(";
                                                                 Label labelPhone = new Label();
                                                                 labelPhone.FontFamily = new FontFamily("Area");
                                                                 labelPhone.Margin = new Thickness(0, 15, 0, 0);
                                                                 labelPhone.FontSize = 20;
                                                                 labelPhone.Content = "Телефон: ";

                                                                 stackPanelForm.Children.Add(labelFio);
                                                                 stackPanelForm.Children.Add(textBoxFio);

                                                                 stackPanelForm.Children.Add(labelPhone);
                                                                 stackPanelForm.Children.Add(textBoxPhone);

                                                                 wrapPanelPreRegistrationStage4.Children.Add(stackPanelForm);
                                                                 btnNextStage.Content = "Записаться";

                                                                 //клавиатура буквы
                                                                 StackPanel stackPanelKeyboard = new StackPanel();
                                                                 stackPanelKeyboard.Margin = new Thickness(0, 30, 0, 0);
                                                                 stackPanelKeyboard.Children.Clear();
                                                                 stackPanelKeyboard.Children.Add((StackPanel)MaimWindow.Resources["Keyboard"]);

                                                                 //клавиатура цыфры
                                                                 StackPanel stackPanelKeyboardNumbers = new StackPanel();
                                                                 stackPanelKeyboardNumbers.Visibility = Visibility.Collapsed;
                                                                 stackPanelKeyboardNumbers.Children.Clear();
                                                                 stackPanelKeyboardNumbers.Children.Add((StackPanel)MaimWindow.Resources["KeyboardNumberPreRegistration"]);

                                                                 //финальная кнопка
                                                                 Button btnPreRegistrationFinal = new Button();
                                                                 DropShadowEffect shadowPreRegistrationFinal = new DropShadowEffect();
                                                                 shadowPreRegistrationFinal.Color = Colors.White;
                                                                 shadowPreRegistrationFinal.ShadowDepth = 3;
                                                                 btnPreRegistrationFinal.Effect = shadowPreRegistrationFinal;
                                                                 btnPreRegistrationFinal.Name = "btnPreRegistrationFinal";
                                                                 btnPreRegistrationFinal.Content = "Записаться";
                                                                 btnPreRegistrationFinal.HorizontalAlignment = HorizontalAlignment.Right;
                                                                 btnPreRegistrationFinal.VerticalAlignment = VerticalAlignment.Bottom;
                                                                 btnPreRegistrationFinal.Height = 50;
                                                                 btnPreRegistrationFinal.Width = 175;
                                                                 btnPreRegistrationFinal.Background = new SolidColorBrush(Colors.DarkGreen);
                                                                 btnPreRegistrationFinal.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
                                                                 btnPreRegistrationFinal.FontFamily = new FontFamily("Area");
                                                                 btnPreRegistrationFinal.FontSize = 20;
                                                                 btnPreRegistrationFinal.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                                                 btnPreRegistrationFinal.TabIndex = 999;
                                                                 btnPreRegistrationFinal.Visibility = Visibility.Hidden;

                                                                 bool upperCase = true;
                                                                 foreach (StackPanel item in stackPanelKeyboard.Children)
                                                                 {
                                                                     foreach (StackPanel stackPanel in item.Children)
                                                                     {
                                                                         foreach (Button buttonKeyboard in stackPanel.Children)
                                                                         {
                                                                             buttonKeyboard.Background = new SolidColorBrush(Colors.Brown);
                                                                             buttonKeyboard.Margin = new Thickness(4);
                                                                             buttonKeyboard.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));

                                                                             buttonKeyboard.Click += (s, e) =>
                                                                             {
                                                                                 textBoxFio.BorderBrush = new SolidColorBrush(Colors.Black);
                                                                                 labelFio.Foreground = new SolidColorBrush(Colors.Black);

                                                                                 Button buttonClick = (Button)s;
                                                                                 switch (buttonKeyboard.Content.ToString())
                                                                                 {

                                                                                     case "Удалить":
                                                                                         textBoxFio.Text = textBoxFio.Text.Length == 0 ? "" : textBoxFio.Text.Substring(0, textBoxFio.Text.Length - 1);
                                                                                         textBoxFio.CaretIndex = textBoxFio.Text.Length;
                                                                                         textBoxFio.Focus();
                                                                                         break;
                                                                                     case "Пробел":
                                                                                         textBoxFio.Text += " ";
                                                                                         textBoxFio.CaretIndex = textBoxFio.Text.Length;
                                                                                         textBoxFio.Focus();
                                                                                         break;
                                                                                     case "Очистить":
                                                                                         textBoxFio.Text = "";
                                                                                         textBoxFio.CaretIndex = textBoxFio.Text.Length;
                                                                                         textBoxFio.Focus();
                                                                                         break;
                                                                                     case "Далее":
                                                                                         if (textBoxFio.Text.Length == 0)
                                                                                         {
                                                                                             textBoxFio.BorderBrush = new SolidColorBrush(Colors.Red);
                                                                                             labelFio.Foreground = new SolidColorBrush(Colors.Red);
                                                                                             labelFio.Content = "ФИО: не заполнено !";
                                                                                         }
                                                                                         else
                                                                                         {
                                                                                             stackPanelKeyboardNumbers.Visibility = Visibility.Visible;
                                                                                             stackPanelKeyboard.Visibility = Visibility.Collapsed;
                                                                                             textBoxPhone.CaretIndex = textBoxPhone.Text.Length;
                                                                                             textBoxPhone.Focus();
                                                                                         }
                                                                                         break;
                                                                                     case "Регистр":
                                                                                         upperCase = !upperCase;
                                                                                         if (!upperCase)
                                                                                         {
                                                                                             buttonKeyboard.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                                                                             buttonKeyboard.Foreground = new SolidColorBrush(Colors.Brown);
                                                                                         }
                                                                                         else
                                                                                         {
                                                                                             buttonKeyboard.Background = new SolidColorBrush(Colors.Brown);
                                                                                             buttonKeyboard.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                                                                         }
                                                                                         break;
                                                                                     default:
                                                                                         labelFio.Content = "ФИО: ";
                                                                                         textBoxFio.Text += upperCase ? buttonKeyboard.Content.ToString().ToLower() : buttonKeyboard.Content.ToString().ToUpper();
                                                                                         textBoxFio.CaretIndex = textBoxFio.Text.Length;
                                                                                         textBoxFio.Focus();
                                                                                         break;
                                                                                 }
                                                                             };
                                                                         }
                                                                     }
                                                                 }

                                                                 foreach (StackPanel item in stackPanelKeyboardNumbers.Children)
                                                                 {
                                                                     foreach (StackPanel stackPanel in item.Children)
                                                                     {
                                                                         foreach (Button buttonKeyboard in stackPanel.Children)
                                                                         {
                                                                             buttonKeyboard.Background = new SolidColorBrush(Colors.Brown);
                                                                             buttonKeyboard.Margin = new Thickness(4);
                                                                             buttonKeyboard.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                                                             buttonKeyboard.FontSize = 18;
                                                                             buttonKeyboard.Click += (s, e) =>
                                                                             {
                                                                                 Button buttonClick = (Button)s;

                                                                                 switch (buttonKeyboard.Content.ToString())
                                                                                 {
                                                                                     case "Удалить":

                                                                                         textBoxPhone.Text = textBoxPhone.Text.Length == 0 ? "" : textBoxPhone.Text.Length > 3 ? textBoxPhone.Text.Substring(0, textBoxPhone.Text.Length - 1) : textBoxPhone.Text;

                                                                                         break;
                                                                                     default:
                                                                                         switch (textBoxPhone.Text.Length)
                                                                                         {
                                                                                             case 3:
                                                                                                 textBoxPhone.Text += buttonKeyboard.Content;
                                                                                                 break;
                                                                                             case 4:
                                                                                                 textBoxPhone.Text += buttonKeyboard.Content;
                                                                                                 break;
                                                                                             case 5:
                                                                                                 textBoxPhone.Text += buttonKeyboard.Content + ")";
                                                                                                 break;
                                                                                             case 7:
                                                                                                 textBoxPhone.Text += buttonKeyboard.Content;
                                                                                                 break;
                                                                                             case 8:
                                                                                                 textBoxPhone.Text += buttonKeyboard.Content;
                                                                                                 break;
                                                                                             case 9:
                                                                                                 textBoxPhone.Text += buttonKeyboard.Content + "-";
                                                                                                 break;
                                                                                             case 11:
                                                                                                 textBoxPhone.Text += buttonKeyboard.Content;
                                                                                                 break;
                                                                                             case 12:
                                                                                                 textBoxPhone.Text += buttonKeyboard.Content + "-";
                                                                                                 break;
                                                                                             case 14:
                                                                                                 textBoxPhone.Text += buttonKeyboard.Content;
                                                                                                 break;
                                                                                             case 15:
                                                                                                 textBoxPhone.Text += buttonKeyboard.Content;
                                                                                                 btnPreRegistrationFinal.Visibility = Visibility.Visible;
                                                                                                 break;
                                                                                         }
                                                                                         break;
                                                                                 }
                                                                                 if (textBoxPhone.Text.Length != 16) btnPreRegistrationFinal.Visibility = Visibility.Collapsed;
                                                                                 textBoxPhone.CaretIndex = textBoxPhone.Text.Length;
                                                                                 textBoxPhone.Focus();
                                                                             };
                                                                         }
                                                                     }
                                                                 }

                                                                 textBoxFio.PreviewMouseDown += (s, e) =>
                                                                 {
                                                                     stackPanelKeyboardNumbers.Visibility = Visibility.Collapsed;
                                                                     stackPanelKeyboard.Visibility = Visibility.Visible;
                                                                     textBoxFio.Focus();
                                                                 };

                                                                 textBoxPhone.PreviewMouseDown += (s, e) =>
                                                                 {
                                                                     stackPanelKeyboardNumbers.Visibility = Visibility.Visible;
                                                                     stackPanelKeyboard.Visibility = Visibility.Collapsed;
                                                                     textBoxPhone.Focus();
                                                                 };

                                                                 wrapPanelPreRegistrationStage4.Children.Add(stackPanelKeyboardNumbers);
                                                                 wrapPanelPreRegistrationStage4.Children.Add(stackPanelKeyboard);

                                                                 btnPreRegistrationFinal.Click += (s, e) =>
                                                                 {
                                                                     if (textBoxFio.Text.Length == 0)
                                                                     {
                                                                         textBoxFio.BorderBrush = new SolidColorBrush(Colors.Red);
                                                                         labelFio.Foreground = new SolidColorBrush(Colors.Red);
                                                                         labelFio.Content = "ФИО: не заполнено !";
                                                                     }
                                                                     else
                                                                     {

                                                                         var codePrerecord = GenerateUniqueCode(eqContext.DTicketPrerecords.Where(w => w.DatePrerecord == DateOnly.Parse(ter.Date.ToString("d"))).Select(s => s.CodePrerecord).ToList());
                                                                         //записиваю в базу
                                                                         DTicketPrerecord dTicketPrerecord = new DTicketPrerecord();
                                                                         dTicketPrerecord.SServiceId = button.SServiceId.Value;
                                                                         dTicketPrerecord.SOfficeId = officeId;
                                                                         dTicketPrerecord.SSourсePrerecordId = 2;
                                                                         dTicketPrerecord.CustomerFullName = textBoxFio.Text;
                                                                         dTicketPrerecord.CustomerPhoneNumber = textBoxPhone.Text;
                                                                         dTicketPrerecord.DatePrerecord = DateOnly.Parse(ter.Date.ToString("d"));
                                                                         dTicketPrerecord.StartTimePrerecord = TimeOnly.Parse(ter.StartTimePrerecord.ToString("hh\\:mm"));
                                                                         dTicketPrerecord.StopTimePrerecord = TimeOnly.Parse(ter.StopTimePrerecord.ToString("hh\\:mm"));
                                                                         dTicketPrerecord.IsConfirmation = false;
                                                                         dTicketPrerecord.CodePrerecord = codePrerecord;
                                                                         eqContext.DTicketPrerecords.Add(dTicketPrerecord);
                                                                         eqContext.SaveChanges();

                                                                         wrapPanelPreRegistrationStage4.Visibility = Visibility.Collapsed;

                                                                         //показываю код
                                                                         StackPanel stackPanelResultPreRegistration = new StackPanel();
                                                                         TextBlock ResultPreRegistrationCode = new TextBlock();
                                                                         ResultPreRegistrationCode.Text = "Код:\n" + codePrerecord.ToString();
                                                                         ResultPreRegistrationCode.HorizontalAlignment = HorizontalAlignment.Center;
                                                                         ResultPreRegistrationCode.FontSize = 100;
                                                                         stackPanelResultPreRegistration.Children.Add(ResultPreRegistrationCode);

                                                                         //печать кода
                                                                         Button buttonPrintResultPreRegistration = new Button();
                                                                         buttonPrintResultPreRegistration.Content = "Печать";
                                                                         buttonPrintResultPreRegistration.HorizontalAlignment = HorizontalAlignment.Right;
                                                                         buttonPrintResultPreRegistration.VerticalAlignment = VerticalAlignment.Bottom;
                                                                         buttonPrintResultPreRegistration.Height = 50;
                                                                         buttonPrintResultPreRegistration.Width = 175;
                                                                         buttonPrintResultPreRegistration.Margin = new Thickness(0, 50, 0, 0);
                                                                         buttonPrintResultPreRegistration.Background = new SolidColorBrush(Colors.DarkGreen);
                                                                         buttonPrintResultPreRegistration.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
                                                                         buttonPrintResultPreRegistration.FontFamily = new FontFamily("Area");
                                                                         buttonPrintResultPreRegistration.FontSize = 20;
                                                                         buttonPrintResultPreRegistration.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));

                                                                         buttonPrintResultPreRegistration.Click += (s, e) =>
                                                                         {
                                                                             System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
                                                                             pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PrintPageHandler);
                                                                             pd.PrinterSettings.PrinterName = pd.PrinterSettings.PrinterName;
                                                                             pd.Print();

                                                                             Home(s, e);
                                                                         };
                                                                         void PrintPageHandler(object sender, System.Drawing.Printing.PrintPageEventArgs e)
                                                                         {
                                                                             // Установите шрифт и размер текста для печати
                                                                             System.Drawing.Font font = new System.Drawing.Font("Arial", 80);

                                                                             // Установите позицию, с которой нужно начать печать текста
                                                                             System.Drawing.PointF location = new System.Drawing.PointF(0,0);

                                                                             // Нарисуйте текст на странице
                                                                             e.Graphics.DrawString(codePrerecord.ToString(), font, System.Drawing.Brushes.Black, location);

                                                                         }

                                                                         stackPanelResultPreRegistration.Children.Add(buttonPrintResultPreRegistration);
                                                                         wrapPanelPreRegistrationMain.Children.Add(stackPanelResultPreRegistration);
                                                                     }
                                                                 };

                                                                 wrapPanelPreRegistrationStage4.Children.Add(btnPreRegistrationFinal);
                                                             };
                                                         };
                                                         wrapPanelPreRegistrationStage3.Children.Add(btnTime);
                                                     }
                                                 };
                                             };
                                             wrapPanelPreRegistrationStage2.Children.Add(btnDate);
                                         }
                                     };
                                 };

                                 wrapPanelButtons.Children.Add(btnStage1);
                             });

                             wrapPanelStage1Buttons.Children.Add(wrapPanelButtons);

                             btnMenu.Click += (s, e) =>
                             {
                                 StackClose.Visibility = Visibility.Visible;
                                 wrapPanelButtons.Visibility = Visibility.Visible;
                                 wrapPanelStage1Buttons.Visibility = Visibility.Visible;
                                 wrapPanelStage1Menu.Visibility = Visibility.Collapsed;
                                 btnBack.Visibility = Visibility.Visible;
                                 btnNextStage.Visibility = Visibility.Hidden;

                                 foreach (Button button in wrapPanelStage1Menu.Children)
                                 {
                                     if (button.Name != "menu")
                                     {
                                         button.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                                     }
                                 }
                             };

                             btnBack.Click += (s, e) =>
                             {
                                 wrapPanelButtons.Visibility = Visibility.Collapsed;
                                 wrapPanelStage1Buttons.Visibility = Visibility.Collapsed;
                                 wrapPanelStage1Menu.Visibility = Visibility.Visible;
                                 btnBack.Visibility = Visibility.Hidden;
                                 btnNextStage.Visibility = Visibility.Hidden;

                                 foreach (Button button in wrapPanelButtons.Children)
                                 {
                                     button.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                                 }
                             };

                             wrapPanelStage1Menu.Children.Add(btnMenu);
                         }
                         else
                         if (b.ParentId == 0)
                         {
                             SService sServices = eqContext.SServices.First(f => f.Id == b.SServiceId);
                             Button btnStage1 = new Button();
                             btnStage1.Name = "button";
                             btnStage1.Content = b.ButtonName;
                             btnStage1.HorizontalAlignment = HorizontalAlignment.Center;
                             btnStage1.VerticalAlignment = VerticalAlignment.Top;
                             btnStage1.Height = 75;
                             btnStage1.Width = 200;
                             btnStage1.Margin = new Thickness(0, 18, 32, 0);
                             btnStage1.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                             btnStage1.BorderBrush = new SolidColorBrush(Color.FromRgb(55, 55, 55));
                             btnStage1.FontFamily = new FontFamily("Area");
                             btnStage1.FontSize = 25;
                             btnStage1.Foreground = new SolidColorBrush(Color.FromRgb(135, 98, 27));
                             DropShadowEffect btnShadowEffectStage1 = new DropShadowEffect();
                             btnShadowEffectStage1.Color = Color.FromRgb(22, 22, 22);
                             btnShadowEffectStage1.Direction = 50;
                             btnShadowEffectStage1.ShadowDepth = 2;
                             btnStage1.Effect = btnShadowEffectStage1;
                             ControlTemplate myControlTemplateStage1 = new ControlTemplate(typeof(Button));
                             FrameworkElementFactory borderStage1 = new FrameworkElementFactory(typeof(Border));
                             borderStage1.Name = "border";
                             borderStage1.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
                             borderStage1.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
                             borderStage1.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
                             borderStage1.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
                             FrameworkElementFactory contentPresenterStage1 = new FrameworkElementFactory(typeof(ContentPresenter));
                             contentPresenterStage1.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                             contentPresenterStage1.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                             borderStage1.AppendChild(contentPresenterStage1);
                             myControlTemplateStage1.VisualTree = borderStage1;
                             btnStage1.Template = myControlTemplateStage1;
                             btnStage1.Click += (s, e) =>
                             {
                                 foreach (Button button in wrapPanelStage1Menu.Children)
                                 {
                                     if (wrapPanelStage1Menu.Name == "button") button.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                                 };

                                 btnStage1.Background = new SolidColorBrush(Color.FromRgb(100, 250, 255));

                                 btnNextStage.Visibility = Visibility.Visible;

                                 //переход на 2 этап
                                 btnNextStage.Click += (s, e) =>
                                 {
                                     btnNextStage.Visibility = Visibility.Hidden;
                                     btnStage1.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                                     wrapPanelPreRegistrationStage1.Visibility = Visibility.Collapsed;
                                     wrapPanelPreRegistrationStage2.Visibility = Visibility.Visible;
                                     wrapPanelPreRegistrationStage2.Children.Clear();

                                     // Кнопки с датами записи
                                     foreach (var ter in Prerecord.GetPrerecordData(sServices.Id, DateOnly.FromDateTime(DateTime.Now)).DistinctBy(x => x.Date).ToList())
                                     {
                                         Button btnDate = new Button();
                                         btnDate.Content = ter.Date.ToString("d") + "\n" + ter.DayName;
                                         btnDate.HorizontalAlignment = HorizontalAlignment.Center;
                                         btnDate.VerticalAlignment = VerticalAlignment.Center;
                                         btnDate.Height = 75;
                                         btnDate.Width = 200;
                                         btnDate.Visibility = Visibility.Visible;
                                         btnDate.Margin = new Thickness(32, 18, 0, 0);
                                         btnDate.Background = new SolidColorBrush(Color.FromRgb(81, 96, 151));
                                         btnDate.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                                         btnDate.FontFamily = new FontFamily("Area");
                                         btnDate.FontSize = 20;
                                         btnDate.Foreground = new SolidColorBrush(Color.FromRgb(252, 252, 240));
                                         btnDate.Effect = btnShadowEffectStage1;
                                         btnDate.Template = myControlTemplateStage1;
                                         btnDate.Click += (s, e) =>
                                         {
                                             //горение выбраанной кнопки
                                             foreach (Button button in wrapPanelPreRegistrationStage2.Children) button.Background = new SolidColorBrush(Color.FromRgb(81, 96, 151));
                                             btnDate.Background = new SolidColorBrush(Color.FromRgb(100, 250, 255));

                                             btnNextStage.Visibility = Visibility.Visible;

                                             //переход на 3 этап
                                             btnNextStage.Click += (s, e) =>
                                             {
                                                 wrapPanelPreRegistrationStage2.Visibility = Visibility.Collapsed;
                                                 wrapPanelPreRegistrationStage3.Children.Clear();
                                                 wrapPanelPreRegistrationStage3.Visibility = Visibility.Visible;
                                                 // Кнопки с временем записи
                                                 foreach (var ter in Prerecord.GetPrerecordData(sServices.Id, DateOnly.FromDateTime(DateTime.Now)).DistinctBy(x => x.StopTimePrerecord).ToList())
                                                 {
                                                     Button btnTime = new Button();
                                                     btnTime.Content = ter.StartTimePrerecord.ToString("hh\\:mm") + " - " + ter.StopTimePrerecord.ToString("hh\\:mm");
                                                     btnTime.HorizontalAlignment = HorizontalAlignment.Center;
                                                     btnTime.VerticalAlignment = VerticalAlignment.Center;
                                                     btnTime.Height = 75;
                                                     btnTime.Width = 200;
                                                     btnTime.Margin = new Thickness(32, 18, 0, 0);
                                                     btnTime.Background = new SolidColorBrush(Color.FromRgb(81, 96, 151));
                                                     btnTime.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                                                     btnTime.FontFamily = new FontFamily("Area");
                                                     btnTime.FontSize = 20;
                                                     btnTime.Foreground = new SolidColorBrush(Color.FromRgb(252, 252, 240));
                                                     btnTime.Effect = btnShadowEffectStage1;
                                                     btnTime.Template = myControlTemplateStage1;
                                                     btnTime.Click += (s, e) =>
                                                     {
                                                         foreach (Button button in wrapPanelPreRegistrationStage3.Children)
                                                         {
                                                             button.Background = new SolidColorBrush(Color.FromRgb(81, 96, 151));
                                                         };
                                                         btnTime.Background = new SolidColorBrush(Color.FromRgb(100, 250, 255));
                                                         btnNextStage.Visibility = Visibility.Visible;

                                                         //переход на 4 этап
                                                         btnNextStage.Click += (s, e) =>
                                                         {
                                                             textBlockPreRegistration.Margin = new Thickness(0, 0, 0, 50);
                                                             wrapPanelPreRegistrationStage4.Orientation = Orientation.Vertical;
                                                             wrapPanelPreRegistrationStage3.Visibility = Visibility.Collapsed;

                                                             //поля фио и телефон
                                                             StackPanel stackPanelForm = new StackPanel();
                                                             stackPanelForm.HorizontalAlignment = HorizontalAlignment.Center;


                                                             TextBox textBoxFio = new TextBox();
                                                             textBoxFio.FontSize = 25;
                                                             textBoxFio.FontFamily = new FontFamily("Area");
                                                             textBoxFio.Padding = new Thickness(5, 8, 5, 8);
                                                             textBoxFio.Height = 45;
                                                             textBoxFio.Width = 600;
                                                             textBoxFio.Focus();
                                                             Label labelFio = new Label();
                                                             labelFio.FontFamily = new FontFamily("Area");
                                                             labelFio.FontSize = 20;
                                                             labelFio.Content = "ФИО: ";

                                                             TextBox textBoxPhone = new TextBox();
                                                             textBoxPhone.FontFamily = new FontFamily("Area");
                                                             textBoxPhone.Padding = new Thickness(5, 8, 5, 8);
                                                             textBoxPhone.FontSize = 25;
                                                             textBoxPhone.Width = 600;
                                                             textBoxPhone.Height = 45;
                                                             textBoxPhone.Text = "+7(";
                                                             Label labelPhone = new Label();
                                                             labelPhone.FontFamily = new FontFamily("Area");
                                                             labelPhone.Margin = new Thickness(0, 15, 0, 0);
                                                             labelPhone.FontSize = 20;
                                                             labelPhone.Content = "Телефон: ";

                                                             stackPanelForm.Children.Add(labelFio);
                                                             stackPanelForm.Children.Add(textBoxFio);

                                                             stackPanelForm.Children.Add(labelPhone);
                                                             stackPanelForm.Children.Add(textBoxPhone);

                                                             wrapPanelPreRegistrationStage4.Children.Add(stackPanelForm);
                                                             btnNextStage.Content = "Записаться";

                                                             //клавиатура буквы
                                                             StackPanel stackPanelKeyboard = new StackPanel();
                                                             stackPanelKeyboard.Margin = new Thickness(0, 30, 0, 0);
                                                             stackPanelKeyboard.Children.Clear();
                                                             stackPanelKeyboard.Children.Add((StackPanel)MaimWindow.Resources["Keyboard"]);

                                                             //клавиатура цыфры
                                                             StackPanel stackPanelKeyboardNumbers = new StackPanel();
                                                             stackPanelKeyboardNumbers.Visibility = Visibility.Collapsed;
                                                             stackPanelKeyboardNumbers.Children.Clear();
                                                             stackPanelKeyboardNumbers.Children.Add((StackPanel)MaimWindow.Resources["KeyboardNumberPreRegistration"]);

                                                             //финальная кнопка
                                                             Button btnPreRegistrationFinal = new Button();
                                                             DropShadowEffect shadowPreRegistrationFinal = new DropShadowEffect();
                                                             shadowPreRegistrationFinal.Color = Colors.White;
                                                             shadowPreRegistrationFinal.ShadowDepth = 3;
                                                             btnPreRegistrationFinal.Effect = shadowPreRegistrationFinal;
                                                             btnPreRegistrationFinal.Name = "btnPreRegistrationFinal";
                                                             btnPreRegistrationFinal.Content = "Записаться";
                                                             btnPreRegistrationFinal.HorizontalAlignment = HorizontalAlignment.Right;
                                                             btnPreRegistrationFinal.VerticalAlignment = VerticalAlignment.Bottom;
                                                             btnPreRegistrationFinal.Height = 50;
                                                             btnPreRegistrationFinal.Width = 175;
                                                             btnPreRegistrationFinal.Background = new SolidColorBrush(Colors.DarkGreen);
                                                             btnPreRegistrationFinal.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
                                                             btnPreRegistrationFinal.FontFamily = new FontFamily("Area");
                                                             btnPreRegistrationFinal.FontSize = 20;
                                                             btnPreRegistrationFinal.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                                             btnPreRegistrationFinal.TabIndex = 999;
                                                             btnPreRegistrationFinal.Visibility = Visibility.Hidden;

                                                             bool upperCase = true;
                                                             foreach (StackPanel item in stackPanelKeyboard.Children)
                                                             {
                                                                 foreach (StackPanel stackPanel in item.Children)
                                                                 {
                                                                     foreach (Button buttonKeyboard in stackPanel.Children)
                                                                     {
                                                                         buttonKeyboard.Background = new SolidColorBrush(Colors.Brown);
                                                                         buttonKeyboard.Margin = new Thickness(4);
                                                                         buttonKeyboard.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));

                                                                         buttonKeyboard.Click += (s, e) =>
                                                                         {
                                                                             textBoxFio.BorderBrush = new SolidColorBrush(Colors.Black);
                                                                             labelFio.Foreground = new SolidColorBrush(Colors.Black);

                                                                             Button buttonClick = (Button)s;
                                                                             switch (buttonKeyboard.Content.ToString())
                                                                             {

                                                                                 case "Удалить":
                                                                                     textBoxFio.Text = textBoxFio.Text.Length == 0 ? "" : textBoxFio.Text.Substring(0, textBoxFio.Text.Length - 1);
                                                                                     textBoxFio.CaretIndex = textBoxFio.Text.Length;
                                                                                     textBoxFio.Focus();
                                                                                     break;
                                                                                 case "Пробел":
                                                                                     textBoxFio.Text += " ";
                                                                                     textBoxFio.CaretIndex = textBoxFio.Text.Length;
                                                                                     textBoxFio.Focus();
                                                                                     break;
                                                                                 case "Очистить":
                                                                                     textBoxFio.Text = "";
                                                                                     textBoxFio.CaretIndex = textBoxFio.Text.Length;
                                                                                     textBoxFio.Focus();
                                                                                     break;
                                                                                 case "Далее":
                                                                                     if (textBoxFio.Text.Length == 0)
                                                                                     {
                                                                                         textBoxFio.BorderBrush = new SolidColorBrush(Colors.Red);
                                                                                         labelFio.Foreground = new SolidColorBrush(Colors.Red);
                                                                                         labelFio.Content = "ФИО: не заполнено !";
                                                                                     }
                                                                                     else
                                                                                     {
                                                                                         stackPanelKeyboardNumbers.Visibility = Visibility.Visible;
                                                                                         stackPanelKeyboard.Visibility = Visibility.Collapsed;
                                                                                         textBoxPhone.CaretIndex = textBoxPhone.Text.Length;
                                                                                         textBoxPhone.Focus();
                                                                                     }
                                                                                     break;
                                                                                 case "Регистр":
                                                                                     upperCase = !upperCase;
                                                                                     if (!upperCase)
                                                                                     {
                                                                                         buttonKeyboard.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                                                                         buttonKeyboard.Foreground = new SolidColorBrush(Colors.Brown);
                                                                                     }
                                                                                     else
                                                                                     {
                                                                                         buttonKeyboard.Background = new SolidColorBrush(Colors.Brown);
                                                                                         buttonKeyboard.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                                                                     }
                                                                                     break;
                                                                                 default:
                                                                                     labelFio.Content = "ФИО: ";
                                                                                     textBoxFio.Text += upperCase ? buttonKeyboard.Content.ToString().ToLower() : buttonKeyboard.Content.ToString().ToUpper();
                                                                                     textBoxFio.CaretIndex = textBoxFio.Text.Length;
                                                                                     textBoxFio.Focus();
                                                                                     break;
                                                                             }
                                                                         };
                                                                     }
                                                                 }
                                                             }

                                                             foreach (StackPanel item in stackPanelKeyboardNumbers.Children)
                                                             {
                                                                 foreach (StackPanel stackPanel in item.Children)
                                                                 {
                                                                     foreach (Button buttonKeyboard in stackPanel.Children)
                                                                     {
                                                                         buttonKeyboard.Background = new SolidColorBrush(Colors.Brown);
                                                                         buttonKeyboard.Margin = new Thickness(4);
                                                                         buttonKeyboard.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                                                         buttonKeyboard.FontSize = 18;
                                                                         buttonKeyboard.Click += (s, e) =>
                                                                         {
                                                                             Button buttonClick = (Button)s;

                                                                             switch (buttonKeyboard.Content.ToString())
                                                                             {
                                                                                 case "Удалить":

                                                                                     textBoxPhone.Text = textBoxPhone.Text.Length == 0 ? "" : textBoxPhone.Text.Length > 3 ? textBoxPhone.Text.Substring(0, textBoxPhone.Text.Length - 1) : textBoxPhone.Text;

                                                                                     break;
                                                                                 default:
                                                                                     switch (textBoxPhone.Text.Length)
                                                                                     {
                                                                                         case 3:
                                                                                             textBoxPhone.Text += buttonKeyboard.Content;
                                                                                             break;
                                                                                         case 4:
                                                                                             textBoxPhone.Text += buttonKeyboard.Content;
                                                                                             break;
                                                                                         case 5:
                                                                                             textBoxPhone.Text += buttonKeyboard.Content + ")";
                                                                                             break;
                                                                                         case 7:
                                                                                             textBoxPhone.Text += buttonKeyboard.Content;
                                                                                             break;
                                                                                         case 8:
                                                                                             textBoxPhone.Text += buttonKeyboard.Content;
                                                                                             break;
                                                                                         case 9:
                                                                                             textBoxPhone.Text += buttonKeyboard.Content + "-";
                                                                                             break;
                                                                                         case 11:
                                                                                             textBoxPhone.Text += buttonKeyboard.Content;
                                                                                             break;
                                                                                         case 12:
                                                                                             textBoxPhone.Text += buttonKeyboard.Content + "-";
                                                                                             break;
                                                                                         case 14:
                                                                                             textBoxPhone.Text += buttonKeyboard.Content;
                                                                                             break;
                                                                                         case 15:
                                                                                             textBoxPhone.Text += buttonKeyboard.Content;
                                                                                             btnPreRegistrationFinal.Visibility = Visibility.Visible;
                                                                                             break;
                                                                                     }
                                                                                     break;
                                                                             }
                                                                             if (textBoxPhone.Text.Length != 16) btnPreRegistrationFinal.Visibility = Visibility.Collapsed;
                                                                             textBoxPhone.CaretIndex = textBoxPhone.Text.Length;
                                                                             textBoxPhone.Focus();
                                                                         };
                                                                     }
                                                                 }
                                                             }

                                                             textBoxFio.PreviewMouseDown += (s, e) =>
                                                             {
                                                                 stackPanelKeyboardNumbers.Visibility = Visibility.Collapsed;
                                                                 stackPanelKeyboard.Visibility = Visibility.Visible;
                                                                 textBoxFio.Focus();
                                                             };

                                                             textBoxPhone.PreviewMouseDown += (s, e) =>
                                                             {
                                                                 stackPanelKeyboardNumbers.Visibility = Visibility.Visible;
                                                                 stackPanelKeyboard.Visibility = Visibility.Collapsed;
                                                                 textBoxPhone.Focus();
                                                             };

                                                             wrapPanelPreRegistrationStage4.Children.Add(stackPanelKeyboardNumbers);
                                                             wrapPanelPreRegistrationStage4.Children.Add(stackPanelKeyboard);

                                                             btnPreRegistrationFinal.Click += (s, e) =>
                                                             {
                                                                 if (textBoxFio.Text.Length == 0)
                                                                 {
                                                                     textBoxFio.BorderBrush = new SolidColorBrush(Colors.Red);
                                                                     labelFio.Foreground = new SolidColorBrush(Colors.Red);
                                                                     labelFio.Content = "ФИО: не заполнено !";
                                                                 }
                                                                 else
                                                                 {
                                                                     var codePrerecord = GenerateUniqueCode(eqContext.DTicketPrerecords.Where(w => w.DatePrerecord == DateOnly.Parse(ter.Date.ToString("d"))).Select(s => s.CodePrerecord).ToList());
                                                                     //записиваю в базу
                                                                     DTicketPrerecord dTicketPrerecord = new DTicketPrerecord();
                                                                     dTicketPrerecord.SServiceId = b.SServiceId.Value;
                                                                     dTicketPrerecord.SOfficeId = officeId;
                                                                     dTicketPrerecord.SSourсePrerecordId = 2;
                                                                     dTicketPrerecord.CustomerFullName = textBoxFio.Text;
                                                                     dTicketPrerecord.CustomerPhoneNumber = textBoxPhone.Text;
                                                                     dTicketPrerecord.DatePrerecord = DateOnly.Parse(ter.Date.ToString("d"));
                                                                     dTicketPrerecord.StartTimePrerecord = TimeOnly.Parse(ter.StartTimePrerecord.ToString("hh\\:mm"));
                                                                     dTicketPrerecord.StopTimePrerecord = TimeOnly.Parse(ter.StopTimePrerecord.ToString("hh\\:mm"));
                                                                     dTicketPrerecord.IsConfirmation = false;
                                                                     dTicketPrerecord.CodePrerecord = codePrerecord;
                                                                     eqContext.DTicketPrerecords.Add(dTicketPrerecord);
                                                                     eqContext.SaveChanges();

                                                                     wrapPanelPreRegistrationStage4.Visibility = Visibility.Collapsed;

                                                                     //показываю код
                                                                     StackPanel stackPanelResultPreRegistration = new StackPanel();
                                                                     TextBlock ResultPreRegistrationCode = new TextBlock();
                                                                     ResultPreRegistrationCode.Text = "Код:\n" + codePrerecord.ToString();
                                                                     ResultPreRegistrationCode.HorizontalAlignment = HorizontalAlignment.Center;
                                                                     ResultPreRegistrationCode.FontSize = 100;
                                                                     stackPanelResultPreRegistration.Children.Add(ResultPreRegistrationCode);

                                                                     //печать кода
                                                                     Button buttonPrintResultPreRegistration = new Button();
                                                                     buttonPrintResultPreRegistration.Content = "Печать";
                                                                     buttonPrintResultPreRegistration.HorizontalAlignment = HorizontalAlignment.Right;
                                                                     buttonPrintResultPreRegistration.VerticalAlignment = VerticalAlignment.Bottom;
                                                                     buttonPrintResultPreRegistration.Height = 50;
                                                                     buttonPrintResultPreRegistration.Width = 175;
                                                                     buttonPrintResultPreRegistration.Margin = new Thickness(0, 50, 0, 0);
                                                                     buttonPrintResultPreRegistration.Background = new SolidColorBrush(Colors.DarkGreen);
                                                                     buttonPrintResultPreRegistration.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
                                                                     buttonPrintResultPreRegistration.FontFamily = new FontFamily("Area");
                                                                     buttonPrintResultPreRegistration.FontSize = 20;
                                                                     buttonPrintResultPreRegistration.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));

                                                                     buttonPrintResultPreRegistration.Click += (s, e) =>
                                                                     {
                                                                         System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
                                                                         pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PrintPageHandler);
                                                                         pd.PrinterSettings.PrinterName = pd.PrinterSettings.PrinterName;
                                                                         pd.Print();

                                                                         Home(s, e);
                                                                     };
                                                                     void PrintPageHandler(object sender, System.Drawing.Printing.PrintPageEventArgs e)
                                                                     {
                                                                         // Установите шрифт и размер текста для печати
                                                                         System.Drawing.Font font = new System.Drawing.Font("Arial", 80);

                                                                         // Установите позицию, с которой нужно начать печать текста
                                                                         System.Drawing.PointF location = new System.Drawing.PointF(e.MarginBounds.Left, e.MarginBounds.Top);

                                                                         // Нарисуйте текст на странице
                                                                         e.Graphics.DrawString(codePrerecord.ToString(), font, System.Drawing.Brushes.Black, location);

                                                                     }

                                                                     stackPanelResultPreRegistration.Children.Add(buttonPrintResultPreRegistration);
                                                                     wrapPanelPreRegistrationMain.Children.Add(stackPanelResultPreRegistration);
                                                                 }
                                                             };
                                                             wrapPanelPreRegistrationStage4.Children.Add(btnPreRegistrationFinal);
                                                         };
                                                     };
                                                     wrapPanelPreRegistrationStage3.Children.Add(btnTime);
                                                 }
                                             };
                                         };
                                         wrapPanelPreRegistrationStage2.Children.Add(btnDate);
                                     }
                                 };
                             };
                             wrapPanelStage1Menu.Children.Add(btnStage1);
                         }
                     });

                     //показываем нужный блок
                     foreach (WrapPanel obj in BodyWindow.Children)
                     {
                         obj.Visibility = Visibility.Collapsed;
                         if (obj.Name == "PreRegistration")
                         {
                             obj.Visibility = Visibility.Visible;
                         }
                     }

                     // меняем активность кнопки
                     StackClose.Visibility = Visibility.Visible;
                     foreach (StackPanel stackPanel in Buttons_Service.Children)
                     {
                         Button button = (Button)stackPanel.Children[0];
                         button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                     }

                     Button_Click_PreRegistration.Background = new SolidColorBrush(Color.FromRgb(240, 250, 220));
                     wrapPanelPreRegistrationMain.Visibility = Visibility.Visible;


                     wrapPanelPreRegistrationStage1.Children.Add(wrapPanelStage1Menu);
                     wrapPanelPreRegistrationStage1.Children.Add(wrapPanelStage1Buttons);
                     wrapPanelPreRegistrationMain.Children.Add(wrapPanelPreRegistrationStage1);
                     wrapPanelPreRegistrationMain.Children.Add(wrapPanelPreRegistrationStage2);
                     wrapPanelPreRegistrationMain.Children.Add(wrapPanelPreRegistrationStage3);
                     wrapPanelPreRegistrationMain.Children.Add(wrapPanelPreRegistrationStage4);


                     #region подвал PreRegistration
                     WrapPanel wrapPanelPreRegistrationFooter = new WrapPanel();
                     wrapPanelPreRegistrationFooter.Name = "PreRegistrationFooter";
                     wrapPanelPreRegistrationFooter.Orientation = Orientation.Horizontal;

                     wrapPanelPreRegistrationFooter.VerticalAlignment = VerticalAlignment.Bottom;
                     wrapPanelPreRegistrationFooter.Margin = new Thickness(0, 50, 0, 0);

                     StackPanel stackPanelbtnBack = new StackPanel();
                     stackPanelbtnBack.HorizontalAlignment = HorizontalAlignment.Left;
                     stackPanelbtnBack.Width = 500;
                     stackPanelbtnBack.Children.Add(btnBack);
                     wrapPanelPreRegistrationFooter.Children.Add(stackPanelbtnBack);

                     StackPanel stackPanelbtnNextStage = new StackPanel();
                     stackPanelbtnNextStage.HorizontalAlignment = HorizontalAlignment.Right;
                     stackPanelbtnNextStage.Width = 500;
                     stackPanelbtnNextStage.Children.Add(btnNextStage);
                     wrapPanelPreRegistrationFooter.Children.Add(stackPanelbtnNextStage);

                     wrapPanelPreRegistrationMain.Children.Add(wrapPanelPreRegistrationFooter);
                     #endregion
                 };

                BodyWindow.Children.Add(wrapPanelPreRegistrationMain);
                #endregion

                #region Кнопка "Льготная категория граждан" 
                WrapPanel wrapPanelPreferentialСategoryСitizens = new WrapPanel();
                wrapPanelPreferentialСategoryСitizens.Orientation = Orientation.Vertical;
                wrapPanelPreferentialСategoryСitizens.VerticalAlignment = VerticalAlignment.Top;
                wrapPanelPreferentialСategoryСitizens.Visibility = Visibility.Collapsed;
                wrapPanelPreferentialСategoryСitizens.HorizontalAlignment = HorizontalAlignment.Center;
                wrapPanelPreferentialСategoryСitizens.Name = "PreferentialСategoryСitizens";

                this.Button_Click_PreferentialСategoryСitizens.Click += (s, e) =>
                {
                    PreferentialСategoryСitizens();
                    void PreferentialСategoryСitizens()
                    {
                        if (wrapPanelPreferentialСategoryСitizens.Children.Count > 0) wrapPanelPreferentialСategoryСitizens.Children.Clear();
                        TextBlock textBlockPreferentialСategoryСitizens = new TextBlock();
                        textBlockPreferentialСategoryСitizens.FontFamily = new FontFamily("Area");
                        textBlockPreferentialСategoryСitizens.FontSize = 40;
                        textBlockPreferentialСategoryСitizens.HorizontalAlignment = HorizontalAlignment.Center;
                        textBlockPreferentialСategoryСitizens.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
                        textBlockPreferentialСategoryСitizens.TextWrapping = TextWrapping.Wrap;
                        textBlockPreferentialСategoryСitizens.Text = "Льготная категория граждан";
                        StackPanel stackPanelHeadPreferentialСategoryСitizens = new StackPanel();
                        stackPanelHeadPreferentialСategoryСitizens.Orientation = Orientation.Vertical;
                        stackPanelHeadPreferentialСategoryСitizens.VerticalAlignment = VerticalAlignment.Top;
                        stackPanelHeadPreferentialСategoryСitizens.Children.Add(textBlockPreferentialСategoryСitizens);
                        wrapPanelPreferentialСategoryСitizens.Children.Add(stackPanelHeadPreferentialСategoryСitizens);

                        #region Кнопка далее и назад
                        Button btnBack = new Button();
                        DropShadowEffect shadowEffectBack = new DropShadowEffect();
                        shadowEffectBack.Color = Colors.White;
                        shadowEffectBack.ShadowDepth = 3;
                        btnBack.Effect = shadowEffectBack;
                        btnBack.Name = "Back";
                        btnBack.Content = "Назад";
                        btnBack.HorizontalAlignment = HorizontalAlignment.Left;
                        btnBack.VerticalAlignment = VerticalAlignment.Bottom;
                        btnBack.Height = 75;
                        btnBack.Width = 200;
                        btnBack.Background = new SolidColorBrush(Colors.DimGray);
                        btnBack.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
                        btnBack.FontFamily = new FontFamily("Area");
                        btnBack.FontSize = 25;
                        btnBack.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        btnBack.TabIndex = 999;
                        btnBack.Visibility = Visibility.Hidden;
                        ControlTemplate myControlTemplateBack = new ControlTemplate(typeof(Button));
                        FrameworkElementFactory borderBack = new FrameworkElementFactory(typeof(Border));
                        borderBack.Name = "border";
                        borderBack.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
                        borderBack.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
                        borderBack.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
                        borderBack.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
                        FrameworkElementFactory contentPresenterBack = new FrameworkElementFactory(typeof(ContentPresenter));
                        contentPresenterBack.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                        contentPresenterBack.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                        borderBack.AppendChild(contentPresenterBack);
                        myControlTemplateBack.VisualTree = borderBack;
                        btnBack.Template = myControlTemplateBack;

                        Button btnNextStage = new Button();
                        DropShadowEffect shadowEffectNextStage = new DropShadowEffect();
                        shadowEffectNextStage.Color = Colors.White;
                        shadowEffectNextStage.ShadowDepth = 3;
                        btnNextStage.Effect = shadowEffectNextStage;
                        btnNextStage.Name = "Next";
                        btnNextStage.Content = "Далее";
                        btnNextStage.HorizontalAlignment = HorizontalAlignment.Right;
                        btnNextStage.VerticalAlignment = VerticalAlignment.Bottom;
                        btnNextStage.Height = 75;
                        btnNextStage.Width = 200;
                        btnNextStage.Background = new SolidColorBrush(Colors.DarkGreen);
                        btnNextStage.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
                        btnNextStage.FontFamily = new FontFamily("Area");
                        btnNextStage.FontSize = 25;
                        btnNextStage.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        btnNextStage.TabIndex = 999;
                        btnNextStage.Visibility = Visibility.Hidden;
                        ControlTemplate myControlTemplateNextStage = new ControlTemplate(typeof(Button));
                        FrameworkElementFactory borderNextStage = new FrameworkElementFactory(typeof(Border));
                        borderNextStage.Name = "border";
                        borderNextStage.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
                        borderNextStage.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
                        borderNextStage.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
                        borderNextStage.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
                        FrameworkElementFactory contentPresenterNextStage = new FrameworkElementFactory(typeof(ContentPresenter));
                        contentPresenterNextStage.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                        contentPresenterNextStage.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                        borderNextStage.AppendChild(contentPresenterNextStage);
                        myControlTemplateNextStage.VisualTree = borderNextStage;
                        btnNextStage.Template = myControlTemplateNextStage;
                        #endregion

                        WrapPanel wrapPanelPriooritetButons = new WrapPanel();
                        wrapPanelPriooritetButons.Orientation = Orientation.Horizontal;
                        wrapPanelPriooritetButons.VerticalAlignment = VerticalAlignment.Center;
                        wrapPanelPriooritetButons.HorizontalAlignment = HorizontalAlignment.Center;

                        //меню
                        WrapPanel wrapPanelPriooritetMenu = new WrapPanel();
                        wrapPanelPriooritetMenu.Orientation = Orientation.Horizontal;
                        wrapPanelPriooritetMenu.VerticalAlignment = VerticalAlignment.Center;
                        wrapPanelPriooritetMenu.HorizontalAlignment = HorizontalAlignment.Center;

                        //кнопки меню
                        WrapPanel wrapPanelPriooritetMenuButons = new WrapPanel();
                        wrapPanelPriooritetMenuButons.Orientation = Orientation.Horizontal;
                        wrapPanelPriooritetMenuButons.VerticalAlignment = VerticalAlignment.Center;
                        wrapPanelPriooritetMenuButons.HorizontalAlignment = HorizontalAlignment.Center;
                        //кнопки категории
                        if (eqContext.SPriorities.Any())
                        {
                            eqContext.SPriorities.ToList().ForEach(priooritet =>
                            {
                                TextBlock textBtnPriooritet = new TextBlock();
                                textBtnPriooritet.FontFamily = new FontFamily("Area");
                                textBtnPriooritet.FontSize = 25;
                                textBtnPriooritet.HorizontalAlignment = HorizontalAlignment.Center;
                                textBtnPriooritet.Foreground = new SolidColorBrush(Colors.White);
                                textBtnPriooritet.TextWrapping = TextWrapping.Wrap;
                                textBtnPriooritet.Padding = new Thickness(15);
                                textBtnPriooritet.Text = priooritet.PriorityName + "\n" + priooritet.Commentt;

                                Button btnPriooritet = new Button();
                                DropShadowEffect shadowEffectPriooritet = new DropShadowEffect();
                                shadowEffectPriooritet.Color = Colors.White;
                                shadowEffectPriooritet.ShadowDepth = 3;
                                btnPriooritet.Effect = shadowEffectPriooritet;
                                btnPriooritet.Name = "buttonPriooritet";
                                btnPriooritet.Content = textBtnPriooritet;
                                btnPriooritet.HorizontalAlignment = HorizontalAlignment.Center;
                                btnPriooritet.VerticalAlignment = VerticalAlignment.Top;
                                btnPriooritet.Margin = new Thickness(0, 18, 32, 0);
                                btnPriooritet.Padding = new Thickness(20);
                                btnPriooritet.Background = new SolidColorBrush(Colors.DarkGoldenrod);
                                btnPriooritet.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
                                btnPriooritet.FontFamily = new FontFamily("Area");
                                btnPriooritet.Width = 250;
                                btnPriooritet.TabIndex = 999;
                                ControlTemplate myControlTemplatePriooritet = new ControlTemplate(typeof(Button));
                                FrameworkElementFactory borderPriooritet = new FrameworkElementFactory(typeof(Border));
                                borderPriooritet.Name = "border";
                                borderPriooritet.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
                                borderPriooritet.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
                                borderPriooritet.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
                                borderPriooritet.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
                                FrameworkElementFactory contentPriooritet = new FrameworkElementFactory(typeof(ContentPresenter));
                                contentPriooritet.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                                contentPriooritet.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                                borderPriooritet.AppendChild(contentPriooritet);
                                myControlTemplatePriooritet.VisualTree = borderPriooritet;
                                btnPriooritet.Template = myControlTemplatePriooritet;

                                btnPriooritet.Click += (s, e) =>
                                {
                                    btnBack.Visibility = Visibility.Visible;
                                    btnBack.Click += (s, e) =>
                                    {
                                        PreferentialСategoryСitizens();
                                    };

                                    wrapPanelPriooritetButons.Visibility = Visibility.Collapsed;
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
                                            btnMenu.Name = "button";
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
                                            textBlockMenu.TextWrapping = TextWrapping.Wrap;
                                            textBlockMenu.Text = b.ButtonName;

                                            WrapPanel warpPanelHeadMenu = new WrapPanel();
                                            warpPanelHeadMenu.Orientation = Orientation.Horizontal;
                                            warpPanelHeadMenu.VerticalAlignment = VerticalAlignment.Center;
                                            warpPanelHeadMenu.Visibility = Visibility.Collapsed;
                                            warpPanelHeadMenu.Margin = new Thickness(25, 0, 0, 0);
                                            warpPanelHeadMenu.Children.Add(textBlockMenu);
                                            wrapPanelPriooritetMenu.Children.Add(warpPanelHeadMenu);

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
                                                    EqContext eqContext = new EqContext();
                                                    IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                                                    string IpOffise = localIPs.Where(w => w.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(w => w.ToString()).First();

                                                    FastReport.Report report = new FastReport.Report();
                                                    var path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))) + "\\FastReport\\Operator.frx";
                                                    report.Load(path);
                                                    var LastTicketNumber = eqContext.DTickets.Where(s => s.SOfficeTerminal.IpAddress == IpOffise && s.SServiceId == sServices.Id && s.DateRegistration == DateOnly.FromDateTime(DateTime.Now)).OrderByDescending(o => o.TicketNumber).Select(s => s.TicketNumber).FirstOrDefault();

                                                    DTicket dTicket_New = new DTicket();
                                                    dTicket_New.SOfficeId = eqContext.SOfficeTerminals.First(s => s.IpAddress == IpOffise).SOfficeId;
                                                    dTicket_New.SOfficeTerminalId = eqContext.SOfficeTerminals.First(s => s.IpAddress == IpOffise).Id;
                                                    dTicket_New.SServiceId = sServices.Id;
                                                    dTicket_New.ServicePrefix = sServices.ServicePrefix;
                                                    dTicket_New.SPriorityId = priooritet.Id;
                                                    dTicket_New.TicketNumber = LastTicketNumber + 1;
                                                    dTicket_New.TicketNumberFull = sServices.ServicePrefix + (LastTicketNumber + 1);
                                                    dTicket_New.SStatusId = 1;
                                                    dTicket_New.DateRegistration = DateOnly.FromDateTime(DateTime.Now);
                                                    dTicket_New.TimeRegistration = TimeOnly.FromDateTime(DateTime.Now);

                                                    DTicketStatus dTicketStatus = new DTicketStatus
                                                    {
                                                        SStatusId = 1
                                                    };

                                                    dTicket_New.DTicketStatuses.Add(dTicketStatus);

                                                    eqContext.DTickets.Add(dTicket_New);
                                                    eqContext.SaveChanges();

                                                    report.SetParameterValue("Operation", sServices.ServiceName);
                                                    report.SetParameterValue("Number", dTicket_New.TicketNumberFull);
                                                    report.SetParameterValue("Time", dTicket_New.TimeRegistration);
                                                    report.SetParameterValue("TotalQueue", eqContext.DTickets.Where(s => s.SOfficeTerminal.IpAddress == IpOffise && s.DateRegistration == DateOnly.FromDateTime(DateTime.Now)).Count());
                                                    report.SetParameterValue("BeforeCount", LastTicketNumber);
                                                    report.SetParameterValue("MFC", eqContext.SOffices.First(s => s.Id == 1).OfficeName);
                                                    report.Prepare();
                                                    report.PrintSettings.ShowDialog = false;
                                                    report.PrintSettings.PrintOnSheetRawPaperSize = 0;
                                                    report.Print();
                                                };
                                                wrapPanel.Children.Add(btn);
                                            });
                                            wrapPanelPriooritetMenuButons.Children.Add(wrapPanel);

                                            btnMenu.Click += (s, e) =>
                                            {
                                                wrapPanel.Visibility = Visibility.Visible;
                                                warpPanelHeadMenu.Visibility = Visibility.Visible;
                                                wrapPanelPriooritetMenuButons.Visibility = Visibility.Visible;
                                                wrapPanelPriooritetMenu.Visibility = Visibility.Collapsed;
                                                btnBack.Visibility = Visibility.Visible;

                                                btnBack.Click += (s, e) =>
                                                {
                                                    btnBack.Visibility = Visibility.Collapsed;
                                                    wrapPanel.Visibility = Visibility.Collapsed;
                                                    warpPanelHeadMenu.Visibility = Visibility.Collapsed;
                                                    wrapPanelPriooritetMenuButons.Visibility = Visibility.Collapsed;
                                                    wrapPanelPriooritetMenu.Visibility = Visibility.Visible;
                                                };
                                            };
                                            wrapPanelPriooritetMenu.Children.Add(btnMenu);
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
                                                EqContext eqContext = new EqContext();
                                                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                                                string IpOffise = localIPs.Where(w => w.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(w => w.ToString()).First();

                                                FastReport.Report report = new FastReport.Report();
                                                var path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))) + "\\FastReport\\Operator.frx";
                                                report.Load(path);
                                                var LastTicketNumber = eqContext.DTickets.Where(s => s.SOfficeTerminal.IpAddress == IpOffise && s.SServiceId == sServices.Id && s.DateRegistration == DateOnly.FromDateTime(DateTime.Now)).OrderByDescending(o => o.TicketNumber).Select(s => s.TicketNumber).FirstOrDefault();

                                                DTicket dTicket_New = new DTicket();
                                                dTicket_New.SOfficeId = eqContext.SOfficeTerminals.First(s => s.IpAddress == IpOffise).SOfficeId;
                                                dTicket_New.SOfficeTerminalId = eqContext.SOfficeTerminals.First(s => s.IpAddress == IpOffise).Id;
                                                dTicket_New.SServiceId = sServices.Id;
                                                dTicket_New.ServicePrefix = sServices.ServicePrefix;
                                                dTicket_New.SPriorityId = priooritet.Id;
                                                dTicket_New.TicketNumber = LastTicketNumber + 1;
                                                dTicket_New.TicketNumberFull = sServices.ServicePrefix + (LastTicketNumber + 1);
                                                dTicket_New.SStatusId = 1;
                                                dTicket_New.DateRegistration = DateOnly.FromDateTime(DateTime.Now);
                                                dTicket_New.TimeRegistration = TimeOnly.FromDateTime(DateTime.Now);

                                                DTicketStatus dTicketStatus = new DTicketStatus
                                                {
                                                    // DTicketId = eqContext.DTickets.First(s => s.SOfficeTerminal.IpAddress == IpOffise && s.DateRegistration == dTicket_New.DateRegistration && s.TimeRegistration == dTicket_New.TimeRegistration).Id,
                                                    SStatusId = 1
                                                };

                                                dTicket_New.DTicketStatuses.Add(dTicketStatus);

                                                eqContext.DTickets.Add(dTicket_New);
                                                eqContext.SaveChanges();

                                                report.SetParameterValue("Operation", sServices.ServiceName);
                                                report.SetParameterValue("Number", dTicket_New.TicketNumberFull);
                                                report.SetParameterValue("Time", dTicket_New.TimeRegistration);
                                                report.SetParameterValue("TotalQueue", eqContext.DTickets.Where(s => s.SOfficeTerminal.IpAddress == IpOffise && s.DateRegistration == DateOnly.FromDateTime(DateTime.Now)).Count());
                                                report.SetParameterValue("BeforeCount", LastTicketNumber);
                                                report.SetParameterValue("MFC", eqContext.SOffices.First(s => s.Id == 1).OfficeName);
                                                report.Prepare();
                                                report.PrintSettings.ShowDialog = false;
                                                report.PrintSettings.PrintOnSheetRawPaperSize = 0;
                                                report.Print();
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
                                            wrapPanelPriooritetMenu.Children.Add(stackPanelBtn);
                                        }
                                    });
                                };
                                wrapPanelPreferentialСategoryСitizens.Children.Add(wrapPanelPriooritetMenu);
                                wrapPanelPreferentialСategoryСitizens.Children.Add(wrapPanelPriooritetMenuButons);
                                wrapPanelPriooritetButons.Children.Add(btnPriooritet);
                            });
                        }
                        wrapPanelPreferentialСategoryСitizens.Children.Add(wrapPanelPriooritetButons);
                        #region подвал PreRegistration 
                        WrapPanel wrapPanelPriooritetFooter = new WrapPanel();
                        wrapPanelPriooritetFooter.Name = "PreRegistrationFooter";
                        wrapPanelPriooritetFooter.Orientation = Orientation.Horizontal;

                        wrapPanelPriooritetFooter.VerticalAlignment = VerticalAlignment.Bottom;
                        wrapPanelPriooritetFooter.Margin = new Thickness(0, 50, 0, 0);

                        StackPanel stackPanelbtnBack = new StackPanel();
                        stackPanelbtnBack.HorizontalAlignment = HorizontalAlignment.Left;
                        stackPanelbtnBack.Width = 500;
                        stackPanelbtnBack.Children.Add(btnBack);
                        wrapPanelPriooritetFooter.Children.Add(stackPanelbtnBack);

                        StackPanel stackPanelbtnNextStage = new StackPanel();
                        stackPanelbtnNextStage.HorizontalAlignment = HorizontalAlignment.Right;
                        stackPanelbtnNextStage.Width = 500;
                        stackPanelbtnNextStage.Children.Add(btnNextStage);
                        wrapPanelPriooritetFooter.Children.Add(stackPanelbtnNextStage);

                        #endregion
                        foreach (WrapPanel obj in BodyWindow.Children)
                        {
                            obj.Visibility = Visibility.Collapsed;
                            if (obj.Name == "PreferentialСategoryСitizens")
                            {
                                obj.Visibility = Visibility.Visible;
                            }
                        }
                        StackClose.Visibility = Visibility.Visible;

                        foreach (StackPanel stackPanel in Buttons_Service.Children)
                        {
                            Button button = (Button)stackPanel.Children[0];
                            button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        }

                        Button_Click_PreferentialСategoryСitizens.Background = new SolidColorBrush(Color.FromRgb(240, 250, 220));


                        wrapPanelPreferentialСategoryСitizens.Children.Add(wrapPanelPriooritetFooter);

                    };
                };

                BodyWindow.Children.Add(wrapPanelPreferentialСategoryСitizens);
                #endregion

                #region Кнопка "Регистрация по предварительной записи"

                WrapPanel wrapPanelRegistrationAppointment = new WrapPanel();
                wrapPanelRegistrationAppointment.Orientation = Orientation.Vertical;
                wrapPanelRegistrationAppointment.VerticalAlignment = VerticalAlignment.Top;
                wrapPanelRegistrationAppointment.Visibility = Visibility.Collapsed;
                wrapPanelRegistrationAppointment.HorizontalAlignment = HorizontalAlignment.Center;
                wrapPanelRegistrationAppointment.Name = "RegistrationAppointment";

                this.Button_Click_RegistrationAppointment.Click += (s, e) =>
                {
                    if (wrapPanelRegistrationAppointment.Children.Count > 0) wrapPanelRegistrationAppointment.Children.Clear();
                    TextBlock textBlockRegistrationAppointment = new TextBlock();
                    textBlockRegistrationAppointment.FontFamily = new FontFamily("Area");
                    textBlockRegistrationAppointment.FontSize = 60;
                    textBlockRegistrationAppointment.TextWrapping = TextWrapping.Wrap;
                    textBlockRegistrationAppointment.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlockRegistrationAppointment.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
                    textBlockRegistrationAppointment.Text = "Введите код";

                    //ошибки при вводе
                    TextBlock textBlockRegistrationAppointmentError = new TextBlock();
                    textBlockRegistrationAppointmentError.FontFamily = new FontFamily("Area");
                    textBlockRegistrationAppointmentError.FontSize = 30;
                    textBlockRegistrationAppointmentError.TextWrapping = TextWrapping.Wrap;
                    textBlockRegistrationAppointmentError.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlockRegistrationAppointmentError.Foreground = new SolidColorBrush(Colors.Red);

                    //поле для ввода
                    TextBox textBoxRegistrationAppointment = new TextBox();
                    textBoxRegistrationAppointment.FontSize = 50;
                    textBoxRegistrationAppointment.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 100));
                    textBoxRegistrationAppointment.Margin = new Thickness(0, 20, 0, 20);
                    textBoxRegistrationAppointment.Width = 310;
                    textBoxRegistrationAppointment.TextWrapping = TextWrapping.Wrap;
                    textBoxRegistrationAppointment.Focus();

                    StackPanel stackPanelHeadRegistrationAppointment = new StackPanel();
                    stackPanelHeadRegistrationAppointment.Orientation = Orientation.Vertical;
                    stackPanelHeadRegistrationAppointment.VerticalAlignment = VerticalAlignment.Top;
                    stackPanelHeadRegistrationAppointment.Children.Add(textBlockRegistrationAppointment);
                    stackPanelHeadRegistrationAppointment.Children.Add(textBlockRegistrationAppointmentError);
                    stackPanelHeadRegistrationAppointment.Children.Add(textBoxRegistrationAppointment);

                    wrapPanelRegistrationAppointment.Children.Add(stackPanelHeadRegistrationAppointment);

                    //клавиатура
                    StackPanel stackPanelKeyboardRegistrationAppointment = new StackPanel();
                    stackPanelKeyboardRegistrationAppointment.Children.Add((StackPanel)this.FindResource("KeyboardNumberRegistrationAppointment"));
                    bool upperCase = false;
                    foreach (StackPanel item in stackPanelKeyboardRegistrationAppointment.Children)
                    {
                        foreach (StackPanel stackPanel in item.Children)
                        {
                            foreach (Button button in stackPanel.Children)
                            {
                                button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                button.Foreground = new SolidColorBrush(Colors.Brown);
                                button.BorderBrush = button.Foreground;
                                button.FontWeight = FontWeights.Black;
                                button.FontSize = 20;
                                button.Margin = new Thickness(5);
                                button.Click += (s, e) =>
                                {
                                    Button buttonClick = (Button)s;
                                    textBlockRegistrationAppointmentError.Text = "";
                                    textBoxRegistrationAppointment.BorderBrush = new SolidColorBrush(Colors.Black);
                                    textBoxRegistrationAppointment.Foreground = new SolidColorBrush(Colors.Black);
                                    switch (buttonClick.Content.ToString())
                                    {
                                        case "Удалить":
                                            textBoxRegistrationAppointment.Text = textBoxRegistrationAppointment.Text.Length == 0 ? "" : textBoxRegistrationAppointment.Text.Substring(0, textBoxRegistrationAppointment.Text.Length - 1);
                                            break;
                                        case "Ввод":
                                            if (textBoxRegistrationAppointment.Text.Length != 4)
                                            {
                                                textBlockRegistrationAppointmentError.Text = "Код не коректен !";
                                                textBoxRegistrationAppointment.BorderBrush = new SolidColorBrush(Colors.Red);
                                                textBoxRegistrationAppointment.Foreground = new SolidColorBrush(Colors.Red);
                                            }
                                            else
                                            {
                                                var prerecord = eqContext.DTicketPrerecords.Where(d => d.CodePrerecord == Convert.ToInt64(textBoxRegistrationAppointment.Text) && d.SOfficeId == officeId).FirstOrDefault();
                                                if (prerecord == null)
                                                {
                                                    textBlockRegistrationAppointmentError.Text = "Неверный код !";
                                                    textBoxRegistrationAppointment.BorderBrush = new SolidColorBrush(Colors.Red);
                                                    textBoxRegistrationAppointment.Foreground = new SolidColorBrush(Colors.Red);
                                                }
                                                else
                                                if (prerecord.DatePrerecord > DateOnly.FromDateTime(DateTime.Now) || prerecord.StartTimePrerecord > TimeOnly.FromDateTime(DateTime.Now))
                                                {
                                                    textBlockRegistrationAppointmentError.Text = "Время предварительной записи не вышло.Вы должны явиться " + prerecord.DatePrerecord + " с " + prerecord.StartTimePrerecord + " по " + prerecord.StopTimePrerecord;
                                                }
                                                else
                                                if (prerecord.DatePrerecord < DateOnly.FromDateTime(DateTime.Now) || prerecord.StopTimePrerecord < TimeOnly.FromDateTime(DateTime.Now))
                                                {
                                                    textBlockRegistrationAppointmentError.Text = "Время предварительной записи вышло.Вы должны были явиться " + prerecord.DatePrerecord + " с " + prerecord.StartTimePrerecord + " по " + prerecord.StopTimePrerecord;
                                                }
                                                else
                                                {
                                                    EqContext eqContext = new EqContext();
                                                    IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                                                    string IpOffise = localIPs.Where(w => w.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(w => w.ToString()).First();
                                                    SService sService = eqContext.SServices.First(f => f.Id == prerecord.SServiceId);
                                                    FastReport.Report report = new FastReport.Report();
                                                    var path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))) + "\\FastReport\\Operator.frx";
                                                    report.Load(path);

                                                    var LastTicketNumber = eqContext.DTickets.Where(s => s.SOfficeTerminal.IpAddress == IpOffise && s.SServiceId == sService.Id && s.DateRegistration == DateOnly.FromDateTime(DateTime.Now)).OrderByDescending(o => o.TicketNumber).Select(s => s.TicketNumber).FirstOrDefault();

                                                    DTicket dTicket_New = new DTicket();
                                                    dTicket_New.SOfficeId = eqContext.SOfficeTerminals.First(s => s.IpAddress == IpOffise).SOfficeId;
                                                    dTicket_New.SOfficeTerminalId = eqContext.SOfficeTerminals.First(s => s.IpAddress == IpOffise).Id;
                                                    dTicket_New.SServiceId = sService.Id;
                                                    dTicket_New.ServicePrefix = sService.ServicePrefix;
                                                    //dTicket.SPriorityId = 1; 
                                                    dTicket_New.TicketNumber = LastTicketNumber + 1;
                                                    dTicket_New.TicketNumberFull = sService.ServicePrefix + (LastTicketNumber + 1);
                                                    dTicket_New.DTicketPrerecordId = prerecord.Id;
                                                    dTicket_New.SStatusId = 1;
                                                    //dTicket.SEmployeeId = 1;
                                                    //dTicket.SOfficeWindowId = 1;
                                                    dTicket_New.DateRegistration = DateOnly.FromDateTime(DateTime.Now);
                                                    dTicket_New.TimeRegistration = TimeOnly.FromDateTime(DateTime.Now);

                                                    DTicketStatus dTicketStatus = new DTicketStatus
                                                    {
                                                        SStatusId = 1
                                                    };

                                                    dTicket_New.DTicketStatuses.Add(dTicketStatus);
                                                    eqContext.DTickets.Add(dTicket_New);
                                                    eqContext.DTicketPrerecords.First(f => f.Id == prerecord.Id).IsConfirmation = true;

                                                    eqContext.SaveChanges();
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
                                            }
                                            break;
                                        default:
                                            textBoxRegistrationAppointment.Text += textBoxRegistrationAppointment.Text.Length == 4 ? "" : upperCase ? button.Content.ToString().ToLower() : button.Content.ToString().ToUpper();
                                            break;
                                    }
                                    textBoxRegistrationAppointment.CaretIndex = textBoxRegistrationAppointment.Text.Length;
                                    textBoxRegistrationAppointment.Focus();
                                };
                            }
                        }
                    }

                    wrapPanelRegistrationAppointment.Children.Add(stackPanelKeyboardRegistrationAppointment);

                    foreach (WrapPanel obj in BodyWindow.Children)
                    {
                        obj.Visibility = Visibility.Collapsed;
                        if (obj.Name == "RegistrationAppointment")
                        {
                            obj.Visibility = Visibility.Visible;
                        }
                    }
                    StackClose.Visibility = Visibility.Visible;

                    foreach (StackPanel stackPanel in Buttons_Service.Children)
                    {
                        Button button = (Button)stackPanel.Children[0];
                        button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    }
                    Button_Click_RegistrationAppointment.Background = new SolidColorBrush(Color.FromRgb(240, 250, 220));
                };

                BodyWindow.Children.Add(wrapPanelRegistrationAppointment);

                #endregion

                #region Кнопка Домой

                try
                {
                    CloseButton.Background = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new System.Uri(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))) + "/img/home_red_icon.jpg"))
                    };
                }
                catch (Exception ex)
                {

                }

                this.CloseButton.Click += (s, e) =>
                {
                    Home(s, e);
                };

                void Home(object sender, RoutedEventArgs e)
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
                    foreach (StackPanel stackPanel in Buttons_Service.Children)
                    {
                        Button button = (Button)stackPanel.Children[0];
                        button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    }
                }

                #endregion
            }

        }


        #region Поставка на очередь
        private async Task Click_Button(object sender, RoutedEventArgs e, SService sService)
        {
            EqContext eqContext = new EqContext();
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            string IpOffise = localIPs.Where(w => w.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(w => w.ToString()).First();

            FastReport.Report report = new FastReport.Report();
            var path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))) + "\\FastReport\\Operator.frx";
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
                System.Windows.Application.Current.Shutdown();
            }
        }
        #endregion

        #region Обновляем дату и время при каждом срабатывании таймера
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Обновляем дату и время при каждом срабатывании таймера
            UpdateDateTime();
        }
        #endregion

        #region Обновляем значения даты и времени
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
        #endregion

        #region Генерация уникального кода
        static long GenerateUniqueCode(List<long> existingNumbers)
        {
            Random random = new Random();

            while (true)
            {
                long code = random.Next(1000, 10000);
                long[] codeDigits = code.ToString().ToCharArray().Select(c => long.Parse(c.ToString())).ToArray();

                bool isUnique = true;
                foreach (long digit in codeDigits)
                {
                    if (existingNumbers.Contains(digit))
                    {
                        isUnique = false;
                        break;
                    }
                }

                if (isUnique)
                {
                    return code;
                }
            }
        }
        #endregion


    }
}