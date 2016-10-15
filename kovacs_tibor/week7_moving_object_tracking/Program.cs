using OpenCvSharp;
using System;

namespace week7_moving_object_detection
{
    class Program
    {
        private static string WINDOW_NAME = "Move Detection";

        static void Main(string[] args)
        {
            Console.Write("Enter the path to the video file: ");
            string inputVideoPath = Console.ReadLine();

            Cv2.NamedWindow(WINDOW_NAME);
            VideoCapture video = new VideoCapture(inputVideoPath);
            BackgroundSubtractorMOG2 mog = BackgroundSubtractorMOG2.Create(250, 68, true);

            Mat videoFrame = new Mat();
            Mat mogFrame = new Mat();

            int inputCharacter = -1;
            while (video.Read(videoFrame) && inputCharacter != 27)
            {
                if (videoFrame.Empty())
                {
                    break;
                }

                mog.Apply(videoFrame, mogFrame);

                Cv2.ImShow(WINDOW_NAME, mogFrame);
                inputCharacter = Cv2.WaitKey(5);
            }
        }
    }
}
