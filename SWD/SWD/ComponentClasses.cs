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
    /// <summary>
    /// Represents a data item for a column in the DataGrid.
    /// </summary>
    public class DataItem
    {
        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        public string Column { get; set; }
    }

    /// <summary>
    /// Represents a row in the DataGrid.
    /// </summary>
    internal class Row
    {
        /// <summary>
        /// Gets or sets the row title.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the list of cells in the row.
        /// </summary>
        public List<Cell> Content { get; set; }
    }

    /// <summary>
    /// Represents a cell in the DataGrid.
    /// </summary>
    internal class Cell
    {
        /// <summary>
        /// Gets or sets the cell title.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the image source for the cell.
        /// </summary>
        public BitmapImage ImageSource { get; set; }
        /// <summary>
        /// Gets or sets the border color of the cell.
        /// </summary>
        public SolidColorBrush BorderColor { get; set; }
        /// <summary>
        /// Gets or sets the background color of the cell.
        /// </summary>
        public SolidColorBrush BackgroundColor { get; set; }
        /// <summary>
        /// Gets or sets the selected border color of the cell.
        /// </summary>
        public SolidColorBrush SelectedBorderColor { get; set; }
        /// <summary>
        /// Gets or sets the selected background color of the cell.
        /// </summary>
        public SolidColorBrush SelectedBackgroundColor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        public Cell()
        {
            Clear();
        }

        /// <summary>
        /// Sets the cell's properties based on the given component.
        /// </summary>
        /// <param name="component">The component to use for cell data.</param>
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

        /// <summary>
        /// Resets the cell to its default state using the current theme.
        /// </summary>
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

    /// <summary>
    /// Represents the structure of content for serialization.
    /// </summary>
    internal class ContentStructure
    {
        /// <summary>
        /// Gets or sets the dictionary of components.
        /// </summary>
        public Dictionary<string, Component> Components { get; set; }
        /// <summary>
        /// Gets or sets the number of rows.
        /// </summary>
        public int RowAmount { get; set; }
        /// <summary>
        /// Gets or sets the number of columns.
        /// </summary>
        public int ColAmount { get; set; }
    }

    /// <summary>
    /// Represents a UI component with style, content, and layout properties.
    /// </summary>
    public class Component : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string Name { get; set; }
        private string _Type { get; set; }
        /// <summary>
        /// Gets or sets the component type.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the component content.
        /// </summary>
        public ComponentContent Content { get; set; } = new ComponentContent();

        /// <summary>
        /// Gets or sets the number of rows the component spans.
        /// </summary>
        public int Rowspan { get; set; }
        /// <summary>
        /// Gets or sets the number of columns the component spans.
        /// </summary>
        public int Colspan { get; set; }
        /// <summary>
        /// Gets or sets the starting row index.
        /// </summary>
        public int StartRow { get; set; }
        /// <summary>
        /// Gets or sets the starting column index.
        /// </summary>
        public int StartColumn { get; set; }

        private ComponentStyle _CompStyle { get; set; } = new ComponentStyle();
        private SolidColorBrush _BorderColor { get; set; }
        /// <summary>
        /// Gets or sets the border color.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the component style.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the selected border color.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the selected background color.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the list of positions occupied by the component.
        /// </summary>
        public List<Position> Positions { get; set; }
        /// <summary>
        /// Gets or sets the alpha (opacity) value of the background color.
        /// </summary>
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

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Updates the component's spanning properties based on its positions.
        /// </summary>
        public void Spanning()
        {
            StartRow = MinRow();
            StartColumn = MinColumn();
            Rowspan = MaxRow() - StartRow;
            Colspan = MaxColumn() - StartColumn;
        }

        /// <summary>
        /// Decreases the start row or column and repopulates positions.
        /// </summary>
        /// <param name="rowOrColumn">"row" or "column" to decrease.</param>
        public void Decrease(string rowOrColumn)
        {
            if (rowOrColumn == "column") this.StartColumn--;
            else this.StartRow--;
            Repopulate();
        }

        /// <summary>
        /// Gets the minimum row index from positions.
        /// </summary>
        public int MinRow() { return Positions.Min(p => p.Row); }
        /// <summary>
        /// Gets the maximum row index from positions.
        /// </summary>
        public int MaxRow() { return Positions.Max(p => p.Row); }
        /// <summary>
        /// Gets the minimum column index from positions.
        /// </summary>
        public int MinColumn() { return Positions.Min(p => p.Column); }
        /// <summary>
        /// Gets the maximum column index from positions.
        /// </summary>
        public int MaxColumn() { return Positions.Max(p => p.Column); }

        /// <summary>
        /// Checks if the component contains the specified row.
        /// </summary>
        public bool ContainsRow(int row)
        {
            foreach (var position in Positions)
            {
                if (position.Row == row) return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if the component contains the specified column.
        /// </summary>
        public bool ContainsColumn(int column)
        {
            foreach (var position in Positions)
            {
                if (position.Column == column) return true;
            }
            return false;
        }

        /// <summary>
        /// Repopulates the Positions list based on current start and span values.
        /// </summary>
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

        /// <summary>
        /// Writes the component's positions to the debug output.
        /// </summary>
        public void DisplayPositions()
        {
            Debug.WriteLine($"Positions of Component {Name}.");
            foreach (var position in Positions)
            {
                Debug.WriteLine($"Row: {position.Row + 1}, Column: {position.Column + 1}");
            }
        }

        /// <summary>
        /// Creates a deep copy of the component.
        /// </summary>
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

        /// <summary>
        /// Clears the Positions list.
        /// </summary>
        public void ClearData()
        {
            Positions.Clear();
        }

        /// <summary>
        /// Deletes a specific position from the Positions list.
        /// </summary>
        /// <param name="position">The position to delete.</param>
        /// <returns>True if no positions remain, otherwise false.</returns>
        public bool DeleteAPosition(Position position)
        {
            Positions.RemoveAll(x => (x.Row == position.Row && x.Column == position.Column));
            if (Positions.Count == 0) { return true; }
            else return false;
        }
    }

    /// <summary>
    /// Represents the style properties for a component.
    /// </summary>
    public class ComponentStyle : INotifyPropertyChanged
    {
        // Padding, margin, border, gradient, background, size, alignment, etc.
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

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Represents the content properties for a component, including text, image, code, and button data.
    /// </summary>
    public class ComponentContent : INotifyPropertyChanged
    {
        // Text properties
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

        // Image properties
        public string ImageName { get; set; } = "";
        public float ImageWidth { get; set; } = 200;
        public float ImageHeight { get; set; } = 200;
        public string ImageHAlign { get; set; } = "Center";
        public string ImageVAlign { get; set; } = "Center";
        public string ImageStretch { get; set; } = "Uniform";
        public string ImageFilter { get; set; } = "None";

        // Code properties
        public string CodeHTML { get; set; } = "";
        public string CodeCSS { get; set; } = "";
        public string CodeJS { get; set; } = "";

        // Button properties
        public string ButtonText { get; set; } = "Button";
        public string ButtonLink { get; set; } = "";
        public string ButtonStyle { get; set; } = "Custom";
        public float ButtonWidth { get; set; } = 100;
        public string ButtonWidthUnit { get; set; } = "%";
        public float ButtonHeight { get; set; } = 50;
        public string ButtonHeightUnit { get; set; } = "px";
        public float ButtonFontSize { get; set; } = 12;
        public string ButtonFontSizeUnit { get; set; } = "px";
        private SolidColorBrush _buttonFontColor = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        public SolidColorBrush ButtonFontColor
        {
            get => _buttonFontColor;
            set
            {
                if (_buttonFontColor != value)
                {
                    _buttonFontColor = value;
                    OnPropertyChanged();
                }
            }
        }

        private SolidColorBrush _buttonBackgroundColor = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        public SolidColorBrush ButtonBackgroundColor
        {
            get => _buttonBackgroundColor;
            set
            {
                if (_buttonBackgroundColor != value)
                {
                    _buttonBackgroundColor = value;
                    OnPropertyChanged();
                }
            }
        }

        private float _buttonBorder = 5;
        public float ButtonBorder
        {
            get => _buttonBorder;
            set
            {
                if (_buttonBorder != value)
                {
                    _buttonBorder = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ButtonBorderUnit { get; set; } = "px";
        private SolidColorBrush _buttonBorderColor = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        public SolidColorBrush ButtonBorderColor
        {
            get => _buttonBorderColor;
            set
            {
                if (_buttonBorderColor != value)
                {
                    _buttonBorderColor = value;
                    OnPropertyChanged();
                }
            }
        }
        private float _buttonBorderRadius = 0;
        public float ButtonBorderRadius
        {
            get => _buttonBorderRadius;
            set
            {
                if (_buttonBorderRadius != value)
                {
                    _buttonBorderRadius = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ButtonBorderRadiusUnit { get; set; } = "px";
        public float ButtonPadding { get; set; } = 5;
        public string ButtonPaddingUnit { get; set; } = "px";
        public float ButtonMargin { get; set; } = 5;
        public string ButtonMarginUnit { get; set; } = "px";
        public string ButtonFunc { get; set; } = "";

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    /// <summary>
    /// Represents a position in the grid (row and column).
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Gets or sets the row index.
        /// </summary>
        public int Row { get; set; }
        /// <summary>
        /// Gets or sets the column index.
        /// </summary>
        public int Column { get; set; }
    }

    /// <summary>
    /// Represents the data for a page, including components and grid size.
    /// </summary>
    internal class PageData
    {
        /// <summary>
        /// Gets or sets the dictionary of components.
        /// </summary>
        public Dictionary<string, Component> Components { get; set; } = new Dictionary<string, Component>();
        /// <summary>
        /// Gets or sets the number of rows.
        /// </summary>
        public int RowAmount { get; set; }
        /// <summary>
        /// Gets or sets the number of columns.
        /// </summary>
        public int ColAmount { get; set; }
    }
}
