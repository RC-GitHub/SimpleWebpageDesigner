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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

//using System.Text.Json;
//using System.Text.Json.Serialization;

using Newtonsoft.Json;
using System.IO;
using Path = System.IO.Path;
using System.Drawing;
using System.Xml.Linq;
using Brushes = System.Windows.Media.Brushes;
using Rectangle = System.Windows.Shapes.Rectangle;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using TextBox = System.Windows.Controls.TextBox;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using System.Runtime.CompilerServices;

namespace SWD
{
    /// <summary>
    /// Interaction logic for CreationWindow.xaml.
    /// Handles project creation, editing, and metadata management for SWD projects.
    /// </summary>
    public partial class CreationWindow : Window
    {
 // Ensure this namespace is included at the top of the file
        private bool editmode = false;
        private MainWindow _mainWindow;
        private ContentWindow _contentWindow;
        public bool projectsFolder = true;
        public string dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private string tempForProjectTitle = "Insert project title";
        private string tempForAuthor = "Insert author's name";
        private string tempForKeyword = "Insert a keyword";
        private string tempForDescription = "Insert description";
        public Dictionary<string, string> tempMessages = new Dictionary<string, string>();
        public BaseLayout baseLayout { get; set; } = new BaseLayout();

        /// <summary>
        /// Initializes a new CreationWindow for a new project.
        /// </summary>
        /// <param name="mw">Reference to the MainWindow (optional).</param>
        public CreationWindow(MainWindow mw = null)
        {
            _mainWindow = mw;

            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
            this.DataContext = App.themeData.CurrentTheme;

            InitializeComponent();
            InitializeEmptyText();
            InitializeMessagesArray();
        }

        /// <summary>
        /// Initializes a new CreationWindow for a new project in a specific directory.
        /// </summary>
        /// <param name="d">Project directory.</param>
        /// <param name="mw">Reference to the MainWindow.</param>
        public CreationWindow(string d, MainWindow mw)
        {
            dir = d;
            _mainWindow = mw;
            projectsFolder = false;

            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
            this.DataContext = App.themeData.CurrentTheme;

            InitializeComponent();
            InitializeEmptyText();
            InitializeMessagesArray();
        }

        string projectName;
        /// <summary>
        /// Initializes a new CreationWindow for editing an existing project.
        /// </summary>
        /// <param name="d">Project directory.</param>
        /// <param name="head">Project metadata.</param>
        /// <param name="cw">Reference to the ContentWindow.</param>
        public CreationWindow(string d, Head head, ContentWindow cw)
        {
            dir = d;
            _contentWindow = cw;
            editmode = true;

            App.themeData.PropertyChanged += ThemeData_PropertyChanged;
            this.DataContext = App.themeData.CurrentTheme;
            InitializeComponent();
            InitializeMessagesArray();

            projectName = head.ProjectName;
            tbProjectTitle.Text = head.ProjectName;
            tbAuthor.Text = head.Author;
            tbDescription.Text = head.Description;
            foreach (string keyword in head.Keywords)
            {
                lsbKeywords.Items.Add(keyword);
            }
            baseLayout = head.Layout;
            foreach (string link in baseLayout.HeaderLinks.Values)
            {
                lsbLinks.Items.Add(link);
            }
            HtmlEditor.Text = head.CodeHTML;
            CssEditor.Text = head.CodeCSS;
            JsEditor.Text = head.CodeJS;
        }

