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

        private void btnNewProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = DisplayNewProjectMessage();
            if (result == MessageBoxResult.Cancel)
            {               
                Debug.WriteLine(result);
            }
            else if (result == MessageBoxResult.No) {
                Debug.WriteLine(Environment.CurrentDirectory);

                CreationWindow fillTheData = new CreationWindow();
                fillTheData.ShowDialog();


            } else
            {
                Debug.WriteLine(result);
            }

        }
    }
}
