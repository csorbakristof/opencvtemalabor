using System;
using OpenCvSharp;

namespace week7
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Video file: ");
            VideoCapture video = new VideoCapture(Console.ReadLine());
            BackgroundSubtractorMOG2 mog = BackgroundSubtractorMOG2.Create(500, 20, true);

            Mat videoFrame = new Mat();
            Mat mogFrame = new Mat();
            Mat dilated = new Mat();

            while (Cv2.WaitKey(30) == -1 && video.Read(videoFrame) && !videoFrame.Empty())
            {
                mog.Apply(videoFrame, mogFrame);
                Cv2.Dilate(mogFrame, dilated, new Mat());
                Cv2.ImShow("Original", videoFrame);
                Cv2.ImShow("MOG2", mogFrame);
                Cv2.ImShow("Dilated", dilated);
            }
        }
    }
}

