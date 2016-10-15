using System;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace week6
{
    class Program
    {
        static void Main(string[] args)
        {
            //Kép betöltése
            Mat original = new Mat("dots.jpg", ImreadModes.Color);
            Cv2.ImShow("Original", original);

            //Alsó és felső határok beállítása
            Scalar lower = new Scalar(0.0, 0.0, 200.0);
            Scalar upper = new Scalar(100.0, 100.0, 255.0);

            //Szűrés
            Mat range = new Mat();
            Cv2.InRange(original, lower, upper, range);
            Cv2.ImShow("InRange", range);

            Mat dilated = new Mat();
            Cv2.Dilate(range, dilated, new Mat());
            Cv2.ImShow("Dilated", dilated);

            int counter = 0;
            Random r = new Random();

            Mat colored = new Mat();
            Cv2.CvtColor(dilated, colored, ColorConversionCodes.GRAY2BGR);
            
            //Minden pixelen
            for (int x = 1; x < dilated.Width; x++)
                for (int y = 1; y < dilated.Height; y++) {
                    Vec3b pixel = dilated.Get<Vec3b>(y, x);

                    //Ha az adott pixel fehér
                    if (pixel.Item0 == 255 && pixel.Item1 == 255 && pixel.Item2 == 255) {


                        Cv2.FloodFill(dilated, new Point(x, y), new Scalar(0,0,0));
                        //Kitöltés random színnel
                        Cv2.FloodFill(colored, new Point(x, y), new Scalar(r.Next(20, 230), r.Next(20, 230), r.Next(20, 230)));
                        counter++;
                    }
                }

            Cv2.ImShow("FloodFill with " + counter + " dots", colored);
            Cv2.WaitKey();
        }
    }
}
