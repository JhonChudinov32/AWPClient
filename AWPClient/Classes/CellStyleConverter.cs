using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWPClient.Classes
{

    public class CellStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string cellValue = value.ToString();
                if (cellValue.Equals("red", StringComparison.OrdinalIgnoreCase))
                {
                    return Brushes.Red;
                }
                else if (cellValue.Equals("yellow", StringComparison.OrdinalIgnoreCase))
                {
                    return Brushes.Yellow;
                }
            }
            // Return default color if value is not "red" or "yellow"
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
