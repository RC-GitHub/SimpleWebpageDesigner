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

namespace SWD
{
    /// <summary>
    /// Partial class for ContentWindow, containing logic for modifying DataGrid content,
    /// such as handling selection changes, keyboard shortcuts, and popup toggling.
    /// </summary>
    public partial class ContentWindow : Window
    {
        /// <summary>
        /// Handles the event when the selected cells in the DataGrid change.
        /// Selects the corresponding component if a single cell is selected, or deselects if not.
        /// </summary>
        /// <param name="sender">The DataGrid control.</param>
        /// <param name="e">Event arguments for selected cells change.</param>
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

                    currentComponent = component;
                    SelectComponent();

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
                    catch (Exception ex) { Errors.DisplayMessage(ex.Message); }
                }
                else
                {
                    DeselectComponent();
                    return;
                }
            }
        }

        /// <summary>
        /// Handles the PreviewKeyDown event for the DataGrid.
        /// Clears selection if Ctrl is pressed.
        /// </summary>
        /// <param name="sender">The DataGrid control.</param>
        /// <param name="e">Key event arguments.</param>
        private void dgContent_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                dgContent.SelectedCells.Clear();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the event to toggle popups for column, row, and component property editing.
        /// </summary>
        /// <param name="sender">The TextBox that triggered the event.</param>
        /// <param name="e">Routed event arguments.</param>
        private void PopupChange(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
            if (tb.Name == "tbColAmount") popCol.IsOpen = !popCol.IsOpen;
            else if (tb.Name == "tbColModify") popColModify.IsOpen = !popColModify.IsOpen;
            else if (tb.Name == "tbRowAmount") popRow.IsOpen = !popRow.IsOpen;
            else if (tb.Name == "tbRowModify") popRowModify.IsOpen = !popRowModify.IsOpen;
            else if (tb.Name == "tbCompRow") popCompRow.IsOpen = !popCompRow.IsOpen;
            else if (tb.Name == "tbCompCol") popCompCol.IsOpen = !popCompCol.IsOpen;
            else if (tb.Name == "tbCompWidth") popCompWidth.IsOpen = !popCompWidth.IsOpen;
            else if (tb.Name == "tbCompHeight") popCompHeight.IsOpen = !popCompHeight.IsOpen;
        }
    }
}
