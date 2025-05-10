using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace SWD
{
    /// <summary>
    /// Interaction logic for ThemeWindow.xaml
    /// </summary>
    public partial class ThemeWindow : Window
    {
        private List<Theme> themes = new List<Theme>();
        public ThemeWindow()
        {
            InitializeComponent();
            LoadThemes();
        }

        private void LoadThemes()
        {
            string projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\.."));
            string configPath = Path.Combine(projectDirectory, "Config", "themes.json");
            if (!File.Exists(configPath))
            {
                Errors.DisplayMessage("Config file not found.");
                return;
            }

            try
            {
                string json = File.ReadAllText(configPath);
                themes = JsonConvert.DeserializeObject<List<Theme>>(json);
            }
            catch (Exception ex)
            {
                Errors.DisplayMessage($"Failed to load JSON page.\n\n{ex}");
            }

            if (!themes.Any(t => t.Name == "Light"))
            {
                themes.Insert(0, new Theme
                {
                    Name = "Light",
                });
            }
            if (!themes.Any(t => t.Name == "Dark"))
            {
                themes.Insert(0, new Theme
                {
                    Name = "Dark",
                });
            }

            cbThemes.ItemsSource = themes;
            cbThemes.DisplayMemberPath = "Name";
            cbThemes.SelectedIndex = 0;

            tbName.Text = cbThemes.SelectedItem.ToString();
            string type = themes[0].BackgroundType;
            this.DataContext = themes[cbThemes.SelectedIndex];

            BackgroundTypeSelection();
            GetThemesNames();
        }

        private List<string> GetThemesNames()
        {
            List<string> list = new List<string>();
            foreach (Theme theme in themes)
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

        private void BackgroundTypeSelection()
        {
            Theme theme = this.DataContext as Theme;
            Debug.WriteLine(theme.BackgroundType);
            if (theme == null || theme.BackgroundType == null) return;

            if (theme.BackgroundType == "Flat")
            {
                tbBackgroundFlat.IsEnabled = true;
                tbBackgroundGradStart.IsEnabled = false;
                tbBackgroundGradEnd.IsEnabled = false;
                tbBackgroundImage.IsEnabled = false;
            }
            else if (theme.BackgroundType == "Gradient")
            {
                tbBackgroundGradStart.IsEnabled = true;
                tbBackgroundGradEnd.IsEnabled = true;
                tbBackgroundFlat.IsEnabled = false;
                tbBackgroundImage.IsEnabled = false;
            }
            else if (theme.BackgroundType == "Image")
            {
                tbBackgroundImage.IsEnabled = true;
                tbBackgroundFlat.IsEnabled = false;
                tbBackgroundGradStart.IsEnabled = false;
                tbBackgroundGradEnd.IsEnabled = false;
            }
        }

        private void cbBackgroundType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BackgroundTypeSelection();
        }

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Theme theme = this.DataContext as Theme;
            theme.Name = tbName.Text;
            if (cbThemes.SelectedIndex >= 0)
                themes[cbThemes.SelectedIndex] = theme;
            else
                themes.Add(theme);
            string projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\.."));
            string configPath = Path.Combine(projectDirectory, "Config", "themes.json");
            string json = JsonConvert.SerializeObject(themes, Newtonsoft.Json.Formatting.Indented);

            try
            {
                File.WriteAllText(configPath, json);
                Infos.DisplayMessage($"Themes have been saved.");
            }
            catch (Exception err)
            {
                Console.WriteLine("The process failed: {0}", err.ToString());
            }
            finally { }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Theme newTheme = (Theme)(this.DataContext as Theme).Clone();
            newTheme.Name = GetUniqueThemeName(tbName.Text, themes.Select(t => t.Name));
            themes.Add(newTheme);
            btnSave_Click(sender, e);
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
            } while (nameSet.Contains(newName));

            return newName;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int index = cbThemes.SelectedIndex;
            Theme theme = this.DataContext as Theme;
            themes.Remove(theme);
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
    }
}
