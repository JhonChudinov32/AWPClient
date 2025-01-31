
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using AWPClient.ViewModels;
using System;
using System.Collections.Generic;


namespace AWPClient.Classes
{
    public static class Helper
    {
        /// <summary>
        ///  Методы по поиску дочерних элементов
        /// </summary>
        public static T FindVisualChild<T>(ILogical parent) where T : class, ILogical
        {
            var children = parent.LogicalChildren;
            foreach (var child in children)
            {
                if (child is T typedChild)
                {
                    return typedChild;
                }

                var result = FindVisualChild<T>(child);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public static T FindControl<T>(Control parent) where T : Control
        {
            if (parent is T result)
            {
                return result;
            }

            foreach (var child in parent.GetLogicalChildren())
            {
                if (child is Control control)
                {
                    T foundControl = FindControl<T>(control);
                    if (foundControl != null)
                    {
                        return foundControl;
                    }
                }
            }

            return null;
        }
        public static T FindLogicalAncestorOfType<T>(Control control) where T : class
        {
            var parent = control.Parent;
            while (parent != null)
            {
                if (parent is T)
                    return parent as T;
                parent = (parent as Control)?.Parent;
            }
            return null;
        }

        public static void UpdateFrameContent(ContentControl frame, string userControlTypeName)
        {
            if (frame != null)
            {
                Type userControlType = Type.GetType(userControlTypeName);
                if (userControlType != null)
                {
                    frame.Content = (UserControl)Activator.CreateInstance(userControlType);
                }
            }

        }
        /// <summary>
        ///  Методы по получению списка дочерних элементов
        /// </summary>
        public static List<Control> FindFrameworkElements(Control visual)
        {
            List<Control> list = new List<Control>();
            foreach (var child in visual.GetVisualChildren())
            {
                if (child is Control)
                {
                    list.Add(child as Control);
                }
                list.AddRange(FindFrameworkElements(child as Control));
            }
            return list;
        }

        /// <summary>
        /// Ищет FrameworkElement по имени в списке поддерживаемых элементов
        /// </summary>
        /// <param name="Name">ипя элемента</param>
        /// <returns>искомый FrameworkElement или null</returns>
        public static Control FindFrameworkElement(string Name)
        {
            Control fe = null;
            foreach (Control element  in MainWindowViewModel.ElementList)
            {
                if (element.Name == Name)
                {
                    fe = element;
                    break;
                }
            }
            return fe;
        }

        /// <summary>
        /// FindControls  получаем список контролов
        /// </summary>
        public static List<T> FindControls<T>(Control parent) where T : Control
        {
            List<T> foundControls = new List<T>();

            AddMatchingControls<T>(parent, foundControls);

            return foundControls;
        }

        private static void AddMatchingControls<T>(Control parent, List<T> foundControls) where T : Control
        {
            if (parent is T result)
            {
                foundControls.Add(result);
            }

            foreach (var child in parent.GetLogicalChildren())
            {
                if (child is Control control)
                {
                    AddMatchingControls<T>(control, foundControls);
                }
            }
        }

        /// <summary>
        /// Поиск конрола по имени элемента
        /// </summary>
        public static Control FindControlByName(string elementName, Control parent)
        {
            if (parent?.Name == elementName)
            {
                return parent;
            }

            foreach (var child in parent.GetLogicalChildren())
            {
                if (child is Control control)
                {
                    Control foundControl = FindControlByName(elementName, control);
                    if (foundControl != null)
                    {
                        return foundControl;
                    }
                }
            }
            return null;
        }

        public static T FindVisualChilds<T>(ILogical control) where T : class
        {
            if (control == null)
                return null;

            if (control is T found)
                return found;

            foreach (var child in control.GetLogicalChildren())
            {
                var result = FindVisualChilds<T>(child);
                if (result != null)
                    return result;
            }

            return null;
        }
    }

}
