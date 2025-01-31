using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using AWPClient.LogServices;
using AWPClient.ViewModels;
using DataGrid = Avalonia.Controls.DataGrid;
using Avalonia.Controls.Templates;
using Avalonia.Media;
using System.Threading;
using System.Threading.Tasks;


namespace AWPClient.Classes
{
    public static class CommonMethods
    {
        /// <summary>
        ///  Вызов кнопки BTNTOP (Выход из приложения)
        /// </summary>
        /// 
        public static void On_BTNTOP_Exit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime) lifetime.Shutdown();
        }

        /// <summary>
        ///  Событие обработки клавиш
        /// </summary>
        private static HashSet<Button> BlockedButtons = new HashSet<Button>();
        public static void OnControlKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {

                    if (MainWindowViewModel.HotKeyList != null && MainWindowViewModel.HotKeyList.Count > 0)
                    {
                        HotKeyItem items = MainWindowViewModel.HotKeyList.FirstOrDefault(i => i.HotKey == e.Key);
                        if (items == null)
                        {
                            try
                            {
                                items = MainWindowViewModel.HotKeyList.FirstOrDefault(i => i.HotKey == e.Key);
                            }
                            catch (Exception)
                            {

                            }
                        }
                        if (items != null)
                        {
                            Button button = (sender as Control).FindControl<Button>(items.ElementName);
                            if (button != null && !BlockedButtons.Contains(button))
                            {
                                //MessageDialog.Show("", items.ElementName.ToString());
                                BlockedButtons.Add(button);
                                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                                // Добавить задержку или логику разблокировки кнопки после выполнения действия
                            }
                        }
                    }

                });
            }
            catch (Exception ex)
            {
                LogWorker.Log2File(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new string[] { ex.Message });
            }
        }

        private static bool _isAdjusting = false;
        private static CancellationTokenSource? _debounceCancellationTokenSource;

        /// <summary>
        ///  Заполнение датагрид данными из БД
        /// </summary>
        public static void LoadDataGridFromDatabase(DataGrid grid, string sql)
        {
            try
            {
                DataSet ds = Setter.BridgeWorker.SQLQuery(sql);
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        // Очистка существующих колонок в DataGrid
                        grid.Columns.Clear();

                        // Создание колонок в DataGrid на основе столбцов DataTable
                        foreach (DataColumn column in dt.Columns)
                        {
                            var columnType = column.DataType;

                            // Проверка на Boolean
                            if (columnType == typeof(bool) || dt.AsEnumerable().All(row => row[column] is string str && (str == "True" || str == "False")))
                            {
                                var checkBoxColumn = new DataGridCheckBoxColumn
                                {
                                    Header = column.ColumnName,
                                    Binding = new Binding($"Row.ItemArray[{column.Ordinal}]")
                                };
                                grid.Columns.Add(checkBoxColumn);
                            }
                            else
                            {
                                // Создание текстовой колонки с обертыванием текста
                                var templateColumn = new DataGridTemplateColumn
                                {
                                    Header = column.ColumnName,
                                    CellTemplate = new FuncDataTemplate<object>((item, _) =>
                                    {
                                        var textBlock = new TextBlock
                                        {
                                            TextWrapping = TextWrapping.Wrap,
                                            Margin = new Thickness(12, 7, 12, 7)
                                        };
                                        textBlock.Bind(TextBlock.TextProperty, new Binding($"Row.ItemArray[{column.Ordinal}]"));
                                        return textBlock;
                                    })
                                };
                                grid.Columns.Add(templateColumn);
                            }
                        }

                        // Обновление данных в DataGrid
                        grid.DataContext = dt;
                        grid.ItemsSource = dt.DefaultView;

                        // Скрытие первого столбца, если это необходимо
                        if (grid.Columns.Count > 0)
                        {
                            var columnToHide = grid.Columns[0];
                            columnToHide.IsVisible = false;

                            if (MainWindowViewModel.TextWrapDataGrid)
                            {
                                grid.LayoutUpdated += (s, e) =>
                                {
                                    if (_isAdjusting) return;

                                    _isAdjusting = true;
                                    _debounceCancellationTokenSource?.Cancel(); // Отменяем предыдущую задачу
                                    _debounceCancellationTokenSource = new CancellationTokenSource();

                                    Task.Delay(100).ContinueWith(t =>
                                    {
                                        if (!_debounceCancellationTokenSource.Token.IsCancellationRequested)
                                        {
                                            Dispatcher.UIThread.Post(() =>
                                            {
                                                AdjustColumnWidths(grid);
                                            });
                                        }

                                        _isAdjusting = false;
                                    }, _debounceCancellationTokenSource.Token);
                                };

                            }
                        }

                        grid.SelectedIndex = 0;
                    });
                  

                   
                }
            }
            catch (Exception ex)
            {
                LogWorker.Log2File(Setter.ID, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace, new[] { ex.Message });
            }
        }
        // Метод для регулирования ширины колонок
        private static void AdjustColumnWidths(DataGrid grid)
        {
            if (grid == null || grid.Columns.Count == 0)
                return;

            // Общая доступная ширина для колонок
            double availableWidth = grid.Bounds.Width;

            if (availableWidth <= 0)
                return;

            // Общая ширина колонок
            totalContentWidth = 0;
            totalContentWidth = grid.Columns.Skip(1).Sum(col => col.ActualWidth);
            // Если ширина контента больше доступной ширины
            if (totalContentWidth > availableWidth)
            {
                double scaleFactor = availableWidth / totalContentWidth;

                for (int i = 1; i < grid.Columns.Count-1; i++)
                {
                    double newWidth = grid.Columns[i].ActualWidth * scaleFactor;
                    grid.Columns[i].Width = new DataGridLength(newWidth, DataGridLengthUnitType.Pixel);

                }
                // Уменьшаем шрифт заголовков
                double fontSize = Math.Max(12 * scaleFactor, 8); // Минимальный размер шрифта - 8

             
            }
        }

        private static double totalContentWidth;
       
    }
}
