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

            Console.WriteLine("Please give me an input file");
            String inputfile = Console.ReadLine();
            VideoCapture input = new VideoCapture(inputfile);

            if (!input.IsOpened()) { Console.WriteLine("Problem with the input!"); Console.ReadKey(); }
            else
            {

                Mat frame = new Mat();
                Mat segmentedFrame = new Mat();
                int key = 0;

                Point p = new Point(10d, 30d);
                Scalar color = new Scalar(0, 0, 255);
                double size = 1d;
                double ratio = 0d;

                while (key != 27)
                {

                    input.Read(frame);
                    Cv2.Canny(frame, segmentedFrame, 70, 100,3);

                    double whitePixel = 0;

                    for(int i = 0; i<segmentedFrame.Width; i++)
                    {
                        for (int j = 0; j < segmentedFrame.Height; j++)
                        {
                            if (segmentedFrame.At<char>(i, j) != 0)
                                whitePixel++;
                        }
                    }

                    ratio = (100 * whitePixel) / (segmentedFrame.Cols * segmentedFrame.Rows);

                    Cv2.PutText(frame, "Ratio: "+ ratio.ToString() + "%", p, HersheyFonts.HersheySimplex, size, color, 1);

                    Cv2.ImShow("Output", segmentedFrame);
                    Cv2.ImShow("Original", frame);
                    
                    key = Cv2.WaitKey(5);

                }

            }

        }
    }
}