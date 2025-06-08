using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;
using System.Drawing;
using System.Windows.Controls.Primitives;
using Rectangle = System.Windows.Shapes.Rectangle;
using ColorConverter = System.Windows.Media.ColorConverter;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Xml.Linq;
using Color = System.Windows.Media.Color;
using System.Windows.Media.Media3D;
using System.IO;
using Path = System.IO.Path;
using System.Windows.Forms;
using Label = System.Windows.Controls.Label;
using System.Windows.Navigation;


namespace SWD.Components
{

    /// <summary>
    /// Interaction logic and data binding for the ComponentWindow editor.
    /// Allows editing of various component types (text, image, code, button) and their styles.
    /// </summary>
    public partial class ComponentWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// The path to the current project.
        /// </summary>
        public string projectPath;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// The component being edited.
        /// </summary>
        public Component MyComponent { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentWindow"/> class.
        /// </summary>
        /// <param name="component">The component to edit.</param>
        /// <param name="path">The project path for resource lookup.</param>
        public ComponentWindow(Component component, string path)
        {
            InitializeComponent();
            projectPath = path;
            if (component == null)
            {
                Errors.DisplayMessage("Component cannot be null. Please provide a valid component.");
                this.Close();
            }

            MyComponent = component;
            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
            this.DataContext = App.themeData.CurrentTheme;

            MyComponent.PropertyChanged += MyComponent_PropertyChanged;
            LoadEditor();
            Loaded += (s, e) => InitializeSliders();

            AttachHoverPopupHandlers(rectBackground, popBackground);
            AttachHoverPopupHandlers(rectBorder, popBorder);
            AttachHoverPopupHandlers(rectSelectedBackground, popSelectedBackground);
            AttachHoverPopupHandlers(rectSelectedBorder, popSelectedBorder);
            AttachHoverPopupHandlers(rectBorderColor, popBorderColor);
            AttachHoverPopupHandlers(rectGradientStart, popGradientStart);
            AttachHoverPopupHandlers(rectGradientEnd, popGradientEnd);
        }

        /// <summary>
        /// Handles theme changes and updates the DataContext.
        /// </summary>
        private void ThemeData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.DataContext = App.themeData.CurrentTheme;
        }

