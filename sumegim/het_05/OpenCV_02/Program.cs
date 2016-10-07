using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace OpenCV_02
{
    class Program
    {
        static void Main(string[] args)
        {
            Size dsize = new Size(640, 480);

            VideoCapture cap = new VideoCapture(0);
            cap.Fps = 25;

            //VideoCapture cap = new VideoCapture("input.avi");


            VideoWriter wrt = new VideoWriter("output.avi", "MJPG", 25, dsize);

            Mat frame = new Mat();
            Mat edges = new Mat();
            

            if (!cap.IsOpened())
                Console.WriteLine("Error when reading video file");


            if (!wrt.IsOpened())
                Console.WriteLine("Invalid output video format");

            float ox = 320;
            float oy = 240;
            float vx = 1f;
            float vy = 3f;
            float r = 75;
            float dr = 0.2f;

            while (true)
            {
                if (cap.Read(frame))
                {
                    if (frame.Empty()) break;


                    if (1 == 1)
                    {

                        if (ox > 640 - r || ox < r) vx *= -1;
                        if (oy > 480 - r || oy < r) vy *= -1;
                        if (r < 50 || r > 100) dr *= -1;

                        ox += vx;
                        oy += vy;
                        r += dr;


                        Cv2.Ellipse(frame, new RotatedRect(new Point2f(ox, oy), new Size2f(2*r, 2*r), 0), new Scalar(0, 0, 255), 6);
                    }
                    else
                    {
                        Cv2.Ellipse(frame, new RotatedRect(new Point2f(400, 300), new Size2f(200, 300), 30), new Scalar(255, 0, 0), 6);
                    }

                    Cv2.Canny(frame, edges, 60, 200, 3, false);
                    Cv2.ImShow("Video", frame);
                    Cv2.ImShow("Edges", edges);

                    wrt.Write(frame);

                    

                    int c = Cv2.WaitKey(10); 
                    if (c != -1) { break; }
                }
            }

        }
    }
}
