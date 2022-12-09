using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = OpenCvSharp.Point;

namespace OpenCvMatchTemplate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (Bitmap bitmap = (Bitmap)Bitmap.FromFile("template.png"))
            using (var templatemat = OpenCvSharp.Extensions.BitmapConverter.ToMat(bitmap))
            //using (Mat templatemat = new Mat("template.png", OpenCvSharp.ImreadModes.Unchanged))
            {
                string[] files = { "img1.png", "img2.png", "img3.png" };
                foreach (string file in files)
                {
                    using (var BaseImage = new OpenCvSharp.Mat(file, OpenCvSharp.ImreadModes.Unchanged))
                    {
                        var result = BaseImage.MatchTemplate(templatemat, OpenCvSharp.TemplateMatchModes.CCoeffNormed);
                        double minVal, maxVal;
                        OpenCvSharp.Point minLoc, maxLoc;
                        result.MinMaxLoc(out minVal, out maxVal, out minLoc, out maxLoc);

                        BaseImage.Rectangle(new Point(maxLoc.X, maxLoc.Y), new Point(maxLoc.X + templatemat.Width, maxLoc.Y + templatemat.Height), new Scalar(0, 0, 255), 2);
                        string text = $"Confidence: {Math.Round((float)(maxVal), 2)}";
                        BaseImage.PutText(text, new Point(maxLoc.X, maxLoc.Y), OpenCvSharp.HersheyFonts.HersheyPlain, 1, new Scalar(0, 0, 0), 2);
                        using (var window = new Window("window", image: BaseImage, flags: WindowMode.AutoSize))
                        {
                            Cv2.WaitKey();
                        }
                    }
                }
            }
        }
    }
}
