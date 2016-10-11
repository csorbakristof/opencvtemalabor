using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV_03
{
    class Program
    {
        static void Main(string[] args)
        {
            Mat raw_01 = new Mat("reddots.jpg", ImreadModes.Color);
            //Mat raw_01 = new Mat("redsplash.jpg", ImreadModes.Color);

            Mat hsv_img = new Mat();
            Cv2.CvtColor(raw_01, hsv_img, ColorConversionCodes.BGR2HSV);

            Mat lower_red_hue_range = new Mat();
            Mat higher_red_hue_range = new Mat();
            Cv2.InRange(hsv_img, new Scalar(0, 100, 100), new Scalar(10, 255, 255), lower_red_hue_range);
            Cv2.InRange(hsv_img, new Scalar(160, 100, 100), new Scalar(179, 255, 255), higher_red_hue_range);

            Mat reddots_gray = new Mat();

            Cv2.AddWeighted(lower_red_hue_range, 1.0, higher_red_hue_range, 1.0, 0.0, reddots_gray);
            Cv2.ImShow("reddots", reddots_gray);

            Mat dilated = new Mat();

            int erosion_size = 2;
            Mat element = Cv2.GetStructuringElement(MorphShapes.Ellipse,
                  new Size(2 * erosion_size + 1, 2 * erosion_size + 1),
                  new Point(erosion_size, erosion_size));

            Cv2.Dilate(reddots_gray, dilated, element);

            Mat reddots_bgr = new Mat();
            Cv2.CvtColor(dilated, reddots_bgr, ColorConversionCodes.GRAY2BGR);

            Random r = new Random();
            int red_cnt = 0;
            
            for (int i = 1; i < reddots_bgr.Width; i++)
            {
                for (int j = 1; j < reddots_bgr.Height; j++)
                {
                    Vec3b a = new Vec3b();
                    a = reddots_bgr.Get<Vec3b>(j, i);

                    if (a.Item0 == 255 && a.Item1 == 255 && a.Item2 == 255)
                    {
                        red_cnt++;
                        Cv2.FloodFill(reddots_bgr, new Point(i, j), new Scalar(r.Next(5, 250), r.Next(5, 250), r.Next(5, 250)));
                    }
                }
            }

            Console.WriteLine("Number of red areas: {0}", red_cnt);

            Cv2.ImShow("raw", raw_01);
            Cv2.ImShow("reds", reddots_gray);
            Cv2.ImShow("dilated", dilated);
            Cv2.ImShow("reddots", reddots_bgr);

            Cv2.WaitKey();
        }
    }
}
