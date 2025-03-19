using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Diagnostics;
using System.IO;

namespace SWD
{
    internal class Head
    {
        public string ProjectName { get; set; }
        public string Author {  get; set; }
        public string[] Keywords { get; set; }
        public string Description { get; set; }

    }

    public class DataItem
    {
        public string Column { get; set; }
    }

    internal class ContentStructure
    {
        public Dictionary<string, LimitedComponent> Components { get; set; }
        public int RowAmount { get; set; }
        public int ColAmount { get; set; }
    }

    internal class LimitedComponent
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Rowspan { get; set; }
        public int Colspan { get; set; }
        public int StartRow { get; set; }
        public int StartColumn { get; set; }

        public LimitedComponent(Component comp)
        {
            Name = comp.Name;
            Type = comp.Type;
            Rowspan = comp.Rowspan;
            Colspan = comp.Colspan;
            StartRow = comp.StartRow;
            StartColumn = comp.StartColumn;
        }
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
            ImageSource = Images.NewIcon();
            BorderColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c2dfedF0"));
            SelectedBorderColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#a7c4dd"));
            SelectedBackgroundColor = Brushes.AliceBlue;
        }

        public void SetCell(Component component)
        {
            Title = component.Name;
            ImageSource = Images.NewIcon($"{component.Type}.png");
            BorderColor = component.BorderColor;
            BackgroundColor = component.BackgroundColor;
            SelectedBorderColor = component.SelectedBorderColor;
            SelectedBackgroundColor = component.SelectedBackgroundColor;
        }
    }

    internal class Images
    {
        public static BitmapImage NewIcon(string icon = "add.png")
        {
            string projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\.."));
            string imagePath = Path.Combine(projectDirectory, "Icons", icon);
            if (!File.Exists(imagePath))
            {
                Debug.WriteLine($"Image not found: {imagePath}");
            }
            Uri uri = new Uri(imagePath);
            BitmapImage bt = new BitmapImage(uri);

            return bt;
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
            StartRow = MinRow();
            StartColumn = MinColumn();
            Rowspan = MaxRow() - StartRow;
            Colspan = MaxColumn() - StartColumn;
        }

        public void Decrease(string rowOrColumn) 
        {
            if (rowOrColumn == "column")
            {
                StartColumn--;
            } else
            {
                StartRow--;
            }
        }

        public int MinRow() { return Positions.Min(p => p.Row); }
        public int MaxRow() { return Positions.Max(p => p.Row); }
        public int MinColumn() { return Positions.Min(p => p.Column); }
        public int MaxColumn() { return Positions.Max(p => p.Column); }

        public bool ContainsRow(int row)
        {
            foreach (var position in Positions)
            {
                if (position.Row == row) return true;
            }
            return false;
        }
        public bool ContainsColumn(int column)
        {
            foreach (var position in Positions)
            {
                if (position.Column == column) return true;
            }
            return false;
        }

        public void Repopulate()
        {
            Positions = new List<Position>();
            for (int i = StartRow; i < StartRow + Rowspan + 1; i++)
            {
                for (int j = StartColumn; j < StartColumn + Colspan + 1; j++)
                {
                    Positions.Add(new Position() { Row = i, Column = j });
                }
            }
        }

        public void DisplayPositions()
        {
            foreach(var position in Positions)
            {
                Debug.WriteLine($"Row: {position.Row+1}, Column: {position.Column+1}");
            }
        }

        public void ClearData()
        {
            Positions.Clear();
        }

        public bool DeleteAPosition(Position position)
        {
            Positions.RemoveAll(x => (x.Row == position.Row && x.Column == position.Column));
            if (Positions.Count == 0) { return true; }
            else return false;
        }
    }

    internal class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }

}
