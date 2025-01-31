using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using AWPClient.Classes;
using AWPClient.ViewModels;

namespace AWPClient.LogServices
{
    public static class LogWorker
    {
      
        private static string _LogPath = MainWindowViewModel.LogPath;
        public static string LogPath
        {
            get
            {
                return _LogPath;
            }
            set
            {
                _LogPath = value;
            }
        }

        static List<PerformanceInfo> perfomances = new List<PerformanceInfo>();

        static List<LogInfo> LogBuffer = new List<LogInfo>();

		public static void Log2All(string GUID, string MethodName, string Message, string Description, string[] Data)
        {
            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                {
                   
                    Setter.BridgeWorker.SetError( MethodName, Message, Description, Data);
                });
                worker.RunWorkerAsync();
            }
            catch { }
        }

		public static void Log2File(string GUID, string MethodName, string Message, string Description, string[] Data)
        {
            lock (typeof(LogWorker))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();

                    int pos;
                    string dir;
                    string result = string.Empty;
                    string resdir = string.Empty;

                    if (OperatingSystem.IsLinux())
                    {
                        pos = AppContext.BaseDirectory.LastIndexOf("/"); // позиция последнего слеша
                        dir = AppContext.BaseDirectory.Substring(0, pos);
                        result = Path.GetDirectoryName(dir);
                        resdir = result + "/";
                      
                    }
                    if (OperatingSystem.IsWindows())
                    {
                        pos = AppContext.BaseDirectory.LastIndexOf("\\"); // позиция последнего слеша
                        dir = AppContext.BaseDirectory.Remove(pos, AppContext.BaseDirectory.Length - pos);
                        result = Path.GetDirectoryName(dir);
                        resdir = result + "\\";
                    }
                  

                    if (!File.Exists(LogPath))
                    {
                        if (!Directory.Exists(Path.Combine(resdir, LogPath)))
                       // if (!Directory.Exists(Path.GetDirectoryName(LogPath)))
                        {
                           // Directory.CreateDirectory(Path.GetDirectoryName(LogPath));
                            Directory.CreateDirectory(Path.Combine(resdir, "logs"));
                        }

                        xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);

                        XmlElement element = xmlDoc.CreateElement("Root");
                        xmlDoc.AppendChild(element);
                    }
                    else
                    {
                        //xmlDoc.Load(LogPath);
                        xmlDoc.Load(Path.Combine(resdir, LogPath));
                    }

                    XmlElement subRoot = xmlDoc.CreateElement("Item");

                    XmlElement appendedElement = xmlDoc.CreateElement("ID");
                    XmlText xmlText = xmlDoc.CreateTextNode(GUID);
                    appendedElement.AppendChild(xmlText);
                    subRoot.AppendChild(appendedElement);

                    appendedElement = xmlDoc.CreateElement("DateTime");
                    xmlText = xmlDoc.CreateTextNode(DateTime.Now.ToString());
                    appendedElement.AppendChild(xmlText);
                    subRoot.AppendChild(appendedElement);

                    appendedElement = xmlDoc.CreateElement("MethodName");
                    xmlText = xmlDoc.CreateTextNode(MethodName);
                    appendedElement.AppendChild(xmlText);
                    subRoot.AppendChild(appendedElement);

                    appendedElement = xmlDoc.CreateElement("Exception");
                    xmlText = xmlDoc.CreateTextNode(Message);
                    appendedElement.AppendChild(xmlText);
                    subRoot.AppendChild(appendedElement);

                    appendedElement = xmlDoc.CreateElement("Description");
                    xmlText = xmlDoc.CreateTextNode(Description);
                    appendedElement.AppendChild(xmlText);
                    subRoot.AppendChild(appendedElement);

                    XmlElement DataElement = xmlDoc.CreateElement("Data");

                    if (Data != null)
                    {
                        foreach (string item in Data)
                        {
                            appendedElement = xmlDoc.CreateElement("Item");
                            xmlText = xmlDoc.CreateTextNode(item);
                            appendedElement.AppendChild(xmlText);
                            DataElement.AppendChild(appendedElement);
                        }
                    }

                    subRoot.AppendChild(DataElement);

                    xmlDoc.DocumentElement.AppendChild(subRoot);

                    if (OperatingSystem.IsLinux())
                    {
                        xmlDoc.Save(Path.Combine(resdir, "logs/log"));
                    }
                    if(OperatingSystem.IsWindows())
                    {
                        xmlDoc.Save(Path.Combine(resdir, LogPath));
                    }
                }
                catch { }
            }
        }

        public static void MakeEvent(string source, string message)
        {
            try
            {
                //EventLog.WriteEntry(source, message);
            }
            catch
            {
                //todo handle
            }
        }

        public static void AddPerfomanceInfo(string WatchName, DateTime datetime, long ElapsedMilliseconds)
        {
            PerformanceInfo info = new PerformanceInfo(WatchName, datetime, ElapsedMilliseconds);
            perfomances.Add(info);
        }

		public static void ExportPerfomanceListToTextFile(string filename)
        {
            string res = string.Empty;
            List<IGrouping<string, PerformanceInfo>> list = perfomances.GroupBy(i => i.WatchName).ToList();
            foreach (IGrouping<string, PerformanceInfo> item in list)
            {
                res += item.Key + Environment.NewLine;
                foreach (PerformanceInfo xxx in item)
                {
                    res += xxx.datetime.ToString("HH:mm:ss") + ": " + xxx.ElapsedMilliseconds.ToString() + Environment.NewLine;
                }
                res += Environment.NewLine;
            }

            using (StreamWriter wr = File.AppendText(filename))
            {
                wr.WriteLine("Замер производительности от '{0}'", DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                wr.WriteLine();
                wr.Write(res);
            }
        }
    }
}
