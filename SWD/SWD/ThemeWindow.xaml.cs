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
    /// Interaction logic for ThemeWindow.xaml.
    /// Provides a UI for creating, editing, and managing application themes.
    /// </summary>
    public partial class ThemeWindow : Window
    {
        private ContentWindow contentWindow = null;
        private Dictionary<string, Theme> themes = new Dictionary<string, Theme>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeWindow"/> class for standalone theme editing.
        /// </summary>
        public ThemeWindow()
        {
            InitializeComponent();
            LoadThemes();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeWindow"/> class for editing themes in the context of a content window.
        /// </summary>
        /// <param name="cw">The parent <see cref="ContentWindow"/>.</param>
        public ThemeWindow(ContentWindow cw)
        {
            contentWindow = cw;
            InitializeComponent();
            LoadThemes();
        }

        /// <summary>
        /// Loads themes from the application data and sets up the UI.
        /// </summary>
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

        /// <summary>
        /// Returns a list of all theme names.
        /// </summary>
        private List<string> GetThemesNames()
        {
            List<string> list = new List<string>();
            foreach (Theme theme in themes.Values)
            {
                list.Add(theme.Name);
            }
            return list;
        }

        /// <summary>
        /// Handles the selection change event for the theme ComboBox.
        /// Updates the DataContext and name field.
        /// </summary>
        private void cbThemes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbThemes.SelectedItem is Theme selectedTheme)
            {
                this.DataContext = selectedTheme;
                tbName.Text = selectedTheme.Name;
            }
        }

        /// <summary>
        /// Handles the color selection for a theme property using a color picker dialog.
        /// </summary>
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

        /// <summary>
        /// Handles the image selection for the background image property.
        /// </summary>
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
                string savePath = Path.Combine(configPath, fileName);

                if (selectedPath != savePath) File.Copy(selectedPath, savePath, true);

                tbBackgroundImage.Text = fileName;
                BindingExpression binding = tbBackgroundImage.GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();
            }
        }

        /// <summary>
        /// Updates the visibility of background image-related controls based on the theme settings.
        /// </summary>
        private void BackgroundTypeSelection()
        {
            if (!(DataContext is Theme theme)) return;

            if (theme.BackgroundImageOn)
            {
                rdImage.Height = new GridLength(1, GridUnitType.Auto);
                rdImageAlignX.Height = new GridLength(1, GridUnitType.Auto);
                rdImageAlignY.Height = new GridLength(1, GridUnitType.Auto);
                rdImageStretch.Height = new GridLength(1, GridUnitType.Auto);
                rdImageOpacity.Height = new GridLength(1, GridUnitType.Auto);
                rdImageOverlay.Height = new GridLength(1, GridUnitType.Auto);
            }
            else
            {
                rdImage.Height = new GridLength(0);
                rdImageAlignX.Height = new GridLength(0);
                rdImageAlignY.Height = new GridLength(0);
                rdImageStretch.Height = new GridLength(0);
                rdImageOpacity.Height = new GridLength(0);
                rdImageOverlay.Height = new GridLength(0);
            }
        }

        /// <summary>
        /// Handles the Checked event for the background image toggle.
        /// </summary>
        private void cbBackgroundImageOn_Checked(object sender, RoutedEventArgs e)
        {
            BackgroundTypeSelection();
        }

        /// <summary>
        /// Handles the Unchecked event for the background image toggle.
        /// </summary>
        private void cbBackgroundImageOn_Unchecked(object sender, RoutedEventArgs e)
        {
            BackgroundTypeSelection();
        }

        /// <summary>
        /// Shows the trivia popup when the title label is hovered.
        /// </summary>
        private void lblTitle_MouseEnter(object sender, MouseEventArgs e)
        {
            popTrivia.IsOpen = true;
        }

        /// <summary>
        /// Hides the trivia popup when the title label is no longer hovered.
        /// </summary>
        private void lblTitle_MouseLeave(object sender, MouseEventArgs e)
        {
            popTrivia.IsOpen = false;
        }

        /// <summary>
        /// Shows the star popup when the star textbox is hovered.
        /// </summary>
        private void tbStar_MouseEnter(object sender, MouseEventArgs e)
        {
            popStar.IsOpen = true;
        }

        /// <summary>
        /// Hides the star popup when the star textbox is no longer hovered.
        /// </summary>
        private void tbStar_MouseLeave(object sender, MouseEventArgs e)
        {
            popStar.IsOpen = false;
        }

        /// <summary>
        /// Handles the click event to create a new theme based on the current one.
        /// </summary>
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Theme newTheme = (Theme)(this.DataContext as Theme).Clone();
            newTheme.Name = Names.GetUniqueThemeName(tbName.Text, themes.Values.Select(t => t.Name));

            themes[newTheme.Name] = newTheme;

            cbThemes.ItemsSource = null;
            cbThemes.ItemsSource = themes.Values;

            this.DataContext = newTheme;
            cbThemes.SelectedItem = newTheme;

            SaveTheme();
        }

        /// <summary>
        /// Handles the click event to delete the currently selected theme.
        /// </summary>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int index = cbThemes.SelectedIndex;
            Theme theme = this.DataContext as Theme;
            themes.Remove(theme.Name);
            cbThemes.Items.Refresh();
            if (cbThemes.Items.Count - 1 >= index)
            {
                cbThemes.SelectedIndex = index;
            }
            else
            {
                cbThemes.SelectedIndex = 0;
            }
            Infos.DisplayMessage("Theme deleted successfully.");
        }

        /// <summary>
        /// Saves the current theme to the configuration file and updates the application theme data.
        /// </summary>
        /// <param name="currentTheme">If true, sets the saved theme as the current theme.</param>
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

            if (theme.BackgroundImageOn)
            {
                if (theme.BackgroundImage == null)
                {
                    Errors.DisplayMessage("No background image selected!");
                    return;
                }
                string backgroundImageDirectory = Path.Combine(configDirectory, theme.BackgroundImage);
                if (theme.BackgroundImage != null && !File.Exists(backgroundImageDirectory))
                {
                    Errors.DisplayMessage("Background image does not exist in the config folder!");
                    return;
                }
                theme.BackgroundImageFullPath = Path.Combine(configDirectory, theme.BackgroundImage);
                themes[theme.Name] = theme;
            }

            App.themeData.Themes = themes;
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

            contentWindow.DataContext = App.themeData.CurrentTheme;
            contentWindow.BuildDataGrid(true);
        }

        /// <summary>
        /// Handles the click event to save the current theme.
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveTheme();
        }

        /// <summary>
        /// Handles the click event to set the current theme and close the window.
        /// </summary>
        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            SaveTheme(true);
            this.Close();
        }
    }
}
