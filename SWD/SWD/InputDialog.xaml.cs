﻿using System;
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
            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
            this.DataContext = App.themeData.CurrentTheme;
        }

        private void ThemeData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == nameof(ThemeData.CurrentTheme))
            this.DataContext = App.themeData.CurrentTheme;
        }

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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}
