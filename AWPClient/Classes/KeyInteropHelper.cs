using Avalonia.Input;

namespace AWPClient.Classes
{
    public class KeyInteropHelper
    {
       
        //public static Key KeyFromVirtualKey(int virtualKey)
        //{
        //    Key key = Key.None;
        //    switch (virtualKey)
        //    {
        //        case 3:
        //            return Key.Cancel;
        //        case 8:
        //            return Key.Back;
        //        case 9:
        //            return Key.Tab;
        //        case 12:
        //            return Key.Clear;
        //        case 13:
        //            return Key.Return;
        //        case 19:
        //            return Key.Pause;
        //        case 20:
        //            return Key.Capital;
        //        case 21:
        //            return Key.KanaMode;
        //        case 23:
        //            return Key.JunjaMode;
        //        case 24:
        //            return Key.FinalMode;
        //        case 25:
        //            return Key.HanjaMode;
        //        case 27:
        //            return Key.Escape;
        //        case 28:
        //            return Key.ImeConvert;
        //        case 29:
        //            return Key.ImeNonConvert;
        //        case 30:
        //            return Key.ImeAccept;
        //        case 31:
        //            return Key.ImeModeChange;
        //        case 32:
        //            return Key.Space;
        //        case 33:
        //            return Key.Prior;
        //        case 34:
        //            return Key.Next;
        //        case 35:
        //            return Key.End;
        //        case 36:
        //            return Key.Home;
        //        case 37:
        //            return Key.Left;
        //        case 38:
        //            return Key.Up;
        //        case 39:
        //            return Key.Right;
        //        case 40:
        //            return Key.Down;
        //        case 41:
        //            return Key.Select;
        //        case 42:
        //            return Key.Print;
        //        case 43:
        //            return Key.Execute;
        //        case 44:
        //            return Key.Snapshot;
        //        case 45:
        //            return Key.Insert;
        //        case 46:
        //            return Key.Delete;
        //        case 47:
        //            return Key.Help;
        //        case 48:
        //            return Key.D0;
        //        case 49:
        //            return Key.D1;
        //        case 50:
        //            return Key.D2;
        //        case 51:
        //            return Key.D3;
        //        case 52:
        //            return Key.D4;
        //        case 53:
        //            return Key.D5;
        //        case 54:
        //            return Key.D6;
        //        case 55:
        //            return Key.D7;
        //        case 56:
        //            return Key.D8;
        //        case 57:
        //            return Key.D9;
        //        case 65:
        //            return Key.A;
        //        case 66:
        //            return Key.B;
        //        case 67:
        //            return Key.C;
        //        case 68:
        //            return Key.D;
        //        case 69:
        //            return Key.E;
        //        case 70:
        //            return Key.F;
        //        case 71:
        //            return Key.G;
        //        case 72:
        //            return Key.H;
        //        case 73:
        //            return Key.I;
        //        case 74:
        //            return Key.J;
        //        case 75:
        //            return Key.K;
        //        case 76:
        //            return Key.L;
        //        case 77:
        //            return Key.M;
        //        case 78:
        //            return Key.N;
        //        case 79:
        //            return Key.O;
        //        case 80:
        //            return Key.P;
        //        case 81:
        //            return Key.Q;
        //        case 82:
        //            return Key.R;
        //        case 83:
        //            return Key.S;
        //        case 84:
        //            return Key.T;
        //        case 85:
        //            return Key.U;
        //        case 86:
        //            return Key.V;
        //        case 87:
        //            return Key.W;
        //        case 88:
        //            return Key.X;
        //        case 89:
        //            return Key.Y;
        //        case 90:
        //            return Key.Z;
        //        case 91:
        //            return Key.LWin;
        //        case 92:
        //            return Key.RWin;
        //        case 93:
        //            return Key.Apps;
        //        case 95:
        //            return Key.Sleep;
        //        case 96:
        //            return Key.NumPad0;
        //        case 97:
        //            return Key.NumPad1;
        //        case 98:
        //            return Key.NumPad2;
        //        case 99:
        //            return Key.NumPad3;
        //        case 100:
        //            return Key.NumPad4;
        //        case 101:
        //            return Key.NumPad5;
        //        case 102:
        //            return Key.NumPad6;
        //        case 103:
        //            return Key.NumPad7;
        //        case 104:
        //            return Key.NumPad8;
        //        case 105:
        //            return Key.NumPad9;
        //        case 106:
        //            return Key.Multiply;
        //        case 107:
        //            return Key.Add;
        //        case 108:
        //            return Key.Separator;
        //        case 109:
        //            return Key.Subtract;
        //        case 110:
        //            return Key.Decimal;
        //        case 111:
        //            return Key.Divide;
        //        case 112:
        //            return Key.F1;
        //        case 113:
        //            return Key.F2;
        //        case 114:
        //            return Key.F3;
        //        case 115:
        //            return Key.F4;
        //        case 116:
        //            return Key.F5;
        //        case 117:
        //            return Key.F6;
        //        case 118:
        //            return Key.F7;
        //        case 119:
        //            return Key.F8;
        //        case 120:
        //            return Key.F9;
        //        case 121:
        //            return Key.F10;
        //        case 122:
        //            return Key.F11;
        //        case 123:
        //            return Key.F12;
        //        case 124:
        //            return Key.F13;
        //        case 125:
        //            return Key.F14;
        //        case 126:
        //            return Key.F15;
        //        case 127:
        //            return Key.F16;
        //        case 128:
        //            return Key.F17;
        //        case 129:
        //            return Key.F18;
        //        case 130:
        //            return Key.F19;
        //        case 131:
        //            return Key.F20;
        //        case 132:
        //            return Key.F21;
        //        case 133:
        //            return Key.F22;
        //        case 134:
        //            return Key.F23;
        //        case 135:
        //            return Key.F24;
        //        case 144:
        //            return Key.NumLock;
        //        case 145:
        //            return Key.Scroll;
        //        case 16:
        //        case 160:
        //            return Key.LeftShift;
        //        case 161:
        //            return Key.RightShift;
        //        case 17:
        //        case 162:
        //            return Key.LeftCtrl;
        //        case 163:
        //            return Key.RightCtrl;
        //        case 18:
        //        case 164:
        //            return Key.LeftAlt;
        //        case 165:
        //            return Key.RightAlt;
        //        case 166:
        //            return Key.BrowserBack;
        //        case 167:
        //            return Key.BrowserForward;
        //        case 168:
        //            return Key.BrowserRefresh;
        //        case 169:
        //            return Key.BrowserStop;
        //        case 170:
        //            return Key.BrowserSearch;
        //        case 171:
        //            return Key.BrowserFavorites;
        //        case 172:
        //            return Key.BrowserHome;
        //        case 173:
        //            return Key.VolumeMute;
        //        case 174:
        //            return Key.VolumeDown;
        //        case 175:
        //            return Key.VolumeUp;
        //        case 176:
        //            return Key.MediaNextTrack;
        //        case 177:
        //            return Key.MediaPreviousTrack;
        //        case 178:
        //            return Key.MediaStop;
        //        case 179:
        //            return Key.MediaPlayPause;
        //        case 180:
        //            return Key.LaunchMail;
        //        case 181:
        //            return Key.SelectMedia;
        //        case 182:
        //            return Key.LaunchApplication1;
        //        case 183:
        //            return Key.LaunchApplication2;
        //        case 186:
        //            return Key.Oem1;
        //        case 187:
        //            return Key.OemPlus;
        //        case 188:
        //            return Key.OemComma;
        //        case 189:
        //            return Key.OemMinus;
        //        case 190:
        //            return Key.OemPeriod;
        //        case 191:
        //            return Key.Oem2;
        //        case 192:
        //            return Key.Oem3;
        //        case 193:
        //            return Key.AbntC1;
        //        case 194:
        //            return Key.AbntC2;
        //        case 219:
        //            return Key.Oem4;
        //        case 220:
        //            return Key.Oem5;
        //        case 221:
        //            return Key.Oem6;
        //        case 222:
        //            return Key.Oem7;
        //        case 223:
        //            return Key.Oem8;
        //        case 226:
        //            return Key.Oem102;
        //        case 229:
        //            return Key.ImeProcessed;
        //        case 240:
        //            return Key.OemAttn;
        //        case 241:
        //            return Key.OemFinish;
        //        case 242:
        //            return Key.OemCopy;
        //        case 243:
        //            return Key.OemAuto;
        //        case 244:
        //            return Key.OemEnlw;
        //        case 245:
        //            return Key.OemBackTab;
        //        case 246:
        //            return Key.Attn;
        //        case 247:
        //            return Key.CrSel;
        //        case 248:
        //            return Key.ExSel;
        //        case 249:
        //            return Key.EraseEof;
        //        case 250:
        //            return Key.Play;
        //        case 251:
        //            return Key.Zoom;
        //        case 252:
        //            return Key.NoName;
        //        case 253:
        //            return Key.Pa1;
        //        case 254:
        //            return Key.OemClear;
        //        default:
        //            return Key.None;
        //    }
        //}

