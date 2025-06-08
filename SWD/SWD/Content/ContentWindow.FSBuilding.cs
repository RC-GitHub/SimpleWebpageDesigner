using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;
using Window = System.Windows.Window;

namespace SWD.Content
{
    /// <summary>
    /// Partial class for ContentWindow, containing logic for building and handling the file selector DataGrid,
    /// including row header customization, row selection, and column generation.
    /// </summary>
    public partial class ContentWindow : Window
    {
        /// <summary>
        /// Handles the LoadingRow event for the pages DataGrid.
        /// Adds a clickable image header to each row for editing or selection.
        /// </summary>
        /// <param name="sender">The DataGrid control.</param>
        /// <param name="e">Event arguments for the row being loaded.</param>
        private void dgPages_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;

            if (e.Row.GetType() != typeof(DataGridRowHeader))
            {

                var image = new Image
                {
                    Source = Images.NewIcon("edit.png"),
                    Stretch = Stretch.Fill,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                };

                DataGridRowHeader header = new DataGridRowHeader
                {
                    Content = image,
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch,
                };

                header.Content = image;
                int rowIndex = row.GetIndex();
                header.Click += (s, ev) => LoadingRow(s, e, rowIndex);

                row.Header = header;

            }

        }

        /// <summary>
        /// Handles the click event for a row header image, loads the selected page's data,
        /// and updates the UI with the page name and path.
        /// </summary>
        /// <param name="sender">The row header or image that was clicked.</param>
        /// <param name="e">Event arguments for the row.</param>
        /// <param name="v">The index of the row.</param>
        private void LoadingRow(object sender, DataGridRowEventArgs e, int v)
        {
            var row = dgPages.Items[v] as DataRowView;
            if (row != null)
            {
                pageName = row["Filename"].ToString();
                lblGridTitle.Content = $"{row["Filename"]}.html";
                readyPath = row["Path"].ToString();
                LoadJsonPage(readyPath);
            }
        }

        /// <summary>
        /// Handles the AutoGeneratingColumn event for the pages DataGrid.
        /// Cancels the generation of columns for "Path" and "id" properties.
        /// </summary>
        /// <param name="sender">The DataGrid control.</param>
        /// <param name="e">Event arguments for the column being generated.</param>
        private void dgPages_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Path" || e.PropertyName == "id")
            {
                e.Cancel = true;
            }
        }
    }
}
