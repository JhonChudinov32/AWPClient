using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using AWPClient.Classes;
using LEAD_UPD_CREATOR.Helpers;
using AWPClient.Models;
using AWPClient.ViewModels;

namespace AWPClient.Connection
{
    public static class UpdateSetting
    {
        private static string jsonFileName = "config/setting.json";
        public static void SetingUp()
        {
            string jsonFilePath = Path.Combine(DirectoryHelper.GetResDirectory(), jsonFileName);

            string json = File.ReadAllText(jsonFilePath);

            var root = JsonConvert.DeserializeObject<JObject>(json);
            Bridge bridge = root["bridge"].ToObject<Bridge>();
            Client client = root["client"].ToObject<Client>();

            MainWindowViewModel.Url = bridge.Url;
            MainWindowViewModel.Project = bridge.Project;
            MainWindowViewModel.Timeout = bridge.Timeout;
            MainWindowViewModel.Environment = bridge.Environment;

            MainWindowViewModel.TextWrapDataGrid = client.TextWrapDataGrid;
            MainWindowViewModel.FullScreen = client.FullScreen;
            MainWindowViewModel.Width = client.Width;
            MainWindowViewModel.Height = client.Height;
            MainWindowViewModel.LongCommandTimeout = client.LongCommandTimeout;
            MainWindowViewModel.ShortCommandTimeout = client.ShortCommandTimeout;
            MainWindowViewModel.LogPath = client.LogPath;
            MainWindowViewModel.CommDebug = client.CommDebug;
            MainWindowViewModel.DebugTime = client.DebugTime;
            MainWindowViewModel.ShowWaitScreenByDefault = client.ShowWaitScreenByDefault;
            MainWindowViewModel.UseDirectDbConnect = client.UseDirectDbConnect;
            MainWindowViewModel.StyleName = client.StyleName;
            MainWindowViewModel.UseCommCommand = client.UseCommCommand;
            MainWindowViewModel.CollectActualScreens = client.CollectActualScreens;
            MainWindowViewModel.ErrorProcedure = client.ErrorProcedure;
            MainWindowViewModel.MsgBoxEnabled = client.MsgBoxEnabled;
            MainWindowViewModel.LogMaterialListSelection = client.LogMaterialListSelection;
            MainWindowViewModel.ScanModeMsgEnabled = client.ScanModeMsgEnabled;
            MainWindowViewModel.TimerLogEnabled = client.TimerLogEnabled;
            MainWindowViewModel.GridLogEnabled = client.GridLogEnabled;
            MainWindowViewModel.CanSkipScreenDraw = client.CanSkipScreenDraw;
            MainWindowViewModel.SpecialCharacterBrackets = client.SpecialCharacterBrackets;
            MainWindowViewModel.AutoSubmitTimeoutMS = client.AutoSubmitTimeoutMS;
            MainWindowViewModel.UseComPort = client.UseComPort;
            MainWindowViewModel.StartProcedure = client.StartProcedure;
            MainWindowViewModel.ProcedureReplacementsSt = client.ProcedureReplacements;
            MainWindowViewModel.CommPort = client.CommPort;
            MainWindowViewModel.Config = client.Config;
            MainWindowViewModel.AwpApi = client.AwpApi;
            MainWindowViewModel.ClientName = client.ClientName;
            MainWindowViewModel.ScanPrefix = client.ScanPrefix;
            MainWindowViewModel.ClientVersion = client.ClientVersion;
            MainWindowViewModel.XamlPath = client.XamlPath;

        
        }

        public static void Update()
        {
            string jsonFilePath = Path.Combine(DirectoryHelper.GetResDirectory(), jsonFileName);

            string json = File.ReadAllText(jsonFilePath);

            var root = JsonConvert.DeserializeObject<JObject>(json);

            List<Var> vars = root["vars"].ToObject<List<Var>>();

            foreach (Var var in vars)
            {
                Setter.BridgeWorker.SetVariable(var.Name, var.Value);
            }
        }

        public static void UpdateOLD()
        {
            string jsonFilePath = Path.Combine(DirectoryHelper.GetResDirectory(), jsonFileName);

            string json = File.ReadAllText(jsonFilePath);

            var root = JsonConvert.DeserializeObject<JObject>(json);

            List<Var> vars = root["vars"].ToObject<List<Var>>();

            foreach (Var var in vars)
            {
                if (var.Name.StartsWith("var") && var.Name.Length > 3)
                {
                    Setter.BridgeWorker.SetVariable(var.Name.Remove(0, 3), var.Value.ToString());
                }
            }
        }
    }

}
