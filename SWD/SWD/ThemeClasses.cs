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
    public class Theme : ICloneable, INotifyPropertyChanged
    {
        // Name
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }


        // Base Font
        private string FontValue = "#FF000000";
        public string Font
        {
            get => FontValue;
            set => SetProperty(ref FontValue, value, nameof(Font));
        }


        // Backgrounds
        public string BackgroundGradStartValue = "#FF97C9EF";
        public string BackgroundGradStart
        {
            get => BackgroundGradStartValue;
            set => SetProperty(ref BackgroundGradStartValue, value, nameof(BackgroundGradStart));
        }

        public string BackgroundGradEndValue  = "#FFF0F8FF"; // AliceBlue
        public string BackgroundGradEnd
        {
            get => BackgroundGradEndValue;
            set => SetProperty(ref BackgroundGradEndValue, value, nameof(BackgroundGradEnd));
        }

        private bool BackgroundImageOnValue = false;
        public bool BackgroundImageOn
        {
            get => BackgroundImageOnValue;
            set => SetProperty(ref BackgroundImageOnValue, value, nameof(BackgroundImageOn));
        }

        private string BackgroundImageValue { get; set; }
        public string BackgroundImage
        {
            get => BackgroundImageValue;
            set
            {
                if (BackgroundImageValue != value)
                {
                    BackgroundImageValue = value;

                    string projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\.."));
                    string configDirectory = Path.Combine(projectDirectory, "Config");
                    BackgroundImageFullPath = Path.Combine(configDirectory, BackgroundImageValue);

                    OnPropertyChanged(nameof(BackgroundImage));
                }
            }
        }

        private string BackgroundImagePath { get; set; }
        public string BackgroundImageFullPath
        {
            get => BackgroundImagePath;
            set
            {
                if (BackgroundImagePath != value)
                {
                    BackgroundImagePath = value;
                    OnPropertyChanged(nameof(BackgroundImageFullPath));
                }
            }
        }
        private string BackgroundImageAlignXValue = "Center";
        public string BackgroundImageAlignX
        {
            get => BackgroundImageAlignXValue;
            set => SetProperty(ref BackgroundImageAlignXValue, value, nameof(BackgroundImageAlignX));
        }

        private string BackgroundImageAlignYValue = "Center";
        public string BackgroundImageAlignY
        {
            get => BackgroundImageAlignYValue;
            set => SetProperty(ref BackgroundImageAlignYValue, value, nameof(BackgroundImageAlignY));
        }

        private string BackgroundImageStretchValue = "UniformToFill";
        public string BackgroundImageStretch
        {
            get => BackgroundImageStretchValue;
            set => SetProperty(ref BackgroundImageStretchValue, value, nameof(BackgroundImageStretch));
        }

        private float BackgroundImageOpacityValue = 1;
        public float BackgroundImageOpacity
        {
            get => BackgroundImageOpacityValue;
            set => SetProperty(ref BackgroundImageOpacityValue, value, nameof(BackgroundImageOpacity));
        }

        public string BackgroundImageOverlayValue = "#00000000";
        public string BackgroundImageOverlay
        {
            get => BackgroundImageOverlayValue;
            set => SetProperty(ref BackgroundImageOverlayValue, value, nameof(BackgroundImageOverlay));
        }


        // Buttons
        private string ButtonFontValue = "#FF000000";
        public string ButtonFont
        {
            get => ButtonFontValue;
            set => SetProperty(ref ButtonFontValue, value, nameof(ButtonFont));
        }

        private string ButtonPrimaryValue = "#FFFFFFFF";
        public string ButtonPrimary
        {
            get => ButtonPrimaryValue;
            set => SetProperty(ref ButtonPrimaryValue, value, nameof(ButtonPrimary));
        }

        private string ButtonSecondaryValue = "#FF458BC0";
        public string ButtonSecondary
        {
            get => ButtonSecondaryValue;
            set => SetProperty(ref ButtonSecondaryValue, value, nameof(ButtonSecondary));
        }

        private string SelectedButtonPrimaryValue = "#FFC2DBFC";
        public string SelectedButtonPrimary
        {
            get => SelectedButtonPrimaryValue;
            set => SetProperty(ref SelectedButtonPrimaryValue, value, nameof(SelectedButtonPrimary));
        }

        private string SelectedButtonSecondaryValue = "#FF458BC0";
        public string SelectedButtonSecondary
        {
            get => SelectedButtonSecondaryValue;
            set => SetProperty(ref SelectedButtonSecondaryValue, value, nameof(SelectedButtonSecondary));
        }


        // Cells
        public string CellPrimary { get; set; } = "#00000000";
        public string CellSecondary { get; set; } = "#C2DFEDF0";
        public string SelectedCellPrimary { get; set; } = "#FFF0F8FF";
        public string SelectedCellSecondary { get; set; } = "#FFA7C4DD";


        // Headers
        private string HeaderFontValue = "#FF000000";
        public string HeaderFont
        {
            get => HeaderFontValue;
            set => SetProperty(ref HeaderFontValue, value, nameof(HeaderFont));
        }

        private string HeaderPrimaryValue = "#FFFFFFFF";
        public string HeaderPrimary
        {
            get => HeaderPrimaryValue;
            set => SetProperty(ref HeaderPrimaryValue, value, nameof(HeaderPrimary));
        }

        private string HeaderSecondaryValue = "#FF778899";
        public string HeaderSecondary
        {
            get => HeaderSecondaryValue;
            set => SetProperty(ref HeaderSecondaryValue, value, nameof(HeaderSecondary));
        }


        // Decorations, Accents, etc.
        private string FSFontValue = "#FF000000";
        public string FSFont
        {
            get => FSFontValue;
            set => SetProperty(ref FSFontValue, value, nameof(FSFont));
        }

        private string AccentColorValue = "#FF65ACE2";
        public string AccentColor
        {
            get => AccentColorValue;
            set => SetProperty(ref AccentColorValue, value, nameof(AccentColor));
        }

        private string DecorationPrimaryValue = "#FFFFFFFF";
        public string DecorationPrimary
        {
            get => DecorationPrimaryValue;
            set => SetProperty(ref DecorationPrimaryValue, value, nameof(DecorationPrimary));
        }

        private string DecorationSecondaryValue = "#FFD3D3D3";
        public string DecorationSecondary
        {
            get => DecorationSecondaryValue;
            set => SetProperty(ref DecorationSecondaryValue, value, nameof(DecorationSecondary));
        }


        // Underlays
        private string UnderlayPrimaryValue = "#44DDDDDD";
        public string UnderlayPrimary
        {
            get => UnderlayPrimaryValue;
            set => SetProperty(ref UnderlayPrimaryValue, value, nameof(UnderlayPrimary));
        }

        private string UnderlaySecondaryValue = "#44444444";
        public string UnderlaySecondary
        {
            get => UnderlaySecondaryValue;
            set => SetProperty(ref UnderlaySecondaryValue, value, nameof(UnderlaySecondary));
        }


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
            BackgroundGradStart = "#FF2D2D2D";
            BackgroundGradEnd = "#FF454545";
            BackgroundImageOverlay = "#00000000";

            ButtonFont = "#FFFFFFFF";
            ButtonPrimary = "#44AAAAAA";
            ButtonSecondary = "#FFFFFFFF";            
            SelectedButtonPrimary = "#55e3e3e3";
            SelectedButtonSecondary = "#FFFFFFFF";

            CellPrimary = "#FFBBBBBB";
            CellSecondary = "#FF777777";
            SelectedCellPrimary = "#FFDDDDDD";
            SelectedCellSecondary = "#FF999999";

            HeaderFont = "#FF000000";
            HeaderPrimary = "#FFF0F0F0";
            HeaderSecondary = "#FF999999";

            FSFont = "#FFFFFFFF";
            AccentColor = "#FF5C9EFF";
            DecorationPrimary = "#FFC8C8C8";
            DecorationSecondary = "#FFA0A0A0";

            UnderlayPrimary = "#22DDDDDD";
            UnderlaySecondary = "#99222222";
        }

        public object Clone()
        {
            return new Theme
            {
                Name = this.Name,
                Font = this.Font,
                BackgroundGradStart = this.BackgroundGradStart,
                BackgroundGradEnd = this.BackgroundGradEnd,
                BackgroundImageOn = this.BackgroundImageOn,
                BackgroundImage = this.BackgroundImage,
                BackgroundImagePath = this.BackgroundImagePath,
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
                UnderlayPrimary = this.UnderlayPrimary,
                UnderlaySecondary = this.UnderlaySecondary
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected bool SetProperty<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }

    public class ThemeData : INotifyPropertyChanged, ICloneable
    {
        private string currentKey;
        private Theme currentTheme;
        public Dictionary<string, Theme> Themes { get; set; } = new Dictionary<string, Theme>();

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

        public ThemeData()
        {
        }

        public object Clone()
        {
            return new ThemeData()
            {
                CurrentKey = this.CurrentKey,
                CurrentTheme = this.CurrentTheme,
                Themes = this.Themes
            };
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
