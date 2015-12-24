using ImageProcessing.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageProcessing.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string  UploadImage(IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var routhPath = Server.MapPath("~/Upload");
                        string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        fileName += "_" + Guid.NewGuid().ToString("N");
                        fileName += Path.GetExtension(file.FileName);
                        var path = Path.Combine(routhPath, fileName);
                        file.SaveAs(path);

                        Bitmap image = new Bitmap(path);                       
                    
                        PictureAnalysis.GetMostDominantColor(image);

                        string colorName = GetColorName(PictureAnalysis.MostDominantColor);
                       
                        return "/Upload/" +fileName+"æ"+ colorName;                   
                    }
                }
            }
            return "";
        }
        private static System.Collections.ArrayList WebColors;
        public static String GetColorName(Color color)
        {

            WebColors = GetWebColors();
            double dbl_input_red = Convert.ToDouble(color.R);
            double dbl_input_green = Convert.ToDouble(color.G);
            double dbl_input_blue = Convert.ToDouble(color.B);
            double dbl_test_red, dbl_test_green, dbl_test_blue, temp;
            double distance = 500.0;

            Color nearest_color = Color.Empty;
            foreach (object o in WebColors)
            {
                if ((Color)o != Color.Transparent)
                {
                    // compute the Euclidean distance between the two colors
                    // note, that the alpha-component is not used in this example
                    dbl_test_red = Math.Pow(Convert.ToDouble(((Color)o).R) - dbl_input_red, 2.0);
                    dbl_test_green = Math.Pow(Convert.ToDouble
                        (((Color)o).G) - dbl_input_green, 2.0);
                    dbl_test_blue = Math.Pow(Convert.ToDouble
                        (((Color)o).B) - dbl_input_blue, 2.0);
                    // it is not necessary to compute the square root
                    // it should be sufficient to use:
                    // temp = dbl_test_blue + dbl_test_green + dbl_test_red;
                    // if you plan to do so, the distance should be initialized by 250000.0
                    temp = Math.Sqrt(dbl_test_blue + dbl_test_green + dbl_test_red);
                    // explore the result and store the nearest color
                    if (temp == 0.0)
                    {
                        // the lowest possible distance is - of course - zero
                        // so I can break the loop (thanks to Willie Deutschmann)
                        // here I could return the input_color itself
                        // but in this example I am using a list with named colors
                        // and I want to return the Name-property too
                        nearest_color = (Color)o;
                        break;
                    }
                    else if (temp < distance)
                    {
                        distance = temp;
                        nearest_color = (Color)o;
                    }
                }
            }
            return nearest_color.Name;
        }
        private static System.Collections.ArrayList GetWebColors()
        {
            Type color = (typeof(Color));
            System.Reflection.PropertyInfo[] propertyInfos = color.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            System.Collections.ArrayList colors = new System.Collections.ArrayList();
            foreach (System.Reflection.PropertyInfo pi in propertyInfos)
            {
                if (pi.PropertyType.Equals(typeof(Color)))
                {
                    Color c = (Color)pi.GetValue((object)(typeof(Color)), null);
                    colors.Add(c);
                }
            }
            return colors;
        }
    }
}