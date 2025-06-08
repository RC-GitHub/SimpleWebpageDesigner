using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SWD
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// The application entry point for the SWD WPF application.
    /// Handles application-wide resources and startup logic.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Global theme data for the application, loaded at startup.
        /// Used for theming and style management across all windows.
        /// </summary>
        public static ThemeData themeData = ThemeData.LoadData();
    }
}
