
using Avalonia.Controls;
using Avalonia;
using Avalonia.Media;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using System;
using AWPClient.ViewModels;
using Tmds.DBus.Protocol;
using AWPClient.Classes;
using System.Linq;


namespace AWPClient.Msgbox
{
    public static class MessageDialog
    {

        public static void Show(string title, string message)
        {

            //Window S = new Window
            //{
            //    Title = title,
            //    SizeToContent = SizeToContent.WidthAndHeight,
            //    WindowStartupLocation = WindowStartupLocation.CenterScreen,
            //    MinWidth = 150,
            //    MinHeight = 150,
            //    Content = new StackPanel
            //    {
            //        MaxHeight = 400,
            //        MaxWidth = 400,
            //        Children = {
            //            new TextBlock
            //            {
            //                Text = message,
            //                TextWrapping = TextWrapping.Wrap,
            //                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            //                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
            //                MinHeight = 100,
            //                Margin = new Thickness(30),
            //            },
            //            new Button
            //            {
            //                Content = "Продолжить",
            //                HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            //                Width = 100,
            //                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            //                Margin = new Thickness(5),

            //            }
            //        }
            //    },
            //    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            //    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            //    HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            //    VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center

            //};

            //Application.Current.RegisterServices();
            //S.Show();
            Window S = new Window
            {
                Title = title,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                MinWidth = 150,
                MinHeight = 150,
                Content = new StackPanel
                {
                    MaxHeight = 400,
                    MaxWidth = 400,
                    Children = {
                        new TextBlock
                        {
                            Text = message,
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                            MinHeight = 100,
                            Margin = new Thickness(30),
                        },
                       // new Spacer(), // Добавляем Spacer для выравнивания
                        new Button
                        {
                            Content = "Продолжить",
                            HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            Width = 150,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            Margin = new Thickness(5),
                         
                        }
                    }
                },
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center
            };
            var button = (Button)((StackPanel)S.Content).Children.Last();
            button.Click += CommonMethods.On_BTNTOP_Exit_Click;
            Application.Current.RegisterServices();
            S.Show();

        }
    }
}
