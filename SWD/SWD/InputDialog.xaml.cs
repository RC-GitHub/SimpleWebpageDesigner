using System;
using System.Collections.Generic;
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

namespace SWD
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml.
    /// Provides a dialog window for user text input with validation and theming support.
    /// </summary>
    public partial class InputDialog : Window
    {
        /// <summary>
        /// Gets the value entered by the user.
        /// </summary>
        public string InputValue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputDialog"/> class.
        /// Sets up theming and drag-move support.
        /// </summary>
        public InputDialog()
        {
            InitializeComponent();
            this.MouseDown += (sender, e) =>
            {
                if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed && e.ChangedButton == System.Windows.Input.MouseButton.Left)
                {
                    this.DragMove();
                }
            };
            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
            this.DataContext = App.themeData.CurrentTheme;
        }

        /// <summary>
        /// Handles theme changes and updates the DataContext.
        /// </summary>
        private void ThemeData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == nameof(ThemeData.CurrentTheme))
            this.DataContext = App.themeData.CurrentTheme;
        }

        /// <summary>
        /// Handles the OK button click event, validates input, and closes the dialog if valid.
        /// </summary>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue = InputTextBox.Text;
            if (InputValue == string.Empty)
            {
                Errors.DisplayMessage("Name cannot be empty!");
            }
            else if (InputValue.Length > 26)
            {
                Errors.DisplayMessage("Name cannot be longer than 26 characters!");
            }
            else
            {
                DialogResult = true;
                Close();
            }
        }

        /// <summary>
        /// Handles the Cancel button click event and closes the dialog.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}
