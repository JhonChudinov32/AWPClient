using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using AWPClient.Connection;
using AWPClient.ViewModels;
using AWPClient.Views;
using BolapanControl.ItemsFilter.ViewModel;
using System;
using System.Linq;

namespace AWPClient
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // Удаляем валидацию Avalonia
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            //Полноэкранный режим

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                UpdateSetting.SetingUp();
                UpdateSetting.Update();
                if (MainWindowViewModel.FullScreen)
                {
                    desktop.MainWindow = new MainWindow
                    {

                        WindowState = WindowState.FullScreen,
                        DataContext = new MainWindowViewModel(),
                        ExtendClientAreaToDecorationsHint = true,
                        ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome
                    };


                    base.OnFrameworkInitializationCompleted();
                }
                else
                {

                    desktop.MainWindow = new MainWindow
                    {
                        Width = MainWindowViewModel.Width,
                        Height = MainWindowViewModel.Height,
                        Title = "AWPClient",
                        DataContext = new MainWindowViewModel(),
                    };


                    base.OnFrameworkInitializationCompleted();
                }

            }

          
            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Удаляем плагины валидации Avalonia
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}