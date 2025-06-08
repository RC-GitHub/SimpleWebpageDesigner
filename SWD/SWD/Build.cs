using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace SWD
{
    /// <summary>
    /// Provides static methods for building and exporting project files,
    /// including generating HTML and CSS from project JSON data.
    /// </summary>
    public class Build
    {
        /// <summary>
        /// Creates the build directory for the project, converts all JSON files to HTML,
        /// and writes the generated HTML and CSS files.
        /// </summary>
        /// <param name="projectPath">The root path of the project.</param>
        public static void CreateDirectories(string projectPath)
        {
            if (string.IsNullOrEmpty(projectPath))
            {
                Errors.DisplayMessage("Project path cannot be null or empty.");
            }

            string sourceDir = Path.Combine(projectPath, "json");
            string targetDir = Path.Combine(projectPath, "build");

            if (Directory.Exists(targetDir))
                Directory.Delete(targetDir, true);

            foreach (string filePath in Directory.GetFiles(sourceDir, "*.json", SearchOption.AllDirectories))
            {
                string relativePath = GetRelativePath(sourceDir, filePath);
                string targetFilePath = Path.Combine(targetDir, Path.ChangeExtension(relativePath, ".html"));
                string targetFileDir = Path.GetDirectoryName(targetFilePath);

                if (!Directory.Exists(targetFileDir))
                    Directory.CreateDirectory(targetFileDir);

                string jsonContent = File.ReadAllText(filePath);
                string htmlContent = GenerateHtmlFromJson(jsonContent, projectPath);

                File.WriteAllText(targetFilePath, htmlContent);
            }

            Infos.DisplayMessage("Build complete!");
        }

        /// <summary>
        /// Returns the relative path from a base directory to a full file path.
        /// </summary>
        /// <param name="basePath">The base directory path.</param>
        /// <param name="fullPath">The full file path.</param>
        /// <returns>The relative path from basePath to fullPath.</returns>
        private static string GetRelativePath(string basePath, string fullPath)
        {
            Uri baseUri = new Uri(basePath.EndsWith(Path.DirectorySeparatorChar.ToString()) ? basePath : basePath + Path.DirectorySeparatorChar);
            Uri fullUri = new Uri(fullPath);
            return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fullUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        /// <summary>
        /// Generates HTML content from a JSON string and project path.
        /// Handles CSS generation, component rendering, and layout.
        /// </summary>
        /// <param name="json">The JSON string representing the page data.</param>
        /// <param name="projectPath">The root path of the project.</param>
        /// <returns>The generated HTML as a string.</returns>
        private static string GenerateHtmlFromJson(string json, string projectPath)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new SolidColorBrushConverter() }
            };

            var metadataPath = Path.Combine(projectPath, "metadata.json");
            var metadataJson = File.ReadAllText(metadataPath);
            var metadata = JsonSerializer.Deserialize<List<Head>>(metadataJson, options)?[0];

            var data = JsonSerializer.Deserialize<List<PageData>>(json, options);
            if (data == null || data.Count == 0)
                return "<html><body><p>Invalid or empty layout.</p></body></html>";

            var layout = data[0];
            var html = new StringBuilder();

            // Compose color variables
            string headerColor = ToCssColor(metadata?.Layout?.HeaderColor);
            string bodyColor = ToCssColor(metadata?.Layout?.BodyColor);
            string gridColor = ToCssColor(metadata?.Layout?.GridColor);
            string footerColor = ToCssColor(metadata?.Layout?.FooterColor);

            string backgroundColor = "transparent";
            string borderColor = "transparent";

            if (!string.IsNullOrWhiteSpace(metadata.Layout.HeaderLinkStyle) && metadata.Layout.HeaderLinkStyle.Contains("-"))
            {
                var parts = metadata.Layout.HeaderLinkStyle.Split('-');
                string bgKey = parts[0].Trim().ToLower();

                if (bgKey == "body") backgroundColor = bodyColor;
                else if (bgKey == "grid") backgroundColor = gridColor;
                else if (bgKey == "footer") backgroundColor = footerColor;

                string borderKey = parts[1].Trim().ToLower();
                if (borderKey == "body") borderColor = bodyColor;
                else if (borderKey == "grid") borderColor = gridColor;
                else if (borderKey == "footer") borderColor = footerColor;
            }

            // Ensure build folder and main style.css
            string buildFolder = Path.Combine(projectPath, "build");
            Directory.CreateDirectory(buildFolder);

            string styleCssPath = Path.Combine(buildFolder, "style.css");

            // Write main static CSS once if not exists
            if (!File.Exists(styleCssPath))
            {
                var mainCss = $@"
    * {{
        margin: 0;
        padding: 0;
        box-sizing: border-box;
    }}
    body {{
        display: flex;
        align-items: {MapJustifyOrAlign(metadata?.Layout?.GridHAlign)};
        flex-direction: column;
        justify-content: space-between;
        background-color: {bodyColor};
        height: 100vh;
        width: {metadata?.Layout?.BodyWidth}{metadata?.Layout?.BodyWidthUnit};
        margin: 0 auto;
    }}
    header {{
        width: 100vw;
        height: 100px;
        background-color: {headerColor};
        padding: {metadata?.Layout?.HeaderPadding}{metadata?.Layout?.HeaderPaddingUnit};
        display: flex;
        justify-content: space-between;
        align-items: center;
    }}
    .headerlink {{
        background-color: {backgroundColor};
        border: 2px solid {borderColor};
        color: {borderColor};
        border-radius: 5px; 
        padding: 5px;  
    }}       
    footer {{
        width: 100vw;
        height: 300px;
        display: flex;
        justify-content: center;
        align-items: center;
        background-color: {footerColor};
        padding: {metadata?.Layout?.FooterPadding}{metadata?.Layout?.FooterPaddingUnit};
        text-align: center;
    }}
    .grid-container {{
        display: grid;
        width: {metadata?.Layout?.GridWidth}{metadata?.Layout?.GridWidthUnit};
        margin: {metadata?.Layout?.GridMargin}{metadata?.Layout?.GridMarginUnit};
        gap: {metadata?.Layout?.GridGap}{metadata?.Layout?.GridGapUnit};
        padding: {metadata?.Layout?.GridPadding}{metadata?.Layout?.GridPaddingUnit};
        background-color: {gridColor};
        border-radius: {metadata?.Layout?.GridBorderRadius}{metadata?.Layout?.GridBorderRadiusUnit};
    }}
    .grid-item {{
        box-sizing: border-box;
        overflow: hidden;
    }}
    .button {{
        text-decoration: none;
        transition: 0.35s ease-in-out;
    }}
    .button:hover {{
        transform: scale(1.05);
        transition: 0.4s ease-in-out;
    }}

    @media (max-width: 600px) {{
        .grid-container {{
            width: 100vw !important;
            grid-template-columns: 1fr !important;
            grid-template-rows: none !important;
        }}
        .grid-container > * {{
            grid-column: auto !important;
            grid-row: auto !important;
        }}
      }}
    }}
    {metadata?.CodeCSS}
    ";
                File.WriteAllText(styleCssPath, mainCss);
            }

            // Build dynamic styles string for this page/components inside <style>
            var dynamicStyles = new StringBuilder();
            int index = 0;
            try
            {
                foreach (var componentEntry in layout.Components)
                {
                    if (componentEntry.Value == null) continue;

                    var comp = componentEntry.Value;
                    var className = $"item-{index++}";
                    var styles = new List<string>();

                    styles.Add($"grid-column: {comp.StartColumn + 1} / span {Math.Max(1, comp.Colspan + 1)};");
                    styles.Add($"grid-row: {comp.StartRow + 1} / span {Math.Max(1, comp.Rowspan + 1)};");

                    var gradient = comp.CompStyle;
                    if (gradient.GradientStart != null && gradient.GradientEnd != null)
                    {
                        styles.Add($"background: linear-gradient(to bottom, {ToCssColor(gradient.GradientStart)} {gradient.GradientStartPercent}%, {ToCssColor(gradient.GradientEnd)} {gradient.GradientEndPercent}%);");
                    }
                    else
                    {
                        styles.Add($"background-color: {ToCssColor(comp.BackgroundColor)};");
                    }

                    if (comp.CompStyle.UseBackgroundImage)
                    {
                        var imagePath = Path.Combine(projectPath, "images", comp.CompStyle.BackgroundImage);
                        var imageUri = new Uri(imagePath).AbsoluteUri;
                        styles.Add($"background-image: url(\"{imageUri}\");");
                        styles.Add($"background-position:  {comp.CompStyle.BackgroundImageAlignment?.ToLower() ?? "center"};");

                        var stretch = comp.CompStyle.BackgroundImageStretch?.ToLower();
                        switch (stretch)
                        {
                            case "fill":
                                styles.Add("background-size: 100% 100%;");
                                styles.Add("background-repeat: no-repeat;");
                                break;
                            case "uniform":
                                styles.Add("background-size: contain;");
                                styles.Add("background-repeat: no-repeat;");
                                break;
                            case "uniformtofill":
                                styles.Add("background-size: cover;");
                                styles.Add("background-repeat: no-repeat;");
                                break;
                            case "none":
                                styles.Add("background-size: auto;");
                                break;
                            default:
                                styles.Add("background-size: contain;");
                                styles.Add("background-repeat: no-repeat;");
                                break;
                        }
                    }

                    float compWidth = comp.CompStyle.Width;
                    float compMinWidth = comp.CompStyle.MinWidth;
                    float compMaxWidth = comp.CompStyle.MaxWidth;
                    float compHeight = comp.CompStyle.Height;
                    float compMinHeight = comp.CompStyle.MinHeight;
                    float compMaxHeight = comp.CompStyle.MaxHeight;

                    if (compWidth > 0)
                        styles.Add($"width: {compWidth}{comp.CompStyle.WidthUnit};");

                    if (compMinWidth > 0)
                        styles.Add($"min-width: {compMinWidth}{comp.CompStyle.MinWidthUnit};");

                    if (compMaxWidth > 0)
                        styles.Add($"max-width: {compMaxWidth}{comp.CompStyle.MaxWidthUnit};");

                    if (compHeight > 0)
                        styles.Add($"height: {compHeight}{comp.CompStyle.HeightUnit};");

                    if (compMinHeight > 0)
                        styles.Add($"min-height: {compMinHeight}{comp.CompStyle.MinHeightUnit};");

                    if (compMaxHeight > 0)
                        styles.Add($"max-height: {compMaxHeight}{comp.CompStyle.MaxHeightUnit};");

                    styles.Add($"border: {comp.CompStyle.BorderThickness}{comp.CompStyle.BorderThicknessUnit} solid {ToCssColor(comp.CompStyle.BorderColor)};");
                    styles.Add($"border-radius: {comp.CompStyle.BorderRadius}{comp.CompStyle.BorderRadiusUnit};");
                    styles.Add($"padding: {comp.CompStyle.PaddingTop}{comp.CompStyle.PaddingUnit} {comp.CompStyle.PaddingRight}{comp.CompStyle.PaddingUnit} {comp.CompStyle.PaddingBottom}{comp.CompStyle.PaddingUnit} {comp.CompStyle.PaddingLeft}{comp.CompStyle.PaddingUnit};");
                    styles.Add($"margin: {comp.CompStyle.MarginTop}{comp.CompStyle.MarginUnit} {comp.CompStyle.MarginRight}{comp.CompStyle.MarginUnit} {comp.CompStyle.MarginBottom}{comp.CompStyle.MarginUnit} {comp.CompStyle.MarginLeft}{comp.CompStyle.MarginUnit};");
                    styles.Add($"opacity: {comp.CompStyle.Opacity.ToString(CultureInfo.InvariantCulture)};");
                    styles.Add($"justify-content: {comp.CompStyle.Justify.ToLower()};");
                    styles.Add($"align-items: {comp.CompStyle.AlignItems.ToLower()};");
                    styles.Add($"box-shadow: {comp.CompStyle.BoxShadow};");
                    styles.Add($"overflow: {comp.CompStyle.Overflow.ToLower()};");
                    if (comp.CompStyle.ZIndex != "Auto")
                    {
                        styles.Add($"z-index: {comp.CompStyle.ZIndex};");
                    }
                    styles.Add($"display: flex;");

                    dynamicStyles.AppendLine($".{className} {{ {string.Join(" ", styles)} }}");
                }
            }
            catch (Exception ex) { Errors.DisplayMessage(ex.Message); }

            // Build the HTML
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html><head>");
            html.AppendLine("<meta charset=\"UTF-8\">");
            html.AppendLine($"<title>{metadata?.ProjectName ?? "Generated Page"}</title>");
            html.AppendLine($"<meta name=\"author\" content=\"{metadata?.Author}\">");
            html.AppendLine($"<meta name=\"description\" content=\"{metadata?.Description}\">");
            html.AppendLine($"<meta name=\"keywords\" content=\"{string.Join(", ", metadata?.Keywords ?? Array.Empty<string>())}\">");

            // Link main external CSS
            html.AppendLine("<link rel=\"stylesheet\" href=\"style.css\">");

            // Write metadata.CodeJS to script.js
            if (!string.IsNullOrWhiteSpace(metadata?.CodeJS))
            {
                var scriptPath = Path.Combine(buildFolder, "script.js");
                File.WriteAllText(scriptPath, metadata.CodeJS);
            }
            html.AppendLine("<script src=\"script.js\" defer></script>");

            // Inject dynamic styles inside <style> tag
            html.AppendLine("<style>");
            html.AppendLine(dynamicStyles.ToString());
            html.AppendLine("</style>");

            html.AppendLine("</head><body>");

            // HEADER
            html.AppendLine("<header>");
            if (!string.IsNullOrEmpty(metadata?.Layout?.HeaderLogo))
            {
                var logoPath = Path.Combine(projectPath, "images", metadata.Layout.HeaderLogo);
                var logoUri = new Uri(logoPath).AbsoluteUri;
                html.AppendLine($"<img src=\"{logoUri}\" alt=\"Logo\" style=\"height:40px;\" />");
            }
            if (metadata?.Layout?.HeaderLinks != null)
            {
                html.AppendLine("<nav>");
                foreach (var link in metadata.Layout.HeaderLinks)
                {
                    html.AppendLine($"<a class=\"headerlink\" href=\"{link.Value}\" style=\"margin-left:15px;\">{link.Key}</a>");
                }
                html.AppendLine("</nav>");
            }
            html.AppendLine("</header>");

            if (!string.IsNullOrEmpty(metadata?.CodeHTML))
                html.AppendLine(metadata.CodeHTML);

            // Grid container with dynamic columns and rows
            html.AppendLine($"<div class=\"grid-container\" style=\"grid-template-columns: repeat({layout.ColAmount}, minmax(100px, 1fr)); grid-template-rows: repeat({layout.RowAmount}, auto);\">");

            // Generate components HTML
            int componentIndex = 0;
            try
            {
                foreach (var componentEntry in layout.Components)
                {
                    var comp = componentEntry.Value;
                    var className = $"item-{componentIndex++}";
                    string innerHtml = "";

                    if (comp.Type == "text")
                    {
                        var content = comp.Content;
                        innerHtml = $"<p style=\"margin: 0; font-family: {content.FontFamily}; font-size: {content.FontSize}px; font-weight: {content.FontWeight.ToLower()}; font-style: {content.FontStyle.ToLower()}; color: {ToCssColor(content.ForegroundColor)}; text-align: {content.TextHorizontal.ToLower()};\">" +
                                    $"{System.Net.WebUtility.HtmlEncode(content.Text)}</p>";
                    }
                    else if (comp.Type == "image")
                    {
                        var content = comp.Content;
                        string width = content.ImageWidth > 0 ? $"width: {content.ImageWidth}px;" : "";
                        string height = content.ImageHeight > 0 ? $"height: {content.ImageHeight}px;" : "";

                        var imagePath = Path.Combine(projectPath, "images", content.ImageName);
                        var imageUri = new Uri(imagePath).AbsoluteUri;

                        innerHtml = $"<img src=\"{imageUri}\" " +
                                    $"style=\"{width} {height} object-fit: {GetCssImageStretch(content.ImageStretch)}; object-position: {content.ImageHAlign.ToLower()} {content.ImageVAlign.ToLower()};\" />";
                    }
                    else if (comp.Type == "code")
                    {
                        var content = comp.Content;
                        innerHtml = $"{content.CodeHTML}\r\n{content.CodeCSS}\r\n{content.CodeJS}";
                    }
                    else if (comp.Type == "button")
                    {
                        var content = comp.Content;

                        string width = content.ButtonWidth > 0 ? $"width: {content.ButtonWidth}{content.ButtonWidthUnit};" : "";
                        string height = content.ButtonHeight > 0 ? $"height: {content.ButtonHeight}{content.ButtonHeightUnit};" : "";

                        string bgStyle = $"background-color: {ToCssColor(content.ButtonBackgroundColor)};";

                        string border = $"border: {content.ButtonBorder}{content.ButtonBorderUnit} solid {ToCssColor(content.ButtonBorderColor)};";
                        string borderRadius = $"border-radius: {content.ButtonBorderRadius}{content.ButtonBorderRadiusUnit};";
                        string padding = $"padding: {content.ButtonPadding}{content.ButtonPaddingUnit};";
                        string margin = $"margin: {content.ButtonMargin}{content.ButtonMarginUnit};";
                        string font = $"font-size: {content.ButtonFontSize}{content.ButtonFontSizeUnit}; color: {ToCssColor(content.ButtonFontColor)};";

                        string style = $"{width} {height} {bgStyle} {border} {borderRadius} {padding} {margin} {font}";
                        string onclick = "";
                        if (content.ButtonFunc != "") onclick = $" onclick=\"{content.ButtonFunc}()\"";

                        string linkStart = string.IsNullOrEmpty(content.ButtonLink) ? "" : $"<a href=\"{content.ButtonLink}\" style=\"text-decoration: none; {width} {height}\">";
                        string linkEnd = string.IsNullOrEmpty(content.ButtonLink) ? "" : "</a>";

                        innerHtml = $"{linkStart}<button type=\"button\" class=\"button\" style=\"{style}\"{onclick}>{System.Net.WebUtility.HtmlEncode(content.ButtonText)}</button>{linkEnd}";
                    }

                    html.AppendLine($"<div class=\"grid-item {className}\">{innerHtml}</div>");
                }
            }
            catch (Exception ex) { Errors.DisplayMessage(ex.Message); }

            html.AppendLine("</div>");

            html.AppendLine($"<footer>{System.Net.WebUtility.HtmlEncode(metadata?.Layout?.FooterContent ?? "")}</footer>");
            html.AppendLine("</body></html>");

            return html.ToString();
        }

        /// <summary>
        /// Maps a WPF image stretch value to a CSS object-fit value.
        /// </summary>
        /// <param name="stretch">The WPF stretch value as a string.</param>
        /// <returns>The corresponding CSS object-fit value.</returns>
        private static string GetCssImageStretch(string stretch)
        {
            switch (stretch)
            {
                case "Uniform":
                    return "contain";
                case "Fill":
                    return "fill";
                case "None":
                    return "none";
                case "UniformToFill":
                    return "cover";
                default:
                    return "contain";
            }
        }

        /// <summary>
        /// Converts a SolidColorBrush to a CSS color string.
        /// </summary>
        /// <param name="brush">The SolidColorBrush to convert.</param>
        /// <returns>A CSS color string.</returns>
        private static string ToCssColor(SolidColorBrush brush)
        {
            if (brush == null) return "transparent";
            var c = brush.Color;
            if (c.A == 0) return "transparent";

            double alpha = c.A / 255.0;
            string alphaStr = alpha.ToString(CultureInfo.InvariantCulture);

            if (alpha == 1)
                return $"#{c.R:X2}{c.G:X2}{c.B:X2}";
            else
                return $"rgba({c.R}, {c.G}, {c.B}, {alphaStr})";
        }

        /// <summary>
        /// Maps a justification or alignment string to a CSS flexbox value.
        /// </summary>
        /// <param name="val">The alignment value.</param>
        /// <returns>The corresponding CSS value.</returns>
        public static string MapJustifyOrAlign(string val)
        {
            val = val.ToLower();
            if (val == "center") return "center";
            if (val == "left" || val == "start") return "flex-start";
            if (val == "right" || val == "end") return "flex-end";
            if (val == "stretch") return "stretch";
            return "center";
        }

        /// <summary>
        /// JSON converter for SolidColorBrush, used during deserialization of project data.
        /// </summary>
        public class SolidColorBrushConverter : JsonConverter<SolidColorBrush>
        {
            /// <summary>
            /// Reads a SolidColorBrush from a JSON string.
            /// </summary>
            public override SolidColorBrush Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var colorString = reader.GetString();
                if (colorString == null)
                    return null;
                var color = (Color)ColorConverter.ConvertFromString(colorString);
                return new SolidColorBrush(color);
            }

            /// <summary>
            /// Writes a SolidColorBrush as a JSON string.
            /// </summary>
            public override void Write(Utf8JsonWriter writer, SolidColorBrush value, JsonSerializerOptions options)
            {
                var color = value.Color;
                var colorString = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
                writer.WriteStringValue(colorString);
            }
        }
    }
}
