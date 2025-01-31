using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Reflection;
using System;
using AWPClient.Controls;
using AWPClient.LogServices;
using AWPClient.Classes;
using Avalonia.Input;
using System.Linq;
using Avalonia.Threading;
using Avalonia;
using System.Diagnostics;
using System.Xml;
using CoreScanner;
using System.Threading;
using Avalonia.Interactivity;
using AWPClient.Msgbox;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Data;
using Setter = AWPClient.Classes.Setter;
using AWPClient.ViewModels;
using Avalonia.Platform;
using static AWPClient.ViewModels.MainWindowViewModel;
using AWPClient.Connection;
using Avalonia.Data;
using System.IO;
using System.ComponentModel;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using System.Xml.Linq;
using NAudio.Wave;
using System.Net;
using System.Speech.Synthesis;
using LEAD_UPD_CREATOR.Helpers;
using AWPClient.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BolapanControl.ItemsFilter.ViewModel;


namespace AWPClient.Views;
public partial class MainWindow : Window
{
    /// <summary>
    /// Кнопки Буфера сканирования и контроля версий
    /// </summary>
    private Button? queueDControl;
    private Button? AWPClientVersion;

    private List<ScanQueueItem> scanQueue = [];
    private List<VersionUpdate> versionUpdates = [];

    List<DrawScreen> drawScreens = [];

    private List<string> scanPrefixes = [];
   
    private Control? queuedScanbox;

    Guid screenID;

    DispatcherTimer screenTimer = new();
    DispatcherTimer autosubmitTimer = new();
    DispatcherTimer gridTimer = new ();
    DispatcherTimer queueTimer = new ();
    DispatcherTimer queueDTimer = new ();
    DispatcherTimer queueLogTimer = new ();

    private bool screenAcceptsScan = false;

    class selectRow
    {
        public DataGrid grid;
        public DataRowView row;
    }
    List<selectRow> selectRows = [];

    List<ReplaceInfo> ReplaceList = [];

    List<Commer> CommersList = [];

    private Grid grid;

    const int STATUS_SUCCESS = 0;
    const int STATUS_FALSE = 1;

    public const short SCALE_TYPES_IBM = 10;
    public const short SCALE_TYPES_SSI_BT = 11;
    public const short CAMERA_TYPES_UVC = 14;
    public const short TOTAL_SCANNER_TYPES = CAMERA_TYPES_UVC;
    const int CLAIM_DEVICE = 1500;
    const int RELEASE_DEVICE = 1501;
    const int MAX_NUM_DEVICES = 255;
    const int REGISTER_FOR_EVENTS = 1001;
    const int NUM_SCANNER_EVENTS = 6;
    const int SUBSCRIBE_BARCODE = 1;
    const int SUBSCRIBE_IMAGE = 2;
    const int SUBSCRIBE_VIDEO = 4;
    const int SUBSCRIBE_RMD = 8;
    const int SUBSCRIBE_PNP = 16;
    const int SUBSCRIBE_OTHER = 32;
    bool SNAPI_DRIVER_LOADED = true;
    bool COM_PORT_CONNECTED = true;

    CCoreScannerClass? m_pCoreScanner;
    Scanner[]? m_arScanners;
    XmlReader? m_xml;

    List<string> claimlist = [];
    private string clientVersion;
    private bool msgBoxEnabled = false;
    private bool scanModeMsgEnabled = false;
    private bool timerLogEnabled = false;
    private bool gridLogEnabled = false;
    private bool debugLogEnabled = false;
    private bool canSkipScreenDraw = true;
    private string CurrentXAML = "";
    private bool logMaterialListSelection = false;
    private string clientName = "";
    private string[] specialCharacterBrackets = new string[2];
    private int autoSubmitTimeoutMS = 0;
 
    private string useComPort = "";

    bool _allowCheckBoxChangeOnSelection = false;
    SerialPort scannerSerialPort = null;

    

