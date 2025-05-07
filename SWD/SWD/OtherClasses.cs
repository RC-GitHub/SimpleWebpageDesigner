using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SWD
{
    internal class Head
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
            string imagePath = Path.Combine(projectDirectory, "Icons", icon);
            if (!File.Exists(imagePath))
            {
                Debug.WriteLine($"Image not found: {imagePath}");
            }
            Uri uri = new Uri(imagePath);
            BitmapImage bt = new BitmapImage(uri);

            return bt;
        }
    }
}
