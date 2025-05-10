using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SWD
{
    internal class Head
    {
        public string ProjectName { get; set; }
        public string Author { get; set; }
        public string[] Keywords { get; set; }
        public string Description { get; set; }

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

    internal class Theme : ICloneable
    {
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }

        public string Font { get; set; } = "#FF000000";
        public string BackgroundType { get; set; } = "Gradient";
        public string BackgroundFlat { get; set; } = "#FF97C9EF";
        public string BackgroundGradStart { get; set; } = "#FF97C9EF";
        public string BackgroundGradEnd { get; set; } = "#FFF0F8FF";
        public BitmapImage BackgroundImage { get; set; }

        public string ButtonFont { get; set; } = "#FF000000";
        public string ButtonPrimary { get; set; } = "#FFDDDDDD";
        public string ButtonSecondary { get; set; } = "#FF458BC0";

        public string CellPrimary { get; set; } = "#00000000";
        public string CellSecondary { get; set; } = "#C2DFEDF0";        
        public string SelectedCellPrimary { get; set; } = "#FFA7C4DD";
        public string SelectedCellSecondary { get; set; } = "#FFF0F8FF"; // AliceBlue

        public string HeaderFont { get; set; } = "#FF000000";
        public string HeaderPrimary { get; set; } = "#FFFFFFFF";
        public string HeaderSecondary { get; set; } = "#FF778899";

        public string AccentColor { get; set; } = "#FF65ACE2";
        public string DecorationPrimary { get; set; } = "#FFFFFFFF";
        public string DecorationSecondary { get; set; } = "#FFD3D3D3";

        public string FSFont { get; set; } = "#FF000000";
        public string FSBackground { get; set; } = "#FFFFFFFF";

        public string this[string propertyName]
        {
            get
            {
                var prop = this.GetType().GetProperty(propertyName);
                return prop?.GetValue(this)?.ToString();
            }
            set
            {
                var prop = this.GetType().GetProperty(propertyName);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(this, value);
                }
            }
        }

        public object Clone()
        {
            return new Theme
            {
                Name = this.Name,
                Font = this.Font,
                BackgroundType = this.BackgroundType,
                BackgroundFlat = this.BackgroundFlat,
                BackgroundGradStart = this.BackgroundGradStart,
                BackgroundGradEnd = this.BackgroundGradEnd,
                BackgroundImage = this.BackgroundImage, // Note: this is a shallow copy
                ButtonFont = this.ButtonFont,
                ButtonPrimary = this.ButtonPrimary,
                ButtonSecondary = this.ButtonSecondary,
                CellPrimary = this.CellPrimary,
                CellSecondary = this.CellSecondary,
                SelectedCellPrimary = this.SelectedCellPrimary,
                SelectedCellSecondary = this.SelectedCellSecondary,
                HeaderFont = this.HeaderFont,
                HeaderPrimary = this.HeaderPrimary,
                HeaderSecondary = this.HeaderSecondary,
                AccentColor = this.AccentColor,
                DecorationPrimary = this.DecorationPrimary,
                DecorationSecondary = this.DecorationSecondary,
                FSFont = this.FSFont,
                FSBackground = this.FSBackground
            };
        }
    }
}
