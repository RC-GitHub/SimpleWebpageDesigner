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
    /// Interaction logic for ComponentWindow.xaml
    /// </summary>
    public partial class ComponentWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public Component MyComponent { get; set; }
        public ComponentWindow(Component component)
        {
            InitializeComponent();
            if (component == null)
            {
                Errors.DisplayMessage("Component cannot be null. Please provide a valid component.");
                this.Close();
            }

            MyComponent = component;
            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
            this.DataContext = App.themeData.CurrentTheme;

            InitializeTextSimple();
            Loaded += (s, e) => InitializeSliders();

            AttachHoverPopupHandlers(rectBackground, popBackground);
            AttachHoverPopupHandlers(rectBorder, popBorder);
            AttachHoverPopupHandlers(rectSelectedBackground, popSelectedBackground);
            AttachHoverPopupHandlers(rectSelectedBorder, popSelectedBorder);
            AttachHoverPopupHandlers(rectBorderColor, popBorderColor);
            AttachHoverPopupHandlers(rectGradientStart, popGradientStart);
            AttachHoverPopupHandlers(rectGradientEnd, popGradientEnd);

        }
        private void ThemeData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.DataContext = App.themeData.CurrentTheme;
        }

        private void InitializeTextSimple()
        {
            frameHost.Visibility = Visibility.Visible;

            var editorPage = new TextSimple(MyComponent.Content);
            editorPage.Loaded += EditorPage_Loaded;

            frameHost.Navigate(editorPage);
        }

        private void EditorPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextSimple editorPage)
            {
                editorPage.NavigationService.Navigated += NavigationService_Navigated;
            }
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is TextSimple editorPage)
                return; 

            if (frameHost.Content is TextSimple oldEditor)
            {
                MyComponent.Content = oldEditor.ComponentContent;
            }

            frameHost.Visibility = Visibility.Collapsed;
            frameHost.Content = null;
        }

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

        private void SetColor(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle rect)
            {
                ApplyColor(rect);
            }
        }

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

        private void btnBackgroundImage_Click(object sender, RoutedEventArgs e)
        {
            string projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\.."));
            string configPath = Path.Combine(projectDirectory, "Images");

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp, *.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Select an image",
                InitialDirectory = configPath
            };

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = openFileDialog.FileName;
                string fileName = System.IO.Path.GetFileName(selectedPath);
                string savePath = Path.Combine(configPath, fileName);

                if (selectedPath != savePath) File.Copy(selectedPath, savePath, true);

                tbBackgroundImage.Text = fileName;
                BindingExpression binding = tbBackgroundImage.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty);
                binding?.UpdateSource();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (frameHost.Content is TextSimple editorPage)
            {
                MyComponent.Content = editorPage.ComponentContent;
            }
            this.Close();
        }
    }
}
