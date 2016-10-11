using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace HF_3
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Please give me an input image");
            String inputLocation = Console.ReadLine();

            Mat inputImage = new Mat(inputLocation);

            Scalar lowerb = new Scalar(0, 0, 225);
            Scalar upperb = new Scalar(60, 60, 255);

            Mat redObjects = new Mat();

            Cv2.InRange(inputImage, lowerb, upperb, redObjects);
            Cv2.ImShow("Red Objects", redObjects);
            Cv2.ImWrite("RedObject.jpg", redObjects);

            Mat dilatedImage = new Mat();
            dilatedImage = redObjects;

            for (int i = 1; i <= 1; i++)
            {
                Cv2.Dilate(dilatedImage, dilatedImage, new Mat());
                Cv2.ImShow("Dilateded Image_"+i, dilatedImage);
                Cv2.WaitKey(250);
                Cv2.DestroyWindow("Dilateded Image_" + i);
            }
            Cv2.ImShow("Dilateted Image", dilatedImage);
            Cv2.ImWrite("Dilated.jpg", dilatedImage);

            int counter = 0;

            for (int x = 0; x < dilatedImage.Width; x++)
            {
                for (int y = 0; y < dilatedImage.Height; y++)
                {
                    Vec3b color = dilatedImage.At<Vec3b>(y, x);

                    if(whiteCheck(color))
                    {
                        counter++;
                        dilatedImage.FloodFill(new Point(x, y), 100);
                        Cv2.ImShow("Counter", dilatedImage);
                        Cv2.WaitKey(100);
                    }

                }
            }
            Cv2.DestroyWindow("Counter");
            Console.WriteLine("Number of red fields = "+ counter);
            Cv2.WaitKey();
        }

        public static Boolean whiteCheck(Vec3b v)
        {
            return v.Item0 == 255 && v.Item1 == 255 && v.Item2 == 255;
        }

    }
}
