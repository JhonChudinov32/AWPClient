using System.Collections.Generic;
using System;
using AWPClient.Classes;
using Avalonia.Controls;
using AWPClient.LogServices;
using System.Runtime.InteropServices;
using System.Diagnostics;
using CoreScanner;
using static AWPClient.Classes.ProcedureRepl;
using Avalonia.Threading;
using System.Linq;
using NAudio.Wave;
using Key = Avalonia.Input.Key;
using System.IO;
using System.Net;
using ContentControl = Avalonia.Controls.ContentControl;
using AWPClient.Msgbox;
using AWPClient.Views;
using System.Reflection;
using AWPClient.Controls;
using System.Speech.Synthesis;
using System.Threading;
using Avalonia.Media;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AWPClient.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    ///В класс включены методы по скрытию и возобвлению видимости панели задач на Цштвщцы
    /// <summary>
    public static class TaskbarHelper
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hwnd, int command);

        public static void HideTaskbar()
        {
            IntPtr taskbarHandle = FindWindow("Shell_TrayWnd", "");
            _ = ShowWindow(taskbarHandle, 0); // 0 - SW_HIDE
        }

        public static void ShowTaskbar()
        {
            IntPtr taskbarHandle = FindWindow("Shell_TrayWnd", "");
            _ = ShowWindow(taskbarHandle, 5); // 5 - SW_SHOW
        }
    }

    /// <summary>
    /// Обрабатывает пару тэг-значение полученную из SQL
    /// </summary>
    //public static void ParseTag(KeyValuePair<string, string> tag)
    //{
    //    try
    //    {
    //        //MessageDialog.Show("", tag.Value.ToString());
    //        string[] key = tag.Key.Split('.');
    //        if (key.Length > 0)
    //        {
    //            switch (key[0].ToUpper())
    //            {
    //                //Страница загружается из файла (по пути)
    //                case "XAML_FILE":

    //                    //NewScreen();
    //                    using (StreamReader reader = File.OpenText(tag.Value))
    //                    {
    //                        buf = reader.ReadToEnd();
                         
    //                        NewScreen();
    //                    }
    //                    //todo actualscreens
    //                    break;

    //                //Страница загружается из файла находящегося в каталоге для экранов (по имени)
    //                case "XAML_NAME":

    //                    //NewScreen();
    //                    int pos;
    //                    string dir = string.Empty;
    //                    string res = string.Empty;
    //                    string resdir = string.Empty;
    //                    string filePath = string.Empty;

    //                    if (OperatingSystem.IsLinux())
    //                    {
    //                        pos = AppContext.BaseDirectory.LastIndexOf("/"); // позиция последнего слеша
    //                        dir = AppContext.BaseDirectory[..pos];
    //                        res = Path.GetDirectoryName(dir);

    //                        filePath = Path.Combine(res, "Screens/" + tag.Value.Replace(".xaml", ".axaml"));
    //                    }
    //                    if (OperatingSystem.IsWindows())
    //                    {
    //                        pos = AppContext.BaseDirectory.LastIndexOf("\\"); // позиция последнего слеша
    //                        dir = AppContext.BaseDirectory.Remove(pos, AppContext.BaseDirectory.Length - pos);
    //                        res = Path.GetDirectoryName(dir);

    //                        filePath = Path.Combine(res, "Screens\\" + tag.Value.Replace(".xaml", ".axaml"));
                            
    //                    }

    //                    using (StreamReader reader = new(filePath))
    //                    {
    //                        //MessageDialog.Show("", filePath);
    //                        buf = reader.ReadToEnd();
    //                        NewScreen();
    //                    }
    //                    //Сбор экранов которые используются в отдельную папку при необходимости
    //                    if (Settings.Default.CollectActualScreens)
    //                    {
    //                        string actualScreenPath = Path.Combine("ActualScreens", tag.Value);
    //                        try
    //                        {
    //                            if (!File.Exists(actualScreenPath))
    //                            {
    //                                if (!Directory.Exists("ActualScreens"))
    //                                {
    //                                    Directory.CreateDirectory("ActualScreens");
    //                                }

    //                                File.Copy(Path.Combine(Setter.XamlPath, tag.Value), actualScreenPath);
    //                            }
    //                        }
    //                        catch { }
    //                    }
    //                    break;

    //                //Страница загружается из значения
    //                case "XAML":

    //                    NewScreen();
    //                    buf = tag.Value;
                        
    //                    break;

    //                //Этот и сл тэг для замен
    //                case "PRM_NAME":

    //                    prm_name = tag.Value;
    //                    break;

    //                case "PRM_VALUE":
    //                    string rep = tag.Value.Replace("\"", "&quot;").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    //                    if (prm_name.ToUpper().IndexOf("SCANBOX") > -1)
    //                    {
    //                        screenAcceptsScan = true;
    //                        string[] nextProcParameters = rep.Split('.');
    //                        if (nextProcParameters.Count() == 2)
    //                        {
    //                            queuedProcedure = nextProcParameters[0];
    //                            queuedValue = nextProcParameters[1];
    //                            //MessageBox.Show("set queuedValue as " + queuedValue);
    //                        }
    //                    }
    //                    buf = buf.Replace(prm_name, rep);
                     
    //                    prm_name = string.Empty;
    //                    break;

    //                case "SOUND":
    //                    switch (tag.Value.ToLower())
    //                    {
    //                        case "asterisk":
    //                            PlaySound("Sounds/asterisk.wav");
    //                            beep(21);//warble  //fires on error?
    //                            break;
    //                        case "beep":
    //                            PlaySound("Sounds/asterisk.wav");
    //                            beep(0);//one high short
    //                            break;
    //                        case "exclamation":
    //                            PlaySound("Sounds/asterisk.wav");
    //                            beep(5);//one low short
    //                            break;
    //                        case "hand":
    //                            PlaySound("Sounds/asterisk.wav");
    //                            beep(17);//three low long
    //                            break;
    //                        case "question":
    //                            PlaySound("Sounds/asterisk.wav");
    //                            beep(11);//two high long
    //                            break;
    //                    }

    //                    break;

    //                case "PRM_SQL":
    //                    if (sql != string.Empty)
    //                    {
    //                        DataGridSqlDic.Add(prm_name, sql);
    //                        sql = string.Empty;
    //                    }
    //                    break;

    //                case "GRIDSQL":
    //                case "TEXTSQL":
    //                    sql = tag.Value.Trim();
                     
    //                    break;

    //                case "GRIDNAME":

    //                    if (sql != string.Empty)
    //                    {
    //                        DataGridSqlDic.Add(tag.Value, sql);
    //                        sql = string.Empty;
    //                    }
    //                    break;

    //                case "TEXTTIMER":

    //                    if (sql != string.Empty)
    //                    {
    //                        string[] sss = tag.Value.Split('.');

    //                        if (sss != null && sss.Length == 2)
    //                        {
    //                            TextRefreshTimers.Add(new TextElementTimer(sss[0], sss[1], sql, -1));
    //                        }

    //                        if (sss != null && sss.Length == 3)
    //                        {
    //                            TextRefreshTimers.Add(new TextElementTimer(sss[0], sss[1], sql, Convert.ToInt32(sss[2])));
    //                        }

    //                        sql = string.Empty;
    //                    }
    //                    break;

    //                case "GRIDPROC":

    //                    sql = tag.Value.TrimStart();
    //                    Setter.BridgeWorker.SetGridProc(sql);
    //                    sql = string.Empty;

    //                    break;
    //                //добавление в словарь sql для DataGrid (DATAGRID.datagrid1 = 'select * from users')
    //                case "DATAGRID":

    //                    if (key.Length == 2)
    //                    {
    //                        DataGridSqlDic.Add(key[1], tag.Value);
    //                    }
    //                    break;

    //                case "REFRESH":

    //                    //todo write refresh tag
    //                    break;

    //                //Устанавливает фокус на элемент (по имени)
    //                case "SETFOCUS":

    //                    FocusedElementName = tag.Value;
    //                    //Dispatcher.BeginInvoke(new OneParamActionDelegate(SetFocus), DispatcherPriority.Input, tag.Value);
    //                    break;

    //                //Таймер
    //                case "TIMER":

    //                    string[] s = tag.Value.Split('.');

    //                    if (s != null && s.Length == 2)
    //                    {
    //                        MainWindow m = new MainWindow();
    //                        timer = new Timer(new TimerCallback(m.TimerAlive), s[0], (int)Convert.ToInt32(s[1]), Timeout.Infinite);
    //                    }
    //                    break;

    //                //Отключает или включает экран "загрузка"
    //                case "WAITSCREEN":

    //                    if (tag.Value.ToLower() == "off")
    //                    {
    //                        IsShowWaitScreen = false;
    //                    }
    //                    else if (tag.Value.ToLower() == "on")
    //                    {
    //                        IsShowWaitScreen = true;
    //                    }

    //                    break;

    //                //Задает горячую клавишу и действие
    //                case "HOTKEY":

    //                    string[] ss = tag.Value.Split('.');

    //                    if (ss.Length == 3)
    //                    {
    //                        if (Enum.TryParse<Key>(ss[1], out Key hotkey))
    //                        {
    //                            HotKeyList.Add(new HotKeyItem { ElementName = ss[0].TrimStart(), Action = ss[2].TrimStart(), HotKey = hotkey });
    //                        }
    //                    }

    //                    break;

    //                //Проиграть звуковой файл
    //                case "MUSIC":

    //                    string soundPath = "Sounds\\" + tag.Value;
    //                    using (var audioFile = new AudioFileReader(soundPath))
    //                    using (var outputDevice = new WaveOutEvent())
    //                    {
    //                        outputDevice.Init(audioFile);
    //                        outputDevice.Play();
    //                    }
                      

    //                    break;

    //                //Проговаривает текст
    //                case "SAY":
    //                    SpeechSynthesizer _speechSynthesizer = new();
    //                    _speechSynthesizer.SelectVoice("ScanSoft Katerina_Full_22kHz");
    //                    _speechSynthesizer.SpeakAsync(tag.Value);
    //                    break;

    //                //Отправка команды на компорт
    //                case "COMSEND":

    //                    if (key.Length == 2 && tag.Value != string.Empty)
    //                    {
    //                        List<Commer> lst = CommersList.Where(i => i.GroupCode == key[1]).Take(1).ToList();
    //                        if (lst.Count == 1)
    //                        {
    //                            lst[0].StoredCommand.Add(tag.Value);
    //                            //lst[0].SendStoredCommand();
    //                            //lst[0].StoredCommand += tag.Value;
    //                        }
    //                    }
    //                    break;

    //                //Задает настройки
    //                case "OPTION":

    //                    if (key.Length == 2 && tag.Value != string.Empty)
    //                    {
    //                        switch (key[1].Trim().ToLower())
    //                        {
    //                            //Переключает стиль
    //                            case "setstyle":
    //                                break;
    //                            //отключает\включает экран "загрузка"
    //                            case "showwaitsreen":

    //                                Setter.IsShowWaitScreen = Convert.ToBoolean(tag.Value);
    //                                break;
    //                            //использовать или нет отправку команд на компорт при и перед загрузкой экрана
    //                            case "usecommcommand":

    //                                Setter.UseCommCommand = Convert.ToBoolean(tag.Value);
    //                                break;

    //                            //case "ENABLE_GRID_REFRESH":

    //                            //    DisableAllDataGridRefresh = false;
    //                            //    break;
    //                            //case "DISABLE_GRID_REFRESH":

    //                            //    DisableAllDataGridRefresh = true;
    //                            //    break;
    //                            //устанавливает таймаут обновления DataGrid (устар)
    //                            case "gridrefreshtimeout":

    //                                Setter.GridRefreshTimeout = Convert.ToInt32(tag.Value);
    //                                break;

    //                            default:
    //                                break;
    //                        }
    //                    }
    //                    break;

    //                //Работа с экранами
    //                case "SCREEN":

    //                    if (key.Length == 2 && tag.Value != string.Empty)
    //                    {
    //                        switch (key[1].Trim().ToLower())
    //                        {
    //                            //Загрузка экрана с сети
    //                            case "download":

    //                                WebClient Client = new WebClient();
    //                                Client.DownloadFile(tag.Value, Path.Combine(Setter.XamlPath, Path.GetFileName(tag.Value)));
    //                                break;

    //                            default:
    //                                break;
    //                        }
    //                    }
    //                    break;

    //                //Назначает элементу какое либо свойство
    //                case "SET":

    //                    if (key.Length == 3)
    //                    {
    //                        ReplaceList.Add(new ReplaceInfo(key[1], key[2], tag.Value));
    //                    }
    //                    break;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "имя параметра: " + tag.Key, "значение параметра: " + tag.Value });
    //    }

    //}

    public static Dictionary<string, string> processedValues = new();

    /// <summary>
    ///Парсим специальные характеристики
    /// <summary>
    public static string parseSpecialCharacters(string barcode)
    {
       
        if (specialCharacterBrackets[0] == null)
            return barcode;
        else
        {
            foreach (var specialCharacter in _specialCharacters)
            {
                string searchString = specialCharacterBrackets[0] + specialCharacter.Abbreviation + specialCharacterBrackets[1];
                
                if (specialCharacter.Abbreviation != "CR")
                    barcode = barcode.Replace(searchString, specialCharacter.Character.ToString());
                else
                    barcode = barcode.Replace(searchString, "");
            }
            //  logDebug(barcode);
            return barcode;
        }
    }

    /// <summary>
    ///Метод воспроизведения разговора
    /// <summary>
    public static void Speak(string text)
    {
        Process process = new();
        process.StartInfo.FileName = "espeak";
        process.StartInfo.Arguments = $"\"{text}\"";
        process.Start();
    }

    /// <summary>
    ///Метод воспроизведения аудио
    /// <summary>
    public static void PlaySound(string soundFilePath)
    {
        using (WaveOutEvent player = new())
        {
            using (AudioFileReader audioFile = new(soundFilePath))
            {
                player.Init(audioFile);
                player.Play();
            }
        }
    }

    private static string _CHECKBOX;
    public string CHECKBOX
    {
        get { return _CHECKBOX; }
        set
        {
            _CHECKBOX = value;
            OnPropertyChanged(nameof(CHECKBOX));
        }
    }
   

    public string ButtonOK => "OK";

    public static bool isCtrlfokuslistFocused = false;

    public static Dictionary<string, string> _DataGridSqlDic = new();
    public static Dictionary<string, string> DataGridSqlDic
    {
        get
        {
            return _DataGridSqlDic;
        }
        set
        {
            _DataGridSqlDic = value;
        }
    }
    public static void FillTextElement(TextElementTimer info)
    {
        if (IsRunning)
        {
            return;
        }

        Control element = Helper.FindFrameworkElement(info.ElementName);
        if (element != null)
        {
            try
            {
                string scalarValue = "";
                DataSet ds = Setter.BridgeWorker.SQLQuery(info.Sql);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    scalarValue = ds.Tables[0].Rows[0][0].ToString();
                }

                PropertyInfo elementProperty = element.GetType().GetProperty(info.ElementProperty);

                object oldValue = elementProperty.GetValue(element, null);
                object newValue = null;

                if (oldValue is string)
                {
                    newValue = scalarValue;
                }
                if (oldValue is int)
                {
                    newValue = Convert.ToInt32(scalarValue);
                }
                if (oldValue is double)
                {
                    newValue = Convert.ToDouble(scalarValue);
                }
                else if (oldValue is bool)
                {
                    newValue = Convert.ToBoolean(scalarValue);
                }
                else if (oldValue is Brush)
                {
                    var converter = new BrushConverter();
                    newValue = (Brush)converter.ConvertFromString(scalarValue);
                }
                //else if (oldValue is Visibility)
                //{
                //    IsVolatile visibility = Avalonia.Media.Visible.Visible;
                //    Enum.TryParse(scalarValue, out visibility);
                //    newValue = visibility;
                //}
                else if (oldValue is DataGridSelectionMode)
                {
                    DataGridSelectionMode dataGridSelectionMode = DataGridSelectionMode.Single;
                    _ = Enum.TryParse(scalarValue, out dataGridSelectionMode);
                    newValue = dataGridSelectionMode;
                }

                if (newValue != null)
                    elementProperty.SetValue(element, newValue, null);

                if (oldValue is DataGridSelectionMode && scalarValue == "Extended")
                {
                    DataGrid grid = (DataGrid)element;
                    if (grid != null)
                    {
                        try
                        {
                            Log.logDebug("Invoke FillTextElement");
                            Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                GridUpdateComboBoxSelection(grid);
                            }, DispatcherPriority.Input);
                        }
                        catch (Exception ex)
                        {
                            Log.logDebug("Invoke FillTextElement " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, null);
            }
        }
    }

    /// <summary>
    /// Ручная синхронизация Checkbox'ов с выделенными строками в гриде
    /// <param name="grid"></param>
    /// </summary>
    public static void GridUpdateComboBoxSelection(DataGrid grid)
    {
        try
        {
            if (grid != null)
            {
                Log.logDebug("grid != null");
                var checkBoxColumn = grid.Columns.FirstOrDefault(t => t is DataGridCheckBoxColumn);
                Log.logDebug("checkBoxColumn");
                if (checkBoxColumn != null && _allowCheckBoxChangeOnSelection)
                {
                    if (grid.SelectedItems != null)
                    {

                        // Get the selected item (row) from the DataGrid
                        DataRowView selectedRow = (DataRowView)grid.SelectedItem;

                        // Update the flag based on the checkbox state
                        bool isChecked = (bool)selectedRow["В наборе?"]; // Replace "CheckboxColumnName" with the actual column name
                        if (isChecked)
                        {
                            // Set the flag to true when the checkbox is checked
                            selectedRow["isSelected"] = true;
                        }
                        else
                        {
                            // Set the flag to false when the checkbox is unchecked
                            selectedRow["isSelected"] = false;
                        }

                        Log.logDebug("checkBoxColumn.Header");
                        if (checkBoxColumn.Header != null)
                        {
                            Log.logDebug("headCheckBox");
                            CheckBox headCheckBox = checkBoxColumn.Header as CheckBox;
                            Log.logDebug("headCheckBox.IsChecked");
                            if (headCheckBox != null && grid.SelectedItems != null)
                            {
                                Log.logDebug("-");
                                headCheckBox.IsChecked = (grid.SelectedItems.Count == grid.SelectedItems.Count);
                            }
                            else
                            {
                                if (headCheckBox == null)
                                    Log.logDebug("- headCheckBox == null");
                                if (grid.SelectedItems == null)
                                    Log.logDebug("- grid.Items == null");
                            }
                        }
                    }
                    else
                        Log.logDebug("GridUpdateComboBoxSelection GRID.SELECTEDITEMS IS NULL!");
                }
                else
                    Log.logDebug("checkBoxColumn IS NULL!");
            }
            else
                Log.logDebug("GridUpdateComboBoxSelection GRID IS NULL!");
        }
        catch (Exception ex)
        {
            Log.logDebug("GridUpdateComboBoxSelection error: " + ex.ToString());
        }
    }

    public static DateTime LastGridActivity { get; set; }

    public static string _FocusedElementName = string.Empty;

    public static string FocusedElementName
    {
        get
        {
            return _FocusedElementName;
        }
        set
        {
            _FocusedElementName = value;
        }
    }

    private static string _queueDemoButton = "0";

    public static string QueueDemoButton
    {
        get
        {
            return _queueDemoButton;
        }
        set
        {
            _queueDemoButton = value;
        }
    }

    public static string[] specialCharacterBrackets = new string[2];

    public class _specialCharacter
    {
        public char Character { get; set; }
        public string? Abbreviation { get; set; }
    }
    public static List<_specialCharacter> _specialCharacters = [];

    public static string _message = string.Empty;
    public string Message
    {
        get { return _message; }
        set
        {
            _message = value;
            OnPropertyChanged(nameof(Message));
        }
    }

    public static bool logEnabled = false;
    public static bool _logDebugEnabled = false;
    public static string clientVersion = "";

    public static bool timerLogEnabled = false;
    public static string buf = string.Empty;
    public static string prm_name = string.Empty;

    public static string? queuedProcedure;
    public static string? queuedValue;

    public static List<Commer> CommersList = [];

    public static List<HotKeyItem> HotKeyList = [];

    public static List<TextElementTimer> TextRefreshTimers = [];

    public static bool screenAcceptsScan = false;

    public static List<ReplaceInfo> ReplaceList = [];

    public static Stopwatch watch = new();

  

    public static Guid screenID;

    public static bool scanModeMsgEnabled = false;

    public static bool SNAPI_DRIVER_LOADED = true;

    public static int connectedScannerID = -1;

    public static bool m_bSuccessOpen = false;

    public static CCoreScannerClass? m_pCoreScanner;

    public static bool IsRunning { get; set; }
    public static bool IsShowWaitScreen { get; set; }

    public static List<procedureReplacement> procedureReplacements = [];
    public static List<Key> ignoreKeys = [];
    public static Control? queuedScanbox;

    public static bool gridLogEnabled = false;

    public static bool addQueueButton = false;

    public static string _CurrentProcName = string.Empty;
    public static string CurrentProcName
    {
        get
        {
            return _CurrentProcName;
        }
        set
        {
            _CurrentProcName = value;
        }
    }


    // Приватные поля
    private static string _url = string.Empty;
    private static string _project = string.Empty;
    private static string _environment = string.Empty;
    private static int _timeout = 30;

    // Публичные свойства с геттерами и сеттерами
    public static string Url
    {
        get { return _url; }
        set { _url = value; }
    }

    public static string Project
    {
        get { return _project; }
        set { _project = value; }
    }

    public static string Environment
    {
        get { return _environment; }
        set { _environment = value; }
    }

    public static int Timeout
    {
        get { return _timeout; }
        set { _timeout = value; }
    }

    private static bool _textWrapDataGrid = false;
    public static bool TextWrapDataGrid
    {
        get { return _textWrapDataGrid; }
        set { _textWrapDataGrid = value; }
    }

    private static bool _fullScreen = false;
    public static bool FullScreen
    {
        get { return _fullScreen; }
        set { _fullScreen = value; }
    }

    private static int _width = 1024;
    public static int Width
    {
        get { return _width; }
        set { _width = value; }
    }

    private static int _height = 600;
    public static int Height
    {
        get { return _height; }
        set { _height = value; }
    }

    private static int _longCommandTimeout = 600;
    public static int LongCommandTimeout
    {
        get { return _longCommandTimeout; }
        set { _longCommandTimeout = value; }
    }

    private static int _shortCommandTimeout = 30;
    public static int ShortCommandTimeout
    {
        get { return _shortCommandTimeout; }
        set { _shortCommandTimeout = value; }
    }

    private static string _logPath = "logs/log.xml";
    public static string LogPath
    {
        get { return _logPath; }
        set { _logPath = value; }
    }

    private static bool _commDebug = false;
    public static bool CommDebug
    {
        get { return _commDebug; }
        set { _commDebug = value; }
    }

    private static bool _debugTime = false;
    public static bool DebugTime
    {
        get { return _debugTime; }
        set { _debugTime = value; }
    }

    private static bool _showWaitScreenByDefault = true;
    public static bool ShowWaitScreenByDefault
    {
        get { return _showWaitScreenByDefault; }
        set { _showWaitScreenByDefault = value; }
    }

    private static bool _useDirectDbConnect = true;
    public static bool UseDirectDbConnect
    {
        get { return _useDirectDbConnect; }
        set { _useDirectDbConnect = value; }
    }

    private static string _styleName = "NewBureauBlue";
    public static string StyleName
    {
        get { return _styleName; }
        set { _styleName = value; }
    }

    private static bool _useCommCommand = false;
    public static bool UseCommCommand
    {
        get { return _useCommCommand; }
        set { _useCommCommand = value; }
    }

    private static bool _collectActualScreens = false;
    public static bool CollectActualScreens
    {
        get { return _collectActualScreens; }
        set { _collectActualScreens = value; }
    }

    private static string _errorProcedure = "Error.axaml";
    public static string ErrorProcedure
    {
        get { return _errorProcedure; }
        set { _errorProcedure = value; }
    }

    private static bool _msgBoxEnabled = false;
    public static bool MsgBoxEnabled
    {
        get { return _msgBoxEnabled; }
        set { _msgBoxEnabled = value; }
    }

    private static bool _logMaterialListSelection = false;
    public static bool LogMaterialListSelection
    {
        get { return _logMaterialListSelection; }
        set { _logMaterialListSelection = value; }
    }

    private static bool _scanModeMsgEnabled = false;
    public static bool ScanModeMsgEnabled
    {
        get { return _scanModeMsgEnabled; }
        set { _scanModeMsgEnabled = value; }
    }

    private static bool _timerLogEnabled = false;
    public static bool TimerLogEnabled
    {
        get { return _timerLogEnabled; }
        set { _timerLogEnabled = value; }
    }

    private static bool _gridLogEnabled = false;
    public static bool GridLogEnabled
    {
        get { return _gridLogEnabled; }
        set { _gridLogEnabled = value; }
    }

    private static bool _canSkipScreenDraw = true;
    public static bool CanSkipScreenDraw
    {
        get { return _canSkipScreenDraw; }
        set { _canSkipScreenDraw = value; }
    }

    private static string _specialCharacterBrackets = string.Empty;
    public static string SpecialCharacterBrackets
    {
        get { return _specialCharacterBrackets; }
        set { _specialCharacterBrackets = value; }
    }

    private static string _autoSubmitTimeoutMS = string.Empty;
    public static string AutoSubmitTimeoutMS
    {
        get { return _autoSubmitTimeoutMS; }
        set { _autoSubmitTimeoutMS = value; }
    }

    private static string _useComPort = string.Empty;
    public static string UseComPort
    {
        get { return _useComPort; }
        set { _useComPort = value; }
    }

    private static string _startProcedure = "dbo.wpf_ctrl_main";
    public static string StartProcedure
    {
        get { return _startProcedure; }
        set { _startProcedure = value; }
    }

    private static List<string> _procedureReplacementsSt = new List<string>();
    public static List<string> ProcedureReplacementsSt
    {
        get { return _procedureReplacementsSt; }
        set { _procedureReplacementsSt = value; }
    }

    private static List<string> _commPort = new List<string>();
    public static List<string> CommPort
    {
        get { return _commPort; }
        set { _commPort = value; }
    }

    private static string _config = "config/setting.json";
    public static string Config
    {
        get { return _config; }
        set { _config = value; }
    }

    private static List<string> _awpApi = new List<string>();
    public static List<string> AwpApi
    {
        get { return _awpApi; }
        set { _awpApi = value; }
    }

    private static string _clientName = "AWPClient";
    public static string ClientName
    {
        get { return _clientName; }
        set { _clientName = value; }
    }

    private static List<string> _scanPrefix = new List<string>();
    public static List<string> ScanPrefix
    {
        get { return _scanPrefix; }
        set { _scanPrefix = value; }
    }

    private static string _clientVersion = "10122024";
    public static string ClientVersion
    {
        get { return _clientVersion; }
        set { _clientVersion = value; }
    }

    private static string _XamlPath = "Screens";
    public static string XamlPath
    {
        get { return _XamlPath; }
        set { _XamlPath = value; }
    }




    public static bool canSkipScreenDraw = true;
    private static readonly DispatcherTimer dispatcherTimer = new();
    public static DispatcherTimer queueTimer = dispatcherTimer;

    public static string CurrentXAML = "";

    public static short[] m_arScannerTypes;
    public static bool[] m_arSelectedTypes;


    public const short SCANNER_TYPES_SNAPI = 2;

    public static short m_nNumberOfTypes;

    public static Stack<ContentControl> contentControl = new();

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindow(IntPtr hWnd, uint uCmd);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

    static bool DisableScannerOnError = true;
    const int STATUS_FALSE = 1;
    private static void scanEnable(bool on)
    {
        if (!SNAPI_DRIVER_LOADED || !DisableScannerOnError)
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
    private static void ExecCmd(int opCode, ref string inXml, out string outXml, out int status, bool async = false)
    {
        outXml = "";
        status = STATUS_FALSE;
        if (m_bSuccessOpen)
        {
            try
            {
                if (async)
                    m_pCoreScanner.ExecCommandAsync(opCode, ref inXml, out status);
                else
                    m_pCoreScanner.ExecCommand(opCode, ref inXml, out outXml, out status);
            }
            catch (Exception ex)
            {
                MessageDialog.Show("ExecCmd", status.ToString() + " EXEC_COMMAND");
                MessageDialog.Show("ExecCmd", "..." + ex.Message.ToString());
            }
        }
    }

  
    private static void NewScreen()
    {
        if (Setter.UseCommCommand)
        {
            CommOff();
        }

        StopTimers();

        ClearVariables();
    }
    public static void CommOff()
    {
        foreach (Commer item in CommersList)
        {
            item.SendCommand(item.sendOff);
        }
    }
    public static void CommOn()
    {
        foreach (Commer item in CommersList)
        {
            item.SendCommand(item.sendOn);
        }
    }

    /// <summary>
    /// Останавливает все таймеры на форме
    /// </summary>
    public static void StopTimers()
    {
        try
        {
            foreach (LogxGrid item in LogxGrids)
            {
               // item.Stop();
            }

            if (RefreshTimer != null)
            {
                RefreshTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                RefreshTimer = null;
            }
            if (ExRefreshTimer != null)
            {
                ExRefreshTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                ExRefreshTimer = null;
            }
            if (timer != null)
            {
                timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                timer = null;
            }
            foreach (TextElementTimer textRefreshTimer in TextRefreshTimers)
            {
                if (textRefreshTimer.Timer != null)
                {
                    textRefreshTimer.Timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    textRefreshTimer.Timer = null;
                }
            }
            TextRefreshTimers.Clear();
        }
        catch (Exception ex)
        {
            LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, null);
        }
    }

    /// <summary>
    /// Очищает переменные перед загрузкой нового экрана
    /// </summary>
    public static void ClearVariables()
    {
        try
        {
            gridKey = string.Empty;
            FocusedElementName = string.Empty;

            ElementList.Clear();

            DataGridSqlDic.Clear();
            ScanBoxesDic.Clear();

            LogxGrids.Clear();

            HotKeyList.Clear();

            ComboList.Clear();
        }
        catch (Exception ex)
        {
            LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, null);
        }
    }

    private static List<LogxGrid> _LogxGrids = [];
    /// <summary>
    /// Список LogxGrid текущего экрана
    /// </summary>
    public static List<LogxGrid> LogxGrids
    {
        get
        {
            return _LogxGrids;
        }
        set
        {
            _LogxGrids = value;
        }
    }

    private static List<ExCombo> _ComboList = [];
    /// <summary>
    /// Список ComboBox-ов текущего экрана
    /// </summary>
    public static List<ExCombo> ComboList
    {
        get
        {
            return _ComboList;
        }
        set
        {
            _ComboList = value;
        }
    }

    private static Dictionary<ScanBox, string> _ScanBoxesDic = [];
    /// <summary>
    /// Список префиксов для ScanBox
    /// </summary>
    public static Dictionary<ScanBox, string> ScanBoxesDic
    {
        get
        {
            return _ScanBoxesDic;
        }
        set
        {
            _ScanBoxesDic = value;
        }
    }

    private static string _gridKey = string.Empty;
    public static string gridKey
    {
        get
        {
            return _gridKey;
        }
        set
        {
            _gridKey = value;
        }
    }

    public static string sql = string.Empty;
    /// <summary>
    /// Список визуальных элементов
    /// </summary>

    private static List<Control> _ElementList = [];
    public static List<Control> ElementList
    {
        get
        {
            return _ElementList;
        }
        set
        {
            _ElementList = value;
        }
    }
    static bool _allowCheckBoxChangeOnSelection = false;

    public static System.Threading.Timer timer;
    public static System.Threading.Timer RefreshTimer;
    public static System.Threading.Timer ExRefreshTimer;

    /// <summary>
    ///Параметры экрана ошибки
    /// <summary>
    public static List<KeyValuePair<string, string>> ErrorRecordSet =
    [
        new KeyValuePair<string, string>("XAML_NAME", ErrorProcedure),
        new KeyValuePair<string, string>("PRM_NAME", "\\*TITLE*\\"),
        new KeyValuePair<string, string>("PRM_VALUE", "Ошибка"),
        new KeyValuePair<string, string>("PRM_NAME", "\\*MESSAGE*\\"),
        new KeyValuePair<string, string>("PRM_VALUE", "Не удалось выполнить запрос"), //Попробуйте через несколько минут еще раз или обратитесь к администратору
        new KeyValuePair<string, string>("PRM_NAME", "\\*BTNBOTTOMTAG*\\"),
        new KeyValuePair<string, string>("PRM_VALUE", StartProcedure.Replace("dbo.", "")),
        new KeyValuePair<string, string>("PRM_NAME", "\\*BTNBOTTOM*\\"),
        new KeyValuePair<string, string>("PRM_VALUE", "<ПРОБЕЛ>  Продолжить"),
        new KeyValuePair<string, string>("HOTKEY", "BTNBOTTOM.Space.click"),
        new KeyValuePair<string, string>("SETFOCUS", "BTNBOTTOM")
    ];
    private static void beep(int mode)
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

}
