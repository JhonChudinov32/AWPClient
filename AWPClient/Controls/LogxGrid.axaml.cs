using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Core;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.Animation;
using Avalonia.Styling;
using System.Collections.Generic;
using System;

namespace AWPClient.Controls
{
    public partial class LogxGrid : UserControl
    {
        public string Sql { get; set; }

        //public string ColWeights { get; set; }

        public bool IsRefreshing { get; set; }

        public List<KeyValuePair<string, double>> Columns { get; set; }

        public List<List<string>> Items { get; set; }

        public string ColumnWidth { get; set; }

        public bool IsAutoWidth { get; set; }

        public int CurrentItem { get; set; }

        public event EventHandler RefreshEvent;

        //Storyboard story = new Storyboard();
        //DoubleAnimation anim = new DoubleAnimation();
        //DispatcherTimer timer;
        ////DoubleAnimation anim = new DoubleAnimation();
        ////Storyboard story = new Storyboard();

        //private void InitializeAnimation()
        //{
        //    // Создаем анимируемое значение
        //    var animatableValue = new StyledProperty<double>(0);

        //    // Создаем анимацию
        //    var animation = new DoubleAnimation()
        //    {
        //        Duration = TimeSpan.FromSeconds(1), // Продолжительность анимации (1 секунда)
        //        FillBehavior = FillBehavior.Stop
        //    };

        //    // Устанавливаем начальное и конечное значения анимации
        //    animation.From = 0;
        //    animation.To = 30;

        //    // Привязываем анимацию к свойству элемента
        //    animatableValue.Animate(animation);

        //    // Указываем цель анимации (ProgressBar.ValueProperty)
        //    Storyboard.SetTarget(animation, this.RefreshProgress);
        //    Storyboard.SetTargetProperty(animation, new PropertyPath(nameof(ProgressBar.Value)));
        //}

        //public void StartRefreshAnimation()
        //{
        //    _story.Begin();
        //}



        //public LogxGrid()
        //{
        //    this.InitializeComponent();
        //    //arrow.RenderTransformOrigin = new Point(0.5, 0.5);
        //    Items = new List<List<string>>();
        //    Columns = new List<KeyValuePair<string, double>>();
        //    CurrentItem = -1;
        //    Focusable = true;
        //    RefreshEvent += new EventHandler(LogxGrid_RefreshEvent);
        //    anim.From = 0;
        //    anim.To = 30;
        //    anim.By = 1;
        //    anim.FillBehavior = FillBehavior.Stop;
        //    anim.Duration = TimeSpan.FromSeconds(30);
        //    anim.Completed += new EventHandler(anim_Completed);
        //    Storyboard.SetTargetName(anim, RefreshProgress.Name);
        //    Storyboard.SetTargetProperty(anim, new PropertyPath(ProgressBar.ValueProperty));
        //    story.Children.Add(anim);
        //}
        //public void StartRefreshTimer()
        //{
        //    try
        //    {
        //        if (timer == null)
        //        {
        //            timer = new DispatcherTimer();
        //            timer.Tick += new EventHandler(timer_Tick);  //+= new EventHandler(TimerAlive);
        //            timer.Interval = new TimeSpan(0, 0, 30);
        //        }
        //        else
        //        {
        //            timer.Stop();// Change(Timeout.Infinite, Timeout.Infinite);
        //        }
        //        //story.Begin(RefreshProgress, true);
        //        anim.BeginTime = null;
        //        RefreshProgress.BeginAnimation(ProgressBar.ValueProperty, anim);

        //        anim.BeginTime = new TimeSpan(0, 0, 0);
        //        RefreshProgress.BeginAnimation(ProgressBar.ValueProperty, anim);
        //        timer.Start();
        //    }
        //    catch { }
        //}

        //void timer_Tick(object sender, EventArgs e)
        //{
        //    if (IsRefreshing)
        //    {
        //        RefreshEvent(this, null);
        //        //Dispatcher.BeginInvoke(new Action(delegate()
        //        // {
        //        StartRefreshTimer();
        //        //}));
        //    }
        //}

        //void anim_Completed(object sender, EventArgs e)
        //{
        //    //arrow.BeginAnimation(Canvas.RenderTransformProperty,);
        //    //Timeline.
        //    //arrow.RenderTransform = new RotateTransform(360);


