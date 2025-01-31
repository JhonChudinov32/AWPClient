using Avalonia.Controls;
using Avalonia.LogicalTree;
using System.Linq;

namespace AWPClient.Classes
{
    public static class MyExtensions
    {
        public static Control FindTag(this Control control, object tag)
        {
            var elements = control.GetLogicalChildren().OfType<Control>().ToArray();
            foreach (var element in elements)
            {
                if (tag.Equals(element.Tag))
                    return element;

                var foundElement = element.FindTag(tag);
                if (foundElement != null)
                    return foundElement;
            }

            return null;
        }
        public static Control[] FindButtons(this ContentControl contentControl)
        {
            var elements = contentControl.GetLogicalChildren().OfType<Control>().ToArray();
            return elements.Where(element => element is Button).ToArray();
        }
    }
}
