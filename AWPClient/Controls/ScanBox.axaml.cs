using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using AWPClient.Views;

namespace AWPClient.Controls
{
    public partial class ScanBox : UserControl
    {
      
        public string _GroupCode = string.Empty;
        /// <summary>
        /// Код группы
        /// </summary>
        public string GroupCode
        {
            get
            {
                return _GroupCode;
            }
            set
            {
                _GroupCode = value;
            }
        }

        public string ScanText
        {
            get
            {
                return ScanData.Text;
            }
            set
            {
                ScanData.Text = value;
            }
        }

        public bool _IsActive = false;
     

        public string _ScanCode = string.Empty;
        public string ScanCode
        {
            get
            {
                return _ScanCode;
            }
            set
            {
                _ScanCode = value;
            }
        }
        public ScanBox()
        {
           
            this.InitializeComponent();
         
        }

        public void ChangeLanguage(string info)
        {
            //CultureInfo culture = new CultureInfo(info);
            //ScanData. = culture;
        }
        private void ScanData_TextChanged(object sender, TextChangedEventArgs e)
        {
            ScanCode = ScanData.Text;
        }
        
    }
}
