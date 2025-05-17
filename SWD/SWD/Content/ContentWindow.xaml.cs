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
        private readonly MainWindow _mainWindow;

        // Variables concerning the main DataGrid
        string path = string.Empty;
        string pageName = string.Empty;
        List<Row> data = new List<Row>();
        Dictionary<string, Component> components = new Dictionary<string, Component>();
        Component currentComponent = null;

        // Variables concerning the folder structure displayed on the left
        string readyPath = "";
        public DataTable dataTable = MakeDataTable();

        // Constructor used when a new project is created.
        public ContentWindow(string directory, CreationWindow cw, string pagename = "index")
        {
            InitializeComponent();
            _creationWindow = cw;
            _creationWindow.Close();
            wdContent.Title = GetProjectName(directory);

            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
            this.DataContext = App.themeData.CurrentTheme;

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
            SaveFile(true);
        }

        // Constructor used when an existing project is opened.
        public ContentWindow(string directory, MainWindow mw)
        {
            InitializeComponent();
            _mainWindow = mw;
            _mainWindow.Close();
            wdContent.Title = GetProjectName(directory);

            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
            this.DataContext = App.themeData.CurrentTheme;

            path = directory;
            string jsonPath = MakeJsonPath(path);
            string indexPath = Path.Combine(jsonPath, "index.json");
            if (File.Exists(indexPath))
            {
                pageName = "index";
                LoadJsonPage(indexPath);
            }
            else
            {
                bool hasFiles = Directory.EnumerateFiles(jsonPath, "*.json", SearchOption.AllDirectories).Any();
                if (hasFiles)
                {
                    string defaultPath = Directory.EnumerateFiles(jsonPath, "*.json", SearchOption.AllDirectories).FirstOrDefault();
                    string pageName = Path.GetFileNameWithoutExtension(defaultPath);
                    LoadJsonPage(defaultPath);
                }
                else
                {
                    pageName = "index";
                    Row obj = new Row()
                    {
                        Title = "1",
                        Content = new List<Cell> { new Cell() { Title = "1" } }
                    };
                    data.Add(obj);
                    dgContent.ItemsSource = data;
                    BuildDataGrid();
                }
            }
        }

        private void ThemeData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine("it changed");
            this.DataContext = App.themeData.CurrentTheme;
            BuildDataGrid(true);
        }

        private string GetProjectName(string directory)
        {
            string lastFolderName = Path.GetFileName(directory.TrimEnd(Path.DirectorySeparatorChar));
            return $"SWD Project: {lastFolderName.Substring(4)}";
        }

        private void SaveFile(bool noMessage = false)
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
                    File.WriteAllText($"{MakeJsonPath(path)}\\{pageName}.json", json);
                }

                if (!noMessage) Infos.DisplayMessage($"{pageName}.html was saved!");
                RefreshFileData();

            }
            catch (Exception err)
            {
                Console.WriteLine("The process failed: {0}", err.ToString());
            }
            finally { }
        }

        private string MakeJsonPath(string path)
        {
            string newPath = System.IO.Path.Combine(path, $"json");
            if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);
            return newPath;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void btnTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeWindow tw = new ThemeWindow(this);
            tw.Show();
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

                Debug.WriteLine(json);
                File.WriteAllText($"{MakeJsonPath(path)}\\{newFileName}.json", json);
                Infos.DisplayMessage($"The {newFileName} was created!");

                RefreshFileData();

            }
        }

        public void LoadJsonPage(string jsonPath)
        {
            if (!File.Exists(jsonPath))
            {
                Errors.DisplayMessage("JSON file not found.");
                return;
            }

            try
            {
                ControlFile(jsonPath);
                string json = File.ReadAllText(jsonPath);
                var pages = JsonConvert.DeserializeObject<List<PageData>>(json);

                if (pages == null || pages.Count == 0)
                {
                    Errors.DisplayMessage("JSON is empty or invalid.");
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

                dgContent.ItemsSource = data;
                tbRowAmount.Text = rows.ToString();
                tbColAmount.Text = cols.ToString();
                lblGridTitle.Content = $"{pageName}.html";

                BuildDataGrid();
                RefreshFileData();
                RevertModifyButtons();
            }
            catch (Exception ex)
            {
                Errors.DisplayMessage($"Failed to load JSON page.\n\n{ex}");
            }
        }

        public void RevertModifyButtons()
        {
            tbColModify.Text = "1";
            tbRowModify.Text = "1";
            tbCompWidth.Text = "-";
            tbCompHeight.Text = "-";
            tbCompRow.Text = "-";
            tbCompCol.Text = "-";
        }
    }
} 
