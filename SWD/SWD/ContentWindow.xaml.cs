using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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
using Image = System.Windows.Controls.Image;
using Path = System.IO.Path;
using Window = System.Windows.Window;

namespace SWD
{
    /// <summary>
    /// Logika interakcji dla klasy CreationWindow.xaml
    /// </summary>
    public partial class ContentWindow : Window
    {
        private readonly CreationWindow _creationWindow;
        List<Row> data = new List<Row>();
        //List<FuckYou> dt = new List<FuckYou>();
        public ContentWindow(string path, CreationWindow cw)
        {
            InitializeComponent();
            _creationWindow = cw;
            _creationWindow.Close();

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
                Content = new List<Cell> { new Cell() { Title = "1", ImageSource = NewIcon() } }
            };
            data.Add(obj);
            dgContent.ItemsSource = data;
            BuildDataGrid();
        }

        public BitmapImage NewIcon(string icon = "add.png")
        {
            string projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\.."));
            string imagePath = Path.Combine(projectDirectory, "Icons", icon);
            if (!File.Exists(imagePath))
            {
                Debug.WriteLine($"Image not found: {imagePath}");
            }
            Uri uri = new Uri(imagePath);
            BitmapImage bt = new BitmapImage(uri);

            return bt;
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
            dgContent.Columns.Clear();
            int cols = data[0].Content.Count;

            for (int i = 0; i < cols; i++)
            {
                DataGridTemplateColumn column = new DataGridTemplateColumn()
                {
                    Header = (i + 1).ToString(),
                };

                DataTemplate cellTemplate = new DataTemplate();
                FrameworkElementFactory imageFactory = new FrameworkElementFactory(typeof(Image));
                imageFactory.SetBinding(Image.SourceProperty, new System.Windows.Data.Binding($"Content[{i}].ImageSource"));
                imageFactory.SetValue(Image.OpacityProperty, 0.25);

                cellTemplate.VisualTree = imageFactory;
                column.CellTemplate = cellTemplate;

                dgContent.Columns.Add(column);
                dgContent.Columns[i].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }

            dgContent_Loaded(dgContent, null);
            ButtonHandling();

            dgContent.ItemsSource = null;
            dgContent.ItemsSource = data;

            dgContent.SelectedItem = null;
            dgContent.SelectedIndex = -1;
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
                int amount = Math.Max(Int32.Parse(tb.Text) + 1, 1);
                if (information[1] == "Col")
                {
                    // Add a new column to each existing row in `data`
                    foreach (var row in data)
                    {
                        row.Content.Add(new Cell() { Title = $"{row.Content.Count}_{amount}", ImageSource = NewIcon() });
                    }
                }
                else
                {
                    // Add a new row with the same number of columns as the existing ones
                    int cols = data[0].Content.Count;
                    List<Cell> newRowContent = new List<Cell>();

                    // Add a cell for each column
                    for (int i = 0; i < cols; i++)
                    {
                        newRowContent.Add(new Cell() { Title = $"{amount}_{i}", ImageSource = NewIcon() });
                    }

                    // Add the new row to `data`
                    Row newRow = new Row()
                    {
                        Title = amount.ToString(),
                        Content = newRowContent
                    };

                    data.Add(newRow);
                }

                tb.Text = amount.ToString();
                BuildDataGrid();
                ButtonHandling();
            }
            catch (Exception ex)
            {
                tb.Text = "1";
                Errors.DisplayErrorMessage($"Rows and columns should be numeric.\n\n{ex}");
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
                    }
                }
                else
                {
                    int amount = Math.Max(Int32.Parse(tb.Text) - 1, 1);
                    tb.Text = amount.ToString();

                    Debug.WriteLine(data.Count);
                    if (data.Count != 1) data.RemoveAt(data.Count-1);
                }
                BuildDataGrid();
                ButtonHandling();
            }
            catch (Exception ex)
            {
                tb.Text = "1";
                Errors.DisplayErrorMessage($"Rows and columns should be numeric.\n\n{ex}");
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
                                data[i].Content.Add(new Cell() { Title = $"{i}_{j}", ImageSource = NewIcon() });
                            }
                        }
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
                    }
                    BuildDataGrid();
                    ButtonHandling();
                }
                catch (Exception ex)
                {
                    tb.Text = "1";
                    Errors.DisplayErrorMessage($"Rows and columns should be numeric.\n\n{ex}");
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
                            List<Cell> l = new List<Cell>();
                            for (int j = 0; j < data[0].Content.Count; j++)
                            {
                                l.Add(new Cell() { Title = $"{i}_{j}", ImageSource = NewIcon() });
                            }
                            Row r = new Row()
                            {
                                Title = (i+1).ToString(),
                                Content = l
                            };
                            data.Add(r);
                        }
                    }
                    else
                    {
                        for (int i = data.Count-1; i >= rows; i--)
                        {
                            data.RemoveAt(i);
                        }
                    }
                    BuildDataGrid();
                    ButtonHandling();
                }
                catch (Exception ex)
                {
                    tb.Text = "1";
                    Errors.DisplayErrorMessage($"Rows and columns should be numeric.\n\n{ex}");
                }
            }
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
            string[] information = btn.Name.Split('_');

            System.Windows.Controls.TextBox tb;
            if (information[1] == "Col") tb = tbColModify;
            else tb = tbRowModify;

            try
            {
                if (information[1] == "Col")
                {
                    int column = Int32.Parse(tb.Text);
                    if (column > data[0].Content.Count)
                    {
                        tb.Text = data[0].Content.Count.ToString(); return;
                    } 
                    else if (column < 1)
                    {
                        tb.Text = "1"; return;
                    }
                    else if (column >= 12)
                    {
                        tb.Text = "12"; return;
                    } 
                    else if (column == data[0].Content.Count)
                    {
                        Increase_Click(btn_Col_Increase, null);
                        return;
                    }
                    else
                    {
                        tbColAmount.Text = (Int32.Parse(tbColAmount.Text) + 1).ToString();
                        for (int i = 0; i < data.Count; i++)
                        {
                            data[i].Content.Insert(column, new Cell() { Title = $"{i}_{column}", ImageSource = NewIcon() });
                        }
                    }
                }
                else
                {
                    int row = Int32.Parse(tb.Text);
                    if (row > data.Count)
                    {
                        tb.Text = data.Count.ToString(); return;
                    }
                    else if (row < 1)
                    {
                        tb.Text = "1"; return;
                    }
                    else if (row == data.Count)
                    {
                        Increase_Click(btn_Row_Increase, null);
                        return;
                    }
                    else
                    {
                        tbRowAmount.Text = (Int32.Parse(tbRowAmount.Text) + 1).ToString();
                        List<Cell> l = new List<Cell>();
                        for (int j = 0; j < data[0].Content.Count; j++)
                        {
                            l.Add(new Cell() { Title = $"{row}_{j}", ImageSource = NewIcon() });
                        }
                        Row r = new Row()
                        {
                            Title = (row+1).ToString(),
                            Content = l
                        };
                        data.Insert(row, r);
                        for (int i = 0; i < data.Count; i++)
                        {
                            data[i].Title = (i+1).ToString();
                        }
                        dgContent.ItemsSource = null;
                        dgContent.ItemsSource = data;
                    }
                }
                BuildDataGrid();
                ButtonHandling();
            }
            catch (Exception ex)
            {
                tb.Text = "1";
                Errors.DisplayErrorMessage($"Rows and columns should be numeric.\n\n{ex}");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
            string[] information = btn.Name.Split('_');

            System.Windows.Controls.TextBox tb;
            if (information[1] == "Col") tb = tbColModify;
            else tb = tbRowModify;

            try
            {
                if (information[1] == "Col")
                {
                    int column = Int32.Parse(tb.Text);
                    if (column > data[0].Content.Count)
                    {
                        tb.Text = data[0].Content.Count.ToString(); return;
                    }
                    else if (column < 1)
                    {
                        tb.Text = "1"; return;
                    }
                    else
                    {
                        tbColAmount.Text = (Int32.Parse(tbColAmount.Text) - 1).ToString();
                        for (int i = 0; i < data.Count; i++)
                        {
                            data[i].Content.RemoveAt(column-1);
                        }
                    }
                }
                else
                {
                    int row = Int32.Parse(tb.Text);
                    if (row > data.Count)
                    {
                        tb.Text = data.Count.ToString(); return;
                    }
                    else if (row < 1)
                    {
                        tb.Text = "1"; return;
                    }
                    else
                    {
                        data.RemoveAt(row-1);
                        for (int i = 0; i < data.Count; i++)
                        {
                            data[i].Title = (i + 1).ToString();
                        }
                        dgContent.ItemsSource = null;
                        dgContent.ItemsSource = data;
                    }
                }
                BuildDataGrid();
                ButtonHandling();
            }
            catch (Exception ex)
            {
                tb.Text = "1";
                Errors.DisplayErrorMessage($"Rows and columns should be numeric.\n\n{ex}");
            }
        }

        private void tbModify_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
                try
                {
                    int position = Int32.Parse(tb.Text);
                    if (position < 1)
                    {
                        tb.Text = "1";
                        position = 1;
                    }

                    if (tb.Name == "tbColModify")
                    {
                        if (position >= 12)
                        {
                            tb.Text = "12";
                            position = 12;
                        }
                        if (position > data[0].Content.Count) 
                        {
                            tb.Text = data[0].Content.Count.ToString();
                            position = data[0].Content.Count;
                        };
                    } 
                    else
                    {
                        if (position > data.Count)
                        {
                            tb.Text = data.Count.ToString();
                            position = data.Count;
                        };
                    }
                    ButtonHandling();
                }
                catch (Exception ex)
                {
                    tb.Text = "1";
                    Errors.DisplayErrorMessage($"Rows and columns should be numeric.\n\n{ex}");
                }
            }
        }

        private void ButtonHandling()
        {
            int cols, rows, colsModify, rowsModify;

            try { colsModify = Int32.Parse(tbColModify.Text); }
            catch { colsModify = 1; tbColModify.Text = "1"; }

                if (colsModify == 1) btn_Col_DeleteAt.IsEnabled = false;
                else btn_Col_DeleteAt.IsEnabled = true;
                if (colsModify == 12) btn_Col_InsertAt.IsEnabled = false;
                else btn_Col_InsertAt.IsEnabled = true;

            try { rowsModify = Int32.Parse(tbRowModify.Text); }
            catch { rowsModify = 1; tbRowModify.Text = "1";  }

                if (rowsModify == 1) btn_Row_DeleteAt.IsEnabled = false;
                else btn_Row_DeleteAt.IsEnabled = true;

            try { cols = Int32.Parse(tbColAmount.Text); }
            catch { cols = data[0].Content.Count; tbColAmount.Text = data[0].Content.Count.ToString(); }

                if (cols == 1) { btn_Col_Decrease.IsEnabled = false; btn_Col_DeleteAt.IsEnabled = false; }
                else { btn_Col_Decrease.IsEnabled = true; btn_Col_DeleteAt.IsEnabled = true; }
                if (cols == 12) { btn_Col_Increase.IsEnabled = false; btn_Col_InsertAt.IsEnabled = false; }
                else { btn_Col_Increase.IsEnabled = true; btn_Col_InsertAt.IsEnabled = true; }

            try { rows = Int32.Parse(tbRowAmount.Text); }
            catch { rows = data.Count; tbRowAmount.Text = data.Count.ToString(); }

                if (rows == 1) { btn_Row_Decrease.IsEnabled = false; btn_Row_DeleteAt.IsEnabled = false; }
                else { btn_Row_Decrease.IsEnabled = true; btn_Row_DeleteAt.IsEnabled = true; }
        }


        private void PopupChange(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
            if (tb.Name == "tbColAmount") popCol.IsOpen = !popCol.IsOpen;
            else if (tb.Name == "tbColModify") popColModify.IsOpen = !popColModify.IsOpen;
            else if (tb.Name == "tbRowAmount") popRow.IsOpen = !popRow.IsOpen;       
            else  popRowModify.IsOpen = !popRowModify.IsOpen; 
        }

        private void AddComponent(object sender, RoutedEventArgs e)
        {
            if (dgContent.SelectedCells.Count > 0)
            {
                System.Windows.Controls.MenuItem cm = (System.Windows.Controls.MenuItem)sender;

                foreach (var cell in dgContent.SelectedCells)
                {
                    var row = cell.Item;
                    int rowIndex = dgContent.Items.IndexOf(row);
                    int columnIndex = dgContent.Columns.IndexOf(cell.Column);
                    data[rowIndex].Content[columnIndex].ImageSource = NewIcon($"{cm.Name}.png");
                }
                BuildDataGrid();
            }
            return;
        }
    }
}
