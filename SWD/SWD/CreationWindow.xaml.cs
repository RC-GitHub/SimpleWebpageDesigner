using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

//using System.Text.Json;
//using System.Text.Json.Serialization;

using Newtonsoft.Json;
using System.IO;
using SWD.Content;
using Path = System.IO.Path;

namespace SWD
{
    /// <summary>
    /// Logika interakcji dla klasy CreationWindow.xaml
    /// </summary>
    public partial class CreationWindow : Window
    {
        private bool editmode = false;
        private MainWindow _mainWindow;
        private ContentWindow _contentWindow;
        public bool projectsFolder = true;
        public string dir = Environment.CurrentDirectory;
        private string tempForProjectTitle = "Insert project title";
        private string tempForAuthor = "Insert author's name";
        private string tempForKeyword = "Insert a keyword";
        private string tempForDescription = "Insert description";
        public Dictionary<string, string> tempMessages = new Dictionary<string, string>();

        public CreationWindow(MainWindow mw = null)
        {
            _mainWindow = mw;

            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
            this.DataContext = App.themeData.CurrentTheme;

            InitializeComponent();
            InitializeEmptyText();
            InitializeMessagesArray();

        }

        public CreationWindow(string d, MainWindow mw)
        {
            dir = d;
            _mainWindow = mw;
            projectsFolder = false;

            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
            this.DataContext = App.themeData.CurrentTheme;

            InitializeComponent();
            InitializeEmptyText();
            InitializeMessagesArray();
        }

        string projectName;
        public CreationWindow(string d, Head head, ContentWindow cw)
        {
            dir = d;
            _contentWindow = cw;
            editmode = true;

            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
            this.DataContext = App.themeData.CurrentTheme;
            InitializeComponent();
            InitializeMessagesArray();
            
            projectName = head.ProjectName;
            tbProjectTitle.Text = head.ProjectName;
            tbAuthor.Text = head.Author;
            tbDescription.Text = head.Description;
            foreach (string keyword in head.Keywords)
            {
                lsbKeywords.Items.Add(keyword);
            }
        }

