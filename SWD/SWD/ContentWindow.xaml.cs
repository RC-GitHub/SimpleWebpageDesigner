using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
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
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Brush = System.Drawing.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using DataGridCell = System.Windows.Controls.DataGridCell;
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
        Dictionary<string, Component> components = new Dictionary<string, Component>();
        string path = string.Empty;
        string pageName = string.Empty;

        public ContentWindow(string directory, CreationWindow cw, string pagename = "index")
        {
            InitializeComponent();
            _creationWindow = cw;
            _creationWindow.Close();

            path = directory;
            pageName = pagename;

            Row obj = new Row()
            {
                Title = "1",
                Content = new List<Cell> { new Cell() { Title = "1" } }
            };
            data.Add(obj);
            dgContent.ItemsSource = data;
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
            dgContent.Columns.Clear();
            int cols = data[0].Content.Count;

            for (int i = 0; i < cols; i++)
            {
                DataGridTemplateColumn column = new DataGridTemplateColumn()
                {
                    Header = (i + 1).ToString(),
                };

                DataTemplate cellTemplate = new DataTemplate();

                FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));
                FrameworkElementFactory rowDef = new FrameworkElementFactory(typeof(RowDefinition));
                gridFactory.AppendChild(rowDef);
                FrameworkElementFactory colDef = new FrameworkElementFactory(typeof(ColumnDefinition));
                gridFactory.AppendChild(colDef);

                FrameworkElementFactory imageFactory = new FrameworkElementFactory(typeof(Image));
                imageFactory.SetBinding(Image.SourceProperty, new System.Windows.Data.Binding($"Content[{i}].ImageSource"));
                imageFactory.SetValue(Image.HorizontalAlignmentProperty, System.Windows.HorizontalAlignment.Stretch);
                imageFactory.SetValue(Image.VerticalAlignmentProperty, VerticalAlignment.Stretch);
                imageFactory.SetValue(Image.OpacityProperty, 0.25);


                gridFactory.AppendChild(imageFactory);
                gridFactory.SetValue(Grid.BackgroundProperty, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent));

                cellTemplate.VisualTree = gridFactory;
                column.CellTemplate = cellTemplate;

                Style cellStyle = new Style(typeof(DataGridCell));

                Trigger trigger = new Trigger
                {
                    Property = DataGridCell.IsSelectedProperty,
                    Value = true
                };

                trigger.Setters.Add(new Setter(DataGridCell.BorderBrushProperty, new System.Windows.Data.Binding($"Content[{i}].SelectedBorderColor")));
                trigger.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new System.Windows.Data.Binding($"Content[{i}].SelectedBackgroundColor")));

                cellStyle.Triggers.Add(trigger);
                cellStyle.Setters.Add(new Setter(DataGridCell.BorderBrushProperty, new System.Windows.Data.Binding($"Content[{i}].BorderColor")));
                cellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new System.Windows.Data.Binding($"Content[{i}].BackgroundColor")));
                cellStyle.Setters.Add(new Setter(DataGridCell.BorderThicknessProperty, new Thickness(10)));
                cellStyle.Setters.Add(new Setter(DataGridCell.HorizontalContentAlignmentProperty, System.Windows.HorizontalAlignment.Stretch));
                cellStyle.Setters.Add(new Setter(DataGridCell.VerticalContentAlignmentProperty, System.Windows.VerticalAlignment.Stretch));
                cellStyle.Setters.Add(new Setter(DataGridCell.HorizontalAlignmentProperty, System.Windows.HorizontalAlignment.Stretch));
                cellStyle.Setters.Add(new Setter(DataGridCell.VerticalAlignmentProperty, System.Windows.VerticalAlignment.Stretch));

                column.CellStyle = cellStyle;

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
                    foreach (var row in data)
                    {
                        row.Content.Add(new Cell() { Title = $"{row.Content.Count}_{amount}" });
                    }
                }
                else
                {
                    int cols = data[0].Content.Count;
                    List<Cell> newRowContent = new List<Cell>();

                    for (int i = 0; i < cols; i++)
                    {
                        newRowContent.Add(new Cell() { Title = $"{amount}_{i}" });
                    }

                    Row newRow = new Row()
                    {
                        Title = amount.ToString(),
                        Content = newRowContent
                    };

                    data.Add(newRow);
                }

                tb.Text = amount.ToString();
                ChangingComponentHandling(information[1], "add", amount);
                BuildDataGrid();
                ButtonHandling();
            }
            catch (Exception ex)
            {
                if (information[1] == "Col")
                {
                    tb.Text = data[0].Content.Count.ToString();
                }
                else
                {
                    tb.Text = data.Count.ToString();
                }
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
                        ChangingComponentHandling(information[1], "remove", amount);
                        for (int i = 0; i < data.Count; i++)
                        {
                            Debug.WriteLine(data[i].Content.Count);
                            data[i].Content.RemoveAt(data[i].Content.Count - 1);
                        }
                    }
                }
                else
                {
                    int amount = Math.Max(Int32.Parse(tb.Text) - 1, 1);
                    tb.Text = amount.ToString();

                    Debug.WriteLine(data.Count);
                    if (data.Count != 1)
                    {
                        ChangingComponentHandling(information[1], "remove", data.Count - 1);
                        data.RemoveAt(data.Count - 1);
                    }
                }
                DeletedComponentHandling();
                BuildDataGrid();
                ButtonHandling();
            }
            catch (Exception ex)
            {
                if (information[1] == "Col")
                {
                    tb.Text = data[0].Content.Count.ToString();
                }
                else
                {
                    tb.Text = data.Count.ToString();
                }
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
                                data[i].Content.Add(new Cell() { Title = $"{i}_{j}" });
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            for (int j = data[i].Content.Count - 1; j >= columns; j--)
                            {
                                data[i].Content.RemoveAt(j);
                                ChangingComponentHandling("Col", "remove", j);
                            }
                        }

                    }
                    DeletedComponentHandling();
                    BuildDataGrid();
                    ButtonHandling();
                }
                catch (Exception ex)
                {
                    tb.Text = data[0].Content.Count.ToString();
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
                                l.Add(new Cell() { Title = $"{i}_{j}" });
                            }
                            Row r = new Row()
                            {
                                Title = (i + 1).ToString(),
                                Content = l
                            };
                            data.Add(r);
                        }
                    }
                    else
                    {
                        for (int i = data.Count - 1; i >= rows; i--)
                        {
                            data.RemoveAt(i);
                            ChangingComponentHandling("Row", "remove", i);
                        }

                    }
                    DeletedComponentHandling();
                    BuildDataGrid();
                    ButtonHandling();
                }
                catch (Exception ex)
                {
                    tb.Text = data.Count.ToString();
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
                            Cell newCell = new Cell() { Title = $"{i}_{column}" };
                            //Debug.WriteLine($"INFO: {column}, {data[i].Content[column].Title}");
                            if (components.ContainsKey(data[i].Content[column].Title) && data[i].Content[column].ImageSource != Images.NewIcon())
                            {
                                newCell.SetCell(components[data[i].Content[column].Title]);
                            }
                            data[i].Content.Insert(column, newCell);
                        }
                    }
                    ChangingComponentHandling(information[1], "add", column);
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
                            Cell newCell = new Cell() { Title = $"{row}_{j}" };

                            if (components.ContainsKey(data[row].Content[j].Title) && data[row].Content[j].ImageSource != Images.NewIcon())
                            {
                                newCell.SetCell(components[data[row].Content[j].Title]);
                            }
                            l.Add(newCell);
                        }
                        Row r = new Row()
                        {
                            Title = (row + 1).ToString(),
                            Content = l
                        };
                        data.Insert(row, r);
                        for (int i = 0; i < data.Count; i++)
                        {
                            data[i].Title = (i + 1).ToString();
                        }
                        ChangingComponentHandling(information[1], "add", row);
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
                        foreach (Component comp in components.Values)
                        {
                            if (column < comp.StartColumn + 1)
                            {
                                comp.Decrease("column");
                            }
                            else if (column >= comp.StartColumn && comp.ContainsColumn(column - 1))
                            {
                                comp.Colspan--;
                                comp.Repopulate();
                            }
                        }
                        for (int i = 0; i < data.Count; i++)
                        {
                            data[i].Content.RemoveAt(column - 1);
                        }
                        Debug.WriteLine($"DELETE AT {column - 1}");
                        //ChangingComponentHandling(information[1], "remove", column - 1); 

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
                        tbRowAmount.Text = (Int32.Parse(tbRowAmount.Text) - 1).ToString();
                        foreach (Component comp in components.Values)
                        {
                            Debug.WriteLine($"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA {comp.StartRow + 1} and {row}");
                            if (row < comp.StartRow + 1)
                            {
                                comp.Decrease("row");
                            }
                            else if (row >= comp.StartRow + 1 && comp.ContainsRow(row - 1))
                            {
                                comp.Rowspan--;
                                comp.Repopulate();
                            }
                        }
                        data.RemoveAt(row - 1);
                        for (int i = 0; i < data.Count; i++)
                        {
                            data[i].Title = (i + 1).ToString();
                        }
                        dgContent.ItemsSource = null;
                        dgContent.ItemsSource = data;

                        //ChangingComponentHandling(information[1], "remove", row - 2);
                    }
                }
                DeletedComponentHandling();
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
                    DeletedComponentHandling();
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
            catch { rowsModify = 1; tbRowModify.Text = "1"; }

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
            else popRowModify.IsOpen = !popRowModify.IsOpen;
        }

        private void AddComponent(object sender, RoutedEventArgs e)
        {
            InputDialog inputDialog = new InputDialog();
            if (inputDialog.ShowDialog() == true)
            {
                string componentName = inputDialog.InputValue;
                if (components.ContainsKey(componentName))
                {
                    int i = 1;
                    while (components.ContainsKey(componentName))
                    {
                        componentName = $"{componentName}_{i}";
                        i++;
                    }
                }

                Debug.WriteLine(componentName);

                SolidColorBrush brush = Colors.ShowColorPicker();
                if (brush != null)
                {
                    if (dgContent.SelectedCells.Count > 0)
                    {
                        System.Windows.Controls.MenuItem cm = (System.Windows.Controls.MenuItem)sender;
                        SolidColorBrush translucentBrush = brush.Clone();
                        translucentBrush.Opacity = 0.75;

                        Component component = new Component()
                        {
                            Name = componentName,
                            Type = cm.Name,
                            BorderColor = brush,
                            BackgroundColor = translucentBrush,
                            SelectedBorderColor = Colors.CreateASelectedColor(brush, (SolidColorBrush)new BrushConverter().ConvertFrom("#a7c4dd"), 0.5),
                            SelectedBackgroundColor = Colors.CreateASelectedColor(translucentBrush, (SolidColorBrush)new BrushConverter().ConvertFrom("AliceBlue"), 0.5),
                            Positions = new List<Position>()
                        };

                        foreach (var cell in dgContent.SelectedCells)
                        {

                            int rowIndex = dgContent.Items.IndexOf(cell.Item);
                            int columnIndex = dgContent.Columns.IndexOf(cell.Column);
                            component.Positions.Add(new Position() { Row = rowIndex, Column = columnIndex });

                            if (components.ContainsKey(data[rowIndex].Content[columnIndex].Title) && components[data[rowIndex].Content[columnIndex].Title].DeleteAPosition(new Position() { Row = rowIndex, Column = columnIndex }) == true) { components.Remove(data[rowIndex].Content[columnIndex].Title); }

                            data[rowIndex].Content[columnIndex].Title = component.Name;
                            data[rowIndex].Content[columnIndex].ImageSource = Images.NewIcon($"{component.Type}.png");
                            data[rowIndex].Content[columnIndex].BorderColor = component.BorderColor;
                            data[rowIndex].Content[columnIndex].BackgroundColor = component.BackgroundColor;
                            data[rowIndex].Content[columnIndex].SelectedBorderColor = component.SelectedBorderColor;
                            data[rowIndex].Content[columnIndex].SelectedBackgroundColor = component.SelectedBackgroundColor;
                        }
                        component.Spanning();
                        components.Add(componentName, component);
                        BuildDataGrid();
                    }
                }
            }
        }

        private void dgContent_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //DeletedComponentHandling();
            if (dgContent.SelectedCells.Count == 1)
            {
                //foreach (var cell in dgContent.SelectedCells)
                //{
                int rowIndex = dgContent.Items.IndexOf(dgContent.SelectedCells[0].Item);
                int columnIndex = dgContent.Columns.IndexOf(dgContent.SelectedCells[0].Column);
                Debug.WriteLine($"The key is {data[rowIndex].Content[columnIndex].Title}");
                if (components.ContainsKey(data[rowIndex].Content[columnIndex].Title))
                {
                    DataGridCellInfo firstCell = new DataGridCellInfo(dgContent.Items[rowIndex], dgContent.Columns[columnIndex]);
                    Component component = components[data[rowIndex].Content[columnIndex].Title];
                    component.Spanning();

                    try
                    {
                        IList<DataGridCellInfo> cells = new List<DataGridCellInfo>();
                        foreach (Position position in component.Positions)
                        {
                            DataGridCellInfo newCell = new DataGridCellInfo(dgContent.Items[position.Row], dgContent.Columns[position.Column]);
                            //component.DisplayPositions();
                            if (newCell != firstCell)
                            {
                                dgContent.SelectedCells.Add(newCell);
                            }
                        }

                    }

                    catch (Exception ex)
                    {
                        Errors.DisplayErrorMessage($"{ex}");
                        //EmptyComponentHandling(component);
                    }

                } else
                {
                    return;
                }
                //}
            }

        }

        private void dgContent_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                dgContent.SelectedCells.Clear();
                e.Handled = true;
            }
        }

        private void DeletedComponentHandling()
        {
            List<string> itemsToRemove = new List<string>();

            foreach (var item in components.Values.ToList())
            {
                if (EmptyComponentHandling(item)) { continue; }
                Debug.WriteLine($"Essential info: MAxRow{item.MaxRow()}, Count: {data.Count}, Sum: {item.StartRow + item.Rowspan + 1}");

                if (item.MaxRow() >= data.Count || item.MaxRow() > item.StartRow + item.Rowspan + 1)
                {

                    while (item.MaxRow() >= data.Count || item.MaxRow() > item.StartRow + item.Rowspan + 1)
                    {
                        item.Rowspan--;
                        item.Repopulate();
                    }
                }

                if (item.MaxColumn() >= data[0].Content.Count || item.MaxColumn() > item.StartColumn + item.Colspan + 1)
                {
                    while (item.MaxColumn() >= data[0].Content.Count || item.MaxColumn() > item.StartColumn + item.Colspan + 1)
                    {
                        item.Colspan--;
                        item.Repopulate();
                    }
                }

                if (EmptyComponentHandling(item))
                {
                    itemsToRemove.Add(item.Name);
                }
            }

            foreach (var name in itemsToRemove)
            {
                components.Remove(name);
            }
        }

        private bool EmptyComponentHandling(Component component)
        {
            if (component.Rowspan < 0 || component.Colspan < 0 || component.Positions.Count == 0)
            {
                components.Remove(component.Name);
                component.ClearData();
                return true;
            }
            else
            {
                component.DisplayPositions();
                return true;
            }
        }

        private void ChangingComponentHandling(string celltype, string mode, int position)
        {
            if (celltype == "Col")
            {
                foreach (Component component in components.Values)
                {
                    if (component.ContainsColumn(position))
                    {
                        if (mode == "add") { component.Colspan++; } else { component.Colspan--; }
                        component.Repopulate();
                    }
                }
            }
            else
            {
                foreach (Component component in components.Values)
                {
                    Debug.WriteLine(component.ContainsRow(position));
                    if (component.ContainsRow(position))
                    {
                        if (mode == "add") { component.Rowspan++; } else { component.Rowspan--; }
                        component.Repopulate();
                    }
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, LimitedComponent> _limited = new Dictionary<string, LimitedComponent>();
            foreach (KeyValuePair<string, Component> entry in components) {
                _limited.Add(entry.Key, new LimitedComponent(entry.Value));
            }


            List<ContentStructure> _data = new List<ContentStructure>
            {
                new ContentStructure()
                {
                    Components = _limited,
                    RowAmount = data.Count,
                    ColAmount = data[0].Content.Count
                }
            };

            string json = JsonConvert.SerializeObject(_data, Newtonsoft.Json.Formatting.Indented);

            try
            {
                string newPath = System.IO.Path.Combine(path, $"json");
                if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);
                Debug.WriteLine(json);
                File.WriteAllText($"{newPath}\\{pageName}.json", json);

                Infos.DisplayErrorMessage($"The {pageName} was saved!");
                RefreshFileData();
                
            }
            catch (Exception err)
            {
                Console.WriteLine("The process failed: {0}", err.ToString());
            }
            finally { }
        }

        public static DataTable MakeDataTable()
        {
            DataTable dataTable = new DataTable("Files");

            DataColumn idColumn = new DataColumn();
            idColumn.DataType = System.Type.GetType("System.Int32");
            idColumn.ColumnName = "id";
            idColumn.AutoIncrement = true;
            dataTable.Columns.Add(idColumn);

            DataColumn folderStructureColumn = new DataColumn();
            folderStructureColumn.DataType = System.Type.GetType("System.String");
            folderStructureColumn.ColumnName = "Folder structure";
            folderStructureColumn.DefaultValue = "Folder structure";
            dataTable.Columns.Add(folderStructureColumn);

            DataColumn fileNameColumn = new DataColumn();
            fileNameColumn.DataType = System.Type.GetType("System.String");
            fileNameColumn.ColumnName = "Filename";
            fileNameColumn.DefaultValue = "Filename";
            dataTable.Columns.Add(fileNameColumn);

            DataColumn filePathColumn = new DataColumn();
            filePathColumn.DataType = System.Type.GetType("System.String");
            filePathColumn.ColumnName = "Path";
            filePathColumn.DefaultValue = "Path";
            dataTable.Columns.Add(filePathColumn);

            DataColumn[] keys = new DataColumn[1];
            keys[0] = idColumn;
            dataTable.PrimaryKey = keys;

            return dataTable;
        }

        public DataTable dataTable = MakeDataTable();

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            InputDialog inputDialog = new InputDialog();
            if (inputDialog.ShowDialog() == true)
            {
                string newFileName = inputDialog.InputValue;
                List<ContentStructure> _data = new List<ContentStructure>
                {
                    new ContentStructure()
                    {
                        Components = {},
                        RowAmount = 1,
                        ColAmount = 1
                    }
                };

                string json = JsonConvert.SerializeObject(_data, Newtonsoft.Json.Formatting.Indented);
                string newPath = System.IO.Path.Combine(path, $"json");
                if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);
                Debug.WriteLine(json);
                File.WriteAllText($"{newPath}\\{newFileName}.json", json);
                Infos.DisplayErrorMessage($"The {newFileName} was created!");

                RefreshFileData();

            }
          
        }

        public void RefreshFileData()
        {

            dataTable = MakeDataTable();

            string newPath = System.IO.Path.Combine(path, $"json");
            Debug.WriteLine($"{newPath} is JSONS");

            try
            {
                DirectoryInfo place = new DirectoryInfo(newPath);
                FileInfo[] Files = place.GetFiles();

                foreach (string file in System.IO.Directory.GetFiles(newPath, "*", SearchOption.AllDirectories))
                {
                    Debug.WriteLine(file, "What even is that");
                    DataRow newRow;
                    newRow = dataTable.NewRow();

                    //newRow["Filename"] = file.Name.Split('.')[0];
                    string fileAndFolder = file.Split(new string[] { path }, StringSplitOptions.None)[1];
                    string[] split = fileAndFolder.Split('\\');
                    string filename = split[split.Length - 1].Split('.')[0];
                    string folderStructure = "";
                    Debug.WriteLine(String.Join(", ", split));

                    for (int i = 0; i <= split.Length - 2; i++)
                    {
                        if (split[i] != String.Empty) folderStructure += split[i] + " \\ ";
                    }

                    newRow["Folder structure"] = folderStructure;
                    newRow["Filename"] = filename;
                    newRow["Path"] = file;
                    dataTable.Rows.Add(newRow);
                }
                dgPages.ItemsSource = dataTable.AsDataView();
            }
            catch (Exception ex)
            {
                Errors.DisplayErrorMessage($"Components haven't been saved.\n\n{ex}");
            }
        }

        private void dgPages_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;
            if (e.Row.GetType() != typeof(DataGridRowHeader))
            {
                DataGridRowHeader header = new DataGridRowHeader();

                var image = new Image
                {
                    Source = Images.NewIcon(),
                };

                header.Content = image;
                header.Click += System.Windows.RoutedEventHandler(LoadingRow(sender, e, row.GetIndex()));

                row.Header = header;
                ;
            }
            //base.LoadingRow(e);
        }

        private void LoadingRow(object sender, DataGridRowEventArgs e, int v)
        {
            Debug.WriteLine($"{v} KNICK");
        }
    }
} 
