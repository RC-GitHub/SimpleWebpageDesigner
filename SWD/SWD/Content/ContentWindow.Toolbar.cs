using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace SWD.Content
{
    /// <summary>
    /// Partial class for ContentWindow, containing logic for toolbar and menu actions,
    /// such as opening projects, files, folders, and handling build and close operations.
    /// </summary>
    public partial class ContentWindow : Window
    {
        private bool menuOpeningInProgress = false;

        /// <summary>
        /// Opens the context menu for a toolbar button, ensuring only one menu opens at a time.
        /// </summary>
        /// <param name="sender">The button that triggered the event.</param>
        /// <param name="e">Routed event arguments.</param>
        private void OpenContextMenu(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ContextMenu contextMenu = button.ContextMenu;
            if (menuOpeningInProgress || contextMenu == null || contextMenu.IsOpen)
                return;

            menuOpeningInProgress = true;

            contextMenu.PlacementTarget = button;
            contextMenu.Placement = PlacementMode.Bottom;
            contextMenu.IsOpen = true;

            // Reset after small delay
            Dispatcher.InvokeAsync(() => menuOpeningInProgress = false, DispatcherPriority.Background);
        }

        /// <summary>
        /// Handles the menu item click to create a new project.
        /// </summary>
        /// <param name="sender">The menu item.</param>
        /// <param name="e">Routed event arguments.</param>
        private void miNewProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.NewProject();
        }

        /// <summary>
        /// Handles the menu item click to open an existing project.
        /// </summary>
        /// <param name="sender">The menu item.</param>
        /// <param name="e">Routed event arguments.</param>
        private void miOpenProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenProject();
        }

        /// <summary>
        /// Handles the menu item click to open a JSON file, copying it to the current directory if needed.
        /// </summary>
        /// <param name="sender">The menu item.</param>
        /// <param name="e">Routed event arguments.</param>
        private void miOpenFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFile(true);
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                Title = "Select a JSON File",
                IsFolderPicker = false,
                Filters = { new CommonFileDialogFilter("JSON Files", "*.json") }
            };
            //dialog.IsFolderPicker = true;

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

            try
            {
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string filePath = dialog.FileName;
                    string openName = Path.GetFileName(filePath);
                    string extension = Path.GetExtension(openName);

                    string currentDir = Path.GetDirectoryName(currentFilePath);
                    string openedNewDir = Path.Combine(currentDir, openName);

                    if (extension == ".json")
                    {
                        if (File.Exists(openedNewDir))
                        {
                            Errors.DisplayMessage("File of the same name already exists in the current directory!");
                        }
                        else
                        {
                            File.Copy(filePath, openedNewDir);
                            LoadJsonPage(openedNewDir);
                        }
                    }
                    else
                    {
                        Errors.DisplayMessage("Selected file is not of the JSON format!");
                    }

                }
            }
            catch (Exception ex)
            {
                Errors.DisplayMessage(ex.Message);
            }
        }

        /// <summary>
        /// Handles the menu item click to open a folder, copying it to the current directory if needed.
        /// </summary>
        /// <param name="sender">The menu item.</param>
        /// <param name="e">Routed event arguments.</param>
        private void miOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            SaveFile(true);
            string dir;
            string destinationDir = Path.GetDirectoryName(currentFilePath);
            MessageBoxResult result = Infos.DisplayPathMessage();
            if (result == MessageBoxResult.Cancel)
            {
                return;
            }
            else if (result == MessageBoxResult.No)
            {
                dir = Path.Combine(Environment.CurrentDirectory, "Projects");
            }
            else
            {
                dir = "C:\\Users";
            }

            using (var folderDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderDialog.Description = "Select a folder";
                folderDialog.SelectedPath = dir;

                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    string selectedName = Path.GetFileName(selectedPath);
                    string destinationPath = Path.Combine(destinationDir, selectedName);

                    try
                    {
                        if (!Directory.Exists(destinationPath))
                        {
                            DirectoryCopy(selectedPath, destinationPath, true);
                            RefreshFileData();
                            ControlFile(currentFilePath);
                        }
                        else
                        {
                            // POSSIBLE FUNCTIONALITY TO ADD: MERGING CONTENTS
                            Errors.DisplayMessage("Folder of the same name already exists in the current directory!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Errors.DisplayMessage($"Failed to move folder:\n{ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Recursively copies a directory and its contents to a new location.
        /// </summary>
        /// <param name="sourceDir">The source directory path.</param>
        /// <param name="destDir">The destination directory path.</param>
        /// <param name="copySubDirs">Whether to copy subdirectories.</param>
        public static void DirectoryCopy(string sourceDir, string destDir, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
                throw new DirectoryNotFoundException("Source directory does not exist: " + sourceDir);

            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDir, file.Name);
                file.CopyTo(tempPath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDir, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, true);
                }
            }
        }

        /// <summary>
        /// Handles the menu item click to edit the initial project metadata.
        /// </summary>
        /// <param name="sender">The menu item.</param>
        /// <param name="e">Routed event arguments.</param>
        private void miEditInitial_Click(object sender, RoutedEventArgs e)
        {
            SaveFile(true);
            Debug.WriteLine(path);
            string metadataDir = Path.Combine(path, "metadata.json");
            if (File.Exists(metadataDir))
            {
                try
                {
                    string json = File.ReadAllText(metadataDir);
                    List<Head> data = JsonConvert.DeserializeObject<List<Head>>(json);
                    Head head = data[0];
                    CreationWindow cw = new CreationWindow(path, head, this);
                    cw.Show();
                }
                catch (Exception ex) { Errors.DisplayMessage(ex.Message); }
            }

        }

        /// <summary>
        /// Handles the click event to close the ContentWindow.
        /// </summary>
        /// <param name="sender">The button that triggered the event.</param>
        /// <param name="e">Routed event arguments.</param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the click event to build the project by creating necessary directories.
        /// </summary>
        /// <param name="sender">The button that triggered the event.</param>
        /// <param name="e">Routed event arguments.</param>
        private void btnBuild_Click(object sender, RoutedEventArgs e)
        {
            Build.CreateDirectories(path);
        }
    }
}
