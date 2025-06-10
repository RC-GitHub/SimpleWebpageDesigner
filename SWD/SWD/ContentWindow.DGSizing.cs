using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Window = System.Windows.Window;

namespace SWD
{
    /// <summary>
    /// Partial class for ContentWindow, containing logic for resizing the DataGrid,
    /// including adding, removing, and modifying rows and columns.
    /// </summary>
    public partial class ContentWindow : Window
    {
        /// <summary>
        /// Handles the click event to increase the number of columns or rows in the DataGrid.
        /// </summary>
        /// <param name="sender">The button that triggered the event.</param>
        /// <param name="e">Routed event arguments.</param>
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
                Errors.DisplayMessage($"Rows and columns should be numeric.\n\n{ex}");
            }
        }

        /// <summary>
        /// Handles the click event to decrease the number of columns or rows in the DataGrid.
        /// </summary>
        /// <param name="sender">The button that triggered the event.</param>
        /// <param name="e">Routed event arguments.</param>
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
                Errors.DisplayMessage($"Rows and columns should be numeric.\n\n{ex}");
            }
        }

        /// <summary>
        /// Handles the PreviewKeyDown event for the column amount TextBox.
        /// Adjusts the number of columns in the DataGrid when Enter is pressed.
        /// </summary>
        /// <param name="sender">The TextBox control.</param>
        /// <param name="e">Key event arguments.</param>
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
                    Errors.DisplayMessage($"Rows and columns should be numeric.\n\n{ex}");
                }
            }
        }

        /// <summary>
        /// Handles the PreviewKeyDown event for the row amount TextBox.
        /// Adjusts the number of rows in the DataGrid when Enter is pressed.
        /// </summary>
        /// <param name="sender">The TextBox control.</param>
        /// <param name="e">Key event arguments.</param>
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
                    Errors.DisplayMessage($"Rows and columns should be numeric.\n\n{ex}");
                }
            }
        }

        /// <summary>
        /// Handles the click event to insert a new column or row at a specified position in the DataGrid.
        /// </summary>
        /// <param name="sender">The button that triggered the event.</param>
        /// <param name="e">Routed event arguments.</param>
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
                    int column = Int32.Parse(tb.Text) - 1;
                    if (column + 1 > data[0].Content.Count)
                    {
                        tb.Text = data[0].Content.Count.ToString(); return;
                    }
                    else if (column < 0)
                    {
                        tb.Text = "1"; return;
                    }
                    else if (column + 1 >= 12)
                    {
                        tb.Text = "12"; return;
                    }
                    else if (column + 1 == data[0].Content.Count)
                    {
                        Increase_Click(btn_Col_Increase, null);
                        return;
                    }
                    else
                    {
                        tbColAmount.Text = (Int32.Parse(tbColAmount.Text) + 1).ToString();
                        for (int i = 0; i < data.Count; i++)
                        {
                            Cell newCell = new Cell();
                            newCell.Title = $"{i}_{column + 1}";
                            Debug.WriteLine($"INFO: {column}, {data[i].Content[column].Title}");
                            if (components.ContainsKey(data[i].Content[column].Title) && data[i].Content[column].ImageSource != Images.NewIcon())
                            {
                                newCell.SetCell(components[data[i].Content[column].Title]);
                            }
                            data[i].Content.Insert(column + 1, newCell);
                        }
                    }
                    ChangingComponentHandling(information[1], "add", column);
                }
                else
                {
                    int row = Int32.Parse(tb.Text) - 1;
                    if (row + 1 > data.Count)
                    {
                        tb.Text = data.Count.ToString(); return;
                    }
                    else if (row < 0)
                    {
                        tb.Text = "1"; return;
                    }
                    else if (row + 1 == data.Count)
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
                            Cell newCell = new Cell();
                            newCell.Title = $"{row + 1}_{j}";

                            //Debug.WriteLine($"INFO: {row}, {data[row].Content[j].Title}");
                            if (components.ContainsKey(data[row].Content[j].Title) && data[row].Content[j].ImageSource != Images.NewIcon())
                            {
                                Debug.WriteLine("contains");
                                newCell.SetCell(components[data[row].Content[j].Title]);
                            }
                            l.Add(newCell);
                        }
                        Row r = new Row()
                        {
                            Title = (row + 1).ToString(),
                            Content = l
                        };
                        data.Insert(row + 1, r);
                        for (int i = 0; i < data.Count; i++)
                        {
                            data[i].Title = (i + 1).ToString();
                        }
                        ChangingComponentHandling(information[1], "add", row);
                    }
                }
                SelectComponent();
                BuildDataGrid(true);
                ButtonHandling();
            }
            catch (Exception ex)
            {
                tb.Text = "1";
                Errors.DisplayMessage($"Rows and columns should be numeric.\n\n{ex}");
            }
        }

        /// <summary>
        /// Handles the click event to delete a column or row at a specified position in the DataGrid.
        /// </summary>
        /// <param name="sender">The button that triggered the event.</param>
        /// <param name="e">Routed event arguments.</param>
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
                    }
                }
                DeletedComponentHandling();
                BuildDataGrid();
                ButtonHandling();
            }
            catch (Exception ex)
            {
                tb.Text = "1";
                Errors.DisplayMessage($"Rows and columns should be numeric.\n\n{ex}");
            }
        }

        /// <summary>
        /// Handles the PreviewKeyDown event for the column/row modify TextBox.
        /// Validates and adjusts the position value when Enter is pressed.
        /// </summary>
        /// <param name="sender">The TextBox control.</param>
        /// <param name="e">Key event arguments.</param>
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
                    Errors.DisplayMessage($"Rows and columns should be numeric.\n\n{ex}");
                }
            }
        }

        /// <summary>
        /// Handles enabling and disabling of buttons based on the current state of columns and rows.
        /// </summary>
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

    }
}
