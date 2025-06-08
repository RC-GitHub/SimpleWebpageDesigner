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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SWD.Components
{
    /// <summary>
    /// Interaction logic and data binding for the CodeSimple component editor.
    /// Allows editing of HTML, CSS, and JavaScript code for a component.
    /// </summary>
    public partial class CodeSimple : Page, INotifyPropertyChanged
    {
        private ComponentContent _componentContent;

        /// <summary>
        /// Gets or sets the component content (code properties) being edited.
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
        /// Initializes a new instance of the <see cref="CodeSimple"/> class.
        /// </summary>
        /// <param name="compcont">The component content to edit.</param>
        public CodeSimple(ComponentContent compcont)
        {
            InitializeComponent();
            ComponentContent = compcont;
            InitializeCodeEditors();
            this.DataContext = App.themeData.CurrentTheme;
            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
        }

        /// <summary>
        /// Handles theme changes and updates the DataContext.
        /// </summary>
        private void ThemeData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.DataContext = App.themeData.CurrentTheme;
        }

        /// <summary>
        /// Initializes the code editors with the current values from <see cref="ComponentContent"/>.
        /// </summary>
        private void InitializeCodeEditors()
        {
            HtmlEditor.Text = ComponentContent.CodeHTML;
            CssEditor.Text = ComponentContent.CodeCSS;
            JsEditor.Text = ComponentContent.CodeJS;
        }

        /// <summary>
        /// Handles changes in the HTML editor and updates <see cref="ComponentContent.CodeHTML"/>.
        /// </summary>
        private void HtmlEditor_TextChanged(object sender, EventArgs e)
        {
            ComponentContent.CodeHTML = HtmlEditor.Text;
        }

        /// <summary>
        /// Handles changes in the CSS editor and updates <see cref="ComponentContent.CodeCSS"/>.
        /// </summary>
        private void CSSEditor_TextChanged(object sender, EventArgs e)
        {
            ComponentContent.CodeCSS = CssEditor.Text;
        }

        /// <summary>
        /// Handles changes in the JavaScript editor and updates <see cref="ComponentContent.CodeJS"/>.
        /// </summary>
        private void JSEditor_TextChanged(object sender, EventArgs e)
        {
            ComponentContent.CodeJS = JsEditor.Text;
        }
    }
}
