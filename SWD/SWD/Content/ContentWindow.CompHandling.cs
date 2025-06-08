using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows;
using Window = System.Windows.Window;
using System.ComponentModel;

namespace SWD.Content
{
    /// <summary>
    /// Partial class for ContentWindow, containing logic for handling component operations
    /// such as adding, editing, deleting, and updating components within the content grid.
    /// </summary>
    public partial class ContentWindow : Window
    {
        /// <summary>
        /// Handles empty or invalid components by removing them from the collection and clearing their data.
        /// </summary>
        /// <param name="component">The component to check and handle.</param>
        /// <returns>True if the component was empty and handled; otherwise, false.</returns>
        private bool EmptyComponentHandling(Component component)
        {
            if (component == null) return false;
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

        /// <summary>
        /// Determines if a new component can be inserted into the selected cells of the grid.
        /// </summary>
        /// <returns>True if insertion is possible; otherwise, false.</returns>
        private bool CanComponentBeInserted()
        {
            if (components == null) return true;
            foreach (var cell in dgContent.SelectedCells)
            {
                int rowIndex = dgContent.Items.IndexOf(cell.Item);
                int columnIndex = dgContent.Columns.IndexOf(cell.Column);
                if (components.ContainsKey(data[rowIndex].Content[columnIndex].Title))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Adds a new component to the grid and the components collection, after user input and color selection.
        /// </summary>
        private void AddComponent(object sender, RoutedEventArgs e)
        {
            if (!CanComponentBeInserted())
            {
                Errors.DisplayMessage("Before creating a component clear out the space occupied by other ones.");
                return;
            }

            InputDialog inputDialog = new InputDialog();
            inputDialog.Owner = this;
            if (inputDialog.ShowDialog() == true)
            {
                string componentName = inputDialog.InputValue;
                if (components != null)
                    componentName = Names.GetUniqueThemeName(componentName, components.Keys.Select(t => t));

                Debug.WriteLine(componentName);

                SolidColorBrush brush = Colors.BrushColorPicker();
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
                            Type = cm.Name.Substring(2).ToLower(),
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

                            if (components != null &&
                                components.ContainsKey(data[rowIndex].Content[columnIndex].Title) &&
                                components[data[rowIndex].Content[columnIndex].Title].DeleteAPosition(new Position() { Row = rowIndex, Column = columnIndex }) == true)
                            { components.Remove(data[rowIndex].Content[columnIndex].Title); }

                            data[rowIndex].Content[columnIndex].Title = component.Name;
                            data[rowIndex].Content[columnIndex].ImageSource = Images.NewIcon($"{component.Type}.png");
                            data[rowIndex].Content[columnIndex].BorderColor = component.BorderColor;
                            data[rowIndex].Content[columnIndex].BackgroundColor = component.BackgroundColor;
                            data[rowIndex].Content[columnIndex].SelectedBorderColor = component.SelectedBorderColor;
                            data[rowIndex].Content[columnIndex].SelectedBackgroundColor = component.SelectedBackgroundColor;
                        }
                        component.Spanning();
                        if (components != null)
                            components.Add(componentName, component);
                        else
                        {
                            components = new Dictionary<string, Component>();
                            components.Add(componentName, component);
                        }

                        BuildDataGrid();
                    }
                }
            }
        }

