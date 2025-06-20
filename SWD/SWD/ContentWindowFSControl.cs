﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace SWD
{
    /// <summary>
    /// Partial class for ContentWindow, containing logic for file and folder management
    /// in the file selector panel, including renaming, moving, deleting, and refreshing.
    /// </summary>
    public partial class ContentWindow : Window
    {
        string[] currentProjectDir;
        string currentFilePath = string.Empty;

        /// <summary>
        /// Sets up the file and folder context for the current JSON file,
        /// updates UI fields and folder list.
        /// </summary>
        /// <param name="jsonPath">The path to the JSON file.</param>
        public void ControlFile(string jsonPath)
        {
            currentFilePath = jsonPath;

            string filename = Path.GetFileNameWithoutExtension(jsonPath);
            pageName = filename;
            tbFileNameChange.Text = pageName;

            string projectStructure = jsonPath.Split(new string[] { path }, StringSplitOptions.None)[1];
            string projectWithoutName = Path.GetDirectoryName(projectStructure)?.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string[] folders = projectWithoutName.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            string currentDirectory = Path.GetDirectoryName(currentFilePath);
            string[] insideFolders = Directory.GetDirectories(currentDirectory, "*", SearchOption.TopDirectoryOnly).Select(dir => Path.GetFileName(dir))
                                .ToArray();

            currentProjectDir = folders;
            lbFS.ItemsSource = insideFolders;
            tbFolderNameChange.Text = folders[folders.Length - 1];
            lblFS.Content = $"Folders inside: {folders[folders.Length - 1]}";
        }

        /// <summary>
        /// Refreshes the folder list in the file selector panel.
        /// </summary>
        private void btnFolderRefresh_Click(object sender, RoutedEventArgs e)
        {
            string currentDirectory = Path.GetDirectoryName(currentFilePath);
            string[] insideFolders = Directory.GetDirectories(currentDirectory, "*", SearchOption.TopDirectoryOnly).Select(dir => Path.GetFileName(dir))
                                .ToArray();
            lbFS.ItemsSource = insideFolders;
        }

        /// <summary>
        /// Handles selection change in the folder list, updates the folder name textbox.
        /// </summary>
        private void LbFS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbFolderNameChange.Focus();
            tbFolderNameChange.Text = lbFS.SelectedItem.ToString();
            tbFolderNameChange.CaretIndex = tbFileNameChange.Text.Length - 1;
        }

        /// <summary>
        /// Deletes the current file and loads the next available file if present.
        /// </summary>
        private void btnFileDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.Delete(currentFilePath);
            }
            catch (Exception ex)
            {
                Errors.DisplayMessage(ex.Message);
                return;
            }

            RefreshFileData();
            string jsonPath = MakeJsonPath(path);
            string indexPath = Path.Combine(jsonPath, "index.json");
            if (dgPages.Items.Count > 0)
            {
                var row = (DataRowView)dgPages.Items[0];
                readyPath = row["Path"].ToString();
                pageName = row["Filename"].ToString();
                try
                {
                    LoadJsonPage(readyPath);
                }
                catch (Exception ex)
                {
                    Errors.DisplayMessage(ex.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// Saves the file and applies any folder or file renaming as needed.
        /// </summary>
        private void btnFileSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFile(true);
            var folders = currentProjectDir;
            if (folders != null)
            {
                string oldDirectory = Path.GetDirectoryName(currentFilePath);
                string currentFolderName = Path.GetFileName(oldDirectory);
                string newFolderName = tbFolderNameChange.Text;
                string newFileName = tbFileNameChange.Text;

                string newFolderPath = oldDirectory;
                bool folderChanged = newFolderName != currentFolderName;

                if (folderChanged)
                {
                    string parentDir = Path.GetDirectoryName(oldDirectory);
                    newFolderPath = Path.Combine(parentDir, newFolderName);
                    Directory.Move(oldDirectory, newFolderPath);
                }

                string newFilePath = Path.Combine(newFolderPath, $"{newFileName}.json");
                bool fileChanged = newFileName != pageName;

                if (fileChanged)
                {
                    if (!File.Exists(newFilePath))
                    {
                        try
                        {
                            File.Move(Path.Combine(newFolderPath, $"{pageName}.json"), newFilePath);
                            pageName = newFileName;
                            readyPath = newFilePath;
                        }
                        catch (Exception ex) { Errors.DisplayMessage(ex.Message); }
                    }
                    else
                    {
                        Errors.DisplayMessage("File with this name already exists!");
                        return;
                    }
                }

                LoadJsonPage(newFilePath); // Always load the new file
            }
            else
            {
                Errors.DisplayMessage("Folder structure cannot be empty!");
            }
        }

        /// <summary>
        /// Moves the current file to a new folder selected by the user.
        /// </summary>
        private void btnMoveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFile(true);
            using (var folderDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderDialog.Description = "Select a destination folder";
                folderDialog.SelectedPath = MakeJsonPath(path);

                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;

                    // Normalize and check if selectedPath is within rootPath
                    string fullRoot = Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
                    string fullSelected = Path.GetFullPath(selectedPath);

                    if (!fullSelected.StartsWith(fullRoot, StringComparison.OrdinalIgnoreCase))
                    {
                        Errors.DisplayMessage("Selected folder is outside of the allowed root directory.");
                        return;
                    }

                    // Move the file
                    string fileName = Path.GetFileName(currentFilePath);
                    Debug.WriteLine($"{fileName}, NAME, {currentFilePath}");
                    string destination = Path.Combine(selectedPath, fileName);

                    try
                    {
                        File.Move(currentFilePath, destination);
                        Infos.DisplayMessage($"File moved to:\n{destination}");
                        currentFilePath = destination; // Update currentFilePath to the new location
                        readyPath = destination;
                        LoadJsonPage(destination);
                    }
                    catch (Exception ex)
                    {
                        Errors.DisplayMessage($"Failed to move file:\n{ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Moves the current folder to a new location selected by the user.
        /// </summary>
        private void btnMoveFolder_Click(object sender, RoutedEventArgs e)
        {
            SaveFile(true);
            string sourceFolderPath = Path.GetDirectoryName(currentFilePath);
            if (!Directory.Exists(sourceFolderPath))
            {
                Errors.DisplayMessage("Source folder does not exist.");
                return;
            }

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Select destination folder";
                dialog.SelectedPath = MakeJsonPath(path);
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string targetFolder = dialog.SelectedPath;

                    // Optional: Check if targetFolder is within allowed root
                    if (!targetFolder.StartsWith(path, StringComparison.OrdinalIgnoreCase))
                    {
                        Errors.DisplayMessage("Please select a folder inside the allowed root path.");
                        return;
                    }

                    // Determine the new full path including folder name
                    string folderName = new DirectoryInfo(sourceFolderPath).Name;
                    string destinationPath = Path.Combine(targetFolder, folderName);

                    if (Directory.Exists(destinationPath))
                    {
                        Errors.DisplayMessage("Destination folder contains current folder already.");
                        return;
                    }

                    try
                    {
                        Directory.Move(sourceFolderPath, destinationPath);
                        Infos.DisplayMessage($"Folder moved to: {destinationPath}");
                        LoadJsonPage(Path.Combine(destinationPath, $"{pageName}.json"));
                        return;
                    }
                    catch (Exception ex)
                    {
                        Errors.DisplayMessage($"Error moving folder: {ex.Message}");
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new folder inside the current directory, using a unique name.
        /// </summary>
        private void btnInsideFolderAdd_Click(object sender, RoutedEventArgs e)
        {
            InputDialog inputDialog = new InputDialog();
            inputDialog.Owner = this;
            if (inputDialog.ShowDialog() == true)
            {
                string currentDir = Path.GetDirectoryName(currentFilePath);
                string folderName = inputDialog.InputValue;
                folderName = Names.GetUniqueThemeName(folderName, (lbFS.ItemsSource as string[]).Select(t => t));

                try
                {
                    Directory.CreateDirectory(Path.Combine(currentDir, folderName));
                    lbFS.ItemsSource = Directory.GetDirectories(currentDir)
                            .Select(d => Path.GetFileName(d))
                            .ToArray();
                }
                catch (Exception ex)
                {
                    Errors.DisplayMessage($"Error creating folder: {ex.Message}");
                    return;
                }
            }
        }

        /// <summary>
        /// Deletes the selected folder from the current directory, with confirmation if not empty.
        /// </summary>
        private void btnInsideFolderDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lbFS.SelectedItem != null)
            {
                string folderName = lbFS.SelectedItem.ToString();
                string currentDir = Path.GetDirectoryName(currentFilePath);
                string pathToDelete = Path.Combine(currentDir, folderName);

                try
                {
                    if (Directory.Exists(pathToDelete))
                    {
                        Debug.WriteLine(pathToDelete);
                        if (Directory.EnumerateFileSystemEntries(pathToDelete).Any())
                        {
                            var result = MessageBox.Show(
                                "The directory is not empty. Do you want to delete it and all its contents?",
                                "Confirm Deletion",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Warning
                            );

                            if (result == MessageBoxResult.Yes)
                            {
                                Directory.Delete(pathToDelete, true);
                                Infos.DisplayMessage("Directory deleted successfully.");
                                RefreshFileData();
                            }
                            else
                            {
                                Infos.DisplayMessage("Deletion canceled.");
                            }
                        }
                        else
                        {
                            Directory.Delete(pathToDelete);
                        }
                        lbFS.ItemsSource = Directory.GetDirectories(currentDir)
                        .Select(d => Path.GetFileName(d))
                        .ToArray();
                    }
                    else
                    {
                        Errors.DisplayMessage("This folder does not exist!");
                    }
                }
                catch (Exception ex)
                {
                    Errors.DisplayMessage($"Error deleting folder: {ex.Message}");
                    return;
                }
            }
        }
    }
}
