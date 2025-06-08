using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SWD
{
    /// <summary>
    /// Represents the metadata and layout information for a project.
    /// </summary>
    public class Head
    {
        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// Gets or sets the author of the project.
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Gets or sets the keywords associated with the project.
        /// </summary>
        public string[] Keywords { get; set; }
        /// <summary>
        /// Gets or sets the project description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the base layout for the project.
        /// </summary>
        public BaseLayout Layout { get; set; }
        /// <summary>
        /// Gets or sets the custom HTML code for the project.
        /// </summary>
        public string CodeHTML { get; set; } = "";
        /// <summary>
        /// Gets or sets the custom CSS code for the project.
        /// </summary>
        public string CodeCSS { get; set; } = "";
        /// <summary>
        /// Gets or sets the custom JavaScript code for the project.
        /// </summary>
        public string CodeJS { get; set; } = "";
    }

    /// <summary>
    /// Represents the base layout and style properties for a project.
    /// </summary>
    public class BaseLayout : INotifyPropertyChanged
    {
        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets the property value and raises the PropertyChanged event if the value changes.
        /// </summary>
        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        private SolidColorBrush _headerColor;
        /// <summary>
        /// Gets or sets the header color.
        /// </summary>
        public SolidColorBrush HeaderColor
        {
            get => _headerColor;
            set => SetProperty(ref _headerColor, value);
        }

        private float _headerPadding = 10;
        /// <summary>
        /// Gets or sets the header padding.
        /// </summary>
        public float HeaderPadding
        {
            get => _headerPadding;
            set => SetProperty(ref _headerPadding, value);
        }

        private string _headerPaddingUnit = "px";
        /// <summary>
        /// Gets or sets the header padding unit.
        /// </summary>
        public string HeaderPaddingUnit
        {
            get => _headerPaddingUnit;
            set => SetProperty(ref _headerPaddingUnit, value);
        }

        private string _headerLogo;
        /// <summary>
        /// Gets or sets the header logo filename.
        /// </summary>
        public string HeaderLogo
        {
            get => _headerLogo;
            set => SetProperty(ref _headerLogo, value);
        }

        private Dictionary<string, string> _headerLinks = new Dictionary<string, string>();
        /// <summary>
        /// Gets or sets the header links (title and href).
        /// </summary>
        public Dictionary<string, string> HeaderLinks
        {
            get => _headerLinks;
            set => SetProperty(ref _headerLinks, value);
        }

        private string _headerLinkStyle = "Grid-Footer";
        /// <summary>
        /// Gets or sets the header link style.
        /// </summary>
        public string HeaderLinkStyle
        {
            get => _headerLinkStyle;
            set => SetProperty(ref _headerLinkStyle, value);
        }

        private SolidColorBrush _bodyColor;
        /// <summary>
        /// Gets or sets the body color.
        /// </summary>
        public SolidColorBrush BodyColor
        {
            get => _bodyColor;
            set => SetProperty(ref _bodyColor, value);
        }

        private float _bodyWidth = 100;
        /// <summary>
        /// Gets or sets the body width.
        /// </summary>
        public float BodyWidth
        {
            get => _bodyWidth;
            set => SetProperty(ref _bodyWidth, value);
        }

        private string _bodyWidthUnit = "vw";
        /// <summary>
        /// Gets or sets the body width unit.
        /// </summary>
        public string BodyWidthUnit
        {
            get => _bodyWidthUnit;
            set => SetProperty(ref _bodyWidthUnit, value);
        }

        private SolidColorBrush _gridColor;
        /// <summary>
        /// Gets or sets the grid color.
        /// </summary>
        public SolidColorBrush GridColor
        {
            get => _gridColor;
            set => SetProperty(ref _gridColor, value);
        }

        private float _gridWidth = 1140;
        /// <summary>
        /// Gets or sets the grid width.
        /// </summary>
        public float GridWidth
        {
            get => _gridWidth;
            set => SetProperty(ref _gridWidth, value);
        }

        private string _gridWidthUnit = "px";
        /// <summary>
        /// Gets or sets the grid width unit.
        /// </summary>
        public string GridWidthUnit
        {
            get => _gridWidthUnit;
            set => SetProperty(ref _gridWidthUnit, value);
        }

        private float _gridGap = 10;
        /// <summary>
        /// Gets or sets the grid gap.
        /// </summary>
        public float GridGap
        {
            get => _gridGap;
            set => SetProperty(ref _gridGap, value);
        }

        private string _gridGapUnit = "px";
        /// <summary>
        /// Gets or sets the grid gap unit.
        /// </summary>
        public string GridGapUnit
        {
            get => _gridGapUnit;
            set => SetProperty(ref _gridGapUnit, value);
        }

        private float _gridBorderRadius = 0;
        /// <summary>
        /// Gets or sets the grid border radius.
        /// </summary>
        public float GridBorderRadius
        {
            get => _gridBorderRadius;
            set => SetProperty(ref _gridBorderRadius, value);
        }

        private string _gridBorderRadiusUnit = "px";
        /// <summary>
        /// Gets or sets the grid border radius unit.
        /// </summary>
        public string GridBorderRadiusUnit
        {
            get => _gridBorderRadiusUnit;
            set => SetProperty(ref _gridBorderRadiusUnit, value);
        }

        private float _gridPadding = 10;
        /// <summary>
        /// Gets or sets the grid padding.
        /// </summary>
        public float GridPadding
        {
            get => _gridPadding;
            set => SetProperty(ref _gridPadding, value);
        }

        private string _gridPaddingUnit = "px";
        /// <summary>
        /// Gets or sets the grid padding unit.
        /// </summary>
        public string GridPaddingUnit
        {
            get => _gridPaddingUnit;
            set => SetProperty(ref _gridPaddingUnit, value);
        }

        private float _gridMargin = 20;
        /// <summary>
        /// Gets or sets the grid margin.
        /// </summary>
        public float GridMargin
        {
            get => _gridMargin;
            set => SetProperty(ref _gridMargin, value);
        }

        private string _gridMarginUnit = "px";
        /// <summary>
        /// Gets or sets the grid margin unit.
        /// </summary>
        public string GridMarginUnit
        {
            get => _gridMarginUnit;
            set => SetProperty(ref _gridMarginUnit, value);
        }

        private string _gridHAlign = "Center";
        /// <summary>
        /// Gets or sets the grid horizontal alignment.
        /// </summary>
        public string GridHAlign
        {
            get => _gridHAlign;
            set => SetProperty(ref _gridHAlign, value);
        }

        private SolidColorBrush _footerColor;
        /// <summary>
        /// Gets or sets the footer color.
        /// </summary>
        public SolidColorBrush FooterColor
        {
            get => _footerColor;
            set => SetProperty(ref _footerColor, value);
        }

        private float _footerPadding = 10;
        /// <summary>
        /// Gets or sets the footer padding.
        /// </summary>
        public float FooterPadding
        {
            get => _footerPadding;
            set => SetProperty(ref _footerPadding, value);
        }

        private string _footerPaddingUnit = "px";
        /// <summary>
        /// Gets or sets the footer padding unit.
        /// </summary>
        public string FooterPaddingUnit
        {
            get => _footerPaddingUnit;
            set => SetProperty(ref _footerPaddingUnit, value);
        }

        private string _footerContent;
        /// <summary>
        /// Gets or sets the footer content.
        /// </summary>
        public string FooterContent
        {
            get => _footerContent;
            set => SetProperty(ref _footerContent, value);
        }

        /// <summary>
        /// Raises the PropertyChanged event for a property.
        /// </summary>
        public void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Gets or sets a property value by name using reflection.
        /// </summary>
        public object this[string propertyName]
        {
            get
            {
                var prop = GetType().GetProperty(propertyName);
                return prop?.GetValue(this);
            }
            set
            {
                var prop = GetType().GetProperty(propertyName);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(this, value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }

    /// <summary>
    /// Provides utility methods for loading icons from the project directory.
    /// </summary>
    internal class Images
    {
        /// <summary>
        /// Loads a new icon as a BitmapImage from the Icons directory.
        /// </summary>
        /// <param name="icon">The icon filename (default: "add.png").</param>
        /// <returns>A BitmapImage of the icon.</returns>
        public static BitmapImage NewIcon(string icon = "add.png")
        {
            string projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\.."));
            string iconPath = Path.Combine(projectDirectory, "Icons", icon);
            if (!File.Exists(iconPath))
            {
                Debug.WriteLine($"Image not found: {iconPath}");
            }
            Uri uri = new Uri(iconPath);
            BitmapImage bt = new BitmapImage(uri);

            return bt;
        }
    }

    /// <summary>
    /// Provides utility methods for generating unique names.
    /// </summary>
    internal class Names
    {
        /// <summary>
        /// Returns a unique name based on the desired name and a collection of existing names.
        /// </summary>
        /// <param name="desiredName">The desired name.</param>
        /// <param name="existingNames">A collection of existing names.</param>
        /// <returns>A unique name not present in existingNames.</returns>
        public static string GetUniqueThemeName(string desiredName, IEnumerable<string> existingNames)
        {
            var nameSet = new HashSet<string>(existingNames, StringComparer.OrdinalIgnoreCase);

            Debug.WriteLine(desiredName);

            if (!nameSet.Contains(desiredName))
                return desiredName;

            int counter = 1;
            string newName;

            do
            {
                newName = $"{desiredName} ({counter})";
                counter++;
            }
            while (nameSet.Contains(newName));

            return newName;
        }
    }
}