        //}

        //void LogxGrid_RefreshEvent(object sender, EventArgs e)
        //{
        //    //RefreshProgress.Value = 0;
        //}

        //public void addItem(List<string> item)
        //{
        //    try
        //    {
        //        if (item != null && Columns != null && item.Count == Columns.Count)
        //        {
        //            Items.Add(item);
        //        }
        //    }
        //    catch { }
        //}

        //public void SetColumns(string[] columns)
        //{
        //    try
        //    {
        //        Items.Clear();
        //        Columns.Clear();

        //        CurrentItem = -1;
        //        if (columns.Length != 0)
        //        {
        //            string[] ss = ColumnWidth.Split(',');
        //            if (ss.Length != columns.Length || ColumnWidth.Trim().Length == 0 || ColumnWidth.Trim().ToLower() == "auto")
        //            {
        //                IsAutoWidth = true;
        //            }

        //            for (int i = 0; i < columns.Length; i++)
        //            {
        //                double d = double.NaN;

        //                if (!IsAutoWidth)
        //                {
        //                    double.TryParse(ss[i], out d);
        //                }

        //                KeyValuePair<string, double> kvp = new KeyValuePair<string, double>(columns[i], d);
        //                Columns.Add(kvp);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //todo exception
        //    }
        //}


        //public void Re()
        //{
        //    try
        //    {
        //        ItemStack.Children.Clear();

        //        //double d = (Columns!=null)? ((Columns.Count!=0)?(Columns[0]):double.NaN):double.NaN; // (double)1 / Columns.Count;
        //        for (int i = 0; i < Items.Count; i++)
        //        {
        //            Grid mgr = new Grid() { Background = Brushes.White, Focusable = true };
        //            mgr.HorizontalAlignment = HorizontalAlignment.Stretch;
        //            mgr.VerticalAlignment = VerticalAlignment.Stretch;
        //            mgr.ShowGridLines = true;
        //            mgr.Tag = i.ToString();
        //            //mgr.Background = null;
        //            for (int x = 0; x < Items[i].Count; x++)
        //            {
        //                ColumnDefinition colDef = new ColumnDefinition();

        //                if (IsAutoWidth)
        //                {
        //                    colDef.Width = new GridLength((double)1 / Columns.Count, GridUnitType.Star);
        //                }
        //                else
        //                {
        //                    colDef.Width = new GridLength(Columns[x].Value, GridUnitType.Pixel);
        //                }

        //                mgr.ColumnDefinitions.Add(colDef);

        //                mgr.MouseUp += new MouseButtonEventHandler(mgr_MouseUp);

        //                //colDef.Width = new GridLength(w, GridUnitType.Pixel);
        //                Label lbl = new Label()
        //                {
        //                    Focusable = true,
        //                    Content = Items[i][x]
        //                };
        //                //lbl.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        //                //lbl.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 100, 50, 50));
        //                //lbl.BorderThickness = new Thickness(1);
        //                lbl.Visibility = Visibility.Visible;
        //                mgr.Children.Add(lbl);
        //                Grid.SetColumn(lbl, x);
        //                Grid.SetRow(lbl, 0);
        //            }
        //            mgr.Visibility = Visibility.Visible;
        //            ItemStack.Children.Add(mgr);
        //        }

        //        Grid gr = new Grid() { };
        //        gr.HorizontalAlignment = HorizontalAlignment.Stretch;
        //        gr.VerticalAlignment = VerticalAlignment.Stretch;
        //        //gr.Background = new SolidColorBrush(Color.FromArgb(255, 50, 220, 100));
        //        gr.ShowGridLines = true;
        //        //int w = (int)(gr. / Columns.Count);


        //        for (int i = 0; i < Columns.Count; i++)
        //        {
        //            ColumnDefinition colDef = new ColumnDefinition();

        //            if (IsAutoWidth)
        //            {
        //                colDef.Width = new GridLength((double)1 / Columns.Count, GridUnitType.Star);
        //            }
        //            else
        //            {
        //                colDef.Width = new GridLength(Columns[i].Value, GridUnitType.Pixel);
        //            }