        /// <summary>
        /// Handles theme changes and updates the DataContext.
        /// </summary>
        private void ThemeData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ThemeData.CurrentTheme))
                this.DataContext = App.themeData.CurrentTheme;
        }

        /// <summary>
        /// Initializes the temporary messages array for placeholder text.
        /// </summary>
        public void InitializeMessagesArray()
        {
            tempMessages.Add("tbProjectTitle", tempForProjectTitle);
            tempMessages.Add("tbAuthor", tempForAuthor);
            tempMessages.Add("tbKeyword", tempForKeyword);
            tempMessages.Add("tbDescription", tempForDescription);

            foreach (KeyValuePair<string, string> kvp in tempMessages)
            {
                Debug.WriteLine(kvp.Value);
            }
        }

        /// <summary>
        /// Initializes empty text fields with placeholder values and gray color.
        /// </summary>
        public void InitializeEmptyText()
        {
            if (string.IsNullOrWhiteSpace(tbProjectTitle.Text))
            {
                tbProjectTitle.Text = tempForProjectTitle;
                tbProjectTitle.Foreground = Brushes.Gray;
            }
            if (string.IsNullOrWhiteSpace(tbAuthor.Text))
            {
                tbAuthor.Text = tempForAuthor;
                tbAuthor.Foreground = Brushes.Gray;
            }
            if (string.IsNullOrWhiteSpace(tbKeyword.Text))
            {
                tbKeyword.Text = tempForKeyword;
                tbKeyword.Foreground = Brushes.Gray;
            }
            if (string.IsNullOrWhiteSpace(tbDescription.Text))
            {
                tbDescription.Text = tempForDescription;
                tbDescription.Foreground = Brushes.Gray;
            }
        }

        /// <summary>
        /// Adds a keyword to the keyword list.
        /// </summary>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbKeyword.Text != tempForKeyword)
            {
                lsbKeywords.Items.Add(tbKeyword.Text);
                tbKeyword.Text = "";
                tbKeyword.Focus();
            }
        }

        /// <summary>
        /// Removes the selected keyword from the keyword list.
        /// </summary>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lsbKeywords.SelectedItem != null)
            {
                int index = lsbKeywords.SelectedIndex;
                lsbKeywords.Items.RemoveAt(index);
                lsbKeywords.Items.Refresh();
                if (lsbKeywords.Items.Count - 1 >= index)
                    lsbKeywords.SelectedIndex = index;
                else
                    lsbKeywords.SelectedIndex = lsbKeywords.Items.Count - 1;
            }
        }

        /// <summary>
        /// Handles the submit button click, calling NormalSubmit or EditSubmit as appropriate.
        /// </summary>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!editmode)
                NormalSubmit();
            else
                EditSubmit();
        }

        /// <summary>
        /// Handles project creation and saving for new projects.
        /// </summary>
        private void NormalSubmit()
        {
            if (tbProjectTitle.Text == tempMessages[tbProjectTitle.Name] || tbProjectTitle.Text == string.Empty) Errors.DisplayMessage("Project title is required!");
            else
            {
                if (projectsFolder == true) dir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Projects"
                );
                dir = System.IO.Path.Combine(dir, $"SWD-{tbProjectTitle.Text}");
                Debug.WriteLine(dir);

                if (Directory.Exists(dir))
                {
                    Errors.DisplayMessage("A project with the same name already exists!");
                    dir = "";
                }
                else
                {
                    string author;
                    string description;
                    if (tbAuthor.Text == tempMessages[tbAuthor.Name]) author = "";
                    else author = tbAuthor.Text;
                    if (tbDescription.Text == tempMessages[tbDescription.Name]) description = "";
                    else description = tbDescription.Text;

                    List<Head> _data = new List<Head>
                        {
                            new Head()
                            {
                                ProjectName = tbProjectTitle.Text,
                                Author = author,
                                Keywords = lsbKeywords.Items.OfType<string>().ToArray(),
                                Description = description,
                                Layout = baseLayout,
                                CodeHTML = HtmlEditor.Text,
                                CodeCSS = CssEditor.Text,
                                CodeJS = JsEditor.Text
                            }
                        };

                    string json = JsonConvert.SerializeObject(_data, Formatting.Indented);

                    try
                    {
                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                        Debug.WriteLine(json);
                        File.WriteAllText($"{dir}\\metadata.json", json);

                        string imagesFolder = Path.Combine(dir, "images");
                        if (!Directory.Exists(imagesFolder)) Directory.CreateDirectory(imagesFolder);
                        File.WriteAllBytes($"{imagesFolder}\\{baseLayout.HeaderLogo}", headerLogoImageBytes);
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("The process failed: {0}", err.ToString());
                    }
                    finally { }

                    ContentWindow gd = new ContentWindow(dir, this);
                    System.Windows.Application.Current.MainWindow = gd;
                    gd.Show();

                    if (_mainWindow != null) _mainWindow.Hide();
                }
            }
        }

        /// <summary>
        /// Handles project editing and saving for existing projects.
        /// </summary>
        private void EditSubmit()
        {
            try
            {
                if (tbProjectTitle.Text == tempMessages[tbProjectTitle.Name] || tbProjectTitle.Text == string.Empty)
                {
                    Errors.DisplayMessage("Project title is required!");
                    return;
                }

                string author;
                string description;
                if (tbAuthor.Text == tempMessages[tbAuthor.Name]) author = "";
                else author = tbAuthor.Text;
                if (tbDescription.Text == tempMessages[tbDescription.Name]) description = "";
                else description = tbDescription.Text;

                List<Head> _data = new List<Head>
                {
                    new Head()
                    {
                        ProjectName = tbProjectTitle.Text,
                        Author = author,
                        Keywords = lsbKeywords.Items.OfType<string>().ToArray(),
                        Description = description,
                        Layout = baseLayout,
                        CodeHTML = HtmlEditor.Text,
                        CodeCSS = CssEditor.Text,
                        CodeJS = JsEditor.Text
                    }
                };

                string json = JsonConvert.SerializeObject(_data, Formatting.Indented);
                string metadataPath = System.IO.Path.Combine(dir, "metadata.json");
                File.WriteAllText(metadataPath, json);

                if (headerLogoImageBytes != null)
                {
                    string imagesFolder = Path.Combine(dir, "images");
                    if (!Directory.Exists(imagesFolder)) Directory.CreateDirectory(imagesFolder);
                    File.WriteAllBytes($"{imagesFolder}\\{baseLayout.HeaderLogo}", headerLogoImageBytes);
                }

                if (projectName != tbProjectTitle.Text)
                {
                    string projectsDir = Path.GetDirectoryName(dir);
                    string newNameDir = Path.Combine(projectsDir, $"SWD-{tbProjectTitle.Text}");
                    _contentWindow.Close();
                    ContentWindow.DirectoryCopy(dir, newNameDir, true);
                    ContentWindow gd = new ContentWindow(newNameDir, this);
                    gd.Show();
                    Directory.Delete(dir, true);
                }
                Close();
            }
            catch (Exception ex) { Errors.DisplayMessage(ex.Message); }
        }

        /// <summary>
        /// Handles the GotFocus event for textboxes, clearing placeholder text.
        /// </summary>
        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tempMessages.ContainsValue(tb.Text)) tb.Text = "";
            tb.Foreground = Brushes.Black;
        }

        /// <summary>
        /// Handles the LostFocus event for textboxes, restoring placeholder text if empty.
        /// </summary>
        private void tb_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == string.Empty)
            {
                tb.Text = tempMessages[tb.Name];
                tb.Foreground = Brushes.Gray;
            }
        }

        /// <summary>
        /// Handles the PreviewKeyDown event for the keyword textbox, adding a keyword on Enter.
        /// </summary>
        private void tbKeyword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrEmpty(tbKeyword.Text))
                {
                    btnAdd_Click(sender, e);
                }
            }
        }

        private void btnHeaderEdit_Click(object sender, RoutedEventArgs e) => popupHeader.IsOpen = true;
        private void btnBodyEdit_Click(object sender, RoutedEventArgs e) => popupBody.IsOpen = true;
        private void btnGridEdit_Click(object sender, RoutedEventArgs e) => popupGrid.IsOpen = true;
        private void btnFooterEdit_Click(object sender, RoutedEventArgs e) => popupFooter.IsOpen = true;
        private void btnCode_Click(object sender, RoutedEventArgs e) => popupCode.IsOpen = true;

        /// <summary>
        /// Handles color selection for layout properties using a color picker.
        /// </summary>
        private void SetColor(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            if (border == null) return;

            SolidColorBrush brush = Colors.BrushColorPicker();
            if (brush == null) return;

            // Get property name, e.g. "PopupFooter" -> "FooterColor"
            string namebase = border.Name.Substring(3); // e.g., "PopupFooter"
            string propertyName = namebase.Replace("Popup", "") + "Color"; // e.g., "FooterColor"

            var window = Window.GetWindow(this);

            baseLayout[propertyName] = brush;

            Debug.WriteLine($"{propertyName}, {baseLayout[propertyName]}, {brush.ToString()}");

            Rectangle rectangle = border.Child as Rectangle;

            BindingExpression binding = rectangle.GetBindingExpression(Rectangle.FillProperty);
            binding?.UpdateSource();

            // Reopen the popup if needed
            string popupName = char.ToLower(namebase[0]) + namebase.Substring(1);
            Popup popup = window.FindName(popupName) as Popup;
            if (popup != null)
            {
                popup.IsOpen = true;
            }
            else
            {
                Debug.WriteLine($"Popup '{popupName}' not found.");
            }
        }

        private byte[] headerLogoImageBytes;
        /// <summary>
        /// Handles the click event to select and set the header logo image.
        /// </summary>
        private void btnHeaderLogo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp, *.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Select an image",
            };

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = openFileDialog.FileName;
                string fileName = Path.GetFileName(selectedPath);

                // Read image content into byte array
                headerLogoImageBytes = File.ReadAllBytes(selectedPath);

                // Optionally store filename for UI
                tbHeaderLogo.Text = fileName;

                // Update binding source
                BindingExpression binding = tbHeaderLogo.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty);
                binding?.UpdateSource();
            }
        }

        /// <summary>
        /// Adds a new header link to the layout and updates the UI.
        /// </summary>
        private void btnLinkAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbLinkTitle.Text != "" && tbLinkHref.Text != "")
            {
                baseLayout.HeaderLinks.Add(tbLinkTitle.Text, tbLinkHref.Text);

                lsbLinks.Items.Add(tbLinkTitle.Text);
                tbLinkTitle.Text = "";
                tbLinkHref.Text = "";
                tbLinkTitle.Focus();
            }
            else
            {
                Errors.DisplayMessage("Link title and link href cannot be empty!");
            }
        }

        /// <summary>
        /// Removes the selected header link from the layout and UI.
        /// </summary>
        private void btnLinkRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lsbLinks.SelectedItem != null)
            {
                string key = lsbLinks.SelectedItem.ToString();
                baseLayout.HeaderLinks.Remove(key);

                int index = lsbLinks.SelectedIndex;
                lsbLinks.Items.RemoveAt(index);
                lsbLinks.Items.Refresh();
                if (lsbLinks.Items.Count - 1 >= index)
                    lsbLinks.SelectedIndex = index;
                else
                    lsbLinks.SelectedIndex = lsbLinks.Items.Count - 1;
            }
        }

        string currentKey = "";
        /// <summary>
        /// Loads the selected header link into the edit fields for editing.
        /// </summary>
        private void btnLinkEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lsbLinks.SelectedItem != null)
            {
                currentKey = lsbLinks.SelectedItem.ToString();
                tbLinkTitle.Text = currentKey;
                tbLinkHref.Text = baseLayout.HeaderLinks[currentKey];

                tbLinkTitle.Focus();
                btnLinkAccept.IsEnabled = true;
            }
        }

        /// <summary>
        /// Accepts and applies changes to a header link after editing.
        /// </summary>
        private void btnLinkAccept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string newTitle = tbLinkTitle.Text.Trim();
                string newHref = tbLinkHref.Text.Trim();

                if (string.IsNullOrEmpty(newTitle) || string.IsNullOrEmpty(newHref))
                {
                    Errors.DisplayMessage("Link title and link href cannot be empty!");
                    return;
                }

                if (baseLayout.HeaderLinks.ContainsKey(newTitle) && newTitle != currentKey)
                {
                    Errors.DisplayMessage("This title already exists.");
                    return;
                }

                if (newTitle == currentKey)
                {
                    baseLayout.HeaderLinks[currentKey] = newHref;
                }
                else
                {
                    baseLayout.HeaderLinks.Add(newTitle, newHref);
                    if (baseLayout.HeaderLinks.ContainsKey(currentKey))
                        baseLayout.HeaderLinks.Remove(currentKey);

                    lsbLinks.Items.Add(newTitle);
                    lsbLinks.Items.Remove(currentKey);
                }

                tbLinkTitle.Text = "";
                tbLinkHref.Text = "";
                tbLinkTitle.Focus();
                btnLinkAccept.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Errors.DisplayMessage(ex.Message);
            }
            finally
            {
                btnLinkAccept.IsEnabled = false;
            }
        }
    }
}
