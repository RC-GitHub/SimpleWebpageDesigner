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

namespace SWD
{
    /// <summary>
    /// Interaction logic for ContentWindow.xaml.
    /// Main window for editing, managing, and visualizing SWD project content.
    /// Handles project loading, saving, theming, DataGrid operations, and file/folder management.
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

        /// <summary>
        /// Initializes a new ContentWindow when a new project is created.
        /// </summary>
        /// <param name="directory">The project directory.</param>
        /// <param name="cw">Reference to the CreationWindow to close.</param>
        /// <param name="pagename">The initial page name (default: "index").</param>
        public ContentWindow(string directory, CreationWindow cw, string pagename = "index")
        {
            InitializeComponent();
            InitializeIcons();
            if (cw != null)
            {
                _creationWindow = cw;
                _creationWindow.Close();
            }

            wdContent.Title = GetProjectName(directory);

            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
            this.DataContext = App.themeData.CurrentTheme;

            path = directory;
            pageName = pagename;
            ControlFile(Path.Combine(MakeJsonPath(path), $"{pageName}.json"));

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

        /// <summary>
        /// Initializes a new ContentWindow when an existing project is opened.
        /// </summary>
        /// <param name="directory">The project directory.</param>
        /// <param name="mw">Reference to the MainWindow to close.</param>
        public ContentWindow(string directory, MainWindow mw)
        {
            InitializeComponent();
            InitializeIcons();
            if (mw != null)
            {
                _mainWindow = mw;
                _mainWindow.Close();
            }

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

        /// <summary>
        /// Handles theme changes and updates the DataContext and DataGrid.
        /// </summary>
        private void ThemeData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine("it changed");
            this.DataContext = App.themeData.CurrentTheme;
            BuildDataGrid(true);
        }

        /// <summary>
        /// Initializes toolbar and menu icons for the window.
        /// </summary>
        private void InitializeIcons()
        {
            imgNew.Source = Images.NewIcon("new-file.png");
            imgImport.Source = Images.NewIcon("open-file.png");
            imgEdit.Source = Images.NewIcon("edit.png");
            imgSave.Source = Images.NewIcon("save-file.png");
            imgClose.Source = Images.NewIcon("close.png");
            imgBuild.Source = Images.NewIcon("build.png");
            imgReveal.Source = Images.NewIcon("reveal.png");
        }

        /// <summary>
        /// Returns the project name for display in the window title.
        /// </summary>
        /// <param name="directory">The project directory.</param>
        /// <returns>The formatted project name.</returns>
        private string GetProjectName(string directory)
        {
            string lastFolderName = Path.GetFileName(directory.TrimEnd(Path.DirectorySeparatorChar));
            return $"SWD Project: {lastFolderName.Substring(4)}";
        }

        /// <summary>
        /// Saves the current grid and component data to a JSON file.
        /// </summary>
        /// <param name="noMessage">If true, suppresses the success message.</param>
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

        /// <summary>
        /// Returns the path to the JSON directory for the project, creating it if it does not exist.
        /// </summary>
        /// <param name="path">The project root path.</param>
        /// <returns>The path to the JSON directory.</returns>
        private string MakeJsonPath(string path)
        {
            string newPath = System.IO.Path.Combine(path, $"json");
            if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);
            return newPath;
        }

        /// <summary>
        /// Handles the click event to save the current file.
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        /// <summary>
        /// Handles the click event to open the theme editor window.
        /// </summary>
        private void btnTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeWindow tw = new ThemeWindow(this);
            tw.Show();
        }

        /// <summary>
        /// Handles the click event to create a new JSON file for a new page.
        /// </summary>
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
                try
                {
                    File.WriteAllText($"{MakeJsonPath(path)}\\{newFileName}.json", json);
                    Infos.DisplayMessage($"The {newFileName} was created!");
                    RefreshFileData();
                    //LoadJsonPage($"{MakeJsonPath(path)}\\{newFileName}.json");
                }
                catch (Exception ex)
                {
                    Infos.DisplayMessage(ex.Message);
                }
            }
        }

        /// <summary>
        /// Loads a JSON page, updates the grid and component data, and refreshes the UI.
        /// </summary>
        /// <param name="jsonPath">The path to the JSON file to load.</param>
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

        /// <summary>
        /// Resets the modify buttons and component property fields to their default state.
        /// </summary>
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