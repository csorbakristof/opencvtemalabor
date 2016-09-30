using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace HF_1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            if (args.Length>1)
            {
                string input = args[0].ToString();
                string output = args[1].ToString();

                Mat image = new Mat(input, ImreadModes.Color);
                Cv2.Rectangle(image, new Rect(0, 0, image.Cols, image.Rows), new Scalar(0, 255, 255), 3);
                Cv2.ImWrite(output, image);
                Cv2.ImShow("new Image", image);
                Cv2.WaitKey();
            }
            else
            {
                Console.WriteLine("Give some arguments!");
                Console.ReadKey();
            }

        }
    }
}
