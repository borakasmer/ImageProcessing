using System.Collections.Generic;
using System.Linq;
using System.Drawing;
 
namespace ImageProcessing.Models
{
    public static class PictureAnalysis
    {
        public static List<Color> TenDominantColors { get; private set; }
        public static List<int> TenDominantColorIncidences { get; private set; }

        public static Color MostDominantColor { get; private set; }
        public static int MostDominantColorRgb { get; private set; }

        private static int pixelColor;

        private static Dictionary<int, int> dctColorIncidence;

        public static void GetMostDominantColor(Bitmap theBitMap)
        {
            TenDominantColors = new List<Color>();
            TenDominantColorIncidences = new List<int>();

            MostDominantColor = Color.Empty;
            MostDominantColorRgb = 0;             

            dctColorIncidence = new Dictionary<int, int>();

            // bu kod daha da hızlandırılabilir. 
            for (int row = 0; row < theBitMap.Size.Width; row++)
            {
                for (int col = 0; col < theBitMap.Size.Height; col++)
                {
                    pixelColor = theBitMap.GetPixel(row, col).ToArgb();

                    if (dctColorIncidence.Keys.Contains(pixelColor))
                    {
                        dctColorIncidence[pixelColor]++;
                    }
                    else
                    {
                        dctColorIncidence.Add(pixelColor, 1);
                    }
                }
            }
            
            // .NET Generic Dictionary'nin sort işlemi %100 garanti değildir. Daha farklı sıralanabilir.
            var dctSortedByValueHighToLow = dctColorIncidence.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            // bu kısmı Linq kullanılarak yapılması daha sağlıklı olabilir.
            foreach (KeyValuePair<int, int> kvp in dctSortedByValueHighToLow.Take(10))
            {
                TenDominantColors.Add(Color.FromArgb(kvp.Key));
                TenDominantColorIncidences.Add(kvp.Value);
            }

            MostDominantColor = Color.FromArgb(dctSortedByValueHighToLow.First().Key);
            MostDominantColorRgb = dctSortedByValueHighToLow.First().Value;
        }

    }
}

