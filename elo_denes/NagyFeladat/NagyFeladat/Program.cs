using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using OpenCvSharp;

namespace NagyFeladat
{


    class Program
    {
        static int MOG_THRES = 35;
        static int MOG_HIST = 250;
        static BackgroundSubtractorMOG2 mog;
        static CvTrackbar MinArea;
        static CvTrackbar MaxArea;
        static CvTrackbar CriticalArea;
        static int AreaMin = 1000;
        static int AreaMax = 5000;
        static int AreaCritical = 5000;

        public static Mat motionCapture(Mat frame)
        {
            Mat result = new Mat();
            try
            {
                if (!frame.Empty())
                {
                    mog.Apply(frame, result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }

        public static Mat findContours(Mat frame)
        {
            Mat result = Mat.Zeros(frame.Rows, frame.Cols, MatType.CV_8UC3);
            Mat motionMask = motionCapture(frame);
            Point[][] contours0, contours;
            HierarchyIndex[] hierachy;

            Cv2.ImShow("Blur", motionMask.GaussianBlur(new Size(), 5d));
            //motionMask = motionMask.Erode(new Mat(), new Point(-1, 1), 1);
            //motionMask = motionMask.Dilate(new Mat(), new Point(-1, 1), 2);
            //Cv2.ImShow("motionmask", motionMask);
            Cv2.FindContours(motionMask, out contours0, out hierachy, RetrievalModes.CComp, ContourApproximationModes.ApproxSimple);
            contours = new Point[contours0.Length][];
            for (int i = 0; i < contours0.Length; i++)
            {
                contours[i] = Cv2.ApproxPolyDP(contours0[i], 3, true);
            }
            Cv2.DrawContours(result, contours, -1, Scalar.LightBlue, thickness: -1);
            //Cv2.ImShow("Contours", result);

            for (int i = 0; i < contours.Length; i++)
            {
                Rect boundingRect = Cv2.BoundingRect(contours[i]);
                int area = boundingRect.Width * boundingRect.Height;
                if (area > AreaMin && area < AreaMax)
                {
                    if (area > AreaCritical)
                    {
                        Cv2.Rectangle(frame, boundingRect, Scalar.Red, 3);
                        Cv2.PutText(frame, area.ToString(), boundingRect.TopLeft, HersheyFonts.HersheySimplex, 0.5d, Scalar.Aqua);
                    }
                    else {
                        Cv2.Rectangle(frame, boundingRect, Scalar.LightGreen, 3);
                        Cv2.PutText(frame, area.ToString(), boundingRect.TopLeft, HersheyFonts.HersheySimplex, 0.5d, Scalar.Aqua);
                    }
                }
            }
            return frame;
        }


        static void Main(string[] args)
        {
            String path = "F:/mog2sample.MOV";
            Console.WriteLine("Give me the input file (0 == WebCam)");
            path = Console.ReadLine();
            VideoCapture capture;
            if (path.Equals("0"))
            {
                capture = new VideoCapture(0);
            }
            else
            {
                capture = new VideoCapture(path);
            }
           
            Mat frame = new Mat();
            Mat output = new Mat();
            int exit_key = -1;
            Window Trackbars = new Window("Trackbars");
            MinArea = new CvTrackbar("Min_Area", "Trackbars", 200, 1000, onAreaMin);
            MaxArea = new CvTrackbar("Max_Area", "Trackbars", 500, 1000, onAreaMax);
            CriticalArea = new CvTrackbar("Critical", "Trackbars", 500, 1000, onCritical);
            mog = BackgroundSubtractorMOG2.Create(MOG_HIST, MOG_THRES, false);
            if (capture.IsOpened())
            {
                while (capture.Read(frame) && exit_key != 27)
                {
                    output = findContours(frame);
                    Cv2.ImShow("Result", output);
                    exit_key = Cv2.WaitKey(20);
                }
            }
            else
            {
                Console.WriteLine("Input error!");
                Console.ReadKey();
            }
        }

        private static void onCritical(int pos, object userdata)
        {
            AreaCritical = pos * 10;
        }

        private static void onAreaMax(int pos, object userdata)
        {
            AreaMax = pos * 100;
        }

        private static void onAreaMin(int pos, object userdata)
        {
            AreaMin = pos;
        }
    }
}
