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
    /// <summary>
    /// Provides static methods for displaying error dialogs.
    /// </summary>
    internal class Errors
    {
        public Errors() { }

        /// <summary>
        /// Displays an error message dialog with the specified text.
        /// </summary>
        /// <param name="msgBoxText">The message to display.</param>
        /// <returns>The result of the message box.</returns>
        public static MessageBoxResult DisplayMessage(string msgBoxText)
        {
            string caption = "Error!";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            MessageBoxResult result;

            result = MessageBox.Show(msgBoxText, caption, button, icon, MessageBoxResult.Yes);
            return result;
        }
    }

    /// <summary>
    /// Provides static methods for displaying informational dialogs.
    /// </summary>
    internal class Infos
    {
        public Infos() { }

        /// <summary>
        /// Displays an informational message dialog with the specified text.
        /// </summary>
        /// <param name="msgBoxText">The message to display.</param>
        /// <returns>The result of the message box.</returns>
        public static MessageBoxResult DisplayMessage(string msgBoxText)
        {
            string caption = "Result";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult result;

            result = MessageBox.Show(msgBoxText, caption, button, icon, MessageBoxResult.Yes);
            return result;
        }

        /// <summary>
        /// Displays a dialog asking the user to select or confirm a path.
        /// </summary>
        /// <param name="action">If true, asks about deleting a project; otherwise, asks about choosing a path.</param>
        /// <returns>The result of the message box.</returns>
        public static MessageBoxResult DisplayPathMessage(bool action = false)
        {
            string messageBoxText;
            if (action == false) messageBoxText = "Do you want to choose your own path?";
            else messageBoxText = "Is the path to a project you want to delete in the default directory?";

            string caption = "Select a path";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            return result;
        }
    }

    /// <summary>
    /// Provides static methods for color selection and manipulation dialogs.
    /// </summary>
    internal class Colors
    {
        public Colors() { }

        /// <summary>
        /// Opens a color picker dialog and returns the selected color as a SolidColorBrush.
        /// </summary>
        /// <returns>The selected SolidColorBrush, or null if canceled.</returns>
        public static SolidColorBrush BrushColorPicker()
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

        /// <summary>
        /// Opens a color picker dialog and returns the selected color as a string.
        /// </summary>
        /// <returns>The selected color as a string, or null if canceled.</returns>
        public static string StringColorPicker()
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowHelp = true;

            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Color selectedColor = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                string brush = selectedColor.ToString();
                return brush;
            }
            return null;
        }

        /// <summary>
        /// Creates a new SolidColorBrush by blending two brushes with a given ratio.
        /// </summary>
        /// <param name="brush1">The first brush.</param>
        /// <param name="brush2">The second brush.</param>
        /// <param name="ratio">The blend ratio (0 to 1).</param>
        /// <returns>The blended SolidColorBrush.</returns>
        public static SolidColorBrush CreateASelectedColor(SolidColorBrush brush1, SolidColorBrush brush2, double ratio)
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
