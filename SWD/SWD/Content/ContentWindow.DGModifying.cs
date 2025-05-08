using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Window = System.Windows.Window;

namespace SWD.Content
{
    public partial class ContentWindow : Window
    {
        private void dgContent_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (components == null) return;

            if (dgContent.SelectedCells.Count == 1)
            {

                int rowIndex = dgContent.Items.IndexOf(dgContent.SelectedCells[0].Item);
                int columnIndex = dgContent.Columns.IndexOf(dgContent.SelectedCells[0].Column);
                Debug.WriteLine($"The key is {data[rowIndex].Content[columnIndex].Title}");
                if (components.ContainsKey(data[rowIndex].Content[columnIndex].Title))
                {
                    DataGridCellInfo firstCell = new DataGridCellInfo(dgContent.Items[rowIndex], dgContent.Columns[columnIndex]);
                    Component component = components[data[rowIndex].Content[columnIndex].Title];
                    component.Spanning();

                    lblComps.Content = $"Component: {component.Name}";
                    tbCompRow.Text = (component.StartRow+1).ToString();
                    tbCompCol.Text = (component.StartColumn+1).ToString();
                    tbCompWidth.Text = (component.Colspan+1).ToString(); 
                    tbCompHeight.Text = (component.Rowspan+1).ToString();

                    try
                    {
                        IList<DataGridCellInfo> cells = new List<DataGridCellInfo>();
                        foreach (Position position in component.Positions)
                        {
                            DataGridCellInfo newCell = new DataGridCellInfo(dgContent.Items[position.Row], dgContent.Columns[position.Column]);
                            if (newCell != firstCell)
                            {
                                dgContent.SelectedCells.Add(newCell);
                            }
                        }
                    }
                    catch (Exception ex) { Errors.DisplayErrorMessage($"{ex}"); }
                }
                else
                {
                    lblComps.Content = "Component: -";
                    tbCompRow.Text = "-";
                    tbCompCol.Text = "-";
                    tbCompWidth.Text = "-";
                    tbCompHeight.Text = "-";
                    return;
                }
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
        private void PopupChange(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
            if (tb.Name == "tbColAmount") popCol.IsOpen = !popCol.IsOpen;
            else if (tb.Name == "tbColModify") popColModify.IsOpen = !popColModify.IsOpen;
            else if (tb.Name == "tbRowAmount") popRow.IsOpen = !popRow.IsOpen;
            else popRowModify.IsOpen = !popRowModify.IsOpen;
        }
    }
}
