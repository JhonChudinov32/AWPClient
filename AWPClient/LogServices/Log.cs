using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using System;
using System.IO;
using AWPClient.Msgbox;
using AWPClient.ViewModels;

namespace AWPClient.LogServices
{
    public class Log
    {
       
        public static void ThrowError(string text)
        {
            try
            {
                logDebug("Invoke ThrowError");
                Dispatcher.UIThread.InvokeAsync(new Action(delegate
                {
                    MessageDialog.Show(text, "Закрытие программы");
                    if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime) lifetime.Shutdown();
                }));
            }
            catch (Exception ex)
            {
                logDebug("Invoke ThrowError " + ex.Message);
            }
        }
        private static StreamWriter logWriter = null;
        private static StreamWriter logMaterialWriter = null;
        private static StreamWriter logTimerWriter = null;
        static string logPath = null;
        static string logMaterialPath = null;
        static string logTimerPath = null;
        static string logAccum = "";
        static string logMaterialAccum = "";
        public static bool logWrite = false;
        public static void logKey(string text)
        {
           
            try
            {
                if (!MainWindowViewModel.logEnabled)
                    return;

                logAccum += text;
                if (!logWrite)
                    return;
                logWrite = false;
                if (logWriter == null)
                {
                    logPath = DateTime.Now.ToString("yyMMddhhmmss") + ".txt";
                    if (!File.Exists(logPath))
                    {
                        using (logWriter = File.CreateText(logPath))
                            logWriter.Write(logAccum);
                    }
                }
                else
                {
                    using (logWriter = File.AppendText(logPath))
                        logWriter.Write(logAccum);
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static void logDebug(string text)
        {
            try
            {
                if (!MainWindowViewModel._logDebugEnabled)
                    return;
                if (logWriter == null)
                {
                    logPath = DateTime.Now.ToString("DBG_yyMMddhhmmss") + ".txt";
                    if (!File.Exists(logPath))
                    {
                        using (logWriter = File.CreateText(logPath))
                            logWriter.Write(text + Environment.NewLine);
                    }
                }
                else
                {
                    using (logWriter = File.AppendText(logPath))
                        logWriter.Write(text + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {

            }
        }

        static string logTimerAccumulated = "";
        public static void logTimer(string text)
        {
           
            text = "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "] " + text + Environment.NewLine;

            if (logTimerAccumulated.Length < 50000)
                logTimerAccumulated += text;

            try
            {
                if (logTimerWriter == null)
                {
                    logTimerPath = "[TIMELOG][" + MainWindowViewModel.clientVersion + "]" + DateTime.Now.ToString("yyMMddHH") + ".txt";
                    if (!File.Exists(logTimerPath))
                    {
                        using (logTimerWriter = File.CreateText(logTimerPath))
                            logTimerWriter.Write(logTimerAccumulated);
                    }
                }
                else
                {
                    using (logTimerWriter = File.AppendText(logTimerPath))
                        logTimerWriter.Write(logTimerAccumulated);
                }
                logTimerAccumulated = "";
            }
            catch (Exception e)
            {

            }
        }
    }
}
