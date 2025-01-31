using System.Collections.Generic;

namespace AWPClient.Models
{
    public class Client
    {
        public bool TextWrapDataGrid { get; set; }
        public bool FullScreen { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int LongCommandTimeout { get; set; }
        public int ShortCommandTimeout { get; set; }
        public string LogPath { get; set; }
        public bool CommDebug { get; set; }
        public bool DebugTime { get; set; }
        public bool ShowWaitScreenByDefault { get; set; }
        public bool UseDirectDbConnect { get; set; }
        public string StyleName { get; set; }
        public bool UseCommCommand { get; set; }
        public bool CollectActualScreens { get; set; }
        public string ErrorProcedure { get; set; }
        public bool MsgBoxEnabled { get; set; }
        public bool LogMaterialListSelection { get; set; }
        public bool ScanModeMsgEnabled { get; set; }
        public bool TimerLogEnabled { get; set; }
        public bool GridLogEnabled { get; set; }
        public bool CanSkipScreenDraw { get; set; }
        public string SpecialCharacterBrackets { get; set; }
        public string AutoSubmitTimeoutMS { get; set; }
        public string UseComPort { get; set; }
        public string StartProcedure { get; set; }
        public List<string> ProcedureReplacements { get; set; }
        public List<string> CommPort { get; set; }
        public string Config { get; set; }
        public List<string> AwpApi { get; set; }
        public string ClientName { get; set; }
        public List<string> ScanPrefix { get; set; }
        public string ClientVersion { get; set; }
        public string XamlPath { get; set; }
    }
}
