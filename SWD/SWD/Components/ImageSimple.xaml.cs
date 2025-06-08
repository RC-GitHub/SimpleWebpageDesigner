using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using Path = System.IO.Path;

namespace SWD.Components
{
    /// <summary>
    /// Interaction logic and data binding for the ImageSimple component editor.
    /// Allows users to select, display, and configure an image for a component,
    /// including alignment, size, stretch, and visual effects.
    /// </summary>
    public partial class ImageSimple : Page, INotifyPropertyChanged
    {
        private string projectPath;
        private ComponentContent _componentContent;
        private BitmapImage originalImage;

        /// <summary>
        /// Gets or sets the component content (image properties) being edited.
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
        /// Initializes a new instance of the <see cref="ImageSimple"/> class.
        /// </summary>
        /// <param name="compcont">The component content to edit.</param>
        /// <param name="path">The project path for image lookup and storage.</param>
        public ImageSimple(ComponentContent compcont, string path)
        {
            InitializeComponent();
            ComponentContent = compcont;
            projectPath = path;
            Debug.WriteLine(projectPath);

            this.DataContext = App.themeData.CurrentTheme;
            App.themeData.PropertyChanged += ThemeData_PropertyChanged;

            string imageDir = Path.Combine(projectPath, "images");
            if (!Directory.Exists(imageDir))
            {
                Directory.CreateDirectory(imageDir);
                NoImage.Visibility = Visibility.Visible;
            }
            else
            {
                if (ComponentContent.ImageName != "" && ComponentContent != null)
                {
                    if (ComponentContent != null && !string.IsNullOrWhiteSpace(ComponentContent.ImageName))
                    {
                        LoadImage(imageDir);
                        ApplyFilter(ComponentContent.ImageFilter);
                        NoImage.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        Errors.DisplayMessage($"{ComponentContent.ImageName} is not present in the images folder!");
                        NoImage.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        /// <summary>
        /// Handles theme changes and updates the DataContext.
        /// </summary>
        private void ThemeData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.DataContext = App.themeData.CurrentTheme;
        }

        /// <summary>
        /// Loads the image from the images directory based on the current <see cref="ComponentContent.ImageName"/>.
        /// </summary>
        /// <param name="imageDir">The directory containing images.</param>
        private void LoadImage(string imageDir)
        {
            string imagePath = Path.Combine(imageDir, ComponentContent.ImageName);
            PathInputBox.Text = imagePath;

            if (File.Exists(imagePath))
            {
                LoadImageFromPath(imagePath);
                NoImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoImage.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Handles the Open Image button click, allowing the user to select an image file.
        /// Copies the selected image to the project images directory if needed.
        /// </summary>
        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                InitialDirectory = Path.Combine(projectPath, "images"),
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ComponentContent.ImageName = Path.GetFileName(dlg.FileName);
                string newDir = Path.Combine(projectPath, "images", ComponentContent.ImageName);
                if (!File.Exists(newDir))
                    File.Copy(dlg.FileName, newDir);
                PathInputBox.Text = newDir;
                LoadImageFromPath(newDir);
            }
        }

        /// <summary>
        /// Loads an image from the specified file path and displays it in the UI.
        /// </summary>
        /// <param name="path">The full file path to the image.</param>
        private void LoadImageFromPath(string path)
        {
            try
            {
                originalImage = new BitmapImage();
                originalImage.BeginInit();
                originalImage.UriSource = new Uri(path);
                originalImage.CacheOption = BitmapCacheOption.OnLoad;
                originalImage.EndInit();

                ImageViewer.Source = originalImage;
                PathInputBox.Text = path;

                NoImage.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Errors.DisplayMessage($"Failed to load image: {ex.Message}");
            }
        }

        /// <summary>
        /// Applies the current settings (size, alignment, stretch, filter) to the displayed image.
        /// </summary>
        private void ApplySettings_Click(object sender, RoutedEventArgs e)
        {
            // Size
            if (double.TryParse(WidthBox.Text, out double width))
                ImageViewer.Width = width;
            else
                ImageViewer.Width = double.NaN;

            if (double.TryParse(HeightBox.Text, out double height))
                ImageViewer.Height = height;
            else
                ImageViewer.Height = double.NaN;

            string hAlign = (HAlignBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (hAlign == "Left")
                ImageViewer.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            else if (hAlign == "Center")
                ImageViewer.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            else if (hAlign == "Right")
                ImageViewer.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            else if (hAlign == "Stretch")
                ImageViewer.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            else
                ImageViewer.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            string vAlign = (VAlignBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (vAlign == "Top")
                ImageViewer.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            else if (vAlign == "Center")
                ImageViewer.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            else if (vAlign == "Bottom")
                ImageViewer.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            else if (vAlign == "Stretch")
                ImageViewer.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            else
                ImageViewer.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            string stretch = (StretchModeBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (stretch == "Uniform")
                ImageViewer.Stretch = Stretch.Uniform;
            else if (stretch == "UniformToFill")
                ImageViewer.Stretch = Stretch.UniformToFill;
            else if (stretch == "Fill")
                ImageViewer.Stretch = Stretch.Fill;
            else if (stretch == "None")
                ImageViewer.Stretch = Stretch.None;
            else
                ImageViewer.Stretch = Stretch.Uniform;

            // Filter
            string filter = (FilterBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            ApplyFilter(filter);
        }

        /// <summary>
        /// Applies a visual filter (blur, shadow, grayscale, or none) to the displayed image.
        /// </summary>
        /// <param name="filter">The filter name to apply.</param>
        private void ApplyFilter(string filter)
        {
            if (ImageViewer.Source == null) return;

            switch (filter)
            {
                case "Blur":
                    ImageViewer.Effect = new BlurEffect { Radius = 5 };
                    break;

                case "Shadow":
                    ImageViewer.Effect = new DropShadowEffect
                    {
                        Color = System.Windows.Media.Colors.Black,
                        BlurRadius = 10,
                        ShadowDepth = 0,
                        Opacity = 0.5
                    };
                    break;

                case "Grayscale":
                    if (originalImage != null)
                    {
                        var grayBitmap = new FormatConvertedBitmap();
                        grayBitmap.BeginInit();
                        grayBitmap.Source = originalImage;
                        grayBitmap.DestinationFormat = PixelFormats.Gray8;
                        grayBitmap.EndInit();

                        ImageViewer.Source = grayBitmap;
                    }
                    ImageViewer.Effect = null;
                    break;

                default:
                    ImageViewer.Effect = null;
                    if (originalImage != null)
                        ImageViewer.Source = originalImage;
                    break;
            }
        }
    }
}
