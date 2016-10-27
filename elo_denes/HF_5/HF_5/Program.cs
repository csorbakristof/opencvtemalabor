using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace HF_5
{
    class Program
    {
        static void Main(string[] args)
        {

            VideoCapture input = new VideoCapture(@"F:\mog2sample.MOV");

            if (!input.IsOpened()) { Console.WriteLine("Problem with the input!"); Console.ReadKey(); }
            else
            {

                Mat frame = new Mat();
                Mat segmentedFrame = new Mat();
                int key = 0;

                while (key != 27)
                {

                    input.Read(frame);
                    Cv2.Canny(frame, segmentedFrame, 70, 100,3);
                    
                    Cv2.ImShow("Output", segmentedFrame);
                    
                    key = Cv2.WaitKey(5);

                }

            }

        }
    }
}
