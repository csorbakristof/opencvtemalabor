using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF_2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Please give me an input file:");
            //String input = Console.ReadLine();
            Console.WriteLine("Please give me an output file:");
            String output = Console.ReadLine();

            try
            {

                VideoCapture inputvideo = new VideoCapture(0);
                if (!inputvideo.IsOpened())
                {
                    throw new Exception("Input Error!");
                }

                VideoWriter outputwriter = new VideoWriter(output, "MJPG", 25, new Size(inputvideo.FrameWidth, inputvideo.FrameHeight));
                if (!outputwriter.IsOpened())
                {
                    throw new Exception("Output error");
                }

                Mat frame = new Mat();
                Random r = new Random(0);
                Random r2 = new Random(2);

                int keypressed = -1;

                while (inputvideo.Read(frame) && keypressed == -1)
                {

                    float fx = r.Next(720);
                    float fy = r.Next(480);

                    frame.Ellipse(new RotatedRect(new Point2f(fx, fy), new Size2f(r2.Next(150), r2.Next(150)), 90), new Scalar(r.Next(255), r.Next(255), r.Next(255)), -1);
                    
                    outputwriter.Write(frame);

                    Cv2.ImShow("Output Video", frame);

                    keypressed = Cv2.WaitKey(1000 / 25);

                }

                inputvideo.Release();
                outputwriter.Release();

            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
