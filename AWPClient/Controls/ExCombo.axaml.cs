using Avalonia.Controls;
using System.Collections.Generic;

namespace AWPClient.Controls
{
    public partial class ExCombo : UserControl
    {
        public string KeyField { get; set; }
        public string Sql { get; set; }
        public ComboBox combo { get; set; }
        public Dictionary<string, string> Data { get; set; }

        public ExCombo()
        {
            this.InitializeComponent();

            KeyField = string.Empty;
            Sql = string.Empty;
            combo = Combo;
        }
    }
}
