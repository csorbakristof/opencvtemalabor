using OpenCvSharp;
using System;
using System.IO;

namespace OpenCV_01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the input filename:");
            string in_filename = Console.ReadLine();

            try
            {
                Mat raw_pic_01 = new Mat(in_filename, ImreadModes.Color);

                Console.WriteLine("Please enter the output filename:");
                string out_filename = Console.ReadLine();

                Mat framed_pic_01 = new Mat();
                raw_pic_01.CopyTo(framed_pic_01);

                Cv2.Rectangle(framed_pic_01, new Rect(0, 0, framed_pic_01.Width, framed_pic_01.Height), new Scalar(0, 255, 255), 5);
                Cv2.ImWrite(out_filename, framed_pic_01);

                Cv2.ImShow("image", raw_pic_01);
                Cv2.ImShow("framed", framed_pic_01);

                Cv2.WaitKey();

                return;

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(in_filename + " can't be found.");
                Console.ReadLine();
            }
            catch (OpenCVException e)
            {
                Console.WriteLine("Invalid output filname.");
                Console.ReadLine();
            }
            
        }
    }
}
