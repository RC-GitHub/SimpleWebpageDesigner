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
    public partial class ContentWindow : Window
    {
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
                ;
            }

        }

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
    }
}
