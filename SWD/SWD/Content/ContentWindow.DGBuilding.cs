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

namespace SWD.Content
{
    public partial class ContentWindow : Window
    {
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
        public void BuildDataGrid()
        {
            if (components != null)
            {
                foreach (var component in components.Values)
                {
                    foreach (var pos in component.Positions)
                    {
                        if (pos.Row >= 0 && pos.Row < data.Count &&
                            pos.Column >= 0 && pos.Column < data[pos.Row].Content.Count)
                        {
                            Cell cell = data[pos.Row].Content[pos.Column];
                            cell.SetCell(component);

                        }
                    }
                }
            }

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
                cellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new System.Windows.Data.Binding($"Content[{i}].BackgroundColor")));
                cellStyle.Setters.Add(new Setter(DataGridCell.BorderThicknessProperty, new Thickness(10)));
                cellStyle.Setters.Add(new Setter(DataGridCell.BorderBrushProperty, new System.Windows.Data.Binding($"Content[{i}].BorderColor")));
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
    }
}
