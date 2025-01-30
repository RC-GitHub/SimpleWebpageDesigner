using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace SWD
{
    internal class Head
    {
        public string ProjectName { get; set; }
        public string Author {  get; set; }
        public string[] Keywords { get; set; }
        public string Description { get; set; }

    }

    internal class Row
    {
        public string Title { get; set; }
        public List<Cell> Content { get; set; }
    }

    internal class Cell
    {
        public string Title { get; set; }
        public BitmapImage ImageSource { get; set; }
        public SolidColorBrush BorderColor { get; set; }
        public SolidColorBrush BackgroundColor { get; set; }        
        public SolidColorBrush SelectedBorderColor { get; set; }
        public SolidColorBrush SelectedBackgroundColor { get; set; }
        public Cell()
        {
            BorderColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c2dfedF0"));
            SelectedBorderColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#a7c4dd"));
            SelectedBackgroundColor = Brushes.AliceBlue;
        }
    }

    internal class Component
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Rowspan { get; set; }
        public int Colspan { get; set; }        
        public int StartRow { get; set; }
        public int StartColumn { get; set; }
        public SolidColorBrush BorderColor { get; set; }
        public SolidColorBrush BackgroundColor { get; set; }
        public SolidColorBrush SelectedBorderColor { get; set; }
        public SolidColorBrush SelectedBackgroundColor { get; set; }
        public List<Position> Positions { get; set; }

        public void Spanning()
        {
            int minRow = Positions.Min(p => p.Row);
            int maxRow = Positions.Max(p => p.Row);
            int minColumn = Positions.Min(p => p.Column);
            int maxColumn = Positions.Max(p => p.Column);
            StartRow = minRow;
            StartColumn = minColumn;
            Rowspan = maxRow - minRow;
            Colspan = maxColumn - minColumn;
        }
    }

    internal class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }

}
