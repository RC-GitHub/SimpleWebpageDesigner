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
        public string dir;
        public CreationWindow(string directory)
        {
            InitializeComponent();
            dir = directory;
            if (string.IsNullOrWhiteSpace(tbAuthor.Text))
            {
                tbAuthor.Text = "Insert author's name";
            }
            Debug.WriteLine("Udało się!");
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
            List<Head> _data = new List<Head>();
            _data.Add(new Head()
            {
                Author = tbAuthor.Text,
                Copyright = "XXX",
                Keywords = lsbKeywords.Items.OfType<string>().ToArray(),
                Description = tbDescription.Text,
            });

            string json = JsonConvert.SerializeObject(_data, Formatting.Indented);
            //string json = JsonSerializer.Serialize(_data);
            Debug.WriteLine(dir);
            Debug.WriteLine(json);
            File.WriteAllText($"{dir}/path.json", json);
        }
    }
}
