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
    /// <summary>
    /// Represents a theme with color, font, and style properties for the application UI.
    /// </summary>
    public class Theme : ICloneable, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the theme name.
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        // Base Font
        private string FontValue = "#FF000000";
        /// <summary>
        /// Gets or sets the base font color.
        /// </summary>
        public string Font
        {
            get => FontValue;
            set => SetProperty(ref FontValue, value, nameof(Font));
        }

        // Backgrounds
        public string BackgroundGradStartValue = "#FF97C9EF";
        /// <summary>
        /// Gets or sets the background gradient start color.
        /// </summary>
        public string BackgroundGradStart
        {
            get => BackgroundGradStartValue;
            set => SetProperty(ref BackgroundGradStartValue, value, nameof(BackgroundGradStart));
        }

        public string BackgroundGradEndValue = "#FFF0F8FF";
        /// <summary>
        /// Gets or sets the background gradient end color.
        /// </summary>
        public string BackgroundGradEnd
        {
            get => BackgroundGradEndValue;
            set => SetProperty(ref BackgroundGradEndValue, value, nameof(BackgroundGradEnd));
        }

        private bool BackgroundImageOnValue = false;
        /// <summary>
        /// Gets or sets whether a background image is enabled.
        /// </summary>
        public bool BackgroundImageOn
        {
            get => BackgroundImageOnValue;
            set => SetProperty(ref BackgroundImageOnValue, value, nameof(BackgroundImageOn));
        }

        private string BackgroundImageValue { get; set; }
        /// <summary>
        /// Gets or sets the background image filename.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the full path to the background image.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the horizontal alignment of the background image.
        /// </summary>
        public string BackgroundImageAlignX
        {
            get => BackgroundImageAlignXValue;
            set => SetProperty(ref BackgroundImageAlignXValue, value, nameof(BackgroundImageAlignX));
        }

        private string BackgroundImageAlignYValue = "Center";
        /// <summary>
        /// Gets or sets the vertical alignment of the background image.
        /// </summary>
        public string BackgroundImageAlignY
        {
            get => BackgroundImageAlignYValue;
            set => SetProperty(ref BackgroundImageAlignYValue, value, nameof(BackgroundImageAlignY));
        }

        private string BackgroundImageStretchValue = "UniformToFill";
        /// <summary>
        /// Gets or sets the stretch mode of the background image.
        /// </summary>
        public string BackgroundImageStretch
        {
            get => BackgroundImageStretchValue;
            set => SetProperty(ref BackgroundImageStretchValue, value, nameof(BackgroundImageStretch));
        }

        private float BackgroundImageOpacityValue = 1;
        /// <summary>
        /// Gets or sets the opacity of the background image.
        /// </summary>
        public float BackgroundImageOpacity
        {
            get => BackgroundImageOpacityValue;
            set => SetProperty(ref BackgroundImageOpacityValue, value, nameof(BackgroundImageOpacity));
        }

        public string BackgroundImageOverlayValue = "#00000000";
        /// <summary>
        /// Gets or sets the overlay color for the background image.
        /// </summary>
        public string BackgroundImageOverlay
        {
            get => BackgroundImageOverlayValue;
            set => SetProperty(ref BackgroundImageOverlayValue, value, nameof(BackgroundImageOverlay));
        }

        // Buttons
        private string ButtonFontValue = "#FF000000";
        /// <summary>
        /// Gets or sets the button font color.
        /// </summary>
        public string ButtonFont
        {
            get => ButtonFontValue;
            set => SetProperty(ref ButtonFontValue, value, nameof(ButtonFont));
        }

        private string ButtonPrimaryValue = "#FFFFFFFF";
        /// <summary>
        /// Gets or sets the primary button color.
        /// </summary>
        public string ButtonPrimary
        {
            get => ButtonPrimaryValue;
            set => SetProperty(ref ButtonPrimaryValue, value, nameof(ButtonPrimary));
        }

        private string ButtonSecondaryValue = "#FF458BC0";
        /// <summary>
        /// Gets or sets the secondary button color.
        /// </summary>
        public string ButtonSecondary
        {
            get => ButtonSecondaryValue;
            set => SetProperty(ref ButtonSecondaryValue, value, nameof(ButtonSecondary));
        }

        private string SelectedButtonPrimaryValue = "#FFC2DBFC";
        /// <summary>
        /// Gets or sets the selected primary button color.
        /// </summary>
        public string SelectedButtonPrimary
        {
            get => SelectedButtonPrimaryValue;
            set => SetProperty(ref SelectedButtonPrimaryValue, value, nameof(SelectedButtonPrimary));
        }

        private string SelectedButtonSecondaryValue = "#FF458BC0";
        /// <summary>
        /// Gets or sets the selected secondary button color.
        /// </summary>
        public string SelectedButtonSecondary
        {
            get => SelectedButtonSecondaryValue;
            set => SetProperty(ref SelectedButtonSecondaryValue, value, nameof(SelectedButtonSecondary));
        }

        // Cells
        /// <summary>
        /// Gets or sets the primary cell color.
        /// </summary>
        public string CellPrimary { get; set; } = "#00000000";
        /// <summary>
        /// Gets or sets the secondary cell color.
        /// </summary>
        public string CellSecondary { get; set; } = "#C2DFEDF0";
        /// <summary>
        /// Gets or sets the selected primary cell color.
        /// </summary>
        public string SelectedCellPrimary { get; set; } = "#FFF0F8FF";
        /// <summary>
        /// Gets or sets the selected secondary cell color.
        /// </summary>
        public string SelectedCellSecondary { get; set; } = "#FFA7C4DD";

        // Headers
        private string HeaderFontValue = "#FF000000";
        /// <summary>
        /// Gets or sets the header font color.
        /// </summary>
        public string HeaderFont
        {
            get => HeaderFontValue;
            set => SetProperty(ref HeaderFontValue, value, nameof(HeaderFont));
        }

        private string HeaderPrimaryValue = "#FFFFFFFF";
        /// <summary>
        /// Gets or sets the primary header color.
        /// </summary>
        public string HeaderPrimary
        {
            get => HeaderPrimaryValue;
            set => SetProperty(ref HeaderPrimaryValue, value, nameof(HeaderPrimary));
        }

        private string HeaderSecondaryValue = "#FF778899";
        /// <summary>
        /// Gets or sets the secondary header color.
        /// </summary>
        public string HeaderSecondary
        {
            get => HeaderSecondaryValue;
            set => SetProperty(ref HeaderSecondaryValue, value, nameof(HeaderSecondary));
        }

        // Decorations, Accents, etc.
        private string FSFontValue = "#FF000000";
        /// <summary>
        /// Gets or sets the font color for the file selector.
        /// </summary>
        public string FSFont
        {
            get => FSFontValue;
            set => SetProperty(ref FSFontValue, value, nameof(FSFont));
        }

        private string AccentColorValue = "#FF65ACE2";
        /// <summary>
        /// Gets or sets the accent color.
        /// </summary>
        public string AccentColor
        {
            get => AccentColorValue;
            set => SetProperty(ref AccentColorValue, value, nameof(AccentColor));
        }

        private string DecorationPrimaryValue = "#FFFFFFFF";
        /// <summary>
        /// Gets or sets the primary decoration color.
        /// </summary>
        public string DecorationPrimary
        {
            get => DecorationPrimaryValue;
            set => SetProperty(ref DecorationPrimaryValue, value, nameof(DecorationPrimary));
        }

        private string DecorationSecondaryValue = "#FFD3D3D3";
        /// <summary>
        /// Gets or sets the secondary decoration color.
        /// </summary>
        public string DecorationSecondary
        {
            get => DecorationSecondaryValue;
            set => SetProperty(ref DecorationSecondaryValue, value, nameof(DecorationSecondary));
        }

        // Underlays
        private string UnderlayPrimaryValue = "#44DDDDDD";
        /// <summary>
        /// Gets or sets the primary underlay color.
        /// </summary>
        public string UnderlayPrimary
        {
            get => UnderlayPrimaryValue;
            set => SetProperty(ref UnderlayPrimaryValue, value, nameof(UnderlayPrimary));
        }

        private string UnderlaySecondaryValue = "#11444444";
        /// <summary>
        /// Gets or sets the secondary underlay color.
        /// </summary>
        public string UnderlaySecondary
        {
            get => UnderlaySecondaryValue;
            set => SetProperty(ref UnderlaySecondaryValue, value, nameof(UnderlaySecondary));
        }

        /// <summary>
        /// Gets or sets a property value by name using reflection.
        /// </summary>
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

        /// <summary>
        /// Sets the theme to a predefined dark color scheme.
        /// </summary>
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

        /// <summary>
        /// Creates a deep copy of the theme.
        /// </summary>
        /// <returns>A new <see cref="Theme"/> instance with the same values.</returns>
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

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for a property.
        /// </summary>
        /// <param name="name">The property name.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Sets the property value and raises the PropertyChanged event if the value changes.
        /// </summary>
        protected bool SetProperty<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    /// <summary>
    /// Holds all theme data, including the current theme and a dictionary of available themes.
    /// </summary>
    public class ThemeData : INotifyPropertyChanged, ICloneable
    {
        private string currentKey;
        private Theme currentTheme;

        /// <summary>
        /// Gets or sets the dictionary of available themes.
        /// </summary>
        public Dictionary<string, Theme> Themes { get; set; } = new Dictionary<string, Theme>();

        /// <summary>
        /// Gets or sets the key of the current theme.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the current theme.
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeData"/> class.
        /// </summary>
        public ThemeData()
        {
        }

        /// <summary>
        /// Creates a deep copy of the theme data.
        /// </summary>
        /// <returns>A new <see cref="ThemeData"/> instance with the same values.</returns>
        public object Clone()
        {
            return new ThemeData()
            {
                CurrentKey = this.CurrentKey,
                CurrentTheme = this.CurrentTheme,
                Themes = this.Themes
            };
        }

        /// <summary>
        /// Loads theme data from the configuration file, or creates default themes if not found.
        /// </summary>
        /// <returns>A <see cref="ThemeData"/> instance with loaded or default themes.</returns>
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

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for a property.
        /// </summary>
        /// <param name="propName">The property name.</param>
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
