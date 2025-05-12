using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWD
{
    public class Theme : ICloneable
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
        public string BackgroundGradEnd { get; set; } = "#FFF0F8FF"; // AliceBlue
        public string BackgroundImage { get; set; }
        public string BackgroundImageAlignX { get; set; } = "Center";
        public string BackgroundImageAlignY { get; set; } = "Center";
        public string BackgroundImageStretch { get; set; } = "UniformToFill";
        public float BackgroundImageOpacity { get; set; } = 1;
        public string BackgroundImageUnderlay { get; set; } = "None";

        public string ButtonFont { get; set; } = "#FF000000";
        public string ButtonPrimary { get; set; } = "#FFFFFFFF";
        public string ButtonSecondary { get; set; } = "#FF458BC0";

        public string CellPrimary { get; set; } = "#00000000";
        public string CellSecondary { get; set; } = "#C2DFEDF0";
        public string SelectedCellPrimary { get; set; } = "#FFA7C4DD";
        public string SelectedCellSecondary { get; set; } = "#FFF0F8FF"; 

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

        public void Dark()
        {
            Name = "Dark";
            Font = "#FFFFFFFF"; 
            BackgroundFlat = "#FF1E1E1E";
            BackgroundGradStart = "#FF2D2D2D";
            BackgroundGradEnd = "#FF454545";

            ButtonFont = "#FFFFFFFF";
            ButtonPrimary = "#FF3A3A3A";
            ButtonSecondary = "#FFFFFFFF";

            CellPrimary = "#FF2D2D2D";
            CellSecondary = "#FF3A3A3A";
            SelectedCellPrimary = "#FF5C9EFF";
            SelectedCellSecondary = "#FF303030";

            HeaderFont = "#FFFFFFFF";
            HeaderPrimary = "#FF1E1E1E";
            HeaderSecondary = "#FFAAAAAA";

            AccentColor = "#FF5C9EFF";
            DecorationPrimary = "#FF2D2D2D";
            DecorationSecondary = "#FF3A3A3A";

            FSFont = "#FFFFFFFF";
            FSBackground = "#FF1E1E1E";
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
                BackgroundImage = this.BackgroundImage,
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

    public class ThemeData : INotifyPropertyChanged
    {
        private string currentKey;
        private Theme currentTheme;

        public string CurrentKey
        {
            get => currentKey;
            set
            {
                if (currentKey != value)
                {
                    currentKey = value;
                    OnPropertyChanged(nameof(CurrentKey));
                }
            }
        }

        public Theme CurrentTheme
        {
            get => currentTheme;
            set
            {
                if (currentTheme != value)
                {
                    currentTheme = value;
                    OnPropertyChanged(nameof(CurrentTheme));
                }
            }
        }
        public Dictionary<string, Theme> Themes { get; set; } = new Dictionary<string, Theme>();

        public ThemeData()
        {
        }

        public static ThemeData LoadData()
        {
            string projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\.."));
            string configPath = Path.Combine(projectDirectory, "Config", "themes.json");

            if (!File.Exists(configPath))
            {
                Errors.DisplayMessage("Config file not found.");
                return new ThemeData();
            }

            try
            {
                string json = File.ReadAllText(configPath);
                var data = JsonConvert.DeserializeObject<ThemeData>(json);

                if (data == null) data = new ThemeData();

                if (!data.Themes.ContainsKey("Light"))
                {
                    data.Themes["Light"] = new Theme { Name = "Light" };
                }

                if (!data.Themes.ContainsKey("Dark"))
                {
                    Theme darkTheme = new Theme { Name = "Dark" };
                    darkTheme.Dark();
                    data.Themes["Dark"] = darkTheme;
                }

                if (data.CurrentKey == null)
                {
                    data.CurrentKey = "Light";
                    data.CurrentTheme = data.Themes["Light"];
                }
                

                return data;
            }
            catch (Exception ex)
            {
                Errors.DisplayMessage($"Failed to load JSON page.\n\n{ex}");
                return new ThemeData();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