        public static int VirtualKeyFromKey(Key key)
        {
            switch (key)
            {
                
                case Key.Cancel: return 3;
                case Key.Back : return 8;
                case Key.Tab : return 9;
                case Key.Clear : return 12;
                case Key.Return : return 13;
                case Key.Pause : return 19;
                case Key.Capital : return 20;
                case Key.KanaMode : return 21;
                case Key.JunjaMode : return 23;
                case Key.FinalMode : return 24;
                case Key.HanjaMode : return 25;
                case Key.Escape : return 27;
                case Key.ImeConvert : return 28;
                case Key.ImeNonConvert : return 29;
                case Key.ImeAccept : return 30;
                case Key.ImeModeChange : return 31;
                case Key.Space : return 32;
                case Key.Prior : return 33;
                case Key.Next : return 34;
                case Key.End : return 35;
                case Key.Home : return 36;
                case Key.Left : return 37;
                case Key.Up : return 38;
                case Key.Right : return 39;
                case Key.Down : return 40;
                case Key.Select : return 41;
                case Key.Print : return 42;
                case Key.Execute : return 43;
                case Key.Snapshot : return 44;
                case Key.Insert : return 45;
                case Key.Delete : return 46;
                case Key.Help : return 47;
                case Key.D0 : return 48;
                case Key.D1 : return 49;
                case Key.D2 : return 50;
                case Key.D3 : return 51;
                case Key.D4 : return 52;
                case Key.D5 : return 53;
                case Key.D6 : return 54;
                case Key.D7 : return 55;
                case Key.D8 : return 56;
                case Key.D9 : return 57;
                case Key.A : return 65;
                case Key.B : return 66;
                case Key.C : return 67;
                case Key.D : return 68;
                case Key.E : return 69;
                case Key.F : return 70;
                case Key.G : return 71;
                case Key.H : return 72;
                case Key.I : return 73;
                case Key.J : return 74;
                case Key.K : return 75;
                case Key.L : return 76;
                case Key.M : return 77;
                case Key.N : return 78;
                case Key.O : return 79;
                case Key.P : return 80;
                case Key.Q : return 81;
                case Key.R : return 82;
                case Key.S : return 83;
                case Key.T : return 84;
                case Key.U : return 85;
                case Key.V : return 86;
                case Key.W : return 87;
                case Key.X : return 88;
                case Key.Y : return 89;
                case Key.Z : return 90;
                case Key.LWin : return 91;
                case Key.RWin : return 92;
                case Key.Apps : return 93;
                case Key.Sleep : return 95;
                case Key.NumPad0 : return 96;
                case Key.NumPad1 : return 97;
                case Key.NumPad2 : return 98;
                case Key.NumPad3 : return 99;
                case Key.NumPad4 : return 100;
                case Key.NumPad5 : return 101;
                case Key.NumPad6 : return 102;
                case Key.NumPad7 : return 103;
                case Key.NumPad8 : return 104;
                case Key.NumPad9 : return 105;
                case Key.Multiply : return 106;
                case Key.Add : return 107;
                case Key.Separator : return 108;
                case Key.Subtract : return 109;
                case Key.Decimal : return 110;
                case Key.Divide : return 111;
                case Key.F1 : return 112;
                case Key.F2 : return 113;
                case Key.F3 : return 114;
                case Key.F4 : return 115;
                case Key.F5 : return 116;
                case Key.F6 : return 117;
                case Key.F7 : return 118;
                case Key.F8 : return 119;
                case Key.F9 : return 120;
                case Key.F10 : return 121;
                case Key.F11 : return 122;
                case Key.F12 : return 123;
                case Key.F13 : return 124;
                case Key.F14 : return 125;
                case Key.F15 : return 126;
                case Key.F16 : return 127;
                case Key.F17 : return 128;
                case Key.F18 : return 129;
                case Key.F19 : return 130;
                case Key.F20 : return 131;
                case Key.F21 : return 132;
                case Key.F22 : return 133;
                case Key.F23 : return 134;
                case Key.F24 : return 135;
                case Key.NumLock : return 144;
                case Key.Scroll : return 145;
                case Key.LeftShift : return 160;
                case Key.RightShift : return 161;
                case Key.LeftCtrl : return 162;
                case Key.RightCtrl : return 163;
                case Key.LeftAlt : return 164;
                case Key.RightAlt : return 165;
                case Key.BrowserBack : return 166;
                case Key.BrowserForward : return 167;
                case Key.BrowserRefresh : return 168;
                case Key.BrowserStop : return 169;
                case Key.BrowserSearch : return 170;
                case Key.BrowserFavorites : return 171;
                case Key.BrowserHome : return 172;
                case Key.VolumeMute : return 173;
                case Key.VolumeDown : return 174;
                case Key.VolumeUp : return 175;
                case Key.MediaNextTrack : return 176;
                case Key.MediaPreviousTrack : return 177;
                case Key.MediaStop : return 178;
                case Key.MediaPlayPause : return 179;
                case Key.LaunchMail : return 180;
                case Key.SelectMedia : return 181;
                case Key.LaunchApplication1 : return 182;
                case Key.LaunchApplication2 : return 183;
                case Key.Oem1 : return 186;
                case Key.OemPlus : return 187;
                case Key.OemComma : return 188;
                case Key.OemMinus : return 189;
                case Key.OemPeriod : return 190;
                case Key.Oem2 : return 191;
                case Key.Oem3 : return 192;
                case Key.AbntC1 : return 193;
                case Key.AbntC2 : return 194;
                case Key.Oem4 : return 219;
                case Key.Oem5 : return 220;
                case Key.Oem6 : return 221;
                case Key.Oem7 : return 222;
                case Key.Oem8 : return 223;
                case Key.Oem102 : return 226;
                case Key.ImeProcessed : return 229;
                case Key.OemAttn : return 240;
                case Key.OemFinish : return 241;
                case Key.OemCopy : return 242;
                case Key.OemAuto : return 243;
                case Key.OemEnlw : return 244;
                case Key.OemBackTab : return 245;
                case Key.Attn : return 246;
                case Key.CrSel : return 247;
                case Key.ExSel : return 248;
                case Key.EraseEof : return 249;
                case Key.Play : return 250;
                case Key.Zoom : return 251;
                case Key.NoName : return 252;
                case Key.Pa1 : return 253;
                case Key.OemClear : return 254;
                case Key.DeadCharProcessed : return 0;

                default: return 0; // Возвращаем 0 для неизвестной клавиши
            }
        }

