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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Window = System.Windows.Window;

namespace SWD
{
    /// <summary>
    /// Logika interakcji dla klasy CreationWindow.xaml
    /// </summary>
    public partial class ContentWindow : Window
    {
        ObservableCollection<Row> data = new ObservableCollection<Row>();
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
        private void dgContent_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.DataGrid dg = (System.Windows.Controls.DataGrid)sender;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (dg.ActualHeight > 0)
                {
                    double availableHeight = dg.ActualHeight - 30 - 2;
                    if (data.Count <= 12)
                    {
                        dg.RowHeight = availableHeight / data.Count;
                    }
                    else
                    {
                        dg.RowHeight = availableHeight / 12;
                    }
                    dg.RowHeight = Math.Max(dg.RowHeight, 1);
                }
            }), System.Windows.Threading.DispatcherPriority.Loaded);
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
                dgContent.Columns[i].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            dgContent_Loaded(dgContent, null);
            ButtonHandling();
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
                ButtonHandling();
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
                ButtonHandling();
            }
            catch (Exception err)
            {
                Errors.DisplayErrorMessage($"Rows and columns should be numeric.\n{err}");
            }
        }

        private void tbColAmount_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
                try
                {
                    int columns = Int32.Parse(tb.Text);
                    if (columns == data[0].Content.Count) return;

                    if (columns >= 12)
                    {
                        tb.Text = "12";
                        columns = 12;
                    }
                    if (columns < 1)
                    {
                        tb.Text = "1";
                        columns = 1;
                    }

                    if (columns > data[0].Content.Count)
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            for (int j = data[i].Content.Count; j < columns; j++)
                            {
                                data[i].Content.Add($"{i + 1}_{j + 1}");
                            }
                        }
                        dgContent.Columns.Clear();
                        BuildDataGrid();
                    }
                    else
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            for (int j = data[i].Content.Count-1; j >= columns; j--)
                            {
                                data[i].Content.RemoveAt(j);
                            }
                        }
                        dgContent.Columns.Clear();
                        BuildDataGrid();
                    }
                    ButtonHandling();
                }
                catch (Exception err)
                {
                    Errors.DisplayErrorMessage($"Rows and columns should be numeric.\n{err}");
                }
            }
            
        }

        private void tbRowAmount_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
                try
                {
                    int rows = Int32.Parse(tb.Text);
                    if (rows == data.Count) return;

                    if (rows < 1)
                    {
                        tb.Text = "1";
                        rows = 1;
                    }

                    if (rows > data.Count)
                    {
                        for (int i = data.Count; i < rows; i++)
                        {
                            List<string> l = new List<string>();
                            for (int j = 0; j < data[0].Content.Count; j++)
                            {
                                l.Add($"{i + 1}_{j + 1}");
                            }
                            Row r = new Row()
                            {
                                Title = (i+1).ToString(),
                                Content = l
                            };
                            data.Add(r);
                        }
                        dgContent.Columns.Clear();
                        BuildDataGrid();
                    }
                    else
                    {
                        for (int i = data.Count-1; i >= rows; i--)
                        {
                            data.RemoveAt(i);
                        }
                        dgContent.Columns.Clear();
                        BuildDataGrid();
                    }
                    ButtonHandling();
                }
                catch (Exception err)
                {
                    Errors.DisplayErrorMessage($"Rows and columns should be numeric.\n{err}");
                }
            }
        }

        private void PopupOpen(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
            if (tb.Name == "tbColAmount") 
            { 
                popCol.IsOpen = true; 
            } 
            else if (tb.Name == "tbColModify")
            {
                popColModify.IsOpen = true;
            } 
            else if (tb.Name == "tbRowAmount")
            {
                popRow.IsOpen = true;
            }
            else { popRowModify.IsOpen = true; }
        }

        private void PopupClose(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
            if (tb.Name == "tbColAmount")
            {
                popCol.IsOpen = false;
            }
            else if (tb.Name == "tbColModify")
            {
                popColModify.IsOpen = false;
            }
            else if (tb.Name == "tbRowAmount")
            {
                popRow.IsOpen = false;
            }
            else { popRowModify.IsOpen = false; }
        }

        private void ButtonHandling()
        {
            int cols = Int32.Parse(tbColAmount.Text);
            int rows = Int32.Parse(tbRowAmount.Text);            
            int colsModify = Int32.Parse(tbColModify.Text);
            int rowsModify = Int32.Parse(tbRowModify.Text);

            if (cols == 1) btn_Col_Decrease.IsEnabled = false;
            else btn_Col_Decrease.IsEnabled = true;
            if (cols == 12) btn_Col_Increase.IsEnabled = false;
            else btn_Col_Increase.IsEnabled = true;

            if (rows == 1) btn_Row_Decrease.IsEnabled = false;
            else btn_Row_Decrease.IsEnabled = true;

            if (colsModify == 1) btn_Col_DeleteAt.IsEnabled = false;
            else btn_Col_DeleteAt.IsEnabled = true;
            if (colsModify == 12) btn_Col_InsertAt.IsEnabled = false;
            else btn_Col_InsertAt.IsEnabled = true;

            if (rowsModify == 1) btn_Row_DeleteAt.IsEnabled = false;
            else btn_Row_DeleteAt.IsEnabled = true;
        }
    }
}
