using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpneCV_04
{
    class Program
    {
        static double erosion_size = 1;
        static Mat element = Cv2.GetStructuringElement(MorphShapes.Ellipse,
              new Size(2 * erosion_size + 1, 2 * erosion_size + 1),
              new Point(erosion_size, erosion_size));
        
        static void Main(string[] args)
        {
            VideoCapture cap = new VideoCapture("ErE3_IBike.mov");
            //VideoCapture cap = new VideoCapture(0);

            if (!cap.IsOpened())
            {
                Console.WriteLine("Error when reading video file");
                Console.ReadKey();
            }

            BackgroundSubtractorMOG2 mog2 = BackgroundSubtractorMOG2.Create(100, 60);

            Mat frame = new Mat();
            Mat mog_frame = new Mat();

            while (true)
            {
                if (cap.Read(frame))
                {
                    if (frame.Empty()) break;

                    mog2.Apply(frame, mog_frame);

                    Cv2.ImShow("Video", frame);
                    Cv2.ImShow("Moving objects", mog_frame);

                    int c = Cv2.WaitKey(10);
                    if (c != -1) { break; }
                }
            }
        }
    }
}