        /// <summary>
        /// Handles changes to the component's type and reloads the editor page.
        /// </summary>
        private void MyComponent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Component.Type))
            {
                LoadEditor();
            }
        }

        /// <summary>
        /// Loads the appropriate editor page based on the component type.
        /// </summary>
        private void LoadEditor()
        {
            frameHost.Visibility = Visibility.Visible;

            Page editorPage = null;

            switch (MyComponent.Type?.ToLowerInvariant())
            {
                case "text":
                    editorPage = (new TextSimple(MyComponent.Content));
                    break;
                case "image":
                    editorPage = (new ImageSimple(MyComponent.Content, projectPath));
                    break;
                case "code":
                    editorPage = new CodeSimple(MyComponent.Content);
                    break;
                case "button":
                    editorPage = new ButtonSimple(MyComponent.Content, projectPath);
                    break;
                default:
                    Errors.DisplayMessage("Unknown component type: " + MyComponent.Type);
                    frameHost.Visibility = Visibility.Collapsed;
                    return;
            }

            frameHost.Navigate(editorPage);
        }

        /// <summary>
        /// Handles the Loaded event for editor pages and attaches navigation event handlers.
        /// </summary>
        private void EditorPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextSimple textPage)
            {
                textPage.NavigationService.Navigated += NavigationService_Navigated;
            }
            else if (sender is ImageSimple imagePage)
            {
                imagePage.NavigationService.Navigated += NavigationService_Navigated;
            }
            else if (sender is CodeSimple codePage)
            {
                codePage.NavigationService.Navigated += NavigationService_Navigated;
            }
            else if (sender is ButtonSimple buttonPage)
            {
                buttonPage.NavigationService.Navigated += NavigationService_Navigated;
            }
        }

        /// <summary>
        /// Handles navigation events and updates the component's content from the editor page.
        /// </summary>
        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is TextSimple textPage)
            {
                MyComponent.Content = textPage.ComponentContent;
            }
            else if (e.Content is ImageSimple imagePage)
            {
                MyComponent.Content = imagePage.ComponentContent;
            }
            else if (e.Content is CodeSimple codePage)
            {
                MyComponent.Content = codePage.ComponentContent;
            }
            else if (e.Content is ButtonSimple buttonPage)
            {
                MyComponent.Content = buttonPage.ComponentContent;
            }

            frameHost.Visibility = Visibility.Collapsed;
            frameHost.Content = null;
        }

        /// <summary>
        /// Initializes the alpha sliders for color rectangles based on their current color values.
        /// </summary>
        private void InitializeSliders()
        {
            Debug.WriteLine("HELP");
            if (rectBackground.Fill is SolidColorBrush brushBackground)
            {
                Color color = brushBackground.Color;
                Debug.WriteLine(color.A);
                slidBackground.Value = (double)color.A;
            }
            if (rectBorder.Fill is SolidColorBrush brushBorder)
            {
                Color color = brushBorder.Color;
                slidBorder.Value = (double)color.A;
            }
            if (rectSelectedBackground.Fill is SolidColorBrush brushSelectedBackground)
            {
                Color color = brushSelectedBackground.Color;
                slidSelectedBackground.Value = (double)color.A;
            }
            if (rectSelectedBorder.Fill is SolidColorBrush brushSelectedBorder)
            {
                Color color = brushSelectedBorder.Color;
                slidSelectedBorder.Value = (double)color.A;
            }
            if (rectBorderColor.Fill is SolidColorBrush brushBorderColor)
            {
                Color color = brushBorderColor.Color;
                slidBorderColor.Value = (double)color.A;
            }
            if (rectGradientStart.Fill is SolidColorBrush brushGradientStart)
            {
                Color color = brushGradientStart.Color;
                slidGradientStart.Value = (double)color.A;
            }
            if (rectGradientEnd.Fill is SolidColorBrush brushGradientEnd)
            {
                Color color = brushGradientEnd.Color;
                slidGradientEnd.Value = (double)color.A;
            }
        }

        /// <summary>
        /// Attaches hover event handlers to show and hide popups for color rectangles.
        /// </summary>
        /// <param name="trigger">The UI element that triggers the popup.</param>
        /// <param name="popup">The popup to show/hide.</param>
        private void AttachHoverPopupHandlers(FrameworkElement trigger, Popup popup)
        {
            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) };

            timer.Tick += (s, e) =>
            {
                timer.Stop();
                if (!trigger.IsMouseOver && !popup.IsMouseOver)
                    popup.IsOpen = false;
            };

            trigger.MouseEnter += (s, e) =>
            {
                timer.Stop();
                popup.IsOpen = true;
            };

            trigger.MouseLeave += (s, e) => timer.Start();
            popup.MouseEnter += (s, e) => timer.Stop();
            popup.MouseLeave += (s, e) => timer.Start();
        }

        /// <summary>
        /// Handles mouse click on a color rectangle and opens the color picker dialog.
        /// </summary>
        private void SetColor(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle rect)
            {
                ApplyColor(rect);
            }
        }

        /// <summary>
        /// Handles mouse click on a label associated with a color rectangle and opens the color picker dialog.
        /// </summary>
        private void LabelSetColor(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("this hap");
            if (sender is Label label)
            {
                string labelName = label.Name.Substring(3);
                Rectangle rect = (Rectangle)this.FindName($"rect{labelName}");
                ApplyColor(rect);
            }
        }

        string[] compStyle = new string[] { "BorderColor", "GradientStart", "GradientEnd" };

        /// <summary>
        /// Applies a selected color to the specified rectangle and updates the corresponding property.
        /// </summary>
        /// <param name="rectangle">The rectangle to update.</param>
        private void ApplyColor(Rectangle rectangle)
        {
            if (rectangle == null) return;

            string rectangleName = rectangle.Name.Substring(4);
            Slider slider = (Slider)this.FindName($"slid{rectangleName}");
            if (slider == null) return;

            string colorString = Colors.StringColorPicker();
            if (ColorConverter.ConvertFromString(colorString) is System.Windows.Media.Color color)
            {
                color.A = (byte)slider.Value;

                if (compStyle.Contains(rectangleName))
                {
                    var property = typeof(ComponentStyle).GetProperty(rectangleName);
                    if (property != null && property.PropertyType == typeof(SolidColorBrush))
                    {
                        property.SetValue(MyComponent.CompStyle, new SolidColorBrush(color));
                    }
                    else
                    {
                        Errors.DisplayMessage($"Property '{rectangleName}' not found or invalid on ComponentStyle.");
                    }
                }
                else
                {
                    var property = typeof(Component).GetProperty($"{rectangleName}Color");
                    if (property != null && property.PropertyType == typeof(SolidColorBrush))
                    {
                        property.SetValue(MyComponent, new SolidColorBrush(color));
                    }
                    else
                    {
                        Errors.DisplayMessage($"Property '{rectangleName}Color' not found or invalid.");
                    }
                }
            }
            else
            {
                Errors.DisplayMessage("Invalid color string returned by StringColorPicker.");
            }
        }

        /// <summary>
        /// Handles slider value changes to update the alpha channel of the associated color rectangle.
        /// </summary>
        private void SliderChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            string sliderName = slider.Name.Substring(4);

            Rectangle rectangle = (Rectangle)this.FindName($"rect{sliderName}");
            if (rectangle.Fill is SolidColorBrush sliderBrush)
            {
                Color color = sliderBrush.Color;
                color.A = (byte)slider.Value;
                sliderBrush.Color = color;

                BindingExpression binding = BindingOperations.GetBindingExpression(rectangle, Rectangle.FillProperty);
                binding?.UpdateSource();
            }
        }

        /// <summary>
        /// Handles the background image browse button click, allowing the user to select and copy an image to the project.
        /// </summary>
        private void btnBackgroundImage_Click(object sender, RoutedEventArgs e)
        {
            string configPath = Path.Combine(projectPath, "images");
            if (!Directory.Exists(configPath))
                Directory.CreateDirectory(configPath);

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp, *.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Select an image",
                InitialDirectory = configPath
            };

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = openFileDialog.FileName;
                string fileName = Path.GetFileName(selectedPath);
                string savePath = Path.Combine(configPath, fileName);

                if (selectedPath != savePath) File.Copy(selectedPath, savePath, true);

                tbBackgroundImage.Text = fileName;
                BindingExpression binding = tbBackgroundImage.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty);
                binding?.UpdateSource();
            }
        }

        /// <summary>
        /// Handles the Save button click, updates the component content from the editor, and closes the window.
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (frameHost.Content is TextSimple editorPage)
            {
                MyComponent.Content = editorPage.ComponentContent;
            }
            this.Close();
        }

        /// <summary>
        /// Handles changes to the component type ComboBox and loads the corresponding editor page.
        /// </summary>
        private void ComponentTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (ComponentTypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            switch (selected)
            {
                case "text":
                    frameHost.Content = new TextSimple(MyComponent.Content);
                    break;
                case "image":
                    frameHost.Content = new ImageSimple(MyComponent.Content, projectPath);
                    break;
                case "code":
                    frameHost.Content = new CodeSimple(MyComponent.Content);
                    break;
                case "button":
                    frameHost.Content = new ButtonSimple(MyComponent.Content, projectPath);
                    break;
                default:
                    frameHost.Content = null;
                    break;
            }
        }

        /// <summary>
        /// Shows the size popup when the mouse enters the size header.
        /// </summary>
        private void SizeHeader_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            popSize.IsOpen = true;
        }

        /// <summary>
        /// Hides the size popup when the mouse leaves the size header.
        /// </summary>
        private void SizeHeader_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            popSize.IsOpen = false;
        }
    }
}