        //public static int VirtualKeyFromKey(Key key)
        //{
        //    int num = 0;
        //    return key switch
        //    {
        //        Key.Cancel => 3,
        //        Key.Back => 8,
        //        Key.Tab => 9,
        //        Key.Clear => 12,
        //        Key.Return => 13,
        //        Key.Pause => 19,
        //        Key.Capital => 20,
        //        Key.KanaMode => 21,
        //        Key.JunjaMode => 23,
        //        Key.FinalMode => 24,
        //        Key.HanjaMode => 25,
        //        Key.Escape => 27,
        //        Key.ImeConvert => 28,
        //        Key.ImeNonConvert => 29,
        //        Key.ImeAccept => 30,
        //        Key.ImeModeChange => 31,
        //        Key.Space => 32,
        //        Key.Prior => 33,
        //        Key.Next => 34,
        //        Key.End => 35,
        //        Key.Home => 36,
        //        Key.Left => 37,
        //        Key.Up => 38,
        //        Key.Right => 39,
        //        Key.Down => 40,
        //        Key.Select => 41,
        //        Key.Print => 42,
        //        Key.Execute => 43,
        //        Key.Snapshot => 44,
        //        Key.Insert => 45,
        //        Key.Delete => 46,
        //        Key.Help => 47,
        //        Key.D0 => 48,
        //        Key.D1 => 49,
        //        Key.D2 => 50,
        //        Key.D3 => 51,
        //        Key.D4 => 52,
        //        Key.D5 => 53,
        //        Key.D6 => 54,
        //        Key.D7 => 55,
        //        Key.D8 => 56,
        //        Key.D9 => 57,
        //        Key.A => 65,
        //        Key.B => 66,
        //        Key.C => 67,
        //        Key.D => 68,
        //        Key.E => 69,
        //        Key.F => 70,
        //        Key.G => 71,
        //        Key.H => 72,
        //        Key.I => 73,
        //        Key.J => 74,
        //        Key.K => 75,
        //        Key.L => 76,
        //        Key.M => 77,
        //        Key.N => 78,
        //        Key.O => 79,
        //        Key.P => 80,
        //        Key.Q => 81,
        //        Key.R => 82,
        //        Key.S => 83,
        //        Key.T => 84,
        //        Key.U => 85,
        //        Key.V => 86,
        //        Key.W => 87,
        //        Key.X => 88,
        //        Key.Y => 89,
        //        Key.Z => 90,
        //        Key.LWin => 91,
        //        Key.RWin => 92,
        //        Key.Apps => 93,
        //        Key.Sleep => 95,
        //        Key.NumPad0 => 96,
        //        Key.NumPad1 => 97,
        //        Key.NumPad2 => 98,
        //        Key.NumPad3 => 99,
        //        Key.NumPad4 => 100,
        //        Key.NumPad5 => 101,
        //        Key.NumPad6 => 102,
        //        Key.NumPad7 => 103,
        //        Key.NumPad8 => 104,
        //        Key.NumPad9 => 105,
        //        Key.Multiply => 106,
        //        Key.Add => 107,
        //        Key.Separator => 108,
        //        Key.Subtract => 109,
        //        Key.Decimal => 110,
        //        Key.Divide => 111,
        //        Key.F1 => 112,
        //        Key.F2 => 113,
        //        Key.F3 => 114,
        //        Key.F4 => 115,
        //        Key.F5 => 116,
        //        Key.F6 => 117,
        //        Key.F7 => 118,
        //        Key.F8 => 119,
        //        Key.F9 => 120,
        //        Key.F10 => 121,
        //        Key.F11 => 122,
        //        Key.F12 => 123,
        //        Key.F13 => 124,
        //        Key.F14 => 125,
        //        Key.F15 => 126,
        //        Key.F16 => 127,
        //        Key.F17 => 128,
        //        Key.F18 => 129,
        //        Key.F19 => 130,
        //        Key.F20 => 131,
        //        Key.F21 => 132,
        //        Key.F22 => 133,
        //        Key.F23 => 134,
        //        Key.F24 => 135,
        //        Key.NumLock => 144,
        //        Key.Scroll => 145,
        //        Key.LeftShift => 160,
        //        Key.RightShift => 161,
        //        Key.LeftCtrl => 162,
        //        Key.RightCtrl => 163,
        //        Key.LeftAlt => 164,
        //        Key.RightAlt => 165,
        //        Key.BrowserBack => 166,
        //        Key.BrowserForward => 167,
        //        Key.BrowserRefresh => 168,
        //        Key.BrowserStop => 169,
        //        Key.BrowserSearch => 170,
        //        Key.BrowserFavorites => 171,
        //        Key.BrowserHome => 172,
        //        Key.VolumeMute => 173,
        //        Key.VolumeDown => 174,
        //        Key.VolumeUp => 175,
        //        Key.MediaNextTrack => 176,
        //        Key.MediaPreviousTrack => 177,
        //        Key.MediaStop => 178,
        //        Key.MediaPlayPause => 179,
        //        Key.LaunchMail => 180,
        //        Key.SelectMedia => 181,
        //        Key.LaunchApplication1 => 182,
        //        Key.LaunchApplication2 => 183,
        //        Key.Oem1 => 186,
        //        Key.OemPlus => 187,
        //        Key.OemComma => 188,
        //        Key.OemMinus => 189,
        //        Key.OemPeriod => 190,
        //        Key.Oem2 => 191,
        //        Key.Oem3 => 192,
        //        Key.AbntC1 => 193,
        //        Key.AbntC2 => 194,
        //        Key.Oem4 => 219,
        //        Key.Oem5 => 220,
        //        Key.Oem6 => 221,
        //        Key.Oem7 => 222,
        //        Key.Oem8 => 223,
        //        Key.Oem102 => 226,
        //        Key.ImeProcessed => 229,
        //        Key.OemAttn => 240,
        //        Key.OemFinish => 241,
        //        Key.OemCopy => 242,
        //        Key.OemAuto => 243,
        //        Key.OemEnlw => 244,
        //        Key.OemBackTab => 245,
        //        Key.Attn => 246,
        //        Key.CrSel => 247,
        //        Key.ExSel => 248,
        //        Key.EraseEof => 249,
        //        Key.Play => 250,
        //        Key.Zoom => 251,
        //        Key.NoName => 252,
        //        Key.Pa1 => 253,
        //        Key.OemClear => 254,
        //        Key.DeadCharProcessed => 0,
        //        _ => 0,
        //    };
        //}
    }

}
