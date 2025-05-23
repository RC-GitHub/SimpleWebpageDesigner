﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using ColorConverter = System.Windows.Media.ColorConverter;
using Color = System.Windows.Media.Color;
using System.ComponentModel;

namespace SWD
{
    public class DataItem
    {
        public string Column { get; set; }
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
            Clear();
        }

        public void SetCell(Component component)
        {
            SolidColorBrush translucentBrush = component.BackgroundColor.Clone();
            translucentBrush.Opacity = 0.75;

            Title = component.Name;
            ImageSource = Images.NewIcon($"{component.Type}.png");
            BorderColor = component.BorderColor;
            BackgroundColor = translucentBrush;
            SelectedBorderColor = component.SelectedBorderColor;
            SelectedBackgroundColor = component.SelectedBackgroundColor;
        }

        public void Clear()
        {
            Theme currentTheme = App.themeData.Themes[App.themeData.CurrentKey];
            Debug.WriteLine(currentTheme.CellPrimary);

            Title = "";
            ImageSource = Images.NewIcon();
            BorderColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(currentTheme.CellSecondary));
            BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(currentTheme.CellPrimary));
            SelectedBorderColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(currentTheme.SelectedCellSecondary));
            SelectedBackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(currentTheme.SelectedCellPrimary));
        }
    }

    internal class ContentStructure
    {
        public Dictionary<string, Component> Components { get; set; }
        public int RowAmount { get; set; }
        public int ColAmount { get; set; }
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
            if (rowOrColumn == "column") this.StartColumn--;
            else this.StartRow--;
            Repopulate();
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
            Debug.WriteLine($"Positions of Component {Name}.");
            foreach(var position in Positions)
            {
                Debug.WriteLine($"Row: {position.Row+1}, Column: {position.Column+1}");
            }
        }

        public Component DeepCopy()
        {
            return new Component()
            {
                Name = Name,
                Type = Type,
                Rowspan = Rowspan,
                Colspan = Colspan,
                StartRow = StartRow,
                StartColumn = StartColumn,
                Positions = Positions,
                BorderColor = BorderColor,
                BackgroundColor = BackgroundColor,
                SelectedBorderColor = SelectedBorderColor,
                SelectedBackgroundColor = SelectedBackgroundColor
            };
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

    internal class PageData
    {
        public Dictionary<string, Component> Components { get; set; }
        public int RowAmount { get; set; }
        public int ColAmount { get; set; }
    }

}
