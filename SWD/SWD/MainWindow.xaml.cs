using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;

namespace SWD
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MessageBoxResult DisplayNewProjectMessage()
        {
            string messageBoxText = "Do you want to choose your own path?";
            string caption = "Select a path";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            return result;
        }

        public void CreateAProject(string projectName, string path)
        {
            path = path + "\\" + projectName;
            try
            {
                // Sprawdzam, czy ścieżka istnieje
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path exists already.");
                    return;
                }

                // Tworzę folder z projektem.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine($"The directory was created successfully at {path}.", Directory.GetCreationTime(path));
            }
            catch (Exception e)
            {
                Console.WriteLine($"The process failed: {path}", e.ToString());
            }
            finally { }
        }

        private void btnNewProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = DisplayNewProjectMessage();
            if (result == MessageBoxResult.Cancel)
            {               
                Debug.WriteLine(result);
            }
            else if (result == MessageBoxResult.No) {
                var rootPath = Environment.CurrentDirectory;

                string projectName = "SWD-Hello";

                CreateAProject(projectName, rootPath);

                Debug.WriteLine(rootPath);
            } else
            {
                Debug.WriteLine(result);
            }
        }
    }
}
