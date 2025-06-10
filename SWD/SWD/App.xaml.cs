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
    /// Handles application-wide resources, theme data, and startup logic.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Global theme data for the application, loaded at startup.
        /// Used for theming and style management across all windows.
        /// </summary>
        public static ThemeData themeData = ThemeData.LoadData();

        /// <summary>
        /// Handles application startup logic, including global exception handling and
        /// ensuring the projects directory exists in the user's local application data folder.
        /// </summary>
        /// <param name="e">Startup event arguments.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Exception ex = (Exception)args.ExceptionObject;
                MessageBox.Show($"Unhandled exception: {ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            DispatcherUnhandledException += (sender, args) =>
            {
                MessageBox.Show($"Dispatcher exception: {args.Exception.Message}\n\n{args.Exception.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Handled = true; // Prevent app from crashing, if you want
            };

            base.OnStartup(e);

            string projectsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Projects"
            );
            Directory.CreateDirectory(projectsPath);
        }
    }
}
