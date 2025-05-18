using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SWD
{
    public class Head
    {
        public string ProjectName { get; set; }
        public string Author { get; set; }
        public string[] Keywords { get; set; }
        public string Description { get; set; }

    }

    internal class Images
    {
        public static BitmapImage NewIcon(string icon = "add.png")
        {
            string projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\.."));
            string iconPath = Path.Combine(projectDirectory, "Icons", icon);
            if (!File.Exists(iconPath))
            {
                Debug.WriteLine($"Image not found: {iconPath}");
            }
            Uri uri = new Uri(iconPath);
            BitmapImage bt = new BitmapImage(uri);

            return bt;
        }
    }

    internal class Names
    {
        public static string GetUniqueThemeName(string desiredName, IEnumerable<string> existingNames)
        {
            var nameSet = new HashSet<string>(existingNames, StringComparer.OrdinalIgnoreCase);

            Debug.WriteLine(desiredName);

            if (!nameSet.Contains(desiredName))
                return desiredName;

            int counter = 1;
            string newName;

            do
            {
                newName = $"{desiredName} ({counter})";
                counter++;
            }
            while (nameSet.Contains(newName));

            return newName;
        }
    }
}
