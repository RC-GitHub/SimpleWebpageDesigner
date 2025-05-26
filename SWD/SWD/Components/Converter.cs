using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SWD.Components
{
    public class CapitalizeConverter : IValueConverter
    {
        public bool ToLower { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && s.Length > 0)
            {
                // lowercase "button" => "Button" for display
                return char.ToUpper(s[0]) + s.Substring(1);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && s.Length > 0)
            {
                // "Button" => "button" for model
                return s.ToLower();
            }
            return value;
        }
    }

}
