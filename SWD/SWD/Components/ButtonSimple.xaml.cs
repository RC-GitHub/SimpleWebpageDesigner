using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextBox = System.Windows.Controls.TextBox;
using Path = System.IO.Path;
using System.IO;
using static SWD.Build;
using System.Text.Json;
using System.Diagnostics;

namespace SWD.Components
{
    /// <summary>
    /// Interaction logic and data binding for the ButtonSimple component editor.
    /// Allows editing of button appearance and style, including custom and predefined styles.
    /// </summary>
    public partial class ButtonSimple : Page, INotifyPropertyChanged
    {
        private string projectPath;
        private ComponentContent _componentContent;

        /// <summary>
        /// Gets or sets the component content (button properties) being edited.
        /// </summary>
        public ComponentContent ComponentContent
        {
            get => _componentContent;
            set
            {
                if (_componentContent != value)
                {
                    _componentContent = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="name">The name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonSimple"/> class.
        /// </summary>
        /// <param name="compcont">The component content to edit.</param>
        /// <param name="path">The project path for metadata lookup.</param>
        public ButtonSimple(ComponentContent compcont, string path)
        {
            InitializeComponent();
            projectPath = path;
            ComponentContent = compcont;
            this.DataContext = App.themeData.CurrentTheme;
            App.themeData.PropertyChanged += ThemeData_PropertyChanged;

            this.Loaded += ButtonSimple_Loaded; // Attach loaded handler
        }

        /// <summary>
        /// Handles the Loaded event to ensure ComboBox is initialized.
        /// </summary>
        private void ButtonSimple_Loaded(object sender, RoutedEventArgs e)
        {
            cbPredefinedStyle_SelectionChanged(cbPredefinedStyle, null); // Now the ComboBox is ready
        }

        /// <summary>
        /// Handles theme changes and updates the DataContext.
        /// </summary>
        private void ThemeData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.DataContext = App.themeData.CurrentTheme;
        }

        /// <summary>
        /// Handles the font color picker button click.
        /// Updates the ButtonFontColor property.
        /// </summary>
        private void btnFontColor_Click(object sender, RoutedEventArgs e)
        {
            ComponentContent.ButtonFontColor = Colors.BrushColorPicker();
            BindingExpression binding = tbFontColor.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();
        }

        /// <summary>
        /// Handles the background color picker button click.
        /// Updates the ButtonBackgroundColor property.
        /// </summary>
        private void btnBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            ComponentContent.ButtonBackgroundColor = Colors.BrushColorPicker();
            BindingExpression binding = tbBackgroundColor.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();
        }

        /// <summary>
        /// Handles the border color picker button click.
        /// Updates the ButtonBorderColor property.
        /// </summary>
        private void btnBorderColor_Click(object sender, RoutedEventArgs e)
        {
            ComponentContent.ButtonBorderColor = Colors.BrushColorPicker();
            BindingExpression binding = tbBorderColor.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();
        }

        /// <summary>
        /// Handles changes to the predefined style ComboBox.
        /// Applies predefined or custom styles to the button.
        /// </summary>
        private void cbPredefinedStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPredefinedStyle.SelectedItem == null)
                return;

            string style = cbPredefinedStyle.SelectedValue.ToString();
            if (style == "Custom")
            {
                tbBackgroundColor.IsEnabled = true;
                btnBackgroundColor.IsEnabled = true;
                tbBorderColor.IsEnabled = true;
                btnBorderColor.IsEnabled = true;
            }
            else
            {
                tbBackgroundColor.IsEnabled = false;
                btnBackgroundColor.IsEnabled = false;
                tbBorderColor.IsEnabled = false;
                btnBorderColor.IsEnabled = false;
            }

            if (!style.Contains("-"))
                return;

            var parts = style.Split('-');
            if (parts.Length != 2)
                return;

            string bgKey = parts[0].Trim().ToLower();
            string borderKey = parts[1].Trim().ToLower();

            string metadataPath = Path.Combine(projectPath, "metadata.json");
            if (!File.Exists(metadataPath))
                return;

            var json = File.ReadAllText(metadataPath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new SolidColorBrushConverter() } // Ensure this converter exists
            };
            var metadata = JsonSerializer.Deserialize<List<Head>>(json, options)?.FirstOrDefault();
            if (metadata?.Layout == null)
                return;

            // Resolves a color key to a SolidColorBrush from metadata.
            SolidColorBrush ResolveColor(string key)
            {
                if (key == "grid")
                    return metadata.Layout.GridColor;
                else if (key == "footer")
                    return metadata.Layout.FooterColor;
                else if (key == "body")
                    return metadata.Layout.BodyColor;
                else if (key == "header")
                    return metadata.Layout.HeaderColor;
                else
                {
                    Debug.WriteLine(key);
                    return new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                }
            }

            var bgColor = ResolveColor(bgKey);
            var borderColor = ResolveColor(borderKey);

            if (ComponentContent != null)
            {
                ComponentContent.ButtonBackgroundColor = bgColor;
                ComponentContent.ButtonBorderColor = borderColor;
                BindingExpression bindingBg = tbBackgroundColor.GetBindingExpression(TextBox.TextProperty);
                bindingBg?.UpdateSource();
                BindingExpression bindingBrd = tbBorderColor.GetBindingExpression(TextBox.TextProperty);
                bindingBrd?.UpdateSource();
            }
        }
    }
}
