using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO.Ports;
using AWPClient.LogServices;

namespace AWPClient.Classes
{
    public class Commer
    {
        public List<string> StoredCommand { get; set; }

        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public string GroupCode { get; set; }

        public string StopSymbols { get; set; }

        public string sendOff { get; set; }
        public string sendOn { get; set; }

        SerialPort sp = new SerialPort();

        private string buf { get; set; }

        public delegate void CustomEventHandler(object sender, CustomEventArgs e);

        public class CustomEventArgs : EventArgs
        {
            public CustomEventArgs(string s)
            {
                message = s;
            }
            private string message;
            public string Message
            {
                get { return message; }
            }
        }

        public event CustomEventHandler? DataCome;

        public Commer(string PortName, int BaudRate, string GroupCode, bool StartAndKeepAlive, string SendOff, string SendOn)
        {
            buf = string.Empty;
            StoredCommand = new List<string>();

            this.StopSymbols = Environment.NewLine;

            this.PortName = PortName;
            this.BaudRate = BaudRate;
            this.GroupCode = GroupCode;

            this.sendOff = SendOff;
            this.sendOn = SendOn;

            //if (!sp.IsOpen)//Status && StartAndKeepAlive)
            //{
            try
            {
                if (sp != null)
                {
                    if (sp.IsOpen)
                    {
                        sp.Close();
                    }
                    sp = new SerialPort(PortName, BaudRate, Parity.None, 7, StopBits.One);
                    sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                    sp.Open();
                }
            }
            catch (Exception ex)
            {
              // LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "Com: " + PortName, "BaudRate: " + BaudRate });
            }
            //}
        }

        public void SendCommand(string Command)
        {
            try
            {
                Send(Command);
            }
            catch (Exception ex)
            {
                LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "Send command: " + Command });
            }
        }

        public void SendListCommand()
        {
            try
            {
                foreach (string item in StoredCommand)
                {
                    Send(item);
                }
                StoredCommand.Clear();
            }
            catch (Exception ex)
            {
                LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, null);
            }
        }

        private void Send(string text)
        {
            try
            {
                if (sp != null && sp.IsOpen)
                {
                    sp.Write(text);
                    System.Threading.Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "Send text: " + text });
            }
        }
        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                buf += ((SerialPort)sender).ReadExisting();

                //if (buf.Contains(StopSymbols))
                if (buf.Contains("\r") || buf.Contains("\n"))
                {
                    int i = buf.IndexOf("\r");
                    int j = buf.IndexOf("\n");

                    if (j > i) i = j;
                    buf = buf.Substring(0, i + 1);

                    //buf = buf.Replace(StopSymbols, "");
                    buf = buf.Replace("\r", "");
                    buf = buf.Replace("\n", "");
                    if (DataCome != null)
                    {
                        DataCome(this, new CustomEventArgs(buf));
                    }
                    buf = string.Empty;
                }
            }
            catch (Exception ex)
            {
                LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { "Buf: " + buf });
            }
        }

        ~Commer()
        {
            try
            {
                if (sp != null && sp.IsOpen)
                {
                    sp.Close();
                }
            }
            catch (Exception ex)
            {
                LogWorker.Log2All(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, null);
            }
        }
    }
}
