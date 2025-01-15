using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SWD
{
    /// <summary>
    /// Logika interakcji dla klasy GridWindow.xaml
    /// </summary>
    public partial class GridWindow : Window
    {
        public GridWindow(string path)
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
        }
    }
}
