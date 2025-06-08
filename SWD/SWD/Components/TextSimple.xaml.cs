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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SWD.Components
{
    /// <summary>
    /// Interaction logic and data binding for the TextSimple component editor.
    /// Allows users to edit text content and related properties for a component.
    /// </summary>
    public partial class TextSimple : Page, INotifyPropertyChanged
    {
        private ComponentContent _componentContent;

        /// <summary>
        /// Gets or sets the component content (text properties) being edited.
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
        /// Initializes a new instance of the <see cref="TextSimple"/> class.
        /// </summary>
        /// <param name="compcont">The component content to edit.</param>
        public TextSimple(ComponentContent compcont)
        {
            InitializeComponent();
            ComponentContent = compcont;
            DataContext = ComponentContent;
            InitializeActivation();
        }

        /// <summary>
        /// Initializes the toggle button states based on the current <see cref="ComponentContent"/> values.
        /// </summary>
        private void InitializeActivation()
        {
            if (ComponentContent.FontWeight == "Bold")
                togBtnBold.IsChecked = true;
            if (ComponentContent.FontStyle == "Italic")
                togBtnItalic.IsChecked = true;
            if (ComponentContent.FontLine == "Underline")
                togBtnUnderline.IsChecked = true;
            if (ComponentContent.TextHorizontal == "Left")
                togBtnLeft.IsChecked = true;
            if (ComponentContent.TextHorizontal == "Right")
                togBtnRight.IsChecked = true;
            if (ComponentContent.TextHorizontal == "Center")
                togBtnCenter.IsChecked = true;
            if (ComponentContent.TextHorizontal == "Justify")
                togBtnJustify.IsChecked = true;
            if (ComponentContent.TextVertical == "Top")
                togBtnTop.IsChecked = true;
            if (ComponentContent.TextVertical == "Center")
                togBtnMiddle.IsChecked = true;
            if (ComponentContent.TextVertical == "Bottom")
                togBtnBottom.IsChecked = true;
        }

        /// <summary>
        /// Cuts the selected text in the editor.
        /// </summary>
        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            txtEditor.Cut();
        }

        /// <summary>
        /// Copies the selected text in the editor.
        /// </summary>
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            txtEditor.Copy();
        }

        /// <summary>
        /// Pastes text from the clipboard into the editor.
        /// </summary>
        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            txtEditor.Paste();
        }

        /// <summary>
        /// Undoes the last action in the editor.
        /// </summary>
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (txtEditor.CanUndo) txtEditor.Undo();
        }

        /// <summary>
        /// Redoes the last undone action in the editor.
        /// </summary>
        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (txtEditor.CanRedo) txtEditor.Redo();
        }

        /// <summary>
        /// Toggles bold formatting for the selected text.
        /// </summary>
        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ComponentContent content)
            {
                if (content.FontWeight == "Normal")
                {
                    content.FontWeight = "Bold";
                    txtEditor.FontWeight = FontWeights.Bold;
                }
                else
                {
                    content.FontWeight = "Normal";
                    txtEditor.FontWeight = FontWeights.Normal;
                }
            }
        }

        /// <summary>
        /// Toggles italic formatting for the selected text.
        /// </summary>
        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ComponentContent content)
            {
                if (content.FontStyle == "Normal")
                {
                    content.FontStyle = "Italic";
                    txtEditor.FontStyle = FontStyles.Italic;
                }
                else
                {
                    content.FontStyle = "Normal";
                    txtEditor.FontStyle = FontStyles.Normal;
                }
            }
        }

        /// <summary>
        /// Toggles underline formatting for the selected text.
        /// </summary>
        private void Underline_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ComponentContent content)
            {
                if (content.FontLine == "None")
                {
                    content.FontLine = "Underline";
                    txtEditor.TextDecorations = TextDecorations.Underline;
                }
                else
                {
                    content.FontLine = "Normal";
                    txtEditor.TextDecorations = null;
                }
            }
        }

        /// <summary>
        /// Opens a color picker and sets the font color.
        /// </summary>
        private void FontColor_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ComponentContent content)
            {
                string colorString = Colors.StringColorPicker();
                if (ColorConverter.ConvertFromString(colorString) is System.Windows.Media.Color color)
                {
                    content.ForegroundColor = new SolidColorBrush(color);
                    txtEditor.Foreground = content.ForegroundColor;
                }
            }
        }

        /// <summary>
        /// Handles font family selection (not implemented).
        /// </summary>
        private void FontFamily_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Sets horizontal alignment to left.
        /// </summary>
        private void Left_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ComponentContent content)
            {
                if (content.TextHorizontal != "Left")
                {
                    content.TextHorizontal = "Left";
                    txtEditor.HorizontalContentAlignment = HorizontalAlignment.Left;

                    togBtnLeft.IsChecked = true;
                    togBtnCenter.IsChecked = false;
                    togBtnRight.IsChecked = false;
                    togBtnJustify.IsChecked = false;
                }
            }
        }

        /// <summary>
        /// Sets horizontal alignment to center.
        /// </summary>
        private void Center_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ComponentContent content)
            {
                if (content.TextHorizontal != "Center")
                {
                    content.TextHorizontal = "Center";
                    txtEditor.HorizontalContentAlignment = HorizontalAlignment.Center;

                    togBtnLeft.IsChecked = false;
                    togBtnCenter.IsChecked = true;
                    togBtnRight.IsChecked = false;
                    togBtnJustify.IsChecked = false;
                }
            }
        }

        /// <summary>
        /// Sets horizontal alignment to right.
        /// </summary>
        private void Right_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ComponentContent content)
            {
                if (content.TextHorizontal != "Right")
                {
                    content.TextHorizontal = "Right";
                    txtEditor.HorizontalContentAlignment = HorizontalAlignment.Right;

                    togBtnLeft.IsChecked = false;
                    togBtnCenter.IsChecked = false;
                    togBtnRight.IsChecked = true;
                    togBtnJustify.IsChecked = false;
                }
            }
        }

        /// <summary>
        /// Sets horizontal alignment to justify.
        /// </summary>
        private void Justify_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ComponentContent content)
            {
                if (content.TextHorizontal != "Justify")
                {
                    content.TextHorizontal = "Justify";
                    txtEditor.HorizontalContentAlignment = HorizontalAlignment.Stretch;

                    togBtnLeft.IsChecked = false;
                    togBtnCenter.IsChecked = false;
                    togBtnRight.IsChecked = false;
                    togBtnJustify.IsChecked = true;
                }
            }
        }

        /// <summary>
        /// Sets vertical alignment to top.
        /// </summary>
        private void Top_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ComponentContent content)
            {
                if (content.TextVertical != "Top")
                {
                    content.TextVertical = "Top";
                    txtEditor.VerticalContentAlignment = VerticalAlignment.Top;

                    togBtnTop.IsChecked = true;
                    togBtnMiddle.IsChecked = false;
                    togBtnBottom.IsChecked = false;
                }
            }
        }

        /// <summary>
        /// Sets vertical alignment to center.
        /// </summary>
        private void Middle_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ComponentContent content)
            {
                if (content.TextVertical != "Center")
                {
                    content.TextVertical = "Center";
                    txtEditor.VerticalContentAlignment = VerticalAlignment.Center;

                    togBtnTop.IsChecked = false;
                    togBtnMiddle.IsChecked = true;
                    togBtnBottom.IsChecked = false;
                }
            }
        }

        /// <summary>
        /// Sets vertical alignment to bottom.
        /// </summary>
        private void Bottom_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ComponentContent content)
            {
                if (content.TextVertical != "Bottom")
                {
                    content.TextVertical = "Bottom";
                    txtEditor.VerticalContentAlignment = VerticalAlignment.Bottom;

                    togBtnTop.IsChecked = false;
                    togBtnMiddle.IsChecked = false;
                    togBtnBottom.IsChecked = true;
                }
            }
        }

        /// <summary>
        /// Increases the font size of the text.
        /// </summary>
        private void IncreaseFont_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ComponentContent content)
            {
                txtEditor.FontSize = content.FontSize + 2;
                content.FontSize = content.FontSize + 2;
            }
        }

        /// <summary>
        /// Decreases the font size of the text.
        /// </summary>
        private void DecreaseFont_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ComponentContent content)
            {
                txtEditor.FontSize = Math.Max(content.FontSize - 2, 0);
                content.FontSize = Math.Max(content.FontSize - 2, 0);
            }
        }
    }
}
