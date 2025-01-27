using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SWD
{
    internal class Row
    {
        public string Title { get; set; }
        public List<Cell> Content { get; set; }
    }

    internal class Cell
    {
        public string Title { get; set; }
        public BitmapImage ImageSource { get; set; }
    }
}
