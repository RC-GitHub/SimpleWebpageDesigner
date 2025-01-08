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
        public string tempForProjectTitle = "Insert project title";
        public string tempForAuthor = "Insert author's name";
        public string tempForKeyword = "Insert a keyword";
        public string tempForDescription = "Insert description";

        public CreationWindow()
        {
            InitializeComponent();
            InitializeEmptyText();

            Debug.WriteLine("Udało się!");

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

        public MessageBoxResult DisplayErrorMessage()
        {
            string messageBoxText = "A project with the same name already exists";
            string caption = "Error!";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            return result;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {

            dir = System.IO.Path.Combine(dir, $"SWD-{tbProjectTitle.Text}");

            if (Directory.Exists(dir))
            {
                DisplayErrorMessage(); // Folder w folderze się robi! Błąd!
            } 
            else
            {

                if (tbProjectTitle.Text != string.Empty && tbProjectTitle.Text != tempForProjectTitle)
                {
                    //Konwertuję wprowadzone dane na plik JSON, z którego wczytywane będą dane do tagów meta w HTML.
                    List<Head> _data = new List<Head>();
                    _data.Add(new Head()
                    {
                        ProjectName = tbProjectTitle.Text,
                        Author = tbAuthor.Text,
                        Keywords = lsbKeywords.Items.OfType<string>().ToArray(),
                        Description = tbDescription.Text,
                    });

                    string json = JsonConvert.SerializeObject(_data, Formatting.Indented);


                    try
                    {
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        Debug.WriteLine(json);
                        File.WriteAllText($"{dir}\\metadata.json", json);
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("The process failed: {0}", err.ToString());
                    }
                    finally { }
                }

                this.Close();
            }
        }

        private void tbProjectTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbProjectTitle.Text == tempForProjectTitle)
            { tbProjectTitle.Text = ""; }
            tbProjectTitle.Foreground = Brushes.Black;
        }

        private void tbProjectTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbProjectTitle.Text == string.Empty)
            { 
                tbProjectTitle.Text = tempForProjectTitle;
                tbProjectTitle.Foreground = Brushes.Gray;
            }
        }

        private void tbAuthor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbAuthor.Text == tempForAuthor)
            { tbAuthor.Text = ""; }
            tbAuthor.Foreground = Brushes.Black;
        }

        private void tbAuthor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbAuthor.Text == string.Empty) 
            { 
                tbAuthor.Text = tempForAuthor;
                tbAuthor.Foreground = Brushes.Gray;
            }

        }

        private void tbKeyword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbKeyword.Text == tempForKeyword)
            { tbKeyword.Text = ""; }
            tbKeyword.Foreground = Brushes.Black;
        }

        private void tbKeyword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbKeyword.Text == string.Empty)
            { 
                tbKeyword.Text = tempForKeyword;
                tbKeyword.Foreground = Brushes.Gray;
            }

        }

        private void tbDescription_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbDescription.Text == tempForDescription)
            { tbDescription.Text = ""; }
            tbDescription.Foreground = Brushes.Black;
        }

        private void tbDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbDescription.Text == string.Empty)
            { 
                tbDescription.Text = tempForDescription;
                tbDescription.Foreground = Brushes.Gray;
            }

        }
    }
}
