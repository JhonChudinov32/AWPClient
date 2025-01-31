using Avalonia;
using Avalonia.Controls;

namespace AWPClient.Msgbox
{
    public class ErrorDialog : Window
    {
        public ErrorDialog(string errorMessage, string portDescription)
        {
            this.Title = "Ошибка подключения к COM порту";
            this.Width = 400;
            this.Height = 200;

            TextBlock msgTextBlock = new TextBlock
            {
                Text = $"Не удалось подключиться к {portDescription}. Проверьте что порт присутствует в системе и не занят.\n\n{errorMessage}",
                Margin = new Thickness(10),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap
            };

            Button okButton = new Button
            {
                Content = "OK",
                Width = 100,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            okButton.Click += (sender, e) => Close();

            StackPanel panel = new StackPanel();
            panel.Children.Add(msgTextBlock);
            panel.Children.Add(okButton);

            this.Content = panel;
        }
    }
}
