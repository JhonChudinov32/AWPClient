using System;
using AWPClient.Connection;
using AWPClient.ViewModels;

namespace AWPClient.Classes
{
    public static class Setter
    {
        //Преобразование строки подключения
        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        static public string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(encodedData);
            string returnValue =
               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

        private static string _ProjectName = string.Empty;
        public static string ProjectName
        {
            get
            {
                return _ProjectName;
            }
            set
            {
                _ProjectName = value;
            }
        }
        private static string _Url = string.Empty;
        public static string Url
        {
            get
            {
                return _Url;
            }
            set
            {
                _Url = value;
            }
        }

        private static string _ID = Guid.NewGuid().ToString();
        public static string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        private static bool _UseDirectDbConnect = UseDirectDbConnect;
        public static bool UseDirectDbConnect
        {
            get
            {
                return _UseDirectDbConnect;
            }
            set
            {
                _UseDirectDbConnect = value;
            }
        }
        private static bool _UseCommCommand = UseCommCommand;
        public static bool UseCommCommand
        {
            get
            {
                return _UseCommCommand;
            }
            set
            {
                _UseCommCommand = value;
            }
        }

        private static int _GridRefreshTimeout = 30;
        public static int GridRefreshTimeout
        {
            get
            {
                return _GridRefreshTimeout;
            }
            set
            {
                _GridRefreshTimeout = value;
            }
        }

        private static bool _IsShowWaitScreen = MainWindowViewModel.ShowWaitScreenByDefault;
        public static bool IsShowWaitScreen
        {
            get
            {
                return _IsShowWaitScreen;
            }
            set
            {
                _IsShowWaitScreen = value;
            }
        }

        private static string _Environment = Environment;
        public static string Environment
        {
            get
            {
                return _Environment;
            }
            set
            {
                _Environment = value;
            }
        }

        private static string _XamlPath = MainWindowViewModel.XamlPath;
        public static string XamlPath
        {
            get
            {
                return _XamlPath;
            }
            set
            {
                _XamlPath = value;
            }
        }

        private static IBridgeWorker _BridgeWorker = (UseDirectDbConnect) ? (IBridgeWorker)(new BridgeWorker()) : (IBridgeWorker)(new BridgeWorker());

        public static IBridgeWorker BridgeWorker
        {

            get
            {
                return _BridgeWorker;

            }
            set
            {
                _BridgeWorker = value;
            }
        }

    }
}
