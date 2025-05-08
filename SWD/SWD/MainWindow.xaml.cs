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
using Microsoft.Win32;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using Path = System.IO.Path;

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

        public MessageBoxResult DisplayPathMessage(bool action)
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

        private void btnNewProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = DisplayPathMessage(false);
            if (result == MessageBoxResult.Cancel)
            {               
                Debug.WriteLine(result);
            } 
            else if (result == MessageBoxResult.No) {
                Debug.WriteLine(Environment.CurrentDirectory);

                CreationWindow fillTheData = new CreationWindow(this);
                fillTheData.ShowDialog();

            } 
            else
            {
                Debug.WriteLine(result);
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.InitialDirectory = "C:\\Users";
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string filePath = dialog.FileName;
                    CreationWindow fillTheData = new CreationWindow(filePath, this);
                    fillTheData.ShowDialog();
                }

            }
        }

        private void btnOpenExisting_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {

                string filePath = dialog.FileName;
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                Debug.WriteLine(filePath);

                if (fileName.StartsWith("SWD-"))
                {
                    Content.ContentWindow fillTheData = new Content.ContentWindow(filePath, this);
                    fillTheData.ShowDialog();
                }
                else
                {
                    Errors.DisplayErrorMessage("The project does not adhere to the naming convention (SWD-[Project name])");
                }

            }
        }

        private void btnDeleteExisting_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = DisplayPathMessage(true);
            if (result == MessageBoxResult.Cancel)
            {
                Debug.WriteLine(result);
            }
            else if (result == MessageBoxResult.Yes)
            {

                string dir = Environment.CurrentDirectory;
                DeleteDirectory(dir);

            }
            else
            {
                string dir = "C:\\Users";
                DeleteDirectory(dir);

            }
        }

        public void DeleteDirectory(string path)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = path; 
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string filePath = dialog.FileName;
                Debug.WriteLine(filePath);
                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                } else
                {
                    Debug.WriteLine("Something went wrong!");
                }
            }
        }

        public void CloseMainWindow()
        {
            this.Close();
        }
    }
}
