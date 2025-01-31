using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System.Xml.Linq;
using System;

namespace AWPClient.Controls
{
    public partial class SearchList : UserControl
    {
        public event EventHandler<SearchEventArgs>? SearchClick;

        public class SearchEventArgs : EventArgs
        {
            public string Tag { get; set; }
            public string Text { get; set; }

            public SearchEventArgs(string tag, string text)
            {
                Tag = tag;
                Text = text;
            }
        }

        public SearchList()
        {
            InitializeComponent();
        }

        //private void InitializeComponent()
        //{
        //    // Инициализация UI-компонентов здесь
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Tag is string tag && !string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchEventArgs args = new SearchEventArgs(tag, SearchTextBox.Text.Trim());
                OnRaiseSearchClickEvent(args);
            }
        }

        protected virtual void OnRaiseSearchClickEvent(SearchEventArgs e)
        {
            SearchClick?.Invoke(this, e);
        }

        public Control AddItem(int idx, string item)
        {
            Button block = new Button { Content = item, Tag = idx };
            block.Click += block_Click;
            Stack.Children.Add(block);
            return block;
        }

        private void block_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                button.BorderBrush = new SolidColorBrush(Colors.Transparent); // На Avalonia нельзя указать альфа-канал в SolidColorBrush, изменено на Transparent
            }
        }
    }
}
