using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Window = System.Windows.Window;

namespace SWD
{
    /// <summary>
    /// Partial class for ContentWindow, containing logic for managing file selector data,
    /// including building the DataTable for files and refreshing file data from disk.
    /// </summary>
    public partial class ContentWindow : Window
    {
        /// <summary>
        /// Creates and returns a DataTable structure for storing file information,
        /// including columns for id, folder structure, filename, and path.
        /// </summary>
        /// <returns>A DataTable configured for file selector usage.</returns>
        public static DataTable MakeDataTable()
        {
            DataTable dataTable = new DataTable("Files");

            DataColumn idColumn = new DataColumn();
            idColumn.DataType = System.Type.GetType("System.Int32");
            idColumn.ColumnName = "id";
            idColumn.AutoIncrement = true;
            dataTable.Columns.Add(idColumn);

            DataColumn folderStructureColumn = new DataColumn();
            folderStructureColumn.DataType = System.Type.GetType("System.String");
            folderStructureColumn.ColumnName = "Folder structure";
            folderStructureColumn.DefaultValue = "Folder structure";
            dataTable.Columns.Add(folderStructureColumn);

            DataColumn fileNameColumn = new DataColumn();
            fileNameColumn.DataType = System.Type.GetType("System.String");
            fileNameColumn.ColumnName = "Filename";
            fileNameColumn.DefaultValue = "Filename";
            dataTable.Columns.Add(fileNameColumn);

            DataColumn filePathColumn = new DataColumn();
            filePathColumn.DataType = System.Type.GetType("System.String");
            filePathColumn.ColumnName = "Path";
            filePathColumn.DefaultValue = "Path";
            dataTable.Columns.Add(filePathColumn);

            DataColumn[] keys = new DataColumn[1];
            keys[0] = idColumn;
            dataTable.PrimaryKey = keys;

            return dataTable;
        }

        /// <summary>
        /// Refreshes the file data by scanning the file system for files in the JSON directory,
        /// populating the DataTable, and updating the DataGrid's ItemsSource.
        /// </summary>
        public void RefreshFileData()
        {
            dataTable = MakeDataTable();

            string newPath = System.IO.Path.Combine(path, $"json");
            Debug.WriteLine($"{newPath} is JSONS");

            try
            {
                DirectoryInfo place = new DirectoryInfo(newPath);
                FileInfo[] Files = place.GetFiles();

                foreach (string file in System.IO.Directory.GetFiles(newPath, "*", SearchOption.AllDirectories))
                {
                    DataRow newRow;
                    newRow = dataTable.NewRow();

                    string fileAndFolder = file.Split(new string[] { path }, StringSplitOptions.None)[1];
                    string[] split = fileAndFolder.Split('\\');
                    string filename = split[split.Length - 1].Split('.')[0];
                    string folderStructure = "";
                    Debug.WriteLine(String.Join(", ", split));

                    for (int i = 0; i <= split.Length - 2; i++)
                    {
                        if (split[i] != String.Empty) folderStructure += split[i] + " \\ ";
                    }

                    newRow["Folder structure"] = folderStructure;
                    newRow["Filename"] = filename;
                    newRow["Path"] = file;
                    dataTable.Rows.Add(newRow);
                }
                dgPages.ItemsSource = dataTable.AsDataView();
            }
            catch (Exception ex)
            {
                Errors.DisplayMessage($"Components haven't been saved.\n\n{ex}");
            }
        }

        /// <summary>
        /// Handles the MouseDown event for the refresh image, triggering a refresh of the file data.
        /// </summary>
        /// <param name="sender">The image control that was clicked.</param>
        /// <param name="e">Mouse button event arguments.</param>
        private void imgRefresh_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RefreshFileData();
        }
    }
}
