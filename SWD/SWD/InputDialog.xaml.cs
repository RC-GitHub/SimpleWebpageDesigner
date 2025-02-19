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
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public string InputValue { get; private set; }

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
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue = InputTextBox.Text;
            if (InputValue == string.Empty)
            {
                Errors.DisplayErrorMessage("Component's name cannot be empty");
            }
            else
            {
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}
