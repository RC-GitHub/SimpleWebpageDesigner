using System;
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
using Brush = System.Windows.Media.Brush;
using System.Runtime.CompilerServices;
using System.Windows;

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

    public class Component : INotifyPropertyChanged
    {
        public string Name { get; set; }
        private string _Type { get; set; }
        public string Type
        {
            get => _Type;
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }
        public ComponentContent Content { get; set; } = new ComponentContent();

        public int Rowspan { get; set; }
        public int Colspan { get; set; }        
        public int StartRow { get; set; }
        public int StartColumn { get; set; }

        private ComponentStyle _CompStyle { get; set; } = new ComponentStyle();
        private SolidColorBrush _BorderColor { get; set; }
        public SolidColorBrush BorderColor
        {
            get => _BorderColor;
            set
            {
                if (_BorderColor != value)
                {
                    _BorderColor = value;
                    OnPropertyChanged(nameof(BorderColor));
                }
            }
        }
        private SolidColorBrush _BackgroundColor { get; set; }
        public SolidColorBrush BackgroundColor
        {
            get => _BackgroundColor;
            set
            {
                if (_BackgroundColor != value)
                {
                    _BackgroundColor = value;
                    OnPropertyChanged(nameof(BackgroundColor));
                }
            }
        }
        public ComponentStyle CompStyle
        {
            get => _CompStyle;
            set
            {
                if (_CompStyle != value)
                {
                    _CompStyle = value;
                    OnPropertyChanged(nameof(ComponentStyle));
                }
            }
        }
        private SolidColorBrush _SelectedBorderColor { get; set; }
        public SolidColorBrush SelectedBorderColor
        {
            get => _SelectedBorderColor;
            set
            {
                if (_SelectedBorderColor != value)
                {
                    _SelectedBorderColor = value;
                    OnPropertyChanged(nameof(SelectedBorderColor));
                }
            }
        }
        private SolidColorBrush _SelectedBackgroundColor { get; set; }
        public SolidColorBrush SelectedBackgroundColor
        {
            get => _SelectedBackgroundColor;
            set
            {
                if (_SelectedBackgroundColor != value)
                {
                    _SelectedBackgroundColor = value;
                    OnPropertyChanged(nameof(SelectedBackgroundColor));
                }
            }
        }
        public List<Position> Positions { get; set; }
        public byte Alpha
        {
            get => BackgroundColor.Color.A;
            set
            {
                Color oldColor = BackgroundColor.Color;
                BackgroundColor = new SolidColorBrush(Color.FromArgb(value, oldColor.R, oldColor.G, oldColor.B));
                OnPropertyChanged(nameof(Alpha));
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
                CompStyle = CompStyle,
                Content = Content,
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

    public class ComponentStyle : INotifyPropertyChanged
    {
        public string PaddingUnit { get; set; } = "px";
        public int PaddingLeft { get; set; }
        public int PaddingTop { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingBottom { get; set; }

        public string MarginUnit { get; set; } = "px";
        public int MarginLeft { get; set; }
        public int MarginTop { get; set; }
        public int MarginRight { get; set; }
        public int MarginBottom { get; set; }

        public string BorderThicknessUnit { get; set; } = "px";
        public int BorderThickness { get; set; }
        public string BorderRadiusUnit { get; set; } = "px";
        public int BorderRadius { get; set; }
        private SolidColorBrush _BorderColor { get; set; } = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        public SolidColorBrush BorderColor
        {
            get => _BorderColor;
            set
            {
                if (_BorderColor != value)
                {
                    _BorderColor = value;
                    OnPropertyChanged(nameof(BorderColor));
                }
            }
        }

        public string GradientType { get; set; } = "Linear";
        public SolidColorBrush _GradientStart { get; set; } = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        public SolidColorBrush GradientStart
        {
            get => _GradientStart;
            set
            {
                if (_GradientStart != value)
                {
                    _GradientStart = value;
                    OnPropertyChanged(nameof(GradientStart));
                }
            }
        }
        public int GradientStartPercent { get; set; } = 0;
        public SolidColorBrush _GradientEnd { get; set; } = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        public SolidColorBrush GradientEnd
        {
            get => _GradientEnd;
            set
            {
                if (_GradientEnd != value)
                {
                    _GradientEnd = value;
                    OnPropertyChanged(nameof(GradientEnd));
                }
            }
        }
        public int GradientEndPercent { get; set; } = 100;

        public bool UseBackgroundImage { get; set; } = false;
        public string BackgroundImage { get; set; }
        public string BackgroundImageAlignment { get; set; } = "Center";
        public string BackgroundImageStretch { get; set; } = "UniformToFill";

        public float Opacity { get; set; } = 1;
        public string Overflow { get; set; } = "Visible";
        public string ZIndex { get; set; } = "Auto";
        public string BoxShadow { get; set; } = "0px 0px 0px #00000000";

        public int Width { get; set; }
        public int MinWidth { get; set; }
        public int MaxWidth { get; set; }
        public string WidthUnit { get; set; } = "px";
        public string MinWidthUnit { get; set; } = "px";
        public string MaxWidthUnit { get; set; } = "px";

        public int Height { get; set; }
        public int MinHeight { get; set; }
        public int MaxHeight { get; set; }
        public string HeightUnit { get; set; } = "px";
        public string MinHeightUnit { get; set; } = "px";
        public string MaxHeightUnit { get; set; } = "px";

        public string Justify { get; set; } = "Center";
        public string AlignItems { get; set; } = "Center";

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class ComponentContent
    {
        public string Text { get; set; } = "";
        public string FontFamily { get; set; } = "Arial";
        public int FontSize { get; set; } = 12;
        public string FontWeight { get; set; } = "Normal";
        public string FontStyle { get; set; } = "Normal";
        public string FontLine { get; set; } = "None";
        public SolidColorBrush ForegroundColor { get; set; } = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        public string TextHorizontal { get; set; } = "Left";
        public string TextVertical { get; set; } = "Top";
        public bool IsTextSelectable { get; set; } = true;


        public string ImageName { get; set; } = "";
        public float ImageWidth { get; set; } = 200;
        public float ImageHeight { get; set; } = 200;
        public string ImageHAlign { get; set; } = "Center";
        public string ImageVAlign { get; set; } = "Center";
        public string ImageStretch { get; set; } = "Uniform";
        public string ImageFilter { get; set; } = "None";
    }

    public class Position
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
