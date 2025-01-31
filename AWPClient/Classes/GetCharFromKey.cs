using Avalonia.Input;
using System;
using System.Runtime.InteropServices;
using System.Text;
using AWPClient.LogServices;


namespace AWPClient.Classes
{
    public class GetCharFromKey
    {
        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);
        /// <summary>
        /// Возвращение символьного кода при нажатии клавиши работает по Windows
        /// </summary>
        public static char FromKey(Key key)
        {
            char ch = '\0';

            int virtualKey = KeyInteropHelper.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
                default:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
            }
            Log.logKey(Environment.NewLine + "CHAR [" + ch + "]" + Environment.NewLine);
            return ch;
        }

        /// <summary>
        /// Возвращение символьного кода при нажатии клавиши работает под  Windows и Linux  (нужна доработка по клавишам)
        /// </summary>
        public static char FromKeyL(KeyEventArgs e)
        {
            char ch = '\0';

            // Пример обработки клавиш
            switch (e.Key)
            {
                case Key.Cancel: ch = ' '; break;
                case Key.Back: ch = '\b'; break;
                case Key.Tab: ch = '\t'; break;
                case Key.Clear: ch = '\f'; break;
                case Key.Return: ch = '\r'; break;
                case Key.Pause: ch = ' '; break;
                case Key.Capital: ch = ' '; break;
                case Key.KanaMode: ch = ' '; break;
                case Key.JunjaMode: ch = ' '; break;
                case Key.FinalMode: ch = ' '; break;
                case Key.HanjaMode: ch = ' '; break;
                case Key.Escape: ch = '\u001B'; break;
                case Key.ImeConvert: ch = ' '; break;
                case Key.ImeNonConvert: ch = ' '; break;
                case Key.ImeAccept: ch = ' '; break;
                case Key.ImeModeChange: ch = ' '; break;
                case Key.Space: ch = ' '; break;
                case Key.Prior: ch = '!'; break;
                case Key.Next: ch = '"'; break;
                case Key.End: ch = '#'; break;
                case Key.Home: ch = '$'; break;
                case Key.Left: ch = '%'; break;
                case Key.Up: ch = '&'; break;
                case Key.Right: ch = '\''; break;
                case Key.Down: ch = '('; break;
                case Key.Select: ch = ')'; break;
                case Key.Print: ch = '*'; break;
                case Key.Execute: ch = '+'; break;
                case Key.Snapshot: ch = ','; break;
                case Key.Insert: ch = '-'; break;
                case Key.Delete: ch = '.'; break;
                case Key.Help: ch = '/'; break;
                case Key.D0: ch = ' '; break;
                case Key.D1: ch = '1'; break;
                case Key.D2: ch = '2'; break;
                case Key.D3: ch = '3'; break;
                case Key.D4: ch = '4'; break;
                case Key.D5: ch = '5'; break;
                case Key.D6: ch = '6'; break;
                case Key.D7: ch = '7'; break;
                case Key.D8: ch = '8'; break;
                case Key.D9: ch = '9'; break;
                case Key.A: ch = 'A'; break;
                case Key.B: ch = 'B'; break;
                case Key.C: ch = 'C'; break;
                case Key.D: ch = 'D'; break;
                case Key.E: ch = 'E'; break;
                case Key.F: ch = 'F'; break;
                case Key.G: ch = 'G'; break;
                case Key.H: ch = 'H'; break;
                case Key.I: ch = 'I'; break;
                case Key.J: ch = 'J'; break;
                case Key.K: ch = 'K'; break;
                case Key.L: ch = 'L'; break;
                case Key.M: ch = 'M'; break;
                case Key.N: ch = 'N'; break;
                case Key.O: ch = 'O'; break;
                case Key.P: ch = 'P'; break;
                case Key.Q: ch = 'Q'; break;
                case Key.R: ch = 'R'; break;
                case Key.S: ch = 'S'; break;
                case Key.T: ch = 'T'; break;
                case Key.U: ch = 'U'; break;
                case Key.V: ch = 'V'; break;
                case Key.W: ch = 'W'; break;
                case Key.X: ch = 'X'; break;
                case Key.Y: ch = 'Y'; break;
                case Key.Z: ch = 'Z'; break;
                case Key.LWin: ch = '['; break;
                case Key.RWin: ch = '\\'; break;
                case Key.Apps: ch = ']'; break;
                case Key.Sleep: ch = '_'; break;
                case Key.NumPad0: ch = '`'; break;
                case Key.NumPad1: ch = 'a'; break;
                case Key.NumPad2: ch = 'b'; break;
                case Key.NumPad3: ch = 'c'; break;
                case Key.NumPad4: ch = 'd'; break;
                case Key.NumPad5: ch = 'e'; break;
                case Key.NumPad6: ch = 'f'; break;
                case Key.NumPad7: ch = 'g'; break;
                case Key.NumPad8: ch = 'h'; break;
                case Key.NumPad9: ch = 'i'; break;
                case Key.Multiply: ch = 'j'; break;
                case Key.Add: ch = 'k'; break;
                case Key.Separator: ch = 'l'; break;
                case Key.Subtract: ch = 'm'; break;
                case Key.Decimal: ch = 'n'; break;
                case Key.Divide: ch = 'o'; break;
                case Key.F1: ch = 'p'; break;
                case Key.F2: ch = 'q'; break;
                case Key.F3: ch = 'r'; break;
                case Key.F4: ch = 's'; break;
                case Key.F5: ch = 't'; break;
                case Key.F6: ch = 'u'; break;
                case Key.F7: ch = 'v'; break;
                case Key.F8: ch = 'w'; break;
                case Key.F9: ch = 'x'; break;
                case Key.F10: ch = 'y'; break;
                case Key.F11: ch = 'z'; break;
                case Key.F12: ch = '{'; break;
                case Key.F13: ch = '|'; break;
                case Key.F14: ch = '}'; break;
                case Key.F15: ch = '~'; break;
                case Key.F16: ch = ' '; break;
                case Key.F17: ch = 'Ђ'; break;
                case Key.F18: ch = 'Ѓ'; break;
                case Key.F19: ch = '‚'; break;
                case Key.F20: ch = 'ѓ'; break;
                case Key.F21: ch = '„'; break;
                case Key.F22: ch = '…'; break;
                case Key.F23: ch = '†'; break;
                case Key.F24: ch = '‡'; break;
                case Key.NumLock: ch = 'Ђ'; break;
                case Key.Scroll: ch = '‘'; break;
                case Key.LeftShift: ch = '\u00A0'; break;
                case Key.RightShift: ch = 'Ў'; break;
                case Key.LeftCtrl: ch = 'ў'; break;
                case Key.RightCtrl: ch = 'Ј'; break;
                case Key.LeftAlt: ch = '¤'; break;
                case Key.RightAlt: ch = 'Ґ'; break;
                case Key.BrowserBack: ch = '¦'; break;
                case Key.BrowserForward: ch = '§'; break;
                case Key.BrowserRefresh: ch = 'Ё'; break;
                case Key.BrowserStop: ch = '©'; break;
                case Key.BrowserSearch: ch = 'Є'; break;
                case Key.BrowserFavorites: ch = '«'; break;
                case Key.BrowserHome: ch = '¬'; break;
                case Key.VolumeMute: ch = '\u00AD'; break;
                case Key.VolumeDown: ch = '®'; break;
                case Key.VolumeUp: ch = 'Ї'; break;
                case Key.MediaNextTrack: ch = '°'; break;
                case Key.MediaPreviousTrack: ch = '±'; break;
                case Key.MediaStop: ch = 'І'; break;
                case Key.MediaPlayPause: ch = 'і'; break;
                case Key.LaunchMail: ch = 'ґ'; break;
                case Key.SelectMedia: ch = 'µ'; break;
                case Key.LaunchApplication1: ch = '¶'; break;
                case Key.LaunchApplication2: ch = '·'; break;
                case Key.Oem1: ch = 'є'; break;
                case Key.OemPlus: ch = '»'; break;
                case Key.OemComma: ch = 'ј'; break;
                case Key.OemMinus: ch = 'Ѕ'; break;
                case Key.OemPeriod: ch = 'ѕ'; break;
                case Key.Oem2: ch = 'ї'; break;
                case Key.Oem3: ch = 'А'; break;
                case Key.AbntC1: ch = 'Б'; break;
                case Key.AbntC2: ch = 'В'; break;
                case Key.Oem4: ch = 'Ы'; break;
                case Key.Oem5: ch = '\\'; break;
                case Key.Oem6: ch = 'Э'; break;
                case Key.Oem7: ch = 'Ю'; break;
                case Key.Oem8: ch = 'Я'; break;
                case Key.Oem102: ch = 'в'; break;
                case Key.ImeProcessed: ch = 'е'; break;
                case Key.OemAttn: ch = 'р'; break;
                case Key.OemFinish: ch = 'с'; break;
                case Key.OemCopy: ch = 'т'; break;
                case Key.OemAuto: ch = 'у'; break;
                case Key.OemEnlw: ch = 'ф'; break;
                case Key.OemBackTab: ch = 'х'; break;
                case Key.Attn: ch = 'ц'; break;
                case Key.CrSel: ch = 'ч'; break;
                case Key.ExSel: ch = 'ш'; break;
                case Key.EraseEof: ch = 'щ'; break;
                case Key.Play: ch = 'ъ'; break;
                case Key.Zoom: ch = 'ы'; break;
                case Key.NoName: ch = 'ь'; break;
                case Key.Pa1: ch = 'э'; break;
                case Key.OemClear: ch = 'ю'; break;
                case Key.DeadCharProcessed: ch = '\0'; break;

                // Добавьте другие клавиши по необходимости
                default:
                    ch = '\0';
                    break;
            }

            Log.logKey(Environment.NewLine + "CHAR [" + ch + "]" + Environment.NewLine);
            return ch;
        }
       
    }
}
