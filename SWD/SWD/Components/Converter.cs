using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SWD.Components
{
    /// <summary>
    /// A WPF value converter that capitalizes the first letter of a string for display,
    /// and converts it to lowercase for storage in the model.
    /// </summary>
    /// <remarks>
    /// Example: "button" → "Button" (display), "Button" → "button" (model).
    /// </remarks>
    public class CapitalizeConverter : IValueConverter
    {
        /// <summary>
        /// If true, always converts to lowercase (not used in current logic).
        /// </summary>
        public bool ToLower { get; set; } = false;

        /// <summary>
        /// Converts a string to have its first character capitalized.
        /// </summary>
        /// <param name="value">The value produced by the binding source (expected string).</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// The input string with the first character capitalized, or the original value if not a string.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && s.Length > 0)
            {
                // lowercase "button" => "Button" for display
                return char.ToUpper(s[0]) + s.Substring(1);
            }
            return value;
        }

        /// <summary>
        /// Converts a string to all lowercase.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target (expected string).</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// The input string in all lowercase, or the original value if not a string.
        /// </returns>
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