        //            gr.ColumnDefinitions.Add(colDef);
        //            //colDef.Width = new GridLength(w, GridUnitType.Pixel);
        //            Label label = new Label() { Content = Columns[i].Key };
        //            gr.Children.Add(label);
        //            Grid.SetColumn(label, i);
        //            Grid.SetRow(label, 0);
        //        }

        //        HeaderGrid.Children.Add(gr);

        //        DrawCurrentItem();

        //    }
        //    catch { }
        //}

        //void mgr_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        int tag = Convert.ToInt32((string)(sender as Grid).Tag);
        //        if (CurrentItem != tag)
        //        {
        //            CurrentItem = tag;
        //            DrawCurrentItem();
        //        }
        //    }
        //    catch { }
        //}

        //public void LogxGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    switch (e.Key)
        //    {
        //        case Key.Down:

        //            e.Handled = true;
        //            if (CurrentItem < Items.Count - 1)
        //            {
        //                CurrentItem += 1;
        //                DrawCurrentItem();
        //            }
        //            break;
        //        case Key.Up:

        //            e.Handled = true;
        //            if (CurrentItem != -1)
        //            {
        //                CurrentItem -= 1;
        //                DrawCurrentItem();
        //            }
        //            break;
        //    }
        //}

        //private void DrawCurrentItem()
        //{
        //    if (CurrentItem == -1)
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        if (CurrentItem < ItemStack.Children.Count)
        //        {
        //            Visual visual = (Visual)VisualTreeHelper.GetChild(ItemStack, CurrentItem);

        //            if (visual != null && visual is Grid)
        //            {
        //                Grid grid = (visual as Grid);

        //                if (grid.Tag != null && Convert.ToInt32(grid.Tag) == CurrentItem)
        //                {
        //                    grid.Background = Brushes.MediumSlateBlue;
        //                    grid.Focus();
        //                }

        //                if (CurrentItem + 1 < ItemStack.Children.Count)
        //                {
        //                    Visual visualback = (Visual)VisualTreeHelper.GetChild(ItemStack, CurrentItem + 1);
        //                    if (visualback != null && visualback is Grid)
        //                    {
        //                        Grid gridback = (visualback as Grid);

        //                        if (gridback.Tag != null && Convert.ToInt32(gridback.Tag) == CurrentItem + 1)
        //                        {
        //                            gridback.Background = Brushes.White;
        //                        }
        //                    }
        //                }

        //                if (CurrentItem > 0)
        //                {
        //                    Visual visualup = (Visual)VisualTreeHelper.GetChild(ItemStack, CurrentItem - 1);
        //                    if (visualup != null && visualup is Grid)
        //                    {
        //                        Grid gridup = (visualup as Grid);

        //                        if (gridup.Tag != null && Convert.ToInt32(gridup.Tag) == CurrentItem - 1)
        //                        {
        //                            gridup.Background = Brushes.White;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    catch { }
        //}
        //public string GetCurrentKey()
        //{
        //    if (VisualTreeHelper.GetChildrenCount(ItemStack) > 0)
        //    {
        //        Visual childVisual = (Visual)VisualTreeHelper.GetChild(ItemStack, CurrentItem);

        //        if (childVisual != null && childVisual is Grid)
        //        {
        //            Grid grid = (childVisual as Grid);
        //            if (VisualTreeHelper.GetChildrenCount(ItemStack) > 0)
        //            {
        //                Visual childLabel = (Visual)VisualTreeHelper.GetChild(grid, 0);
        //                if (childLabel != null && childLabel is Label)
        //                {
        //                    return Convert.ToString((childLabel as Label).Content);
        //                }
        //            }
        //        }
        //    }
        //    return string.Empty;
        //}

        //private void LogxGridControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    this.Focus();
        //}

        //private void RefreshProgress_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    IsRefreshing = !IsRefreshing;
        //    if (IsRefreshing)
        //    {
        //        StartRefreshTimer();
        //    }
        //    else
        //    {
        //        if (timer != null && timer.IsEnabled)
        //        {
        //            timer.Stop();
        //        }

        //        anim.BeginTime = null;
        //        RefreshProgress.BeginAnimation(ProgressBar.ValueProperty, anim);

        //        //story.Stop(RefreshProgress);
        //    }
        //}
        //public void Stop()
        //{
        //    if (timer != null && timer.IsEnabled)
        //    {
        //        timer.Stop();
        //    }
        //}
    }
}
