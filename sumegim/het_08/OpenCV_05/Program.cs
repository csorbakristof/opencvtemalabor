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
            VideoCapture cap = new VideoCapture(0);
            cap.Fps = 25;

            Mat frame = new Mat();
            Mat edges = new Mat();

            if (!cap.IsOpened())
                Console.WriteLine("Error when reading video file");

            Vec3b a = new Vec3b();
            int whitepixels;

            while (true)
            {
                if (cap.Read(frame))
                {
                    if (frame.Empty()) break;
                    
                    Cv2.Canny(frame, edges, 60, 200, 3, false);
                    whitepixels = 0;

                    for (int i = 0; i < edges.Width; i++)
                    {
                        for (int j = 0; j < edges.Height; j++)
                        {
                            a = edges.Get<Vec3b>(j, i);
                            if (a.Item0 == 255 && a.Item1 == 255 && a.Item2 == 255)
                            {
                                whitepixels++;
                            }
                        }
                    }
                    Console.WriteLine("edges: {0}%", whitepixels * 100.0 / (edges.Width * edges.Height));

                    Cv2.ImShow("Video", frame);
                    Cv2.ImShow("Edges", edges);

                    int c = Cv2.WaitKey(10);
                    if (c != -1) { break; }
                }
            }
        }
    }
}
