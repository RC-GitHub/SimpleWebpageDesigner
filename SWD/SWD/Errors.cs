using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
}
