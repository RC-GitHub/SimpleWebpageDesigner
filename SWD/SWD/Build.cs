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
    public class Build
    {
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

        private static string GetRelativePath(string basePath, string fullPath)
        {
            Uri baseUri = new Uri(basePath.EndsWith(Path.DirectorySeparatorChar.ToString()) ? basePath : basePath + Path.DirectorySeparatorChar);
            Uri fullUri = new Uri(fullPath);
            return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fullUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

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
            var css = new StringBuilder();

            html.AppendLine("<html><head>");
            html.AppendLine("<meta charset=\"UTF-8\">");
            html.AppendLine($"<title>{metadata?.ProjectName ?? "Generated Page"}</title>");
            html.AppendLine($"<meta name=\"author\" content=\"{metadata?.Author}\">");
            html.AppendLine($"<meta name=\"description\" content=\"{metadata?.Description}\">");
            html.AppendLine($"<meta name=\"keywords\" content=\"{string.Join(", ", metadata?.Keywords ?? Array.Empty<string>())}\">");
            // Start <style>
            html.AppendLine("<style>");
            html.AppendLine(@"
                * {
                    margin: 0;
                    padding: 0;
                    box-sizing: border-box;
                }
                body {
                    display: flex;
                    align-items: " + MapJustifyOrAlign(metadata?.Layout?.GridHAlign) + @";
                    flex-direction: column;
                    background-color: " + ToCssColor(metadata?.Layout?.BodyColor) + @";
                    width: " + metadata?.Layout?.BodyWidth + metadata?.Layout?.BodyWidthUnit + @";
                    margin: 0 auto;
                }
                header {
                    width: 100vw;
                    height: 100px;
                    background-color: " + ToCssColor(metadata?.Layout?.HeaderColor) + @";
                    padding: " + metadata?.Layout?.HeaderPadding + metadata?.Layout?.HeaderPaddingUnit + @";
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                }
                footer {
                    width: 100vw;
                    height: 300px;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    background-color: " + ToCssColor(metadata?.Layout?.FooterColor) + @";
                    padding: " + metadata?.Layout?.FooterPadding + metadata?.Layout?.FooterPaddingUnit + @";
                    text-align: center;
                }
                .grid-container {
                    display: grid;
                    width: " + metadata?.Layout?.GridWidth + metadata?.Layout?.GridWidthUnit + @";
                    margin: " + metadata?.Layout?.GridMargin + metadata?.Layout?.GridMarginUnit + @";
                    padding: " + metadata?.Layout?.GridPadding + metadata?.Layout?.GridPaddingUnit + @";
                    background-color: " + ToCssColor(metadata?.Layout?.GridColor) + @";
                    border-radius: " + metadata?.Layout?.GridBorderRadius + metadata?.Layout?.GridBorderRadiusUnit + @";
                }
                .grid-item {
                    box-sizing: border-box;
                    overflow: hidden;
                }
            ");

            int index = 0;

            foreach (var componentEntry in layout.Components)
            {
                var comp = componentEntry.Value;
                var className = $"item-{index++}";
                var styles = new List<string>();

                styles.Add($"grid-column: {comp.StartColumn + 1} / span {Math.Max(1, comp.Colspan + 1)};");
                styles.Add($"grid-row: {comp.StartRow + 1} / span {Math.Max(1, comp.Rowspan + 1)};");

                // Gradient background if present
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
                    //var safeUri = Uri.EscapeUriString(imageUri);
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

                styles.Add($"border: {comp.CompStyle.BorderThickness}{comp.CompStyle.BorderThicknessUnit} solid {ToCssColor(comp.CompStyle.BorderColor)};"); 
                styles.Add($"border-radius: {comp.CompStyle.BorderRadius}{comp.CompStyle.BorderRadiusUnit};");
                styles.Add($"padding: {comp.CompStyle.PaddingTop}px {comp.CompStyle.PaddingRight}px {comp.CompStyle.PaddingBottom}px {comp.CompStyle.PaddingLeft}px;");
                styles.Add($"margin: {comp.CompStyle.MarginTop}px {comp.CompStyle.MarginRight}px {comp.CompStyle.MarginBottom}px {comp.CompStyle.MarginLeft}px;");
                styles.Add($"opacity: {comp.CompStyle.Opacity.ToString(CultureInfo.InvariantCulture)};");

                styles.Add($"justify-content: {comp.CompStyle.Justify.ToLower()};");
                styles.Add($"align-items: {comp.CompStyle.AlignItems.ToLower()};");
                styles.Add($"display: flex;");

                css.AppendLine($".{className} {{ {string.Join(" ", styles)} }}");
            }

            html.AppendLine(css.ToString());
            html.AppendLine("</style></head><body>");

            // Insert HEADER
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
                    html.AppendLine($"<a href=\"{link.Value}\" style=\"margin-left:15px;\">{link.Key}</a>");
                }
                html.AppendLine("</nav>");
            }
            html.AppendLine("</header>");

            // Grid
            html.AppendLine($"<div class=\"grid-container\" style=\"grid-template-columns: repeat({layout.ColAmount}, 1fr); grid-template-rows: repeat({layout.RowAmount}, 1fr);\">");

            // Generate HTML content again with assigned class
            index = 0;
            foreach (var componentEntry in layout.Components)
            {
                var comp = componentEntry.Value;
                var className = $"item-{index++}";
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

                html.AppendLine($"<div class=\"grid-item {className}\">{innerHtml}</div>");
            }

            html.AppendLine("</div>");
            html.AppendLine($"<footer>{System.Net.WebUtility.HtmlEncode(metadata?.Layout?.FooterContent ?? "")}</footer>");
            html.AppendLine("</body></html>");
            return html.ToString();
        }


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


        public static string MapJustifyOrAlign(string val)
        {
            val = val.ToLower();
            if (val == "center") return "center";
            if (val == "left" || val == "start") return "flex-start";
            if (val == "right" || val == "end") return "flex-end";
            if (val == "stretch") return "stretch";
            return "center";
        }

        public class SolidColorBrushConverter : JsonConverter<SolidColorBrush>
        {
            public override SolidColorBrush Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var colorString = reader.GetString();
                if (colorString == null)
                    return null;
                var color = (Color)ColorConverter.ConvertFromString(colorString);
                return new SolidColorBrush(color);
            }

            public override void Write(Utf8JsonWriter writer, SolidColorBrush value, JsonSerializerOptions options)
            {
                var color = value.Color;
                var colorString = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
                writer.WriteStringValue(colorString);
            }
        }


    }
}
