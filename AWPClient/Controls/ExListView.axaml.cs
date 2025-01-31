using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AWPClient.Controls
{
    public partial class ExListView : UserControl
    {
        public string? Sql { get; set; }
        public string? RefreshTimeout { get; set; }

        public SelectionMode SelectionMode
        {
            //get { return GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        public static readonly AvaloniaProperty<SelectionMode> SelectionModeProperty =
            AvaloniaProperty.Register<ExListView, SelectionMode>(nameof(SelectionMode));

        public ExListView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

        //    SetBinding(ListView.TagProperty, new Binding("Tag") { Source = this });
        //    SetBinding(ListView.SelectionModeProperty, new Binding("SelectionMode") { Source = this });
        }
    }
}
