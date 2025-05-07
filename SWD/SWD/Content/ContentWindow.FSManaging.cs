using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window = System.Windows.Window;

namespace SWD.Content
{
    public partial class ContentWindow : Window
    {
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
                Errors.DisplayErrorMessage($"Components haven't been saved.\n\n{ex}");
            }
        }
    }
}
