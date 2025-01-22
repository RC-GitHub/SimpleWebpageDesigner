using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<Row> data = new ObservableCollection<Row>();
        //DataView dv;
        //DataTable dt = new DataTable();
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
            Row obj = new Row()
            {
                Title = "1",
                Content = new List<string> { "1_1" }
            };
            data.Add(obj);
            dgContent.ItemsSource = data;
            dgContent.Columns.Clear();
            BuildDataGrid();
        }

        private void BuildDataGrid()
        {
            int cols = data[0].Content.Count;

            for (int i = 0; i < cols; i++)
            {
                DataGridTextColumn column = new DataGridTextColumn
                {
                    Header = (i+1).ToString(),
                    Binding = new System.Windows.Data.Binding($"Content[{i}]")
                };
                dgContent.Columns.Add(column);
                /*                double totalWidth = dgContent.ActualWidth - 50;
                                dgContent.Columns[i].Width = Math.Floor(totalWidth / cols);*/
                dgContent.Columns[i].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
/*                if (data.Count <= 12) dgContent.RowHeight = (dgContent.ActualHeight / data.Count;
                else dgContent.RowHeight = dgContent.RenderSize.Height / 12;*/
            }
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
                if (information[1] == "Col")
                {
                    int amount = Math.Max(Int32.Parse(tb.Text) + 1, 1);
                    amount = Math.Min(amount, 12);
                    tb.Text = amount.ToString();

                    if (data[0].Content.Count < 12)
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            data[i].Content.Add($"{i + 1}_{amount}");
                        }
                        dgContent.Columns.Clear();
                        BuildDataGrid();
                    }
                }
                else
                {
                    int amount = Math.Max(Int32.Parse(tb.Text) + 1, 1);
                    tb.Text = amount.ToString();

                    int cols = data[0].Content.Count;
                    List<string> l = new List<string>();
                    for (int i = 0; i < cols; i++)
                    {
                        l.Add($"{amount}_{i+1}");
                    }
                    Row r = new Row()
                    {
                        Title = amount.ToString(),
                        Content = l
                    };
                    data.Add(r);

                    dgContent.Columns.Clear();
                    BuildDataGrid();
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
                if (information[1] == "Col")
                {
                    int amount = Math.Max(Int32.Parse(tb.Text) - 1, 1);
                    amount = Math.Min(amount, 12);
                    tb.Text = amount.ToString();

                    if (data[0].Content.Count != 1)
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            Debug.WriteLine(data[i].Content.Count);
                            data[i].Content.RemoveAt(data[i].Content.Count-1);
                        }
                        dgContent.Columns.Clear();
                        BuildDataGrid();
                    }
                }
                else
                {
                    int amount = Math.Max(Int32.Parse(tb.Text) - 1, 1);
                    tb.Text = amount.ToString();

                    Debug.WriteLine(data.Count);
                    if (data.Count != 1) data.RemoveAt(data.Count-1);
                    dgContent.Columns.Clear();
                    BuildDataGrid();
                }
            }
            catch (Exception err)
            {
                Errors.DisplayErrorMessage($"Rows and columns should be numeric.\n{err}");
            }
        }
    }
}
