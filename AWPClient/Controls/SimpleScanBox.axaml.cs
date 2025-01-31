using Avalonia;
using Avalonia.Controls;
using System;

namespace AWPClient.Controls
{
    public partial class SimpleScanBox : UserControl
    {
        public string GroupCode { get; set; } = string.Empty;

        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public static readonly AvaloniaProperty<bool> IsActiveProperty =
            AvaloniaProperty.Register<SimpleScanBox, bool>(nameof(IsActive));

        public TextBox Scan { get; private set; }

        public string ScanCode { get; set; } = string.Empty;

        public SimpleScanBox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Scan = new TextBox();
            Scan.TextChanged += ScanData_TextChanged;

            Content = Scan;
        }

        private void ScanData_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                ScanCode = textBox.Text;
            }
        }
   

        public void SetFocus()
        {
            Scan.Focus();
        }
    }
}