    public MainWindow()
    {
       
        Log.logDebug("MainWindow");

        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-us");

        InitializeComponent();


        Frame = this.FindControl<ContentControl>("Frame");
        grid = this.FindControl<Grid>("g1");

        if (OperatingSystem.IsLinux())
        {
            this.Topmost = true;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome;
            this.ExtendClientAreaTitleBarHeightHint = 0; 
        }
        else if (OperatingSystem.IsWindows())
        {
            if (FullScreen)
            {
                grid.Margin = new Thickness(0);
            }
        }

        versionUpdates.Add(new VersionUpdate { Version = "12072024.2", Type = "Разработка", Description = "Перенесен фунционал WPFClient на фреймворк Avalonia" });
        versionUpdates.Add(new VersionUpdate { Version = "20082024.2", Type = "Доработка", Description = "Реализован способ назначение событий вместо PreviewKeyDown" });
        versionUpdates.Add(new VersionUpdate { Version = "21082024.2", Type = "Доработка", Description = "Реализована работа кнопки ОК в ScanBOX поправили стили уведомлений" });
        versionUpdates.Add(new VersionUpdate { Version = "21082024.2", Type = "Исправление", Description = "Исправлено функция по установки фокуса на TextBox" });
        versionUpdates.Add(new VersionUpdate { Version = "01102024.2", Type = "Исправление", Description = "Исправлено функция по автопереходу" });
        versionUpdates.Add(new VersionUpdate { Version = "21112024.2", Type = "Доработка", Description = "Добавлена возможность переноса строки в датагрид с возможностью включения и отключения. Настройка TextWrapDataGrid" });
        versionUpdates.Add(new VersionUpdate { Version = "12122024.2", Type = "Доработка", Description = "Сделал подробный вывод о характере ошибки" });
        versionUpdates.Add(new VersionUpdate { Version = "21012025.1", Type = "Обновлена версия Avalonia - 11.2.3", Description = "Клиент адаптирован под новую версию" });

        _specialCharacters.Add(new _specialCharacter { Character = (char)0, Abbreviation = "NUL" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)1, Abbreviation = "SOH" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)2, Abbreviation = "STX" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)3, Abbreviation = "ETX" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)4, Abbreviation = "EOT" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)5, Abbreviation = "ENQ" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)6, Abbreviation = "ACK" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)7, Abbreviation = "BEL" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)8, Abbreviation = "BS" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)9, Abbreviation = "HT" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)10, Abbreviation = "LF" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)11, Abbreviation = "VT" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)12, Abbreviation = "FF" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)13, Abbreviation = "CR" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)14, Abbreviation = "SO" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)15, Abbreviation = "SI" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)16, Abbreviation = "DLE" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)17, Abbreviation = "DC1" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)18, Abbreviation = "DC2" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)19, Abbreviation = "DC3" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)20, Abbreviation = "DC4" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)21, Abbreviation = "NAK" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)22, Abbreviation = "SYN" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)23, Abbreviation = "ETB" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)24, Abbreviation = "CAN" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)25, Abbreviation = "EM" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)26, Abbreviation = "SUB" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)27, Abbreviation = "ESC" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)28, Abbreviation = "FS" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)29, Abbreviation = "GS" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)30, Abbreviation = "RS" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)31, Abbreviation = "US" });
        _specialCharacters.Add(new _specialCharacter { Character = (char)127, Abbreviation = "DEL" });

        autosubmitTimer.Interval = TimeSpan.FromMilliseconds(10);//new TimeSpan(0, 0, 1);
        autosubmitTimer.Tick += autosubmitTimer_Tick;

        screenTimer.Interval = TimeSpan.FromMilliseconds(10);//new TimeSpan(0, 0, 1);
        screenTimer.Tick += screenTimer_Tick;

        gridTimer.Interval = TimeSpan.FromMilliseconds(100);//new TimeSpan(0, 0, 1);
        gridTimer.Tick += gridTimer_Tick;
        gridTimer.Start();

        queueTimer.Interval = TimeSpan.FromMilliseconds(100);//new TimeSpan(0, 0, 1);
        queueTimer.Tick += queueTimer_Tick;

        queueDTimer.Interval = TimeSpan.FromMilliseconds(100);
        queueDTimer.Tick += queueDTimer_Tick;
        queueDTimer.Start();

        queueLogTimer.Interval = new TimeSpan(0, 0, 1);
        queueLogTimer.Tick += queueLogTimer_Tick;
        queueLogTimer.Start();

        ignoreKeys.Add(Key.Escape);
        ignoreKeys.Add(Key.CapsLock);
        ignoreKeys.Add(Key.F1);
        ignoreKeys.Add(Key.F2);
        ignoreKeys.Add(Key.F3);
        ignoreKeys.Add(Key.F4);
        ignoreKeys.Add(Key.F5);
        ignoreKeys.Add(Key.F6);
        ignoreKeys.Add(Key.F7);
        ignoreKeys.Add(Key.F8);
        ignoreKeys.Add(Key.F9);
        ignoreKeys.Add(Key.F10);
        ignoreKeys.Add(Key.F11);
        ignoreKeys.Add(Key.F12);
        ignoreKeys.Add(Key.Home);
        ignoreKeys.Add(Key.End);
        ignoreKeys.Add(Key.PageUp);
        ignoreKeys.Add(Key.PageDown);
        ignoreKeys.Add(Key.Left);
        ignoreKeys.Add(Key.Right);
        ignoreKeys.Add(Key.Up);
        ignoreKeys.Add(Key.Down);
        ignoreKeys.Add(Key.PrintScreen);
        ignoreKeys.Add(Key.Scroll);
        ignoreKeys.Add(Key.Pause);
        ignoreKeys.Add(Key.LWin);
        ignoreKeys.Add(Key.RWin);
        ignoreKeys.Add(Key.LeftAlt);
        ignoreKeys.Add(Key.LeftCtrl);
        ignoreKeys.Add(Key.LeftShift);
        ignoreKeys.Add(Key.RightShift);
        ignoreKeys.Add(Key.RightAlt);
        ignoreKeys.Add(Key.RightCtrl);
        ignoreKeys.Add(Key.NumLock);
        ignoreKeys.Add(Key.Space);

        m_nNumberOfTypes = 0;
        m_arScannerTypes = new short[TOTAL_SCANNER_TYPES];
        m_arSelectedTypes = new bool[TOTAL_SCANNER_TYPES];

        m_arScanners = new Scanner[MAX_NUM_DEVICES];
        for (int i = 0; i < MAX_NUM_DEVICES; i++)
        {
            Scanner scanr = new Scanner();
            m_arScanners.SetValue(scanr, i);
        }

        for (int index = 0; index < TOTAL_SCANNER_TYPES; index++)
        {
            m_arSelectedTypes[index] = false;
        }
        m_arSelectedTypes[SCANNER_TYPES_SNAPI - 1] = true;
        try
        {
            Log.logDebug("Trying pCoreScanner API");
            m_pCoreScanner = new CoreScanner.CCoreScannerClass();
        }
        catch (Exception)
        {
            Thread.Sleep(1000);
            try
            {
                m_pCoreScanner = new CCoreScannerClass();
            }
            catch (Exception)
            {
                Debug.WriteLine("SNAPI LOADING FAILED");
                SNAPI_DRIVER_LOADED = false;
            }
        }
        if (SNAPI_DRIVER_LOADED)
        {
            Debug.WriteLine("SCAN MODE - SNAPI");
            m_pCoreScanner.BarcodeEvent += new _ICoreScannerEvents_BarcodeEventEventHandler(OnBarcodeEvent);
            ScannerConnect.Connect();
            registerForEvents();
        }
     
        SNAPI_DRIVER_LOADED = false;

        Log.logDebug("Initialized");
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    private void Window_Closing(object sender, WindowClosingEventArgs e)
    {
        Debug.WriteLine("Window_Closing event");
        if (scannerSerialPort != null)
            scannerSerialPort.Close();
    }

    /// <summary>
    /// Обработка кнопок окна версий клиента
    /// </summary>
    private void AWPClientVersionButton_Click(object sender, RoutedEventArgs e)
    {
        //get actual screen dimensions (for fullscreen mode)
        double screenHeight = this.Height;
        double screenWidth = this.Width;
        double listWidth = screenWidth / 2;
        double listHeight = screenHeight - 300;
        double listBottomMargin = 100;

        var orderedQueue = versionUpdates.OrderBy(s => s.id).ToList();
        int i = 1;
        foreach (VersionUpdate vers in orderedQueue)
        {
            vers.id = i++;
        }

        var thisGrid = (Frame.Content as UserControl).Content as Grid;
        ListBox AWPClientVersionControlList = (ListBox)thisGrid.Find<ListBox>("AWPClientVersionControlList");
        if (AWPClientVersionControlList == null)
        {
            AWPClientVersionControlList = new ListBox();

            AWPClientVersionControlList.Name = "AWPClientVersionControlList";
            AWPClientVersionControlList.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            AWPClientVersionControlList.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom;
            AWPClientVersionControlList.Margin = new Thickness(0, 0, 0, listBottomMargin);
            AWPClientVersionControlList.Height = listHeight;
            AWPClientVersionControlList.Width = listWidth;
            AWPClientVersionControlList.FontFamily = new FontFamily("Couier New");
            AWPClientVersionControlList.FontSize = 16;
            AWPClientVersionControlList.FontWeight = FontWeight.Normal;
            AWPClientVersionControlList.Background = Brushes.WhiteSmoke;
            AWPClientVersionControlList.Foreground = Brushes.Black;

            var AWPClientVersionControlListGrid = new DataGrid();

            AWPClientVersionControlList.Items.Clear(); // Очистить коллекцию Items

            AWPClientVersionControlListGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "№",
                Binding = new Binding("id"),
                //Width = 50
            });
            AWPClientVersionControlListGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Версия",
                Binding = new Binding("Version"),
                //Width = 100
            }); 
            AWPClientVersionControlListGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "ТИП",
                Binding = new Binding("Type"),
                //Width = 150
            });
            AWPClientVersionControlListGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "ОПИСАНИЕ",
                Binding = new Binding("Description"),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            });

            // Заполняем DataGrid данными
            AWPClientVersionControlListGrid.ItemsSource = orderedQueue;

            // Добавляем DataGrid в ListBox
            AWPClientVersionControlList.Items.Add(AWPClientVersionControlListGrid);

            // Добавляем ListBox в родительский контейнер
            thisGrid.Children.Add(AWPClientVersionControlList);

            Button AWPClientVersionControlClose = new()
            {
                Content = "ЗАКРЫТЬ ОКНО",
                Name = "AWPClientVersionControlClose"
            };
            thisGrid.Children.Add(AWPClientVersionControlClose);
            AWPClientVersionControlClose.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            AWPClientVersionControlClose.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom;
            AWPClientVersionControlClose.HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            AWPClientVersionControlClose.VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center;
            AWPClientVersionControlClose.Height = 50;
            AWPClientVersionControlClose.Width = listWidth;
            AWPClientVersionControlClose.FontSize = 20;
            AWPClientVersionControlClose.FontWeight = FontWeight.Bold;
            AWPClientVersionControlClose.Margin = new Thickness(0, 0, 0, (screenHeight - listHeight) / 2 - listBottomMargin + AWPClientVersionControlClose.Height);
            AWPClientVersionControlClose.Background = Brushes.Gray;
            AWPClientVersionControlClose.Foreground = Brushes.White;
            AWPClientVersionControlClose.Click += AWPClientVersionControlCloseButton_Click;
        }
    }
    private void AWPClientVersionControlCloseButton_Click(object sender, RoutedEventArgs e)
    {
        var thisGrid = (Frame.Content as ContentControl).Content as Panel;

        var AWPClientVersionControlClose = thisGrid.Children.FirstOrDefault(c => c is Button && ((Button)c).Name == "AWPClientVersionControlClose") as Button;
        if (AWPClientVersionControlClose != null)
        {
            thisGrid.Children.Remove(AWPClientVersionControlClose);
        }

        var AWPClientVersionControlList = thisGrid.Children.FirstOrDefault(c => c is ListBox && ((ListBox)c).Name == "AWPClientVersionControlList") as ListBox;
        if (AWPClientVersionControlList != null)
        {
            thisGrid.Children.Remove(AWPClientVersionControlList);
        }

    }

    /// <summary>
    /// Обработка кнопок буфера сканирования
    /// </summary>
    private void queueControlButton_Click(object sender, RoutedEventArgs e)
    {
        // Get actual screen dimensions (for fullscreen mode)
        double screenHeight = this.Height;
        double screenWidth = this.Width;
        double listWidth = screenWidth / 2;
        double listHeight = screenHeight - 300;
        double listBottomMargin = 100;

        // Sort queue and reset indexes
        var orderedQueue = scanQueue.OrderBy(s => s.id).ToList();
        int i = 1;
        foreach (ScanQueueItem scan in orderedQueue)
        {
            scan.id = i++;
        }

        var thisGrid = (Frame.Content as UserControl).Content as Grid;
        ListBox queueScanList = (ListBox)thisGrid.Find<ListBox>("queueScanList");

        if (queueScanList == null)
        {
            queueScanList = new ListBox();
            queueScanList.Name = "queueScanList";
            queueScanList.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            queueScanList.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
            queueScanList.Margin = new Thickness(0, 0, 0, listBottomMargin);
            queueScanList.Height = listHeight;
            queueScanList.Width = listWidth;
            queueScanList.FontSize = 20;
            queueScanList.FontWeight = FontWeight.Bold;
            queueScanList.Background = new SolidColorBrush(Colors.OrangeRed);
            queueScanList.Foreground = Brushes.White;

            queueScanList.Items.Clear(); // Очистить коллекцию Items

            var dataGrid = new DataGrid();
            dataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "№",
                FontSize = 20,
                FontWeight = FontWeight.Bold,
                Binding = new Binding("id"),
            });
            dataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "СТАТУС",
                FontSize = 20,
                FontWeight = FontWeight.Bold,
                Binding = new Binding("Status"),
            });
            dataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "ШТРИХКОД",
                FontSize = 20,
                FontWeight = FontWeight.Bold,
                Binding = new Binding("scan"),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            });

            // Заполняем DataGrid данными
            dataGrid.ItemsSource = orderedQueue;

            // Добавляем DataGrid в ListBox
            queueScanList.Items.Add(dataGrid);

            // Добавляем ListBox в родительский контейнер
            thisGrid.Children.Add(queueScanList);

            // Добавляем кнопку "ОЧИСТИТЬ ОЧЕРЕДЬ"
            Button queueControlClear = new()
            {
                Content = "ОЧИСТИТЬ ОЧЕРЕДЬ",
                Name = "queueControlClearButton"
            };
            thisGrid.Children.Add(queueControlClear);
            queueControlClear.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            queueControlClear.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom;
            queueControlClear.HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            queueControlClear.VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center;
            queueControlClear.Height = 100;
            queueControlClear.Width = listWidth / 2;
            queueControlClear.FontSize = 25;
            queueControlClear.FontWeight = FontWeight.Bold;
            queueControlClear.Margin = new Thickness(0, 0, listWidth / 2, (screenHeight - listHeight) / 2 - listBottomMargin + queueControlClear.Height / 2);
            queueControlClear.Background = new SolidColorBrush(Colors.DarkRed);
            queueControlClear.Foreground = Brushes.White;
            queueControlClear.Click += queueControlClearButton_Click;

            // Добавляем кнопку "ЗАКРЫТЬ ОКНО"
            Button queueControlClose = new Button();
            queueControlClose.Content = "ЗАКРЫТЬ ОКНО";
            queueControlClose.Name = "queueControlCloseButton";
            thisGrid.Children.Add(queueControlClose);
            queueControlClose.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            queueControlClose.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom;
            queueControlClose.HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            queueControlClose.VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center;
            queueControlClose.Height = 100;
            queueControlClose.Width = listWidth / 2;
            queueControlClose.FontSize = 25;
            queueControlClose.FontWeight = FontWeight.Bold;
            queueControlClose.Margin = new Thickness(listWidth / 2, 0, 0, (screenHeight - listHeight) / 2 - listBottomMargin + queueControlClose.Height / 2);
            queueControlClose.Background = new SolidColorBrush(Colors.Gray);
            queueControlClose.Foreground = Brushes.White;
            queueControlClose.Click += queueControlCloseButton_Click;
        }
    }
    private void queueControlClearButton_Click(object sender, RoutedEventArgs e)
    {
        var thisGrid = (Frame.Content as ContentControl).Content as Panel;
        scanQueue.Clear();

        var queueControlButton = thisGrid.Children.FirstOrDefault(c => c is Button && ((Button)c).Name == "queueControlButton") as Button;
        if (queueControlButton != null)
        {
            thisGrid.Children.Remove(queueControlButton);
        }

        queueControlCloseButton_Click(sender, e);
    }
    private void queueControlCloseButton_Click(object sender, RoutedEventArgs e)
    {
        var thisGrid = (Frame.Content as ContentControl).Content as Panel;

        var queueControlClearButton = thisGrid.Children.FirstOrDefault(c => c is Button && ((Button)c).Name == "queueControlClearButton") as Button;
        if (queueControlClearButton != null)
        {
            thisGrid.Children.Remove(queueControlClearButton);
        }

        var queueControlCloseButton = thisGrid.Children.FirstOrDefault(c => c is Button && ((Button)c).Name == "queueControlCloseButton") as Button;
        if (queueControlCloseButton != null)
        {
            thisGrid.Children.Remove(queueControlCloseButton);
        }

        var queueScanList = thisGrid.Children.FirstOrDefault(c => c is ListBox && ((ListBox)c).Name == "queueScanList") as ListBox;
        if (queueScanList != null)
        {
            thisGrid.Children.Remove(queueScanList);
        }
    }
    private void queueDTimer_Tick(object sender, EventArgs e)
    {
        if (queueDControl != null)
        {
            queueDControl.Content = scanQueue.Count().ToString();
        }
    }
    private void gridTimer_Tick(object sender, EventArgs e)
    {
        try
        {
            if (selectRows.Count > 0)
                Task.Run(() => SelectRowAsync());
        }
        catch (Exception ex)
        {
            Log.logTimer("gridTimer_Tick ERROR: " + ex.Message + System.Environment.NewLine + ex.InnerException);
        }
    }
    internal async Task SelectRowAsync()
    {
        try
        {
            if (selectRows.Count > 0)
            {
                DataGrid grid = selectRows.Last().grid;
                DataRowView row = selectRows.Last().row;
                if (grid != null && row != null)
                {
                    try
                    {
                        Log.logDebug("Invoke selectRowAsync ");

                        // Доступ к UI потоку для обновления элементов интерфейса
                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            try
                            {
                                grid.SelectedItem = row;

                                if (grid.SelectedItem != null)
                                {
                                    var columnToScrollTo = grid.Columns.FirstOrDefault(); // Выбираем первый столбец
                                    if (columnToScrollTo != null)
                                    {
                                        grid.ScrollIntoView(grid.SelectedItem, columnToScrollTo);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Log.logTimer("selectRowAsync ERROR: " + e.Message + System.Environment.NewLine + e.InnerException);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Log.logDebug("Invoke selectRowAsync " + ex.Message);
                    }
                }
                selectRows.Clear();
            }
        }
        catch (Exception e)
        {
            Log.logTimer("selectRowAsync ERROR: " + e.Message + System.Environment.NewLine + e.InnerException);
        }
    }
    
    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        Log.logDebug("MainWindow_Loaded start");

        try
        {
            UpdateSetting.Update();
            clientVersion = ClientVersion;
            msgBoxEnabled = MsgBoxEnabled;
            logMaterialListSelection = LogMaterialListSelection;
            scanModeMsgEnabled = ScanModeMsgEnabled;
            timerLogEnabled = TimerLogEnabled;
            gridLogEnabled = GridLogEnabled;
            canSkipScreenDraw = CanSkipScreenDraw;
            clientName = ClientName;
            useComPort = UseComPort;
            string f = AutoSubmitTimeoutMS;


            if (SpecialCharacterBrackets != null && SpecialCharacterBrackets.Length > 1)
            {
                specialCharacterBrackets[0] = SpecialCharacterBrackets[0].ToString();
                specialCharacterBrackets[1] = SpecialCharacterBrackets[1].ToString();
            }
            if (f != null)
            {
                _ = int.TryParse(AutoSubmitTimeoutMS, out autoSubmitTimeoutMS);
            }
            if (!CanSkipScreenDraw)
                screenTimer.Start();
            if (autoSubmitTimeoutMS > 0)
                autosubmitTimer.Start();

            if (timerLogEnabled || gridLogEnabled)
            {
                Log.logTimer("[WPF] Version: " + clientVersion);
            }

            foreach (string prefix in ScanPrefix)
            {
                scanPrefixes.Add(prefix);
            
                if (timerLogEnabled || gridLogEnabled)
                    Log.logTimer("[WARNING] Using scan prefix [" + prefix + "]");
            }
            foreach (string replacement in MainWindowViewModel.ProcedureReplacementsSt)
            {
                var replacements = replacement.Split('|');
                if (replacements.Length == 2)
                {
                    var procReplacement = new ProcedureRepl.procedureReplacement { originalProcedure = replacements[0], replacementProcedure = replacements[1] };
                    procedureReplacements.Add(procReplacement);
                    if (timerLogEnabled || gridLogEnabled)
                        Log.logTimer("[WARNING] Procedure [" + procReplacement.originalProcedure + "] is being replaced by [" + procReplacement.replacementProcedure + "]");
                    Debug.WriteLine("Adding proc replacement: " + replacement);
                }
            }

            Debug.WriteLine("useComPort setting: " + useComPort);

            if (useComPort != null && useComPort.Length > 0)
            {
                int retries = 10;

                if (useComPort.ToUpper() == "ANY")
                {
                    while (SerialPort.GetPortNames().Any(x => x.IndexOf("COM") == 0) == false && retries > 0)
                    {
                        retries--;
                        Debug.WriteLine("Waiting for any COM port");
                        Thread.Sleep(1000);
                    }
                    if (retries <= 0)
                    {
                        string portDescription = "любому COM порту";
                        if (useComPort.ToUpper() != "ANY")
                            portDescription = "указанному порту " + useComPort.ToUpper();
                        MessageDialog.Show("Ошибка подключения к COM порту", "Не удалось подключиться к " + portDescription + "." + System.Environment.NewLine + "Проверьте что порт присутствует в системе и не занят.");
                    }
                    var comPorts = SerialPort.GetPortNames().Where(x => x.IndexOf("COM") == 0);
                    foreach (var comPort in comPorts)
                    {
                        Debug.WriteLine("Port avaliable: " + comPort);
                    }
                    scannerSerialPort = new SerialPort(comPorts.First());
                }
                else
                {
                    while (SerialPort.GetPortNames().Any(x => x.IndexOf(useComPort.ToUpper()) == 0) == false && retries > 0)
                    {
                        retries--;
                        Debug.WriteLine("Waiting for any " + useComPort.ToUpper() + " port");
                        Thread.Sleep(1000);
                    }
                    if (retries <= 0)
                    {
                        string portDescription = "любому COM порту";
                        if (useComPort.ToUpper() != "ANY")
                            portDescription = "указанному порту " + useComPort.ToUpper();
                        MessageDialog.Show("Ошибка подключения к COM порту", "Не удалось подключиться к " + portDescription + "." + System.Environment.NewLine + "Проверьте что порт присутствует в системе и не занят.");
                    }
                    scannerSerialPort = new SerialPort(useComPort.ToUpper());
                }
                if (scannerSerialPort == null)
                {
                    string portDescription = "любому COM порту";
                    if (useComPort.ToUpper() != "ANY")
                        portDescription = "указанному порту " + useComPort.ToUpper();
                    MessageDialog.Show("Ошибка подключения к COM порту", "Не удалось подключиться к " + portDescription + ". Проверьте что порт присутствует в системе и не занят.");
                }

                scannerSerialPort.BaudRate = 9600;
                scannerSerialPort.Parity = Parity.None;
                scannerSerialPort.StopBits = StopBits.One;
                scannerSerialPort.DataBits = 8;
                scannerSerialPort.Handshake = Handshake.None;

                scannerSerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                Debug.WriteLine("scannerSerialPort.IsOpen " + scannerSerialPort.IsOpen);
                scannerSerialPort.Open();
            }

            LogWorker.LogPath = LogPath;

            //использовать ли команды отправляемые на компорт до и после загрузки экрана 
            Setter.UseCommCommand = UseCommCommand;
            //показывать ли заставку "загружается" во время загрузки экрана
            Setter.IsShowWaitScreen = ShowWaitScreenByDefault;
            //Путь к экрану
            Setter.XamlPath = XamlPath;

            //Создания объектов работы с компортами
            foreach (string item in CommPort)
            {
                try
                {
                    string[] s = null;
                    s = item.Split('\t');
                    string before = string.Empty;
                    string after = string.Empty;
                    if (s != null && s.Length == 5)
                    {
                        before = s[3];
                        after = s[4];
                    }

                    if (s != null && s.Length > 2)
                    {
                        Commer com = new Commer(s[0], Convert.ToInt32(s[1]), s[2], true, before, after);
                        //todo commstring
                        com.StopSymbols = "\n";
                        com.DataCome += new Commer.CustomEventHandler(com_DataCome);
                        CommersList.Add(com);
                    }
                }
                catch (Exception ex)
                {
                    LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, string.Format("{0}: {1}", "Ошибка при инициализации com-порта", ex.Message), ex.StackTrace, new string[] { item });
                }
            }

            this.KeyDown += bckKey;
            this.AddHandler(KeyDownEvent, MainWindow_KeyDown, RoutingStrategies.Tunnel);
            //this.KeyDown += MainWindow_KeyDown;
            AttachedToLogicalTree += MainWindow_AttachedToLogicalTree;

            //Старая фигня. используется для совместимости
            UpdateSetting.UpdateOLD();

            ExecRun(StartProcedure);

            Navigate(buf);
        }
        catch (Exception ex)
        {
            this.KeyDown += bckKey;
            this.AddHandler(KeyDownEvent, MainWindow_KeyDown, RoutingStrategies.Tunnel);

            if (Frame != null)
            {
                string fil = string.Empty;
                using (StreamReader reader = new StreamReader(Path.Combine(DirectoryHelper.GetResDirectory(), "Screens") + "\\Error.axaml"))
                {
                    fil = reader.ReadToEnd().Replace("\\*BTNBOTTOMTAG*\\", "CLOSE").Replace("\\*MESSAGE*\\", ex.Message.ToString()).Replace("\\*BTNBOTTOM*\\","Выход");
                }
                Navigate(fil);
            }
            Log.logKey("MainWindows load error: " + ex.Message);  
        }

        Log.logDebug("MainWindow_Loaded end");

    }

    /// <summary>
    /// Выводит экран обработки и выполняет в фоновом потоке загрузку следуещей wpf-процедуры
    /// </summary>
    /// <param name="procName"></param>
    private void ExecRun(string procName)
    {
        if (DebugTime)
        {
            if (watch.IsRunning)
            {
                watch.Reset();
            }

            watch.Start();
        }

        if (!IsRunning)
        {
            if (IsShowWaitScreen)
            {
                Dispatcher.UIThread.Post(() => ShowWaitScreen());
            }

            IsRunning = true;

            try
            {
                Run(procName);

                Dispatcher.UIThread.InvokeAsync(worker_RunWorkerCompleted);
               
            }
            catch (Exception ex)
            {
                //MessageDialog.Show("", ex.Message);
                LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { procName });
            }
        }
    }

    /// <summary>
    /// Выполняет переход на экран
    /// </summary>
    /// <param name="procName">Имя хранимой процедуры</param>
    private void Run(string procName)
    {

        List<KeyValuePair<string, string>> result;
        //bool execres;

        //Выполнение процедуры
        string execres = Setter.BridgeWorker.ExecProc(procName, out result);

        // проверка вернулись ли правильные значения. первый тэг должен начинаться на XAML
        if (execres != string.Empty || !result.Exists(i => i.Key.StartsWith("XAML")))
        {
            //Создание экрана об ошибке
            result = ErrorRecordSet;

            if (result != null && result.Count > 6)
            {
                result[4] = new KeyValuePair<string, string>("PRM_VALUE", execres);
            }
        }

        buf = string.Empty;

        //MessageBox.Show("set queuedValue as null");
        queuedScanbox = null;
        queuedProcedure = null;
        queuedValue = null;
        screenAcceptsScan = false;


        //разбор тэгов
        foreach (KeyValuePair<string, string> item in result)
        {
            ParseTag(item);
        }

        //Навигация на полученный UserControl
        if (buf != string.Empty)
        {

            bool drawScreen = true;
            addQueueButton = false;
            //screenAcceptsScan = true;

            if (scanQueue.Any(s => s.Status == "Queued"))
            {
                if (queuedProcedure != null && queuedValue != null)
                {
                    drawScreen = false;
                    queueTimer.Start();
                }
                else
                {
                    if (!screenAcceptsScan)
                        addQueueButton = true;
                    queueTimer.Stop();
                }
            }
            else
                queueTimer.Stop();

            CurrentProcName = procName;
            if (drawScreen)
            {
                Dispatcher.UIThread.Post(() =>
                {
                   Navigate(buf);
                });
            }

        }
        else
            Debug.WriteLine("BUF IS EMPTY");

        if (screenAcceptsScan)
            scanEnable(true);
        else
            scanEnable(false);
    }
    /// <summary>
    /// Обрабатывает пару тэг-значение полученную из SQL
    /// </summary>
    public void ParseTag(KeyValuePair<string, string> tag)
    {
        try
        {
            //MessageDialog.Show("", tag.Value.ToString());
            string[] key = tag.Key.Split('.');
            if (key.Length > 0)
            {
                switch (key[0].ToUpper())
                {
                    //Страница загружается из файла (по пути)
                    case "XAML_FILE":

                        //NewScreen();
                        string tag_Screen1 = tag.Value.Replace(".xaml", ".axaml");
                        using (StreamReader reader = File.OpenText(tag_Screen1))
                        {
                            buf = reader.ReadToEnd();

                            NewScreen();
                        }
                        //todo actualscreens
                        break;

                    //Страница загружается из файла находящегося в каталоге для экранов (по имени)
                    case "XAML_NAME":

                        //NewScreen();
                        string tag_Screen = tag.Value.Replace(".xaml", ".axaml");
                        string filePath = string.Empty;

                        filePath = Path.Combine(DirectoryHelper.GetResDirectory(), "Screens", tag_Screen);

                      
                        using (StreamReader reader = new(filePath))
                        {
                            //MessageDialog.Show("", filePath);
                            buf = reader.ReadToEnd();
                            NewScreen();
                        }
                        //Сбор экранов которые используются в отдельную папку при необходимости
                        if (CollectActualScreens)
                        {
                            string actualScreenPath = Path.Combine("ActualScreens", tag_Screen);
                            try
                            {
                                if (!File.Exists(actualScreenPath))
                                {
                                    if (!Directory.Exists("ActualScreens"))
                                    {
                                        Directory.CreateDirectory("ActualScreens");
                                    }

                                    File.Copy(Path.Combine(Setter.XamlPath, tag_Screen), actualScreenPath);
                                }
                            }
                            catch
                            {
                            }
                        }
                        
                    

                       
                        break;

                    //Страница загружается из значения
                    case "XAML":

                        NewScreen();

                        string tag_Screen2 = tag.Value.Replace(".xaml", ".axaml");
                        buf = tag_Screen2;

                        break;

                    //Этот и сл тэг для замен
                    case "PRM_NAME":

                        prm_name = tag.Value;
                        break;

                    case "PRM_VALUE":
                        string rep = tag.Value.Replace("\"", "&quot;").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
                        if (prm_name.ToUpper().IndexOf("SCANBOX") > -1)
                        {
                            screenAcceptsScan = true;
                            string[] nextProcParameters = rep.Split('.');
                            if (nextProcParameters.Count() == 2)
                            {
                                queuedProcedure = nextProcParameters[0];
                                queuedValue = nextProcParameters[1];
                            }
                        }
                        buf = buf.Replace(prm_name, rep);

                        prm_name = string.Empty;
                        break;

                    case "SOUND":
                        switch (tag.Value.ToLower())
                        {
                            case "asterisk":
                                PlaySound("Sounds/asterisk.wav");
                                beep(21);//warble  //fires on error?
                                break;
                            case "beep":
                                PlaySound("Sounds/asterisk.wav");
                                beep(0);//one high short
                                break;
                            case "exclamation":
                                PlaySound("Sounds/asterisk.wav");
                                beep(5);//one low short
                                break;
                            case "hand":
                                PlaySound("Sounds/asterisk.wav");
                                beep(17);//three low long
                                break;
                            case "question":
                                PlaySound("Sounds/asterisk.wav");
                                beep(11);//two high long
                                break;
                        }

                        break;

                    case "PRM_SQL":
                        if (sql != string.Empty)
                        {
                            DataGridSqlDic.Add(prm_name, sql);
                            sql = string.Empty;
                        }
                        break;

                    case "GRIDSQL":
                    case "TEXTSQL":
                        sql = tag.Value.Trim();

                        break;

                    case "GRIDNAME":

                        if (sql != string.Empty)
                        {
                            DataGridSqlDic.Add(tag.Value, sql);
                            sql = string.Empty;
                        }
                        break;

                    case "TEXTTIMER":

                        if (sql != string.Empty)
                        {
                            string[] sss = tag.Value.Split('.');

                            if (sss != null && sss.Length == 2)
                            {
                                TextRefreshTimers.Add(new TextElementTimer(sss[0], sss[1], sql, -1));
                            }

                            if (sss != null && sss.Length == 3)
                            {
                                TextRefreshTimers.Add(new TextElementTimer(sss[0], sss[1], sql, Convert.ToInt32(sss[2])));
                            }

                            sql = string.Empty;
                        }
                        break;

                    case "GRIDPROC":

                        sql = tag.Value.TrimStart();
                        Setter.BridgeWorker.SetGridProc(sql);
                        sql = string.Empty;

                        break;
                    //добавление в словарь sql для DataGrid (DATAGRID.datagrid1 = 'select * from users')
                    case "DATAGRID":

                        if (key.Length == 2)
                        {
                            DataGridSqlDic.Add(key[1], tag.Value);
                        }
                        break;

                    case "REFRESH":

                        //todo write refresh tag
                        break;

                    //Устанавливает фокус на элемент (по имени)
                    case "SETFOCUS":

                        FocusedElementName = tag.Value;
                        //Dispatcher.BeginInvoke(new OneParamActionDelegate(SetFocus), DispatcherPriority.Input, tag.Value);
                        break;

                    //Таймер
                    case "TIMER":

                        string[] s = tag.Value.Split('.');

                        if (s != null && s.Length == 2)
                        {
                        
                            timer = new Timer(new TimerCallback(TimerAlive), s[0], (int)Convert.ToInt32(s[1]), System.Threading.Timeout.Infinite);
                        }
                        break;

                    //Отключает или включает экран "загрузка"
                    case "WAITSCREEN":

                        if (tag.Value.ToLower() == "off")
                        {
                            IsShowWaitScreen = false;
                        }
                        else if (tag.Value.ToLower() == "on")
                        {
                            IsShowWaitScreen = true;
                        }

                        break;

                    //Задает горячую клавишу и действие
                    case "HOTKEY":

                        string[] ss = tag.Value.Split('.');
                        //if (ss[1] == "ESC")
                        //{
                        //    ss[1] = ss[1].Replace("ESC", "Escape");
                        //}
                        if (ss.Length == 3)
                        {
                            if (Enum.TryParse<Key>(ss[1], out Key hotkey))
                            {

                                HotKeyList.Add(new HotKeyItem { ElementName = ss[0].TrimStart(), Action = ss[2].TrimStart(), HotKey = hotkey });
                            }
                        }

                        break;

                    //Проиграть звуковой файл
                    case "MUSIC":

                        string soundPath = "Sounds\\" + tag.Value;
                        using (var audioFile = new AudioFileReader(soundPath))
                        using (var outputDevice = new WaveOutEvent())
                        {
                            outputDevice.Init(audioFile);
                            outputDevice.Play();
                        }


                        break;

                    //Проговаривает текст
                    case "SAY":
                        SpeechSynthesizer _speechSynthesizer = new();
                        _speechSynthesizer.SelectVoice("ScanSoft Katerina_Full_22kHz");
                        _speechSynthesizer.SpeakAsync(tag.Value);
                        break;

                    //Отправка команды на компорт
                    case "COMSEND":

                        if (key.Length == 2 && tag.Value != string.Empty)
                        {
                            List<Commer> lst = CommersList.Where(i => i.GroupCode == key[1]).Take(1).ToList();
                            if (lst.Count == 1)
                            {
                                lst[0].StoredCommand.Add(tag.Value);
                                //lst[0].SendStoredCommand();
                                //lst[0].StoredCommand += tag.Value;
                            }
                        }
                        break;

                    //Задает настройки
                    case "OPTION":

                        if (key.Length == 2 && tag.Value != string.Empty)
                        {
                            switch (key[1].Trim().ToLower())
                            {
                                //Переключает стиль
                                case "setstyle":
                                    break;
                                //отключает\включает экран "загрузка"
                                case "showwaitsreen":

                                    Setter.IsShowWaitScreen = Convert.ToBoolean(tag.Value);
                                    break;
                                //использовать или нет отправку команд на компорт при и перед загрузкой экрана
                                case "usecommcommand":

                                    Setter.UseCommCommand = Convert.ToBoolean(tag.Value);
                                    break;

                                //case "ENABLE_GRID_REFRESH":

                                //    DisableAllDataGridRefresh = false;
                                //    break;
                                //case "DISABLE_GRID_REFRESH":

                                //    DisableAllDataGridRefresh = true;
                                //    break;
                                //устанавливает таймаут обновления DataGrid (устар)
                                case "gridrefreshtimeout":

                                    Setter.GridRefreshTimeout = Convert.ToInt32(tag.Value);
                                    break;

                                default:
                                    break;
                            }
                        }
                        break;

                    //Работа с экранами
                    case "SCREEN":

                        if (key.Length == 2 && tag.Value != string.Empty)
                        {
                            switch (key[1].Trim().ToLower())
                            {
                                //Загрузка экрана с сети
                                case "download":

                                    WebClient Client = new WebClient();
                                    Client.DownloadFile(tag.Value, Path.Combine(Setter.XamlPath, Path.GetFileName(tag.Value)));
                                    break;

                                default:
                                    break;
                            }
                        }
                        break;

                    //Назначает элементу какое либо свойство
                    case "SET":

                        if (key.Length == 3)
                        {
                            ReplaceList.Add(new ReplaceInfo(key[1], key[2], tag.Value));
                        }
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "имя параметра: " + tag.Key, "значение параметра: " + tag.Value });
            MessageDialog.Show("Закрытие программы", ex.Message);
        }

    }
    private void beep(int mode)
    {
        /*******************
        0 - 1 high short beep
        1 - 2 high short beeps
        2 - 3 high short beeps
        3 - 4 high short beeps
        4 - 5 high short beeps
        5 - 1 low short beep
        6 - 2 low short beeps
        7 - 3 low short beeps
        8 - 4 low short beeps
        9 - 5 low short beeps
        10 - 1 high long beep
        11 - 2 high long beeps
        12 - 3 high long beeps
        13 - 4 high long beeps
        14 - 5 high long beeps
        15 - 1 low long beep
        16 - 2 low long beeps
        17 - 3 low long beeps
        18 - 4 low long beeps
        19 - 5 low long beeps
        20 - Fast warble beep
        21 - Slow warble beep
        22 - High-low beep
        23 - Low-high beep
        24 - High-low-high beep
        25 - Low-high-low beep
        26 - High-high-low-low beep
        42 - Green LED off
        43 - Green LED on
        45 - Yellow LED
        46 - Yellow LED off
        47 - Red LED on
        48 - Red LED off
        ****************/
        if (!SNAPI_DRIVER_LOADED)
            return;
        if (mode < 0 || mode > 48)
            return;
        /*string inXml = "<inArgs>" +
                            "<scannerID> 1 </scannerID>" +
                            "<cmdArgs>" +
                                "<arg-xml>" +
                                    "<attrib_list>" +
                                        "<attribute>" +
                                            "<id>6000</id>" +
                                            "<datatype>X</datatype>" +
                                            "<value>"+mode.ToString()+"</value>" +
                                        "</attribute>" +
                                    "</attrib_list>" +
                                "</arg-xml>" +
                            "</cmdArgs>" +
                        "</inArgs>";*/
        string inXml = "<inArgs>" +
                            "<scannerID>" + connectedScannerID + "</scannerID>" +
                            "<cmdArgs>" +
                                "<arg-int>" + mode + "</arg-int>" +
                            "</cmdArgs>" +
                        "</inArgs>";
        int status = STATUS_FALSE;
        string outXml = "";
        ExecCmd(6000, ref inXml, out outXml, out status);
        Debug.WriteLine(status.ToString() + " beep " + mode.ToString());
    }

    private void NewScreen()
    {
        if (Setter.UseCommCommand)
        {
            CommOff();
        }

        StopTimers();

        ClearVariables();
    }

    /// <summary>
    /// Выводит сообщение о загрузке
    /// </summary>
    private void ShowWaitScreen()
    {
        BusyPannel bs = new BusyPannel();
        bs.IsVisible = true;
    }

    /// <summary>
    /// Прячет сообщение о загрузке
    /// </summary>
    private void HideWaitScreen()
    {
        BusyPannel bs = new BusyPannel();
        bs.IsVisible = false;
    }
    private void worker_RunWorkerCompleted()
    {
        IsRunning = false;

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            HideWaitScreen();
        });
    }

    /// <summary>
    /// Событие таймера
    /// </summary>
    public void TimerAlive(object state)
    {
        if (!IsRunning && state is string)
        {
            ExecRun(state as string);
        }
    }

    /// <summary>
    /// Загружает во фрейм AXAML страницу
    /// </summary>
    public void Navigate(string buf)
    {
        try
        {
            string filename = Guid.NewGuid().ToString();

            if (!Directory.Exists(Path.Combine(DirectoryHelper.GetResDirectory(), "temp")))
            {
                Directory.CreateDirectory(Path.Combine(DirectoryHelper.GetResDirectory(), "temp"));
            }

            if (OperatingSystem.IsLinux())
            {
                using (StreamWriter writer = File.CreateText(Path.Combine(DirectoryHelper.GetResDirectory(), "temp") + "/" + filename + ".axaml"))
                {
                    // MessageDialog.Show("",buf.ToString());
                    writer.Write(buf);
                }
            }
            if (OperatingSystem.IsWindows())
            {
                using (StreamWriter writer = File.CreateText(Path.Combine(DirectoryHelper.GetResDirectory(), "temp") + "\\" + filename + ".axaml"))
                {
                    // MessageDialog.Show("",buf.ToString());
                    writer.Write(buf);
                }
            }
         
            buf = string.Empty;

            if (OperatingSystem.IsLinux())
            {
                if (Frame != null)
                {
                    string fil = string.Empty;
                    using (StreamReader reader = new StreamReader(Path.Combine(DirectoryHelper.GetResDirectory(), "temp") + "/" + filename + ".axaml"))
                    {
                        fil = reader.ReadToEnd();
                    }


                    // Создание XAML из строки
                    var control = AvaloniaRuntimeXamlLoader.Parse<UserControl>(fil);
                    Frame.Content = control;
                }
            }
            if (OperatingSystem.IsWindows())
            {
                if (Frame != null)
                {
                    string fil = string.Empty;
                    using (StreamReader reader = new StreamReader(Path.Combine(DirectoryHelper.GetResDirectory(), "temp") + "\\" + filename + ".axaml"))
                    {
                        fil = reader.ReadToEnd();
                    }

                    var control = AvaloniaRuntimeXamlLoader.Parse<UserControl>(fil);
                    Frame.Content = control;
                }
            }
           
            if (Setter.GridRefreshTimeout == 0)
            {
                FillDataGrids();
            }
            else
            {
                RefreshTimer = new Timer(new TimerCallback(delegate (object state)
                {
                    FillDataGrids();
                }), null, new TimeSpan(0, 0, 0), new TimeSpan(0, 0, Setter.GridRefreshTimeout));
            }
            if (DebugTime && watch.IsRunning)
            {
                watch.Stop();
                watch.Reset();
            }


            MakeEvents();

            SetFocus(FocusedElementName);

            if (timerLogEnabled)
                Log.logTimer("Frame events added");

            MakeReplaces();

            //старые временные удаляем фоном
            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
            {
                string[] ss = Directory.GetFiles(Path.Combine(DirectoryHelper.GetResDirectory(), "temp"));
                foreach (string item in ss)
                {
                    if (Path.GetFileNameWithoutExtension(item) != filename)
                    {
                        File.Delete(item);
                    }
                }
            });

            worker.RunWorkerAsync();
            if (MainWindowViewModel.timerLogEnabled)
                Log.logTimer("Screen [" + MainWindowViewModel.CurrentXAML + "] async load");

            drawQueueIcon();

            if (_CurrentProcName != null && _CurrentProcName.IndexOf("wpf_Ctrl_Main") > -1)
            {
                VersionIcon();
            }
        }
        catch (Exception ex)
        {
            LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "buf: " + buf });

        }
    }

    /// <summary>
    /// Получаем и заполняем список данными  датагрид
    /// </summary>
    private void FillDataGrids()
    {

        if (timerLogEnabled)
            Log.logTimer("Data grids fill started");
       
        if (IsRunning || LastGridActivity > DateTime.Now.AddSeconds(-1))
        {
         
            if (gridLogEnabled)
                Log.logTimer("Data grid mainproc IsRunning " + IsRunning + "; return;");

            if (RefreshTimer != null)
                RefreshTimer.Change(TimeSpan.FromMilliseconds(100), new TimeSpan(0, 0, Setter.GridRefreshTimeout));
            return;
        }

        try
        {
            List<DataGrid> datagrid = Helper.FindControls<DataGrid>(this);

            foreach (var grid in datagrid)
            {
              
                if (grid != null)
                {
                    string value = string.Empty;

                    if (grid.Name != null)
                    {
                      
                        DataGridSqlDic.TryGetValue(grid.Name, out value);
                        if (value != null && value != string.Empty)
                        {
                            DateTime gridStartTime = DateTime.Now;
                            DataTable dt = null;
                            if (timerLogEnabled)
                                Log.logTimer("Getting grid dataset");
                            var procReplacement = procedureReplacements.Find(p => p.originalProcedure.ToLower() == value.ToLower());
                            if (procReplacement != null)
                            {
                                value = procReplacement.replacementProcedure;
                            }

                            if (timerLogEnabled || gridLogEnabled)
                            {
                                Log.logTimer("GRID Name: " + grid.Name + System.Environment.NewLine + "GRID SQL: " + value);
                            }

                            CommonMethods.LoadDataGridFromDatabase(grid, value);
                          
                            DateTime gridEndTime = DateTime.Now;
                            double gridLoadTime = (gridEndTime - gridStartTime).TotalMilliseconds;
                            if (timerLogEnabled || gridLogEnabled)
                            {
                                if (gridLoadTime > 5000)
                                    Log.logTimer("WARNING! Dataset returned in " + (gridLoadTime / 1000d).ToString("0.00") + " seconds");
                                else
                                    Log.logTimer("Dataset returned in " + (gridLoadTime / 1000d).ToString("0.00") + " seconds");
                            }
                            else
                            {
                                LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, "Нет данных для DataGrid", string.Empty, new string[] { "Имя DataGrid: " + grid.Name, "SQL: " + value });
                            }
                        }

                    }

                }

            }
        }
        catch (Exception ex)
        {
            //todo all grid params
            LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, null);
        }
        if (timerLogEnabled)
            Log.logTimer("Grid loaded");
    }

    /// <summary>
    /// Устанавливает фокус указанному элементу
    /// </summary>
    private void SetFocus(string ElementName)
    {
        try
        {
            Control fe = Helper.FindControlByName(ElementName, this);
            if (fe != null)
            {
                fe.Focus();
                if (fe is ScanBox)
                {
                    ScanBox scanControl = Helper.FindControl<ScanBox>(this);
                    TextBox scanData = scanControl.ScanData;
                   
                    Task.Delay(50).ContinueWith(t =>
                    {
                        // Установка индекса каретки в конец текста
                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            scanData.Focus();
                            scanData.CaretIndex = scanData.Text.Length;
                            
                        });
                    });
                    queuedScanbox = fe;
                    
                    if (scanQueue.Where(s => s.Status == "Queued").Any())
                    {
                        ScanQueueItem firstScan = scanQueue.Where(s => s.Status == "Queued").OrderBy(s => s.id).ToList().First();
                        Debug.WriteLine("Scanbox autorunning SQL proc " + queuedProcedure + " with " + queuedValue + "=" + firstScan.scan);
                        scanQueue.Remove(firstScan);
                        Setter.BridgeWorker.SetVariable(queuedValue, firstScan.scan);
                        ExecRun(queuedProcedure);
                    }
                }
                if (fe is TextBox)
                {
                    Task.Delay(50).ContinueWith(t =>
                    {
                        // Установка индекса каретки в конец текста
                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            (fe as TextBox).Focus();
                            (fe as TextBox).CaretIndex = (fe as TextBox).Text.Length;
                            (fe as TextBox).SelectAll();

                        });
                    });
                   
                }
                if (fe is ExListView)
                {
                    (fe as ExListView).Focus();
                }
                if (fe is DataGrid)
                {
                    DataGrid myDataGrid = (fe as DataGrid);
                    Task.Delay(50).ContinueWith(t =>
                    {
                      
                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            myDataGrid.Focus();
                        });
                    });

                }
               
            }


        }
        catch (Exception ex)
        {
            LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "имя элемента: " + ElementName });
        }
    }

    /// <summary>
    /// Событие нажатие кнопки на форме MainWindow
    /// </summary>
    private void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
      
        if (HotKeyList != null && HotKeyList.Count > 0)
        {
              HotKeyItem item = HotKeyList.FirstOrDefault(i => i.HotKey == e.Key);
          
            if (item == null)
            {
                try
                {
                    item = HotKeyList.FirstOrDefault(i => i.HotKey == e.Key); 
                }
                catch (Exception)
                { }
            }
            if (item != null)
            {
                
                e.Handled = true;
                Log.logDebug("MainWindow_KeyDown handled");
                Control fe = Helper.FindControlByName(item.ElementName, this);
                if (fe is Button button)
                {
                    
                    fe.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
        }
    }

    /// <summary>
    /// Осуществляет работу события кнопки
    /// </summary>
    private void Btn_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as Button).Tag != null && ((sender as Button).Tag is string) && ((sender as Button).Tag as string).Trim() != string.Empty)
        {
            if (e != null)
            {
                e.Handled = true;
            }
            string[] prms = ((sender as Button).Tag as string).Trim().Split('.');
            if (prms.Length == 2 || prms.Length == 1)//todo think
            {
                string procName = prms[0].Trim();
                if (procName != string.Empty)
                {
                    switch (procName.ToLower())
                    {
                        case "close":
                            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime) lifetime.Shutdown();
                            break;

                        case "DISABLE_GRID_REFRESH":

                            if (RefreshTimer != null)
                            {
                                RefreshTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                            }
                            break;

                        case "ENABLE_GRID_REFRESH":

                            if (RefreshTimer != null)
                            {
                                RefreshTimer.Change(new TimeSpan(0, 0, 0), new TimeSpan(0, 0, Setter.GridRefreshTimeout));
                            }
                            break;


                        default:
                            Setter.BridgeWorker.SetVariable("NextMode", prms.Length == 2 ? prms[1] : null);

                            //SetVariablesByTextBoxes(procName);
                            //SetVariablesByComboBoxes(procName);
                            //SetVariablesByGrids(procName);

                            ExecRun(procName);
                           //Navigate(buf);
                            break;
                    }
                }

            }
        }
    }

    /// <summary>
    /// Осуществляет работу подтверждения ввода в SCANBOX
    /// </summary>
    private void bckKey(object sender, KeyEventArgs e)
    {
       
        Log.logDebug("bckKey");
        
        if (e == null)
            return;
     
        if (screenAcceptsScan && scanQueue.Any(s => s.Status == "Queued"))
        {
            Log.logKey(System.Environment.NewLine + "PREVENTING INPUT UNTIL QUEUE IS EMPTY" + System.Environment.NewLine);
            e.Handled = true;
        }

        if (ignoreKeys.Contains(e.Key))
        {
            Log.logKey(System.Environment.NewLine + "RECEIVED KEY [" + e.Key.ToString() + "] IS MARKED FOR IGNORE" + System.Environment.NewLine);
            return;
        }
        char keyChar = '0';

        if (OperatingSystem.IsLinux())
        {
           
            keyChar = GetCharFromKey.FromKeyL(e);
            Log.logDebug("keyChar " + keyChar.ToString());
        }
        if (OperatingSystem.IsWindows())
        {
            keyChar = GetCharFromKey.FromKey(e.Key);
            Log.logDebug("keyChar " + keyChar.ToString());
        }

        if (keyChar == '\0')
            return;
        ScanQueueItem scan = scanQueue.Find(s => s.Status == "Composing");
        int lastId = 0;
        if (scanQueue.Count() > 0)
            lastId = scanQueue.OrderBy(s => s.id).ToList().Last().id;
        if (scanPrefixes.Contains(keyChar.ToString()))
        {

            e.Handled = true;
            if (scan != null)
                scanQueue.Find(s => s == scan).Status = "Malformed";
            scan = new ScanQueueItem { id = lastId + 1, scan = "", Status = "Composing" };
            scanQueue.Add(scan);

            Log.logKey(System.Environment.NewLine + "FOUND PREFIX, NEW SCAN " + System.Environment.NewLine);
            e.Handled = true;

        }
        else
        {
            if (scan != null)
            {
                if (e.Key == Key.Enter)
                {
                    scanQueue.Find(s => s == scan).Status = "Queued";
                    Debug.WriteLine("queued [" + scanQueue.Find(s => s == scan).scan + "]");
                    queueTimer.Stop();
                    queueTimer.Start();
                    Log.logKey(System.Environment.NewLine + "QUEUED " + scanQueue.Find(s => s == scan).scan + System.Environment.NewLine);
                    e.Handled = true;

                }
                else
                {

                    Log.logKey(System.Environment.NewLine + "ADDING CHAR [" + keyChar + "]");
                    scanQueue.Find(s => s == scan).scan += keyChar;

                    Log.logKey(System.Environment.NewLine + "COMPOSING " + scanQueue.Find(s => s == scan).scan + System.Environment.NewLine);
                    e.Handled = true;
                }
            }
            else
            {
                //isScanning = false;
                Log.logKey(System.Environment.NewLine + "NO SCAN NO PREFIX" + System.Environment.NewLine);
            }
        }

    }
    /// <summary>
    //Создание кнопки буфера сканирования
    /// </summary>
    private void drawQueueIcon()
    {
        var thisDGrid = (Frame.Content as UserControl).Content as Grid;
        queueDControl = new Button();
        queueDControl.Content = "0";
        queueDControl.Name = "queueDemoButton";
        thisDGrid.Children.Add(queueDControl);
        queueDControl.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right;
        queueDControl.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom;
        queueDControl.HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center;
        queueDControl.VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center;
        queueDControl.Height = 40;
        queueDControl.Width = 40;
        queueDControl.FontSize = 20;
        queueDControl.FontWeight = FontWeight.Bold;
        queueDControl.Margin = new Thickness(0, 0, 0, 55);
        queueDControl.Background = new SolidColorBrush(Colors.Blue) { Opacity = 0.5 };
        queueDControl.Foreground = Brushes.White;
        queueDControl.Click += queueControlButton_Click;
        queueDControl.BringIntoView();
    }

    /// <summary>
    //Создание кнопки Контроля версий
    /// </summary>
    private void VersionIcon()
    {
        var thisDGrid = (Frame.Content as UserControl).Content as Grid;

        AWPClientVersion = new Button();
        AWPClientVersion.Content = "Версия программы: " + clientName + "-" + clientVersion;
        AWPClientVersion.Name = "wpfClientVersion";
        thisDGrid.Children.Add(AWPClientVersion);

        AWPClientVersion.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right;
        AWPClientVersion.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        AWPClientVersion.HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center;
        AWPClientVersion.VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center;
        AWPClientVersion.Height = 40;

        AWPClientVersion.FontSize = 16;
        AWPClientVersion.FontWeight = FontWeight.Bold;
        AWPClientVersion.Margin = new Thickness(0, 40, 0, 0);
        AWPClientVersion.Background = new SolidColorBrush(Colors.Yellow) { Opacity = 0.5 };
        AWPClientVersion.Foreground = Brushes.Black;
        AWPClientVersion.Click += AWPClientVersionButton_Click;
        AWPClientVersion.BringIntoView();
    }

    /// <summary>
    //Инициализация выхода по кноаке ESC
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    /// <summary>
    /// Обработка нажатия клавиши в ExListView
    /// </summary>
    private void ExListView_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            e.Handled = true;
            try
            {
                ExListView lv = (sender as ExListView);
                if (lv.Tag != null && (lv.Tag is string) && (lv.Tag as string).Trim() != string.Empty)
                {
                    string[] str = (lv.Tag as string).Split('.');
                    if (str != null && str.Length == 2)
                    {
                        if (lv.MainView.Items.Count > 0)
                        {
                            string val = string.Empty;
                            if (lv.MainView.SelectionMode == SelectionMode.Single)
                            {
                                XmlWriterSettings settings = new XmlWriterSettings();
                                settings.CloseOutput = true;
                                settings.Indent = true;
                                settings.OmitXmlDeclaration = true;
                                using (TextWriter textWriter = new StringWriter())
                                {
                                    using (XmlWriter writer = XmlWriter.Create(textWriter))
                                    {
                                        writer.WriteStartDocument();
                                        writer.WriteStartElement("Data");
                                        foreach (DataRowView item in lv.MainView.SelectedItems)
                                        {
                                            writer.WriteElementString("Item", item.Row[0].ToString());
                                        }
                                        writer.WriteEndElement();
                                        writer.WriteEndDocument();
                                    }
                                    val = textWriter.ToString();
                                }
                            }
                            else
                            {
                                DataRowView row = lv.MainView.SelectedItem as DataRowView;

                                val = row[0].ToString();
                            }

                            Setter.BridgeWorker.SetVariable(str[1], val);
                        }

                        string procName = str[0];// (grid.Tag as string).Substring(0, (grid.Tag as string).IndexOf('.'));

                        if (procName != string.Empty)
                        {
                            SetVariablesByTextBoxes(procName);
                            SetVariablesByComboBoxes(procName);
                            ExecRun(procName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "key: " + e.Key.ToString() });
            }
        }
    }
    private void MainWindow_TextInput(object sender, TextInputEventArgs e)
    {
        if (CheckActiveScanBox(e.Text))
        {
            e.Handled = true;
        }
    }
    private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        string tmpScanData = removeTrailingSpaceOrNewLine(sp.ReadExisting());
        Debug.WriteLine("Serial port data: [" + tmpScanData + "]");

        Task.Run(() => Dispatcher.UIThread.InvokeAsync(() => SetScanText(tmpScanData)));

        int lastId = 0;
        if (scanQueue.Count > 0)
        {
            lastId = scanQueue.OrderBy(s => s.id).Last().id;
        }

        scanQueue.Add(new ScanQueueItem { scan = tmpScanData, Status = "Queued", id = lastId + 1 });

        Log.logKey("COM PORT RECEIVED: " + tmpScanData + System.Environment.NewLine);

        queueTimer_Tick(null, null);
    }
   
    void SetScanText(string text)
    {
        if (ScanBoxesDic.Count > 0)
        {
            List<ScanBox> boxes = ScanBoxesDic.Select(i => i.Key).ToList();
            if (boxes != null)
            {
                var sb = boxes.Where(i => i.Name == "SCANBOX");
                if (sb != null)
                {
                    var scanbox = sb.FirstOrDefault();
                    if (scanbox != null)
                    {
                        Dispatcher.UIThread.InvokeAsync(() => scanbox.ScanText = text);
                    }
                }
            }
        }
    }
    string removeTrailingSpaceOrNewLine(string barcode)
    {
        return barcode.Replace(System.Environment.NewLine, "").Replace(" ", "").Replace("\r", "").Replace("\n", "");
    }
    private void ExecCmd(int opCode, ref string inXml, out string outXml, out int status)
    {
        outXml = "";
        status = STATUS_FALSE;
        if (m_bSuccessOpen)
        {
            try
            {
                m_pCoreScanner.ExecCommand(opCode, ref inXml, out outXml, out status);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(status.ToString(), " EXEC_COMMAND");
                MessageDialog.Show("...", ex.Message.ToString());
            }
        }
    }
    private void com_DataCome(object sender, Commer.CustomEventArgs e)
    {
        try
        {
            if (CommDebug)
            {
                LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, "Comport>>" + e.Message, string.Empty, new string[] { (sender as Commer).PortName });
            }

            if (ScanBoxesDic != null && ScanBoxesDic.Count > 0)
            {
                //установка фокуса ввода в активный сканбокс
                try
                {
                    Log.logDebug("Invoke com_DataCome");
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        CheckActiveScanBox((sender as Commer).GroupCode);
                    });
                }
                catch (Exception ex)
                {
                    Log.logDebug("Invoke com_DataCome " + ex.Message);
                }

                List<KeyValuePair<ScanBox, string>> list = ScanBoxesDic.Where(i => i.Key.IsVisible == true).ToList();
                //todo возможно нет необходимости находить сканбокс
                if (list != null && list.Count > 0)
                {
                    list[0].Key.ScanCode = e.Message;
                    try
                    {
                        Log.logDebug("Invoke ScanBoxDataEnter");
                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            ScanBoxDataEnter(list[0].Key);
                        });
                    }
                    catch (Exception ex)
                    {
                        Log.logDebug("Invoke ScanBoxDataEnter " + ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, null);
        }
    }
    private string GetRegUnregIDs(out int nEvents)
    {
        string strIDs = "";
        nEvents = NUM_SCANNER_EVENTS;
        strIDs = SUBSCRIBE_BARCODE.ToString();
        strIDs += "," + SUBSCRIBE_IMAGE.ToString();
        strIDs += "," + SUBSCRIBE_VIDEO.ToString();
        strIDs += "," + SUBSCRIBE_RMD.ToString();
        strIDs += "," + SUBSCRIBE_PNP.ToString();
        strIDs += "," + SUBSCRIBE_OTHER.ToString();
        return strIDs;
    }
    private void registerForEvents()
    {
        if (Connection.ScannerConnect.IsMotoConnected())
        {
            int nEvents = 0;
            string strEvtIDs = GetRegUnregIDs(out nEvents);
            string inXml = "<inArgs>" +
                                "<cmdArgs>" +
                                "<arg-int>" + nEvents + "</arg-int>" +
                                "<arg-int>" + strEvtIDs + "</arg-int>" +
                                "</cmdArgs>" +
                                "</inArgs>";

            int opCode = REGISTER_FOR_EVENTS;
            string outXml = "";
            int status = STATUS_FALSE;
            ExecCmd(opCode, ref inXml, out outXml, out status);
            Debug.WriteLine(status.ToString() + " REGISTER_FOR_EVENTS");
        }
    }
    private void scanEnable(bool on)
    {
        if (!SNAPI_DRIVER_LOADED)
            return;
        string inXml = "<inArgs>" +
                        "<scannerID>" + connectedScannerID.ToString() + "</scannerID>" +
                        "</inArgs>";
        int opCode = 2014;
        if (!on)
            opCode = 2013;
        int status = STATUS_FALSE;
        string outXml = "";
        ExecCmd(opCode, ref inXml, out outXml, out status);
        Debug.WriteLine(status.ToString() + " scanEnable " + on.ToString());
    }
    private void queueLogTimer_Tick(object sender, EventArgs e)
    {
        Log.logWrite = true;
    }
    private void autosubmitTimer_Tick(object sender, EventArgs e)
    {
        try
        {
            var activeScan = scanQueue.Find(s => s.Status == "Composing");
            if (activeScan != null)
            {
                //logKey(activeScan.UpdTime + " < " + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) + " + " + autoSubmitTimeoutMS);
                if (activeScan.UpdTime < (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - autoSubmitTimeoutMS)
                {
                    if (activeScan.scan.Length > 1 && activeScan.isManual == false /*&& (queuedScanbox as ScanBox).ScanCode.Length==0*/)
                    {
                        activeScan.Status = "Queued";
                        Log.logKey("AUTOSUBMIT QUEUING SCAN BY TIMEOUT");
                        queueTimer_Tick(null, null);
                    }
                    else
                    {
                        //scanQueue.Remove(activeScan);
                        activeScan.isManual = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.logTimer("screenTimer_Tick ERROR: " + ex.Message + System.Environment.NewLine + ex.InnerException);
        }
    }
    private void screenTimer_Tick(object sender, EventArgs e)
    {
        try
        {
            if (drawScreens.Count > 0)
                Task.Run(() => loadScreenAsync());
        }
        catch (Exception ex)
        {
            Log.logTimer("screenTimer_Tick ERROR: " + ex.Message + System.Environment.NewLine + ex.InnerException);
        }
    } 
    internal async Task loadScreenAsync()
    {
        try
        {
            if (drawScreens.Where(s => s.id == screenID).Any() && drawScreens.Last().id == screenID)
            {
                if (timerLogEnabled)
                    Log.logTimer("QUEUED SCREEN DRAW ID: " + drawScreens.Last().id.ToString());
                try
                {
                    Log.logDebug("Invoke loadScreenAsync ");
                  //  Dispatcher.UIThread.InvokeAsync(() => Navigate(drawScreens.Last().buf));
                }
                catch (Exception ex)
                {
                    Log.logDebug("Invoke loadScreenAsync " + ex.Message);
                }
                drawScreens.Clear();
            }
            else
            {
                if (timerLogEnabled)
                    Log.    logTimer("LAST SCREEN ID DOESNT MATCH THE ACTUAL ID. RESETTING QUEUE");
                drawScreens.Clear();
            }
        }
        catch (Exception e)
        {
            Log.logTimer("selectRowAsync ERROR: " + e.Message + System.Environment.NewLine + e.InnerException);
        }
    }
    private void queueTimer_Tick(object sender, EventArgs e)
    {
        if (screenAcceptsScan && scanQueue.Where(s => s.Status == "Queued" && s.isManual == false).Any())
        {
            queueTimer.Stop();
            //Debug.WriteLine("executing proc by timer");
            if (queuedProcedure != null && queuedValue != null)
            {
                ScanQueueItem firstScan = scanQueue.Where(s => s.Status == "Queued").OrderBy(s => s.id).ToList().First();
                firstScan.scan = MainWindowViewModel.parseSpecialCharacters(firstScan.scan);
                if (queuedScanbox != null)
                    (queuedScanbox as ScanBox).ScanCode = firstScan.scan;
                
                Log.logKey(System.Environment.NewLine + "Running TIMER SQL proc " + queuedProcedure + " with " + queuedValue + "=" + firstScan.scan + System.Environment.NewLine);
                scanQueue.Remove(firstScan);
                Setter.BridgeWorker.SetVariable(queuedValue, firstScan.scan);
                
                Run(queuedProcedure);
            }
        }
        else
        {
            queueTimer.Stop();
        }
    }
    private bool m_bSuccessOpen = false;
    private int connectedScannerID = -1;
    private string ShowBarcodeLabel(string strXml)
    {
        Debug.WriteLine("Initial XML" + strXml);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(strXml);

        string strData = string.Empty;
        string barcode = xmlDoc.DocumentElement.GetElementsByTagName("datalabel").Item(0).InnerText;
        string symbology = xmlDoc.DocumentElement.GetElementsByTagName("datatype").Item(0).InnerText;
        string[] numbers = barcode.Split(' ');

        foreach (string number in numbers)
        {
            if (String.IsNullOrEmpty(number))
            {
                break;
            }

            strData += ((char)Convert.ToInt32(number, 16)).ToString();
        }

        return strData;
    }
    void OnBarcodeEvent(short eventType, ref string scanData)
    {
        try
        {
            string tmpScanData = ShowBarcodeLabel(scanData);

            // Обновление пользовательского интерфейса в основном потоке через Dispatcher
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                SetScanText(tmpScanData);
            });

            int lastId = 0;
            if (scanQueue.Count > 0)
                lastId = scanQueue.OrderByDescending(s => s.id).First().id;

            scanQueue.Add(new ScanQueueItem { scan = tmpScanData, Status = "Queued", id = lastId + 1 });
            Log.logKey("SNAPI RECEIVED: " + scanData + System.Environment.NewLine);

            queueTimer.Start();
        }
        catch (Exception e)
        {
            // Обработка исключения
        }
    }
    private bool CheckActiveScanBox(string s)
    {
        bool b = false;
        try
        {
            if (ScanBoxesDic.Count != 0)
            {
                List<KeyValuePair<ScanBox, string>> res = ScanBoxesDic.Where(i => i.Value == s)
                                                                     .OrderBy(i => i.Key.TabIndex)
                                                                     .OrderByDescending(i => i.Key._IsActive)
                                                                     .ToList();
                if (res != null && res.Count != 0)
                {
                    b = true;
                    if (!res[0].Key._IsActive)
                    {
                        MakeScanboxActive(res[0].Key);
                    }
                    else
                    {
                        KeyValuePair<ScanBox, string> scan = res.FirstOrDefault(i => i.Key.TabIndex > res[0].Key.TabIndex);
                        if (scan.Key != null)
                        {
                            MakeScanboxActive(scan.Key);
                        }
                        else
                        {
                            MakeScanboxActive(res[0].Key);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Обработка исключения
            LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, null);
        }
        return b;
    }
    // Установка активным одного из ScanBox группы
    private void MakeScanboxActive(ScanBox control)
    {
        try
        {
            List<ScanBox> list = ScanBoxesDic.Keys.Where(i => i.GroupCode == control.GroupCode).ToList();

            if (list != null && list.Count > 0)
            {
                foreach (ScanBox item in list)
                {
                    item._IsActive = false;
                }

                control.ChangeLanguage("en-US");
                control._IsActive = true;
                control.Focus(); 
            }
        }
        catch (Exception ex)
        {
            LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "ScanBox name: " + control.Name });
        }
    }

    /// <summary>
    /// Обработка нажатия клавиши в TextBox
    /// </summary>
    public void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
       if (sender is TextBox tb) 
       {
            
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                try
                {

                    if (tb.Tag != null && tb.Tag is string)
                    {
                        string tag = (tb.Tag as string);
                        string[] prms = tag.Split('.');

                        if (prms.Length > 1)
                        {
                            tb.Text = tb.Text.Trim();
                            string procName = prms[0];
                            string varname = prms[1];

                            Setter.BridgeWorker.SetVariable(varname, tb.Text);

                            //SetVariablesByTextBoxes(procName);
                            if (procName.Trim() != string.Empty)
                            {
                                ExecRun(procName);
                                Navigate(buf);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "TextBox name: " + (sender as TextBox).Name });
                }
            }
        }
        
    }

    /// <summary>
    /// Обработка нажатия клавиши в DatePicker
    /// </summary>
    public void DatePicker_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            e.Handled = true;
            try
            {
                DatePicker dp = (sender as DatePicker);
                if (dp.Tag != null && dp.Tag is string)
                {
                    string tag = (dp.Tag as string);
                    string[] prms = tag.Split('.');
                    if (prms.Length == 2)
                    {
                        dp.SelectedDate = new DateTimeOffset(new DateTime(1950, 1, 1));

                        string procName = prms[0];
                        string varname = prms[1];

                        DateTime dt;
                        Setter.BridgeWorker.SetVariable(varname, DateTime.TryParse(dp.SelectedDate?.DateTime.ToString(), out dt) ? dt.ToString("yyyy-MM-dd") : null);
                        
                        if (procName.Trim() != string.Empty)
                        {
                            ExecRun(procName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "DatePicker name: " + (sender as Avalonia.Controls.DatePicker).Name });
            }
        }
    }
    private void DataGrid_AutoGeneratingColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        DataGrid grid = (sender as DataGrid);

        if (e.Column.Header.ToString().ToUpper() == "@@CHECKBOX" && grid.SelectionMode == DataGridSelectionMode.Extended)
        {
            CheckBox headerCheckBox = new CheckBox { Content = " ", Margin = new Thickness(7, 7, 0, 0) };

            headerCheckBox.Click += ColumnHeaderCheckBox_Click;

            var newCol = new DataGridCheckBoxColumn() { Binding = new Binding(e.PropertyName) };

            e.Column = newCol;
            e.Column.Header = headerCheckBox;
            e.Column.IsReadOnly = false;
            e.Column.CanUserSort = false;

        }
    }
    private void ColumnHeaderCheckBox_Click(object sender, RoutedEventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        DataGrid grid = Helper.FindVisualChilds<DataGrid>((ILogical)e.Source);

        if (cb.IsChecked == true)
        {
            grid.SelectAll();
        }

        e.Handled = true;
    }

    /// <summary>
    /// Обработка нажатия клавиши в DataGrid
    /// </summary>
    private void DataGrid_KeyDown(object sender, KeyEventArgs e)
    {
        //MessageDialog.Show("", "DataGrid_KeyDown");
        LastGridActivity = DateTime.Now;

        try
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                DataGrid grid = (sender as DataGrid);
                if (grid.Tag != null && (grid.Tag is string) && (grid.Tag as string).Trim() != string.Empty)
                {
                    string[] str = (grid.Tag as string).Split('.');
                    if (str != null && str.Length >= 2)
                    {
                        SetVariableFromGrid(grid, str[1]);

                        //Если задан переход на следующую процедуру, сбор данных с экрана к ней относящихся и вызов её
                        string procName = str[0];

                        if (procName != string.Empty)
                        {
                            if (logEnabled)
                                Log.logTimer("DataGrid_KeyDown exec " + procName);
                            ExecRun(procName);
                        }
                    }
                }
            }
            else if (e.Key == Key.Space)
            {
                DataGrid grid = (sender as DataGrid);
                if (grid.Tag != null && (grid.Tag is string) && (grid.Tag as string).Trim() != string.Empty)
                {
                    string[] str = (grid.Tag as string).Split('.');
                    if (str != null && str.Length == 4)
                    {
                        e.Handled = true;

                        if (grid.SelectedItems.Count > 0)
                        {
                            string val = string.Empty;
                            if (grid.SelectedItem != null)
                            {
                                val = (grid.SelectedItem as DataRowView).Row[0].ToString();
                            }

                            Setter.BridgeWorker.SetVariable(str[1], val);
                        }
                        //Если задан переход на следующую процедуру, сбор данных с экрана к ней относящихся и вызов её
                        string procName = str[0];

                        if (procName != string.Empty)
                        {
                            if (str[3].Length > 2)
                                Setter.BridgeWorker.SetVariable("NextMode", str[3]);

                            if (logEnabled)
                                Log.logTimer("DataGrid_KeyDown exec " + procName);
                            ExecRun(procName);
                        }
                    }
                }
            }
            else if (e.Key == Key.Down || e.Key == Key.Up)//Work with Focus
            {
                DataGrid grid = (sender as DataGrid);

                grid.ScrollIntoView(grid.SelectedItem,null);
                grid.Focus();
                //DataGridRow dgrow = (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
                //if (dgrow != null)
                //{
                //  //  dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                //}
            }
            else if (e.Key == Key.Back)
            {
                e.Handled = true; // Помечаем событие как обработанное, чтобы предотвратить дальнейшую обработку BackSpace
                return; // Прерываем выполнение кода
            }
        }
        catch (Exception ex)
        {
            LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "key: " + e.Key.ToString() });
        }
    }
    private void SetVariableFromGrid(DataGrid grid, string target)
    {
      
        if (grid.SelectedItems.Count > 0)
        {
            string val = string.Empty;

            //Если включен расширенный режим (выбор нескольких записей), отправка результата выбора в XML
            if (grid.SelectionMode == DataGridSelectionMode.Extended)
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.CloseOutput = true;
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                using (TextWriter textWriter = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(textWriter))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("Data");
                        foreach (DataRowView item in grid.SelectedItems)
                        {
                            writer.WriteElementString("Item", item.Row[0].ToString());
                        }
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                    val = textWriter.ToString();
                }
            }
            else
            {
                if (grid.SelectedItem != null)
                {
                    val = (grid.SelectedItem as DataRowView).Row[0].ToString();
                }
            }

            if (timerLogEnabled)
                Log.logTimer("SetVariableFromGrid target" + target + ", val" + val);

            Setter.BridgeWorker.SetVariable(target, val);

            if (timerLogEnabled)
                Log.logTimer("exec wpf_SetVariable @ID = '" + Setter.ID + "', @VariableName = '" + target + "', @VariableValue = '" + val + "'");

            if (timerLogEnabled)
                Log.logTimer("SetVariableFromGrid end");
        }
    }
    private void ScanBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
               
                e.Handled = true;
                ScanBoxDataEnter(sender as ScanBox);
               
                break;
        }
    }
    public void ScanBoxDataEnter(ScanBox scanbox)
    {
        try
        {
            if (scanbox == null)
            {
                scanbox = this.FindControl<ScanBox>("SCANBOX");
            }
            if (scanbox.Tag != null && (scanbox.Tag is string))
            {
                CommOff();

                string tag = (scanbox.Tag as string).Trim();

                scanbox.ScanCode = scanbox.ScanCode.Trim();
                string procName = string.Empty;

                string[] prms = tag.Split('.');
           
                if (prms.Length == 2)
                {
                    if (prms[0] != string.Empty)
                    {
                        procName = prms[0];
                       
                        //Отправляет все переменные с экрана связанные с вызовом процедуры
                        SetVariablesByScanBoxes(scanbox.GroupCode);
                        SetVariablesByTextBoxes(procName);
                        SetVariablesByComboBoxes(procName);
                        ExecRun(procName);

                    }
                    else
                    {
                        MoveFocusToNext(scanbox);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "scanbox name: " + scanbox.Name });
        }
    }
    private void MoveFocusToNext(Control currentControl)
    {
        var nextControl = KeyboardNavigationHandler.GetNext(currentControl, NavigationDirection.Next);
        if (nextControl != null)
        {
            nextControl.Focus();
        }
    }
    private void SetVariablesByComboBoxes(string procName)
    {
        foreach (ExCombo item in ComboList)
        {
            if (item.Tag != null && (item.Tag is string) && (item.Tag as string) != string.Empty)
            {
                string[] str = (item.Tag as string).Split('.');
                if (str.Length == 2)
                {
                    try
                    {
                        //todo check
                        if (item.combo.SelectedIndex != -1 && item.combo.Items[item.combo.SelectedIndex] is ComboBoxItem && (item.combo.Items[item.combo.SelectedIndex] as ComboBoxItem).Tag != null)
                        {
                            string key = (item.combo.Items[item.combo.SelectedIndex] as ComboBoxItem).Tag.ToString();
                            Setter.BridgeWorker.SetVariable(str[1], key);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "procName: " + procName });
                    }
                }
            }
        }

    }
    private void SetVariablesByScanBoxes(string groupcode)
    {
        try
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            List<ScanBox> list = ScanBoxesDic.Where(i => i.Key.GroupCode == groupcode).Select(i => i.Key).ToList();

            foreach (ScanBox item in list)
            {
                string tag = (item.Tag as string);
                string[] parse = tag.Split('.');
                if (parse.Length > 1)
                {
                    if (parse[1] != string.Empty)
                    {
                        try
                        {
                            if (item.ScanCode.Length > 0 && scanPrefixes.Contains(item.ScanCode[..1]))
                                item.ScanCode = item.ScanCode[1..];
                            if (item.ScanCode != string.Empty)
                                dic.Add(parse[1], item.ScanCode);
                        }
                        catch (Exception ex)
                        {
                            LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "groupcode: " + groupcode });
                        }
                    }
                }

            }
            foreach (var it in dic)
            {

                if (timerLogEnabled)
                    Log.logTimer("SetVariable " + it.Key + ", " + it.Value);
                Setter.BridgeWorker.SetVariable(it.Key, it.Value);
               
                if (timerLogEnabled)
                    Log.logTimer("exec wpf_SetVariable @ID = '" + Setter.ID + "', @VariableName = '" + it.Key + "', @VariableValue = '" + it.Value + "'");
            }

        }
        catch (Exception ex)
        {
            LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "groupcode: " + groupcode });
        }
    }
    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        DataGrid grid = sender as DataGrid;

        LastGridActivity = DateTime.Now;

        var checkBoxColumn = grid.Columns.FirstOrDefault(t => t is DataGridCheckBoxColumn);

        if (e != null && checkBoxColumn != null)
        {

            foreach (var item in e.AddedItems)
            {
                if (item is Control control)
                {
                    CheckBox cb = control.FindDescendantOfType<CheckBox>();
                    if (cb != null)
                    {
                        cb.IsChecked = true;
                    }
                }
            }

            foreach (var item in e.RemovedItems)
            {
                if (item is Control control)
                {
                    CheckBox cb = control.FindDescendantOfType<CheckBox>();
                    if (cb != null)
                    {
                        cb.IsChecked = false;
                    }
                }
            }

            CheckBox headCheckBox = checkBoxColumn.Header as CheckBox;
            if (headCheckBox != null)
                headCheckBox.IsChecked = (grid.SelectedItems.Count == grid.SelectedItems.Count);
        }

        if (grid.Tag != null && grid.Tag is string tag && !string.IsNullOrWhiteSpace(tag))
        {
            string[] str = tag.Split('.');
            if (str != null && str.Length >= 3)
            {
                SetVariableFromGrid(grid, str[1]);

                if (str[2].Length > 2)
                    Setter.BridgeWorker.SQLQuery(string.Format("exec {0} @ID = '{1}'", str[2], Setter.ID));

                foreach (TextElementTimer item in TextRefreshTimers)
                {
                    var elem = Helper.FindFrameworkElement(item.ElementName);
                    Dispatcher.UIThread.Post(() => FillTextElement(item));
                }
            }
        }
    }
    private void ScanBox_PreviewTextInput(object sender, TextInputEventArgs e)
    {
        if (e.Text == "\r")
        {
            e.Handled = true;
            ScanBoxDataEnter(sender as ScanBox);
        }
    }
    private void DataGrid_MouseDown(object sender, PointerPressedEventArgs e)
    {
        LastGridActivity = DateTime.Now;
    }

    /// <summary>
    /// Обработка выделения в DataGrid у которых возможен множественный выбор
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DataGrid_PreviewMouseDownHandler(object sender, PointerPressedEventArgs e)
    {
        if ((sender as DataGrid).SelectionMode != DataGridSelectionMode.Extended)
            return;

        if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
        {
            LastGridActivity = DateTime.Now;

            DataGrid grid = sender as DataGrid;

            if (grid.Columns.Any(t => t is DataGridCheckBoxColumn))
            {
                var row = Helper.FindControl<DataGridRow>((Control)e.Source);

                if (row != null)
                {
                    var cb = Helper.FindVisualChild<CheckBox>;

                    if (cb == null)
                        grid.SelectedItems.Clear();

                    row.IsEnabled = !row.IsEnabled;
                    e.Handled = true;
                }
            }
            else
            {
                var row = Helper.FindControl<DataGridRow>((Control)e.Source);

                if (row != null)
                {
                    row.IsEnabled = !row.IsEnabled;
                    e.Handled = true;
                }
            }
        }
    }

    /// <summary>
    /// Инициализация строки автофильтра при создании грида (переделано под авалонию так определение itemgener отсутствует)
    /// </summary>
    private void MainWindow_AttachedToLogicalTree(object sender, LogicalTreeAttachmentEventArgs e)
    {
        if (e.Source is DataGrid dataGrid)
        {
            if (sender is Control item) // Предполагается, что sender - это Control или его потомок
            {
                List<Control> lst = Helper.FindControls<Control>(this);
                foreach (Control elem in lst)
                {
                    DataGrid grid = elem as DataGrid;

                    if (grid == item)
                    {
                        foreach (var child in grid.GetLogicalChildren())
                        {
                            if (child is TextBox tb && tb.Name.ToUpper() == "FILTERTEXTBOX" && tb.Tag == null)
                            {
                                if (tb.DataContext != null && tb.DataContext is string && (string)tb.DataContext != "" && ((string)tb.DataContext).IndexOf("@@") != 0)
                                {
                                    tb.KeyUp += tbFilterTextBox_KeyUp;
                                }
                                else
                                {
                                    tb.IsVisible = false;
                                }
                                tb.Tag = "PROCESSED";
                            }
                        }

                        UpdateDataGridFirstSelection(grid);

                        Dispatcher.UIThread.Post(() =>
                        {
                            GridUpdateComboBoxSelection(grid);
                        }, DispatcherPriority.Input);

                        // Уберите эту строку, если не нужно отписываться от события
                        // grid.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged
                    }
                }
            }
        }
    }

    private void ScanBox_MouseDoubleClick(object sender, PointerPressedEventArgs e)
    {
        e.Handled = true;
        MakeScanboxActive(sender as ScanBox);
    }

    /// <summary>
    /// Осуществляет замены свойств
    /// </summary>
    private void MakeReplaces()
    {
        foreach (ReplaceInfo info in ReplaceList)
        {
            Control element = Helper.FindControlByName(info.ElementName, this); 
            if (element != null)
            {
                element.GetType().GetProperty(info.ElementProperty).SetValue(element, info.ElementValue, null);
            }
        }
    }

    private void MakeEventItem(Control childVisual)
    {
        if (childVisual is Button)
        {
            (childVisual as Button).Click += Btn_Click;

            try
            {
                // Проверяем, есть ли у кнопки установленное свойство Tag
                if ((childVisual as Button).Tag is string tagString)
                {
                    string[] tag = tagString.Split('.');

                    if (tag.Length == 2)
                    {
                        // Преобразуем строковое представление ключа клавиши в объект Key
                        if (Enum.TryParse<Key>(tag[1], out Key hotkey))
                        {
                            // Проверяем, нет ли уже такой горячей клавиши в списке
                            if (HotKeyList.All(zz => zz.HotKey != hotkey))
                            {
                                // Если у кнопки не задано имя, генерируем его
                                if (string.IsNullOrEmpty((childVisual as Button).Name))
                                {
                                    (childVisual as Button).Name = "b" + Guid.NewGuid().ToString().Replace("-", "");
                                }

                                // Добавляем новую горячую клавишу в список
                                HotKeyList.Add(new HotKeyItem { ElementName = (childVisual as Button).Name, Action = "click", HotKey = hotkey });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, null);
            }
        }
        else if (childVisual is DataGrid)
        {

            (childVisual as DataGrid).AutoGeneratingColumn += DataGrid_AutoGeneratingColumns;
            (childVisual as DataGrid).AddHandler(KeyDownEvent, DataGrid_KeyDown, RoutingStrategies.Tunnel);
            (childVisual as DataGrid).PointerPressed += DataGrid_MouseDown;
            (childVisual as DataGrid).PointerPressed += DataGrid_PreviewMouseDownHandler;
            (childVisual as DataGrid).SelectionChanged += DataGrid_SelectionChanged;


        }
        else if (childVisual is ExListView)
        {
            (childVisual as ExListView).AddHandler(KeyDownEvent, ExListView_KeyDown, RoutingStrategies.Tunnel);
        }
        else if (childVisual is TextBox)
        {
            (childVisual as TextBox).AddHandler(KeyDownEvent, TextBox_KeyDown, RoutingStrategies.Tunnel);
            //(childVisual as TextBox).KeyDown += TextBox_KeyDown;
        }
        else if (childVisual is DatePicker)
        {
            (childVisual as DatePicker).AddHandler(KeyDownEvent, DatePicker_KeyDown, RoutingStrategies.Tunnel);
        }
        else if (childVisual is ExDataGrid)
        {
            (childVisual as ExDataGrid).MainDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumns;
            (childVisual as ExDataGrid).MainDataGrid.AddHandler(KeyDownEvent, DataGrid_KeyDown, RoutingStrategies.Tunnel);
        }
        else if (childVisual is ScanBox)
        {
            ScanBoxesDic.Add((childVisual as ScanBox), (childVisual as ScanBox).GroupCode);
            (childVisual as ScanBox).PointerPressed += ScanBox_MouseDoubleClick;
            (childVisual as ScanBox).AddHandler(KeyDownEvent, ScanBox_PreviewKeyDown, RoutingStrategies.Tunnel);
            (childVisual as ScanBox).AddHandler(TextInputEvent, ScanBox_PreviewTextInput, RoutingStrategies.Tunnel);
            (childVisual as ScanBox).ОК.Click += Ok_Button_Click;


        }
        else if (childVisual is Grid || childVisual is Border)
        {
            if (childVisual.Tag != null && (childVisual.Tag is string))
            {
                string tag = (childVisual.Tag as string).Trim().ToLower();
                switch (tag)
                {
                    case "visible":
                        childVisual.IsVisible = true;
                        break;
                    case "collapsed":
                        childVisual.IsVisible = false;
                        break;
                    case "hidden":
                        childVisual.IsVisible = false;
                        break;
                    default:
                        childVisual.IsVisible = true;
                        break;
                }
            }
        }
        else if (childVisual is ExCombo)
        {
            ComboList.Add(childVisual as ExCombo);
        }
        else if (childVisual is TabControl)
        {
            foreach (TabItem tabItem in ((TabControl)childVisual).Items)
            {
                if (tabItem.Content != null && tabItem.Content is Control)
                {
                    MakeEventItem((Control)tabItem.Content);
                }
            }
        }

        ElementList.Add(childVisual);
    }

    private void Ok_Button_Click(object sender, RoutedEventArgs e)
    {
        var scanBox = Helper.FindControlByName("SCANBOX", this);
        ScanBoxDataEnter(scanBox as ScanBox);
    }

    /// <summary>
    /// Вешает события на контролы по дереву начиная с указанного элемента
    /// </summary>
    /// <param name="myVisual"></param>
    private void MakeEvents()
    {
        List<Button> buttons = Helper.FindControls<Button>(this);

        foreach (var button in buttons)
        {
            MakeEventItem(button);
        }

        List<TextBox> textBoxs = Helper.FindControls<TextBox>(this);

        foreach (var textbox in textBoxs)
        {
            if (textbox.Name != "ScanData")
            {
                //textbox.KeyDown += TextBox_KeyDown;
                textbox.AddHandler(KeyDownEvent, TextBox_KeyDown, RoutingStrategies.Tunnel);
                ElementList.Add(textbox);
            }
            //if (textbox.Name =="QUANTITY")
            //{
            //    textbox.AddHandler(KeyDownEvent, TextBox_KeyDown, RoutingStrategies.Tunnel);
            //    ElementList.Add(textbox);
            //}
        }

        List<DatePicker> datePickers = Helper.FindControls<DatePicker>(this);

        foreach (var datePicker in datePickers)
        {
            MakeEventItem(datePicker); 
        }

        List<DataGrid> dataGrids = Helper.FindControls<DataGrid>(this);

        foreach (var dataGrid in dataGrids)
        {
            MakeEventItem(dataGrid);

        }

        List<ScanBox> scanBoxs = Helper.FindControls<ScanBox>(this);

        foreach (var scanBox in scanBoxs)
        {
            ScanBoxesDic.Add(scanBox, scanBox.GroupCode);
            scanBox.PointerPressed += ScanBox_MouseDoubleClick;
            scanBox.AddHandler(KeyDownEvent, ScanBox_PreviewKeyDown, RoutingStrategies.Tunnel);
            scanBox.AddHandler(TextInputEvent, ScanBox_PreviewTextInput, RoutingStrategies.Tunnel);
            scanBox.ОК.Click += Ok_Button_Click;
            ElementList.Add(scanBox);
        }
        List<Grid> grids = Helper.FindControls<Grid>(this);

        foreach (var grid in grids)
        {
            MakeEventItem(grid);
        }

        List<Border> borders = Helper.FindControls<Border>(this);

        foreach (var border in borders)
        {
            MakeEventItem(border);
        }

        List<ExDataGrid> exDataGrids = Helper.FindControls<ExDataGrid>(this);

        foreach (var exDataGrid in exDataGrids)
        {
            MakeEventItem(exDataGrid);
        }

        List<ExListView> exListViews = Helper.FindControls<ExListView>(this);

        foreach (var exListView in exListViews)
        {
            MakeEventItem(exListView);
        }

        List<ExCombo> exCombos = Helper.FindControls<ExCombo>(this);

        foreach (var exCombo in exCombos)
        {
            MakeEventItem(exCombo);
        }

        List<TabControl> tabControls = Helper.FindControls<TabControl>(this);

        foreach (var tabControl in tabControls)
        {
            foreach (TabItem tabItem in tabControl.Items)
            {
                if (tabItem.Content != null && tabItem.Content is Control)
                {
                    MakeEventItem((Control)tabItem.Content);
                }
            }
        }
    }

    private void SetVariablesByGrids(string procName)
    {
        List<DataGrid> lst = Helper.FindControls<DataGrid>(this);
        foreach (var elem in lst)
        {
            DataGrid grid = elem;
            if (grid.Tag != null && (grid.Tag is string) && (grid.Tag as string).Trim() != string.Empty)
            {
                string[] str = (grid.Tag as string).Split('.');
                if (str != null && str.Length >= 2)
                {
                    SetVariableFromGrid(grid, str[1]);
                }
            }
        }
    }

    /// <summary>
    /// Вызывает wpf_SetVariable для всех TextBox в тэге которых указано искомое имя процедуры.
    /// В настоящее время нигде не используется.
    /// </summary>
    /// <param name="ProcName">Имя хранимой процедуры</param>
    /// <remarks>
    /// Находит все компоненты у которых в tag указано имя искомой функции и вызывает wpf_SetVariable для каждого параметра
    /// </remarks>
    private void SetVariablesByTextBoxes(string ProcName)
    {
        try
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            List<TextBox> list = Helper.FindControls<TextBox>(this);
            foreach (var item in list)
            {
                string tag = ((item as TextBox).Tag as string);

 
                if ((tag != null) && (item is TextBox) && tag.StartsWith(ProcName))
                {
                    string[] str = ((item as TextBox).Tag as string).Split('.');

                    if (
                        str.Length == 2 || !(str.Length > 2 && !item.IsKeyboardFocusWithin))
                        dic.Add(str[1], (item as TextBox).Text as string);
                }
            }

            List<DatePicker> list2 = Helper.FindControls<DatePicker>(this);
            foreach (var item in list2)
            {
                string tag = (item.Tag as string);
                if ((tag != null) && (item is not null) && tag.StartsWith(ProcName))
                {
                    if (item.SelectedDate.HasValue)
                    {
                        DateTimeOffset selectedDate = item.SelectedDate.Value;
                        string tbText = selectedDate.ToString("yyyy-MM-dd"); // Format the date as needed
                        DateTime dt;
                        dic.Add(tag[(tag.LastIndexOf('.') + 1)..], DateTime.TryParse(tbText, out dt) ? dt.ToString("yyyy-MM-dd") : null);                                                           // Use the formattedDate as needed
                    }
                   
                   
                }
            }
            foreach (KeyValuePair<string, string> item in dic)
            {
               
                Setter.BridgeWorker.SetVariable(item.Key, item.Value);
               
            }
          
        }
        catch (Exception ex)
        {
            LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "имя процедуры: " + ProcName });
        }
    }
    private void tbFilterTextBox_KeyUp(object sender, KeyEventArgs e)
    {
        string filterText = "";
        LastGridActivity = DateTime.Now;

        try
        {
            TextBox textBox = (TextBox)sender;
            DataGrid grid = (DataGrid)textBox.FindLogicalAncestorOfType<DataGrid>();

            // Получаем DataContext у DataGrid, который должен содержать данные источника
            var dataContext = grid?.DataContext;

            if (dataContext is DataView dv)
            {
                foreach (var child in grid.GetLogicalChildren())
                {
                    if (child is TextBox tb && tb != null && tb.Name == "filterTextBox" && tb.DataContext is string && (string)tb.DataContext != "" && tb.Text.Trim() != "")
                    {
                        DataColumn col = dv.Table.Columns[(string)tb.DataContext];

                        if (col != null)
                        {
                            if (!string.IsNullOrEmpty(filterText))
                                filterText += " AND ";

                            if (col.DataType == typeof(string))
                                filterText += '[' + (string)tb.DataContext + "] like '%" + tb.Text.Trim() + "%'";
                            else
                                filterText += "convert([" + (string)tb.DataContext + "],'System.String') like '%" + tb.Text.Trim() + "%'";
                        }
                    }
                }

                dv.RowFilter = filterText;

                Dispatcher.UIThread.Post(() =>
                {
                    GridUpdateComboBoxSelection(grid);
                }, DispatcherPriority.Input);
            }
        }
        catch (Exception ex)
        {
            LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "key: " + e.Key.ToString() });
        }

        e.Handled = true;
    }
    private void UpdateDataGridFirstSelection(DataGrid grid)
    {
        if (grid.Columns.Count > 0)
        {
            int columnIndex = -1;
            foreach (var col in grid.Columns)
            {
                if (col.Header.ToString().ToUpper() == "@@SELECTED")
                {
                    columnIndex = grid.Columns.IndexOf(col);
                    break;
                }
            }

            if (grid.SelectedItems.Count > 0)
            {
                if (grid.SelectionMode == DataGridSelectionMode.Extended)
                {
                    if (FocusedElementName == grid.Name)
                    {
                        grid.Focus();
                    }

                    if (columnIndex > -1)
                    {
                        grid.SelectedItems.Clear();
                        foreach (var item in grid.SelectedItems)
                        {
                            var dataRow = item as DataRowView;
                            if (dataRow != null && dataRow[columnIndex].ToString() == "1")
                            {
                                grid.SelectedItems.Add(dataRow);
                            }
                        }

                        if (grid.SelectedItems.Count > 0)
                        {
                            var columnToScrollTo = grid.Columns.FirstOrDefault(); // Выбираем первый столбец
                            if (columnToScrollTo != null)
                            {
                                grid.ScrollIntoView(grid.SelectedItems[0], columnToScrollTo);
                            }
                          
                        }
                    }
                }
                else
                {
                    if (FocusedElementName == grid.Name)
                    {
                        grid.Focus();
                        grid.SelectedItem = grid.SelectedItems[0];
                    }

                    if (columnIndex > -1)
                    {
                        foreach (var item in grid.SelectedItems)
                        {
                            var dataRow = item as DataRowView;
                            if (dataRow != null && dataRow[columnIndex].ToString() == "1")
                            {
                                grid.SelectedItem = dataRow;
                                break;
                            }
                        }

                        if (grid.SelectedItem != null)
                        {
                            var columnToScrollTo = grid.Columns.FirstOrDefault(); // Выбираем первый столбец
                            if (columnToScrollTo != null)
                            {
                                grid.ScrollIntoView(grid.SelectedItem, columnToScrollTo);
                            }
                        }
                    }
                }
            }
        }
    }
}
