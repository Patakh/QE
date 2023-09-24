using Microsoft.EntityFrameworkCore;
using QE.Models;
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


            //Блок 1 этапа
            WrapPanel wrapPanelPreRegistrationStage1 = new WrapPanel();
            wrapPanelPreRegistrationStage1.HorizontalAlignment = HorizontalAlignment.Center;
            wrapPanelPreRegistrationStage1.Name = "PreRegistrationStage1";

            //Блок 2 этапа
            WrapPanel wrapPanelPreRegistrationStage2 = new WrapPanel();
            wrapPanelPreRegistrationStage2.HorizontalAlignment = HorizontalAlignment.Center;
            wrapPanelPreRegistrationStage2.Name = "PreRegistrationStage2";

            //нажатие на кнопку "Предварительная запись"
            this.Button_Click_PreRegistration.Click += (s, e) =>
              {

                  #region Этап 1
                  WrapPanel wrapPanelStage1Menu = new WrapPanel();

                  wrapPanelStage1Menu.Name = "PreRegistrationStage1Menu";

                  WrapPanel wrapPanelStage1Buttons = new WrapPanel();
                  wrapPanelStage1Buttons.Name = "PreRegistrationStage1Buttons";

                  if (wrapPanelPreRegistrationMain.Children.Count > 0) wrapPanelPreRegistrationMain.Children.Clear();
                  if (wrapPanelPreRegistrationStage1.Children.Count > 0) wrapPanelPreRegistrationStage1.Children.Clear();


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
                  textBlockPreRegistration.FontSize = 60;
                  textBlockPreRegistration.Margin = new Thickness(0, 0, 0, 100);
                  textBlockPreRegistration.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));

                  wrapPanelPreRegistrationMain.Children.Add(textBlockPreRegistration);

                  // меню и кнопки 
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

                          //создаем кнопки меню
                          List<SService> sServices = new List<SService>();
                          WrapPanel wrapPanel = new WrapPanel();
                          wrapPanel.Orientation = Orientation.Horizontal;
                          wrapPanel.HorizontalAlignment = HorizontalAlignment.Center;
                          wrapPanel.Visibility = Visibility.Collapsed;
                          wrapPanel.MaxWidth = 800;
                          SOfficeTerminalButton.ToList().ForEach(button =>
                          {
                              int Btn_idx = 1;
                              SService sServices = eqContext.SServices.First(f => f.Id == button.SServiceId);
                              Button btn = new Button();
                              btn.Name = "button" + Btn_idx;
                              btn.Tag = sServices.Id;
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

                                  foreach (Button button in wrapPanel.Children)
                                  {
                                      button.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                                  };

                                  btn.Background = new SolidColorBrush(Color.FromRgb(100, 250, 255));

                                  btnNextStage.Visibility = Visibility.Visible;


                                  btnNextStage.Click += (s, e) =>
                                  {
                                      btn.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));

                                  };
                              };
                              wrapPanel.Children.Add(btn);
                          });

                          wrapPanelStage1Buttons.Children.Add(wrapPanel);
                          btnMenu.Click += (s, e) =>
                          {
                              StackClose.Visibility = Visibility.Visible;
                              wrapPanel.Visibility = Visibility.Visible;
                              wrapPanelStage1Buttons.Visibility = Visibility.Visible;
                              wrapPanelStage1Menu.Visibility = Visibility.Collapsed;
                              btnBack.Visibility = Visibility.Visible;
                              btnNextStage.Visibility = Visibility.Hidden;

                              foreach (Button button in wrapPanelStage1Menu.Children)
                              {
                                  if (button.Name != "menu")
                                  {
                                      button.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                                  };
                              };
                          };

                          btnBack.Click += (s, e) =>
                          {
                              wrapPanel.Visibility = Visibility.Collapsed;
                              wrapPanelStage1Buttons.Visibility = Visibility.Collapsed;
                              wrapPanelStage1Menu.Visibility = Visibility.Visible;
                              btnBack.Visibility = Visibility.Collapsed;
                              btnNextStage.Visibility = Visibility.Hidden;

                              foreach (Button button in wrapPanel.Children)
                              {
                                  button.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                              };
                          };

                          wrapPanelStage1Menu.Children.Add(btnMenu);
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
                              btnNextStage.Visibility = Visibility.Visible;
                              btn.Background = new SolidColorBrush(Color.FromRgb(100, 250, 255));

                              btnNextStage.Click += (s, e) =>
                              {
                                  btn.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255)); 
                              };
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
                          wrapPanelStage1Menu.Children.Add(btn);
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
                  #endregion

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
            TextBlock textBlockPreferentialСategoryСitizens = new TextBlock();
            textBlockPreferentialСategoryСitizens.FontFamily = new FontFamily("Area");
            textBlockPreferentialСategoryСitizens.FontSize = 60;
            textBlockPreferentialСategoryСitizens.HorizontalAlignment = HorizontalAlignment.Center;
            textBlockPreferentialСategoryСitizens.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
            textBlockPreferentialСategoryСitizens.Text = "Введите код";

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
                        button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        button.Foreground = new SolidColorBrush(Colors.Brown);
                        button.BorderBrush = button.Foreground;
                        button.FontWeight = FontWeights.Black;
                        button.FontSize = 16;
                        button.Margin = new Thickness(5);
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
                                    textBoxPreferentialСategoryСitizens.Text += button.Content;
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
            };
            #endregion

            #region Кнопка "Регистрация по предварительной записи"

            TextBlock textBlockRegistrationAppointment = new TextBlock();
            textBlockRegistrationAppointment.FontFamily = new FontFamily("Area");
            textBlockRegistrationAppointment.FontSize = 60;
            textBlockRegistrationAppointment.HorizontalAlignment = HorizontalAlignment.Center;
            textBlockRegistrationAppointment.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
            textBlockRegistrationAppointment.Text = "Введите код";

            //ошибки при вводе
            TextBlock textBlockRegistrationAppointmentError = new TextBlock();
            textBlockRegistrationAppointmentError.FontFamily = new FontFamily("Area");
            textBlockRegistrationAppointmentError.FontSize = 30;
            textBlockRegistrationAppointmentError.HorizontalAlignment = HorizontalAlignment.Center;
            textBlockRegistrationAppointmentError.Foreground = new SolidColorBrush(Colors.Red);

            //поле для ввода
            TextBox textBoxRegistrationAppointment = new TextBox();
            textBoxRegistrationAppointment.FontSize = 50;
            textBoxRegistrationAppointment.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 100));
            textBoxRegistrationAppointment.Margin = new Thickness(0, 20, 0, 20);
            textBoxRegistrationAppointment.TextWrapping = TextWrapping.Wrap;
            textBoxRegistrationAppointment.Focus();

            StackPanel stackPanelHeadRegistrationAppointment = new StackPanel();
            stackPanelHeadRegistrationAppointment.Orientation = Orientation.Vertical;
            stackPanelHeadRegistrationAppointment.VerticalAlignment = VerticalAlignment.Top;
            stackPanelHeadRegistrationAppointment.Children.Add(textBlockRegistrationAppointment);
            stackPanelHeadRegistrationAppointment.Children.Add(textBlockRegistrationAppointmentError);
            stackPanelHeadRegistrationAppointment.Children.Add(textBoxRegistrationAppointment);

            WrapPanel wrapPanelRegistrationAppointment = new WrapPanel();
            wrapPanelRegistrationAppointment.Orientation = Orientation.Vertical;
            wrapPanelRegistrationAppointment.VerticalAlignment = VerticalAlignment.Top;
            wrapPanelRegistrationAppointment.Visibility = Visibility.Collapsed;
            wrapPanelRegistrationAppointment.HorizontalAlignment = HorizontalAlignment.Center;
            wrapPanelRegistrationAppointment.Name = "RegistrationAppointment";
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
                            switch (buttonClick.Content.ToString())
                            {
                                case "Удалить":
                                    textBoxRegistrationAppointment.Text = textBoxRegistrationAppointment.Text.Length == 0 ? "" : textBoxRegistrationAppointment.Text.Substring(0, textBoxRegistrationAppointment.Text.Length - 1);
                                    break;
                                case "Ввод":
                                    if (textBoxRegistrationAppointment.Text.Length != 4)
                                    {
                                        textBlockRegistrationAppointmentError.Text = "Неверный код !";
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
            BodyWindow.Children.Add(wrapPanelRegistrationAppointment);

            this.Button_Click_RegistrationAppointment.Click += (s, e) =>
            {
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
                foreach (StackPanel stackPanel in Buttons_Service.Children)
                {
                    Button button = (Button)stackPanel.Children[0];
                    button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
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
    }
}
