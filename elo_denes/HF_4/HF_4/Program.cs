using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace HF_4
{
    class Program
    {
        static void Main(string[] args)
        {
            VideoCapture video = new VideoCapture(0);
            int keyPressed=0;
            BackgroundSubtractorMOG2 mog2 = BackgroundSubtractorMOG2.Create();
            Mat frame = new Mat();
            Mat motionObjects = new Mat();

            if (!video.IsOpened())
            {
                Console.WriteLine("Unable to open video");
                Console.ReadKey();
            }
            else
            {
                while (keyPressed != 27)
                {
                    video.Read(frame);
                    mog2.Apply(frame, motionObjects);

                    Cv2.ImShow("Original", frame);
                    Cv2.ImShow("Motion Objects", motionObjects);

                    keyPressed = Cv2.WaitKey(5);

                }

                Cv2.DestroyAllWindows();

            }

        }
    }
}
