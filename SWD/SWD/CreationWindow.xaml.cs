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

namespace SWD
{
    /// <summary>
    /// Logika interakcji dla klasy CreationWindow.xaml
    /// </summary>
    public partial class CreationWindow : Window
    {
        public string dir = Environment.CurrentDirectory;
        private string tempForProjectTitle = "Insert project title";
        private string tempForAuthor = "Insert author's name";
        private string tempForKeyword = "Insert a keyword";
        private string tempForDescription = "Insert description";
        public Dictionary<string, string> tempMessages = new Dictionary<string, string>();

        public CreationWindow()
        {
            InitializeComponent();
            InitializeEmptyText();
            InitializeMessagesArray();
        }

        public CreationWindow(string d)
        {
            InitializeComponent();
            InitializeEmptyText();
            InitializeMessagesArray();
            dir = d;
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
                tbProjectTitle.Text = "Insert project title";
                tbProjectTitle.Foreground = Brushes.Gray;
            }
            if (string.IsNullOrWhiteSpace(tbAuthor.Text))
            {
                tbAuthor.Text = "Insert author's name";
                tbAuthor.Foreground = Brushes.Gray;
            }            
            if (string.IsNullOrWhiteSpace(tbKeyword.Text))
            {
                tbKeyword.Text = "Insert a keyword";
                tbKeyword.Foreground = Brushes.Gray;
            }
            if (string.IsNullOrWhiteSpace(tbDescription.Text))
            {
                tbDescription.Text = "Insert description";
                tbDescription.Foreground = Brushes.Gray;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //Dodaje słowo kluczowe do listy.
            lsbKeywords.Items.Add(tbKeyword.Text);   
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(lsbKeywords.SelectedIndex.ToString());
            //Usuwam słowo kluczowe z listy.
            lsbKeywords.Items.RemoveAt(lsbKeywords.SelectedIndex);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (tbProjectTitle.Text == tempMessages[tbProjectTitle.Name] || tbProjectTitle.Text == string.Empty) Errors.DisplayErrorMessage("Project title is required!");
            else
            {
                dir = System.IO.Path.Combine(dir, $"Projects");
                dir = System.IO.Path.Combine(dir, $"SWD-{tbProjectTitle.Text}");
                Debug.WriteLine(dir);

                if (Directory.Exists(dir))
                {
                    Errors.DisplayErrorMessage("A project with the same name already exists!"); // Folder w folderze się robi! Błąd!
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

                    //Konwertuję wprowadzone dane na plik JSON, z którego wczytywane będą dane do tagów meta w HTML.
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
              

                    this.Close();
                    ContentWindow gd = new ContentWindow(dir);
                    gd.ShowDialog();
                }
            }
            
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
    }
}
