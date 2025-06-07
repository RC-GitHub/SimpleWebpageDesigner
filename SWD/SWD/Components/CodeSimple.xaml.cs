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
    /// Interaction logic for CodeSimple.xaml
    /// </summary>
    public partial class CodeSimple : Page, INotifyPropertyChanged
    {
        private ComponentContent _componentContent;
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public CodeSimple(ComponentContent compcont)
        {
            InitializeComponent();
            ComponentContent = compcont;
            InitializeCodeEditors();
            this.DataContext = App.themeData.CurrentTheme;
            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
        }

        private void ThemeData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.DataContext = App.themeData.CurrentTheme;
        }

        private void InitializeCodeEditors()
        {
            HtmlEditor.Text = ComponentContent.CodeHTML;
            CssEditor.Text = ComponentContent.CodeCSS;
            JsEditor.Text = ComponentContent.CodeJS;
        }

        private void HtmlEditor_TextChanged(object sender, EventArgs e)
        {
            ComponentContent.CodeHTML = HtmlEditor.Text;
        }

        private void CSSEditor_TextChanged(object sender, EventArgs e)
        {
            ComponentContent.CodeCSS = CssEditor.Text;
        }

        private void JSEditor_TextChanged(object sender, EventArgs e)
        {
            ComponentContent.CodeJS = JsEditor.Text;
        }
    }
}
