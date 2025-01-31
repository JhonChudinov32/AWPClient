using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AWPClient.Controls
{
    public partial class ExDataGrid : UserControl
    {
        public string Sql { get; set; }
        public string RefreshTimeout { get; set; }

        public ExDataGrid()
        {
             this.InitializeComponent();
        }

        private void MainDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
             MainDataGrid.Tag = this.Tag;
        }
    }
}
