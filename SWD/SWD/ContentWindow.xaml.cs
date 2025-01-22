using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SWD
{
    /// <summary>
    /// Logika interakcji dla klasy CreationWindow.xaml
    /// </summary>
    public partial class ContentWindow : Window
    {
        DataView dv;
        DataTable dt = new DataTable();
        public ContentWindow(string path)
        {

            InitializeComponent();
            string[] files = Directory.GetFiles(path);
            foreach (string file in files) {
                if (File.Exists(file))
                {
                    Debug.WriteLine(file);
                }
            }
            Debug.WriteLine(files);

            dt.Columns.Add(new DataColumn("1", typeof(Int32)));
            dv = new DataView(dt);
            dgContent.ItemsSource = dv;
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
            string[] information = btn.Name.Split('_');

            System.Windows.Controls.TextBox tb;
            if (information[1] == "Col") tb = tbColAmount;
            else tb = tbRowAmount;

            try
            {
                int amount = Math.Min(Math.Max(Int32.Parse(tb.Text) + 1, 1), 12);
                tb.Text = amount.ToString();
                if (information[1] == "Col")
                {
                    dt.Columns.Add(new DataColumn($"{amount}", typeof(Int32)));
                    dv = new DataView(dt);
                    dgContent.ItemsSource = dv;
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    dv = new DataView(dt);
                    dgContent.ItemsSource = dv;
                }
            }
            catch (Exception err)
            {
                Errors.DisplayErrorMessage($"Rows and columns should be numeric.\n{err}");
            }
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
            string[] information = btn.Name.Split('_');

            System.Windows.Controls.TextBox tb;
            if (information[1] == "Col") tb = tbColAmount;
            else tb = tbRowAmount;

            try
            {
                int amount = Math.Min(Math.Max(Int32.Parse(tb.Text) - 1, 1), 12);
                tb.Text = amount.ToString();
            }
            catch (Exception err)
            {
                Errors.DisplayErrorMessage($"Rows and columns should be numeric.\n{err}");
            }
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            // Iterate through each DataGridRow and assign a header (e.g., row index)
            for (int i = 0; i < dgContent.Items.Count; i++)
            {
                var row = (DataGridRow)dgContent.ItemContainerGenerator.ContainerFromIndex(i);
                if (row != null)
                {
                    // Assign the row header (e.g., row index or custom string)
                    row.Header = "Row " + (i + 1);  // Or any custom header value
                }
            }
        }
    }
}
