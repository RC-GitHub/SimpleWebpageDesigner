using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using MessageBox = System.Windows.MessageBox;

namespace SWD
{
    internal class Errors
    {
        public Errors() {}
        static public MessageBoxResult DisplayErrorMessage(string msgBoxText)
        {
            string caption = "Error!";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            MessageBoxResult result;

            result = MessageBox.Show(msgBoxText, caption, button, icon, MessageBoxResult.Yes);
            return result;
        }
    }

    internal class Infos
    {
        public Infos() { }
        static public MessageBoxResult DisplayErrorMessage(string msgBoxText)
        {
            string caption = "Result";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult result;

            result = MessageBox.Show(msgBoxText, caption, button, icon, MessageBoxResult.Yes);
            return result;
        }
    }

    internal class Colors
    {
        public Colors() {}
        static public SolidColorBrush ShowColorPicker()
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowHelp = true;

            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Color selectedColor = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                SolidColorBrush brush = new SolidColorBrush(selectedColor);
                return brush;
            }
            return null;
        }

        static public SolidColorBrush CreateASelectedColor(SolidColorBrush brush1, SolidColorBrush brush2, double ratio)
        {
            Color color1 = brush1.Color;
            Color color2 = brush2.Color;
            ratio = Math.Max(0, Math.Min(1, ratio));

            byte r = (byte)(color1.R * (1 - ratio) + color2.R * ratio);
            byte g = (byte)(color1.G * (1 - ratio) + color2.G * ratio);
            byte b = (byte)(color1.B * (1 - ratio) + color2.B * ratio);
            byte a = (byte)(color1.A * (1 - ratio) + color2.A * ratio);

            Color color = (Color.FromArgb(a, r, g, b));
            Debug.WriteLine(color.ToString());

            return new SolidColorBrush(color);
        }
    }
}