        /// <summary>
        /// Handles the opening of the cell context menu, showing or hiding edit/delete options based on selection.
        /// </summary>
        private void cellContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (components == null || components.Count == 0)
            {
                miEdit.Visibility = Visibility.Collapsed;
                miDelete.Visibility = Visibility.Collapsed;
                cmActionSeparator.Visibility = Visibility.Collapsed;
                return;
            }
            foreach (var cell in dgContent.SelectedCells)
            {
                int rowIndex = dgContent.Items.IndexOf(cell.Item);
                int columnIndex = dgContent.Columns.IndexOf(cell.Column);
                try
                {
                    if (components.ContainsKey(data[rowIndex].Content[columnIndex].Title))
                    {
                        miEdit.Visibility = Visibility.Visible;
                        miDelete.Visibility = Visibility.Visible;
                        cmActionSeparator.Visibility = Visibility.Visible;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Errors.DisplayMessage(ex.Message);
                }

            }
            miEdit.Visibility = Visibility.Collapsed;
            miDelete.Visibility = Visibility.Collapsed;
            cmActionSeparator.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Opens the component editor window for the currently selected component.
        /// Updates the component in the collection after editing.
        /// </summary>
        private void EditComponent(object sender, RoutedEventArgs e)
        {
            Components.ComponentWindow componentWindow = new Components.ComponentWindow(currentComponent, path);
            componentWindow.Owner = this;
            componentWindow.ShowDialog();
            components[currentComponent.Name] = componentWindow.MyComponent;
            BuildDataGrid(true);
            SaveFile(true);
        }

        /// <summary>
        /// Deletes the selected components from the grid and the components collection.
        /// </summary>
        private void DeleteComponent(object sender, RoutedEventArgs e)
        {
            List<string> componentNames = new List<string>();
            foreach (var cell in dgContent.SelectedCells)
            {
                int rowIndex = dgContent.Items.IndexOf(cell.Item);
                int columnIndex = dgContent.Columns.IndexOf(cell.Column);
                string key = data[rowIndex].Content[columnIndex].Title;
                if (components.ContainsKey(key) && !componentNames.Contains(key))
                {
                    componentNames.Add(key);
                }
            }
            foreach (string name in componentNames)
            {
                components.Remove(name);
            }
            BuildDataGrid(true);
        }

        /// <summary>
        /// Updates the UI to reflect the currently selected component's properties.
        /// </summary>
        private void SelectComponent()
        {
            if (currentComponent == null) return;
            lblComps.Content = $"Component: {currentComponent.Name}";
            tbCompRow.Text = (currentComponent.StartRow + 1).ToString();
            tbCompCol.Text = (currentComponent.StartColumn + 1).ToString();
            tbCompWidth.Text = (currentComponent.Colspan + 1).ToString();
            tbCompHeight.Text = (currentComponent.Rowspan + 1).ToString();
        }

        /// <summary>
        /// Clears the selection and resets the component-related UI fields.
        /// </summary>
        private void DeselectComponent()
        {
            currentComponent = null;
            dgContent.SelectedIndex = -1;

            lblComps.Content = "Component: -";
            tbCompRow.Text = "-";
            tbCompCol.Text = "-";
            tbCompWidth.Text = "-";
            tbCompHeight.Text = "-";
        }

        /// <summary>
        /// Handles changes to the grid structure (adding/removing rows or columns) and updates component positions and spans.
        /// </summary>
        /// <param name="celltype">"Col" for column, otherwise row.</param>
        /// <param name="mode">"add" or "remove".</param>
        /// <param name="position">The index of the row or column being changed.</param>
        private void ChangingComponentHandling(string celltype, string mode, int position)
        {
            if (components == null) return;
            if (celltype == "Col")
            {
                foreach (Component component in components.Values)
                {
                    if (component.ContainsColumn(position))
                    {
                        if (mode == "add") component.Colspan++;
                        else component.Colspan--;
                        component.Repopulate();
                    }
                    else
                    {
                        if (mode == "add" && component.StartColumn > position)
                        {
                            component.StartColumn++;
                        }
                    }
                    component.Repopulate();
                }
            }
            else
            {
                foreach (Component component in components.Values)
                {
                    Debug.WriteLine(component.ContainsRow(position));
                    if (component.ContainsRow(position))
                    {
                        if (mode == "add") component.Rowspan++;
                        else component.Rowspan--;
                        component.Repopulate();
                    }
                    else
                    {
                        if (mode == "add" && component.StartRow > position)
                        {
                            component.StartRow++;
                        }
                    }
                    component.Repopulate();
                }
            }
        }

        /// <summary>
        /// Handles cleanup and adjustment of components after grid structure changes or deletions.
        /// Removes components that are out of bounds or empty.
        /// </summary>
        private void DeletedComponentHandling()
        {
            if (components == null) return;

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
    }
}
