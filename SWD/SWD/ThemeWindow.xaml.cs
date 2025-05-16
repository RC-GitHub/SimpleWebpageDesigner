using Microsoft.Win32;
using Newtonsoft.Json;
using SWD.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace SWD
{
    /// <summary>
    /// Interaction logic for ThemeWindow.xaml
    /// </summary>
    public partial class ThemeWindow : Window
    {
        private ContentWindow contentWindow = null;
        private Dictionary<string, Theme> themes = new Dictionary<string, Theme>();
        public ThemeWindow()
        {
            InitializeComponent();
            LoadThemes();
        }

        public ThemeWindow(ContentWindow cw)
        {
            contentWindow = cw;
            InitializeComponent();
            LoadThemes();
        }

        private void LoadThemes()
        {
            themes = ((ThemeData)App.themeData.Clone()).Themes;

            cbThemes.ItemsSource = themes.Values;
            cbThemes.DisplayMemberPath = "Name";
            if (App.themeData.CurrentKey != null)
                cbThemes.SelectedItem = themes[App.themeData.CurrentKey];
            else
                cbThemes.SelectedIndex = 0;

            if (cbThemes.SelectedItem != null)
            {
                tbName.Text = ((Theme)cbThemes.SelectedItem).Name;
                this.DataContext = cbThemes.SelectedItem;
            }

            BackgroundTypeSelection();
            GetThemesNames();
        }

        private List<string> GetThemesNames()
        {
            List<string> list = new List<string>();
            foreach (Theme theme in themes.Values)
            {
                list.Add(theme.Name);
            }
            return list;
        }

        private void cbThemes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbThemes.SelectedItem is Theme selectedTheme)
            {
                this.DataContext = selectedTheme;
                tbName.Text = selectedTheme.Name;
            }

        }

        private void SetColor(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string color = Colors.StringColorPicker();
            if (color == null) return;

            string name = btn.Name.Substring(3);
            Theme theme = this.DataContext as Theme;
            theme[name] = color;

            TextBox textBox = (TextBox)this.FindName($"tb{name}");
            textBox.Text = color;

            BindingExpression binding = textBox.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();
        }

        private void SetImage(object sender, RoutedEventArgs e)
        {
            string projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\.."));
            string configPath = Path.Combine(projectDirectory, "Config");

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp, *.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Select an image",
                InitialDirectory = configPath
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedPath = openFileDialog.FileName;
                string fileName = System.IO.Path.GetFileName(selectedPath);
                string savePath = Path.Combine (configPath, fileName);

                File.Copy(selectedPath, savePath, true);

                tbBackgroundImage.Text = fileName;
                BindingExpression binding = tbBackgroundImage.GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();
            }
        }

        private void BackgroundTypeSelection()
        {
            if (!(DataContext is Theme theme) || theme.BackgroundType == null) return;

            switch (theme.BackgroundType)
            {
                case "Flat":
                    rdFlat.Height = new GridLength(1, GridUnitType.Auto);
                    rdGradStart.Height = new GridLength(0);
                    rdGradEnd.Height = new GridLength(0);
                    rdImage.Height = new GridLength(0);
                    rdImageAlignX.Height = new GridLength(0);
                    rdImageAlignY.Height = new GridLength(0);
                    rdImageStretch.Height = new GridLength(0);
                    rdImageOpacity.Height = new GridLength(0);
                    rdImageUnderlay.Height = new GridLength(0);
                    break;
                case "Gradient":
                    rdFlat.Height = new GridLength(0);
                    rdGradStart.Height = new GridLength(1, GridUnitType.Auto);
                    rdGradEnd.Height = new GridLength(1, GridUnitType.Auto);
                    rdImage.Height = new GridLength(0);
                    rdImageAlignX.Height = new GridLength(0);
                    rdImageAlignY.Height = new GridLength(0);
                    rdImageStretch.Height = new GridLength(0);
                    rdImageOpacity.Height = new GridLength(0);
                    rdImageUnderlay.Height = new GridLength(0);
                    break;
                case "Image":
                    rdFlat.Height = new GridLength(0);
                    rdGradStart.Height = new GridLength(0);
                    rdGradEnd.Height = new GridLength(0);
                    rdImage.Height = new GridLength(1, GridUnitType.Auto);
                    rdImageAlignX.Height = new GridLength(1, GridUnitType.Auto);
                    rdImageAlignY.Height = new GridLength(1, GridUnitType.Auto);
                    rdImageStretch.Height = new GridLength(1, GridUnitType.Auto);
                    rdImageOpacity.Height = new GridLength(1, GridUnitType.Auto);
                    rdImageUnderlay.Height = new GridLength(1, GridUnitType.Auto);
                    break;
            }
        }

        private void cbBackgroundType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BackgroundTypeSelection();
        }

        private void lblTitle_MouseEnter(object sender, MouseEventArgs e)
        {
            popTrivia.IsOpen = true;
        }

        private void lblTitle_MouseLeave(object sender, MouseEventArgs e)
        {
            popTrivia.IsOpen = false;
        }

        private void tbStar_MouseEnter(object sender, MouseEventArgs e)
        {
            popStar.IsOpen = true;
        }

        private void tbStar_MouseLeave(object sender, MouseEventArgs e)
        {
            popStar.IsOpen = false;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Theme newTheme = (Theme)(this.DataContext as Theme).Clone();
            newTheme.Name = GetUniqueThemeName(tbName.Text, themes.Values.Select(t => t.Name));

            themes[newTheme.Name] = newTheme;

            cbThemes.ItemsSource = null;
            cbThemes.ItemsSource = themes.Values;

            this.DataContext = newTheme;
            cbThemes.SelectedItem = newTheme;

            SaveTheme();
        }

        public static string GetUniqueThemeName(string desiredName, IEnumerable<string> existingNames)
        {
            var nameSet = new HashSet<string>(existingNames, StringComparer.OrdinalIgnoreCase);

            Debug.WriteLine(desiredName);

            if (!nameSet.Contains(desiredName))
                return desiredName;

            int counter = 1;
            string newName;

            do
            {
                newName = $"{desiredName}_{counter}";
                counter++;
            } 
            while (nameSet.Contains(newName));

            return newName;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int index = cbThemes.SelectedIndex;
            Theme theme = this.DataContext as Theme;
            themes.Remove(theme.Name);
            cbThemes.Items.Refresh();
            if (cbThemes.Items.Count-1 >= index)
            {
                cbThemes.SelectedIndex = index;
            }
            else
            {
                cbThemes.SelectedIndex = 0;
            }
            Infos.DisplayMessage("Theme deleted successfully.");
        }

        public void SaveTheme(bool currentTheme = false)
        {
            Theme theme = this.DataContext as Theme;
            string oldName = theme.Name;
            string newName = tbName.Text;

            if (cbThemes.SelectedIndex >= 0)
            {
                if (oldName != newName)
                {
                    if (themes.ContainsKey(oldName))
                    {
                        theme.Name = newName;
                        themes[newName] = theme;
                        themes.Remove(oldName);
                        theme = themes[newName];
                    }
                }
                else themes[oldName] = theme;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    theme.Name = newName;
                    themes[newName] = theme;
                }
                else
                {
                    Errors.DisplayMessage("Theme name cannot be empty!");
                    return;
                }
            }

            string projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\.."));
            string configDirectory = Path.Combine(projectDirectory, "Config");


            if (theme.BackgroundType == "Image")
            {
                string backgroundImageDirectory = Path.Combine(configDirectory, theme.BackgroundImage);
                if (theme.BackgroundImage == null)
                {
                    Errors.DisplayMessage("No background image selected!");
                    return;
                }
                else if (theme.BackgroundImage != null && !File.Exists(backgroundImageDirectory))
                {
                    Errors.DisplayMessage("Background image does not exist in the config folder!");
                    return;
                }
            }

            if (themes != App.themeData.Themes) App.themeData.Themes = themes;
            if (currentTheme && tbName.Text != App.themeData.CurrentKey) App.themeData.CurrentKey = tbName.Text;
            if (currentTheme && themes[tbName.Text] != App.themeData.CurrentTheme) App.themeData.CurrentTheme = themes[tbName.Text];

            string themeDirectory = Path.Combine(configDirectory, "themes.json");
            string json = JsonConvert.SerializeObject(App.themeData, Newtonsoft.Json.Formatting.Indented);

            try
            {
                File.WriteAllText(themeDirectory, json);
                cbThemes.ItemsSource = null;
                cbThemes.ItemsSource = themes.Values;
                cbThemes.SelectedItem = theme;
                Infos.DisplayMessage($"Themes have been saved.");
            }
            catch (Exception err)
            {
                Console.WriteLine("The process failed: {0}", err.ToString());
            }
            finally { }

            if (contentWindow == null) return;

            Debug.WriteLine("Modify DataGrid");
            contentWindow.DataContext = App.themeData.CurrentTheme;
            contentWindow.BuildDataGrid(true);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveTheme();
        }

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            SaveTheme(true);
            this.Close();
        }
    }
}
