using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Brush = System.Drawing.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using DataGridCell = System.Windows.Controls.DataGridCell;
using Image = System.Windows.Controls.Image;
using Path = System.IO.Path;
using Window = System.Windows.Window;

namespace SWD.Content
{
    /// <summary>
    /// Logika interakcji dla klasy CreationWindow.xaml
    /// </summary>
    public partial class ContentWindow : Window
    {
        // Previous window to be closed
        private readonly CreationWindow _creationWindow;

        // Variables concerning the main DataGrid
        string path = string.Empty;
        string pageName = string.Empty;
        List<Row> data = new List<Row>();
        Dictionary<string, Component> components = new Dictionary<string, Component>();

        // Variables concerning the folder structure displayed on the left
        string readyPath = "";
        public DataTable dataTable = MakeDataTable();

        public ContentWindow(string directory, CreationWindow cw, string pagename = "index")
        {
            InitializeComponent();
            _creationWindow = cw;
            _creationWindow.Close();

            path = directory;
            pageName = pagename;

            Row obj = new Row()
            {
                Title = "1",
                Content = new List<Cell> { new Cell() { Title = "1" } }
            };
            data.Add(obj);
            dgContent.ItemsSource = data;
            BuildDataGrid();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            List<ContentStructure> _data = new List<ContentStructure>
            {
                new ContentStructure()
                {
                    Components = components,
                    RowAmount = data.Count,
                    ColAmount = data[0].Content.Count
                }
            };

            string json = JsonConvert.SerializeObject(_data, Newtonsoft.Json.Formatting.Indented);

            try
            {
                if (readyPath != "")
                {
                    File.WriteAllText(readyPath, json);
                }
                else
                {
                    string newPath = System.IO.Path.Combine(path, $"json");
                    if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);
                    //Debug.WriteLine(json);
                    File.WriteAllText($"{newPath}\\{pageName}.json", json);
                }

                Infos.DisplayErrorMessage($"{pageName}.html was saved!");
                RefreshFileData();
                
            }
            catch (Exception err)
            {
                Console.WriteLine("The process failed: {0}", err.ToString());
            }
            finally { }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            InputDialog inputDialog = new InputDialog();
            if (inputDialog.ShowDialog() == true)
            {
                string newFileName = inputDialog.InputValue;
                List<ContentStructure> _data = new List<ContentStructure>
                {
                    new ContentStructure()
                    {
                        Components = {},
                        RowAmount = 1,
                        ColAmount = 1
                    }
                };

                string json = JsonConvert.SerializeObject(_data, Newtonsoft.Json.Formatting.Indented);
                string newPath = System.IO.Path.Combine(path, $"json");
                if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);
                Debug.WriteLine(json);
                File.WriteAllText($"{newPath}\\{newFileName}.json", json);
                Infos.DisplayErrorMessage($"The {newFileName} was created!");

                RefreshFileData();

            }
        }

        public void LoadJsonPage(string jsonPath)
        {
            if (!File.Exists(jsonPath))
            {
                Errors.DisplayErrorMessage("JSON file not found.");
                return;
            }

            try
            {
                string json = File.ReadAllText(jsonPath);
                var pages = JsonConvert.DeserializeObject<List<PageData>>(json);

                if (pages == null || pages.Count == 0)
                {
                    Errors.DisplayErrorMessage("JSON is empty or invalid.");
                    return;
                }

                PageData page = pages[0]; 


                components = new Dictionary<string, Component>(); 
                components = page.Components;

                int rows = page.RowAmount;
                int cols = page.ColAmount;
                Debug.WriteLine($"{rows} and {cols} and {components}");

                data.Clear();
                for (int r = 0; r < rows; r++)
                {
                    Row newRow = new Row
                    {
                        Title = (r + 1).ToString(),
                        Content = new List<Cell>()
                    };

                    for (int c = 0; c < cols; c++)
                    {
                        newRow.Content.Add(new Cell { Title = $"{r + 1}_{c + 1}" });
                    }

                    data.Add(newRow);
                }

                tbRowAmount.Text = rows.ToString();
                tbColAmount.Text = cols.ToString();

                BuildDataGrid();
            }
            catch (Exception ex)
            {
                Errors.DisplayErrorMessage($"Failed to load JSON page.\n\n{ex}");
            }
        }
    }
} 