        private void ThemeData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ThemeData.CurrentTheme))
                this.DataContext = App.themeData.CurrentTheme;
        }

        public void InitializeMessagesArray()
        {
            tempMessages.Add("tbProjectTitle", tempForProjectTitle);
            tempMessages.Add("tbAuthor", tempForAuthor);
            tempMessages.Add("tbKeyword", tempForKeyword);
            tempMessages.Add("tbDescription", tempForDescription);

            foreach (KeyValuePair<string, string> kvp in tempMessages) {
                Debug.WriteLine(kvp.Value);
            }
        }

        public void InitializeEmptyText()
        {
            if (string.IsNullOrWhiteSpace(tbProjectTitle.Text))
            {
                tbProjectTitle.Text = tempForProjectTitle;
                tbProjectTitle.Foreground = Brushes.Gray;
            }
            if (string.IsNullOrWhiteSpace(tbAuthor.Text))
            {
                tbAuthor.Text = tempForAuthor;
                tbAuthor.Foreground = Brushes.Gray;
            }            
            if (string.IsNullOrWhiteSpace(tbKeyword.Text))
            {
                tbKeyword.Text = tempForKeyword;
                tbKeyword.Foreground = Brushes.Gray;
            }
            if (string.IsNullOrWhiteSpace(tbDescription.Text))
            {
                tbDescription.Text = tempForDescription;
                tbDescription.Foreground = Brushes.Gray;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Adding a keyword to the List.
            if (tbKeyword.Text != tempForKeyword)
            {
                lsbKeywords.Items.Add(tbKeyword.Text);
                tbKeyword.Text = "";
                tbKeyword.Focus();
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            // Deleting a keyword from the List.
            if (lsbKeywords.SelectedItem != null)
            {
                int index = lsbKeywords.SelectedIndex;
                lsbKeywords.Items.RemoveAt(index);
                lsbKeywords.Items.Refresh();
                if (lsbKeywords.Items.Count - 1 >= index)
                    lsbKeywords.SelectedIndex = index;
                else
                    lsbKeywords.SelectedIndex = lsbKeywords.Items.Count-1;
            }

        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!editmode)
                NormalSubmit();
            else
                EditSubmit();
        }

        private void NormalSubmit()
        {
            if (tbProjectTitle.Text == tempMessages[tbProjectTitle.Name] || tbProjectTitle.Text == string.Empty) Errors.DisplayMessage("Project title is required!");
            else
            {
                if (projectsFolder == true) dir = System.IO.Path.Combine(dir, $"Projects");
                dir = System.IO.Path.Combine(dir, $"SWD-{tbProjectTitle.Text}");
                Debug.WriteLine(dir);

                if (Directory.Exists(dir))
                {
                    Errors.DisplayMessage("A project with the same name already exists!");
                    dir = "";
                }
                else
                {

                    string author;
                    string description;
                    if (tbAuthor.Text == tempMessages[tbAuthor.Name]) author = "";
                    else author = tbAuthor.Text;
                    if (tbDescription.Text == tempMessages[tbDescription.Name]) description = "";
                    else description = tbDescription.Text;

                    List<Head> _data = new List<Head>
                    {
                        new Head()
                        {
                            ProjectName = tbProjectTitle.Text,
                            Author = author,
                            Keywords = lsbKeywords.Items.OfType<string>().ToArray(),
                            Description = description,
                        }
                    };

                    string json = JsonConvert.SerializeObject(_data, Formatting.Indented);

                    try
                    {
                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                        Debug.WriteLine(json);
                        File.WriteAllText($"{dir}\\metadata.json", json);
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("The process failed: {0}", err.ToString());
                    }
                    finally { }

                    if (_mainWindow != null) _mainWindow.Close();

                    Content.ContentWindow gd = new Content.ContentWindow(dir, this);
                    gd.Show();

                }
            }
        }

        private void EditSubmit()
        {
            try
            {
                if (tbProjectTitle.Text == tempMessages[tbProjectTitle.Name] || tbProjectTitle.Text == string.Empty)
                {
                    Errors.DisplayMessage("Project title is required!");
                    return;
                }

                string author;
                string description;
                if (tbAuthor.Text == tempMessages[tbAuthor.Name]) author = "";
                else author = tbAuthor.Text;
                if (tbDescription.Text == tempMessages[tbDescription.Name]) description = "";
                else description = tbDescription.Text;

                List<Head> _data = new List<Head>
                    {
                        new Head()
                        {
                            ProjectName = tbProjectTitle.Text,
                            Author = author,
                            Keywords = lsbKeywords.Items.OfType<string>().ToArray(),
                            Description = description,
                        }
                    };

                string json = JsonConvert.SerializeObject(_data, Formatting.Indented);
                string metadataPath = System.IO.Path.Combine(dir, "metadata.json");
                File.WriteAllText(metadataPath, json);

                if (projectName != tbProjectTitle.Text)
                {
                    string projectsDir = Path.GetDirectoryName(dir);
                    string newNameDir = Path.Combine(projectsDir, $"SWD-{tbProjectTitle.Text}");
                    _contentWindow.Close();
                    ContentWindow.DirectoryCopy(dir, newNameDir, true);
                    Content.ContentWindow gd = new Content.ContentWindow(newNameDir, this);
                    gd.Show();
                    Directory.Delete(dir, true);
                }
                Close();
            }
            catch (Exception ex) { Errors.DisplayMessage(ex.Message);  }
        }

        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tempMessages.ContainsValue(tb.Text)) tb.Text = "";
            tb.Foreground = Brushes.Black;
        }

        private void tb_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == string.Empty) { 
                tb.Text = tempMessages[tb.Name];
                tb.Foreground = Brushes.Gray;
            }
        }

        private void tbKeyword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrEmpty(tbKeyword.Text))
                {
                    btnAdd_Click(sender, e);
                }
            }
        }
    }
}
