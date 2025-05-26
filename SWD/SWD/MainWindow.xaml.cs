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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Window = System.Windows.Window;
using Application = System.Windows.Application;

namespace SWD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes the main window, sets up theme data binding.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
            this.DataContext = App.themeData.CurrentTheme;
        }

        /// <summary>
        /// Handles theme changes and updates the window's data context.
        /// </summary>
        private void ThemeData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Always update the DataContext to reflect the current theme.
            this.DataContext = App.themeData.CurrentTheme;
        }

        /// <summary>
        /// Handles the "New Project" button click event.
        /// </summary>
        private void btnNewProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.NewProject(this);
        }

        /// <summary>
        /// Static method to start the new project creation workflow.
        /// </summary>
        /// <param name="mw">Reference to the current MainWindow (optional).</param>
        static public void NewProject(MainWindow mw = null)
        {
            MessageBoxResult result = Infos.DisplayPathMessage(false);
            if (result == MessageBoxResult.Cancel)
            {
                Debug.WriteLine(result);
            }
            else if (result == MessageBoxResult.No)
            {
                Debug.WriteLine(Environment.CurrentDirectory);

                // Open CreationWindow for new project details.
                CreationWindow fillTheData = new CreationWindow(mw);
                fillTheData.ShowDialog();
            }
            else
            {
                Debug.WriteLine(result);
                // Let user pick a folder for the new project.
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.InitialDirectory = "C:\\Users";
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string filePath = dialog.FileName;
                    CreationWindow fillTheData = new CreationWindow(filePath, mw);
                    fillTheData.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Handles the "Open Existing Project" button click event.
        /// </summary>
        private void btnOpenExisting_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenProject(this);
        }

        /// <summary>
        /// Static method to open an existing project, validating its structure.
        /// </summary>
        /// <param name="mw">Reference to the current MainWindow (optional).</param>
        static public void OpenProject(MainWindow mw = null)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            MessageBoxResult result = Infos.DisplayPathMessage();
            if (result == MessageBoxResult.Cancel)
            {
                return;
            }
            else if (result == MessageBoxResult.No)
            {
                string dir = Environment.CurrentDirectory;
                dir = Path.Combine(dir, "Projects");
                dialog.InitialDirectory = dir;
            }
            else
            {
                dialog.InitialDirectory = "C:\\Users";
            }

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string filePath = dialog.FileName;
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                Debug.WriteLine(filePath);

                // Only allow folders following the SWD- naming convention.
                if (fileName.StartsWith("SWD-"))
                {
                    Content.ContentWindow fillTheData = new Content.ContentWindow(filePath, mw);
                    fillTheData.Show();
                }
                else
                {
                    Errors.DisplayMessage("The project does not adhere to the naming convention (SWD-[Project name])");
                }
            }
        }

        /// <summary>
        /// Handles the "Delete Existing Project" button click event.
        /// </summary>
        private void btnDeleteExisting_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = Infos.DisplayPathMessage(true);
            if (result == MessageBoxResult.Cancel)
            {
                Debug.WriteLine(result);
            }
            else if (result == MessageBoxResult.Yes)
            {
                // Default to Projects folder in current directory.
                string dir = Environment.CurrentDirectory;
                dir = System.IO.Path.Combine(dir, "Projects");
                DeleteDirectory(dir);
            }
            else
            {
                // Default to C:\Users.
                string dir = "C:\\Users";
                DeleteDirectory(dir);
            }
        }

        /// <summary>
        /// Prompts the user to select directories and deletes valid SWD projects.
        /// </summary>
        /// <param name="path">Initial directory for the dialog.</param>
        public void DeleteDirectory(string path)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = path;
            dialog.IsFolderPicker = true;
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string errors = "";
                foreach (string filePath in dialog.FileNames)
                {
                    string fileName = Path.GetFileName(filePath);
                    string swdCode = fileName.Substring(0, 4);

                    // Only delete directories that start with "SWD-"
                    if (Directory.Exists(filePath) && swdCode == "SWD-")
                    {
                        try
                        {
                            Directory.Delete(filePath, true);
                            Debug.WriteLine($"Successfully deleted: {fileName}");
                        }
                        catch (Exception ex)
                        {
                            errors += $"Error deleting {fileName}\n";
                            Debug.WriteLine($"Error deleting {fileName}: {ex.Message}");
                        }
                    }
                    else if (Directory.Exists(filePath) && swdCode != "SWD-")
                    {
                        errors += $"Selected item is not a valid SWD Project: {fileName}\n";
                        Debug.WriteLine($"Selected item is not a valid SWD Project: {fileName}");
                    }
                    else
                    {
                        errors += $"Selected item is not a directory: {fileName}\n";
                        Debug.WriteLine($"Selected item is not a directory: {fileName}");
                    }
                }
                if (errors == "")
                    Infos.DisplayMessage("Project(s) deleted successfully.");
                else
                    Errors.DisplayMessage(errors);
            }
        }

        /// <summary>
        /// Handles the "Theme Settings" button click event.
        /// </summary>
        private void btnThemeSettings_Click(object sender, RoutedEventArgs e)
        {
            ThemeWindow themeWindow = new ThemeWindow();
            themeWindow.ShowDialog();
        }

        /// <summary>
        /// Closes the main window.
        /// </summary>
        public void CloseMainWindow()
        {
            this.Close();
        }
    }
}
