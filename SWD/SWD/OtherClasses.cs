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
    public class Head
    {
        public string ProjectName { get; set; }
        public string Author { get; set; }
        public string[] Keywords { get; set; }
        public string Description { get; set; }
        public BaseLayout Layout { get; set; }
    }

    public class BaseLayout : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        private SolidColorBrush _headerColor;
        public SolidColorBrush HeaderColor
        {
            get => _headerColor;
            set => SetProperty(ref _headerColor, value);
        }

        private float _headerPadding = 10;
        public float HeaderPadding
        {
            get => _headerPadding;
            set => SetProperty(ref _headerPadding, value);
        }

        private string _headerPaddingUnit = "px";
        public string HeaderPaddingUnit
        {
            get => _headerPaddingUnit;
            set => SetProperty(ref _headerPaddingUnit, value);
        }

        private string _headerLogo;
        public string HeaderLogo
        {
            get => _headerLogo;
            set => SetProperty(ref _headerLogo, value);
        }

        private Dictionary<string, string> _headerLinks = new Dictionary<string, string>();
        public Dictionary<string, string> HeaderLinks
        {
            get => _headerLinks;
            set => SetProperty(ref _headerLinks, value);
        }

        private SolidColorBrush _bodyColor;
        public SolidColorBrush BodyColor
        {
            get => _bodyColor;
            set => SetProperty(ref _bodyColor, value);
        }

        private float _bodyWidth = 100;
        public float BodyWidth
        {
            get => _bodyWidth;
            set => SetProperty(ref _bodyWidth, value);
        }

        private string _bodyWidthUnit = "vw";
        public string BodyWidthUnit
        {
            get => _bodyWidthUnit;
            set => SetProperty(ref _bodyWidthUnit, value);
        }

        private SolidColorBrush _gridColor;
        public SolidColorBrush GridColor
        {
            get => _gridColor;
            set => SetProperty(ref _gridColor, value);
        }

        private float _gridWidth = 1140;
        public float GridWidth
        {
            get => _gridWidth;
            set => SetProperty(ref _gridWidth, value);
        }

        private string _gridWidthUnit = "px";
        public string GridWidthUnit
        {
            get => _gridWidthUnit;
            set => SetProperty(ref _gridWidthUnit, value);
        }

        private float _gridBorderRadius = 0;
        public float GridBorderRadius
        {
            get => _gridBorderRadius;
            set => SetProperty(ref _gridBorderRadius, value);
        }

        private string _gridBorderRadiusUnit = "px";
        public string GridBorderRadiusUnit
        {
            get => _gridBorderRadiusUnit;
            set => SetProperty(ref _gridBorderRadiusUnit, value);
        }

        private float _gridPadding = 10;
        public float GridPadding
        {
            get => _gridPadding;
            set => SetProperty(ref _gridPadding, value);
        }

        private string _gridPaddingUnit = "px";
        public string GridPaddingUnit
        {
            get => _gridPaddingUnit;
            set => SetProperty(ref _gridPaddingUnit, value);
        }

        private float _gridMargin = 20;
        public float GridMargin
        {
            get => _gridMargin;
            set => SetProperty(ref _gridMargin, value);
        }

        private string _gridMarginUnit = "px";
        public string GridMarginUnit
        {
            get => _gridMarginUnit;
            set => SetProperty(ref _gridMarginUnit, value);
        }

        private string _gridHAlign = "Center";
        public string GridHAlign
        {
            get => _gridHAlign;
            set => SetProperty(ref _gridHAlign, value);
        }

        private SolidColorBrush _footerColor;
        public SolidColorBrush FooterColor
        {
            get => _footerColor;
            set => SetProperty(ref _footerColor, value);
        }

        private float _footerPadding = 10;
        public float FooterPadding
        {
            get => _footerPadding;
            set => SetProperty(ref _footerPadding, value);
        }

        private string _footerPaddingUnit = "px";
        public string FooterPaddingUnit
        {
            get => _footerPaddingUnit;
            set => SetProperty(ref _footerPaddingUnit, value);
        }

        private string _footerContent;
        public string FooterContent
        {
            get => _footerContent;
            set => SetProperty(ref _footerContent, value);
        }

        public void OnPropertyChanged(string propertyName) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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

    internal class Images
    {
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

    internal class Names
    {
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
