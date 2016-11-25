using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneAssist
{
    class Program
    {
        public static List<LineSegmentPoint> linesegments;
        public static AreaOfInterest areaOfInterest;
        public static LaneScanner lineScanner;
        
        public static Size dsize;

        public class AreaOfInterest
        {
            public int x0 = 25;  // UPPER LEFT
            public int y0 = 240;

            public int x1 = 600; // LOWER RIGHT
            public int y1 = 310;
        }

        public class LaneScanner
        {
            public int lx0, lx1, rx0, rx1;
            int framecnt1;
            int framecnt2;
            int framecnt3;
            int framecnt4;

            public LaneScanner()
            {
                lx0 = 0;
                lx1 = 0;
                rx0 = 0;
                rx1 = 0;
                framecnt1 = 10;
                framecnt2 = 10;
                framecnt3 = 10;
                framecnt4 = 10;
            }

            public void Scan(Mat subbed)
            {
                Vec3b a = new Vec3b();

                for (int i = 350; i < areaOfInterest.x1; i++)
                {
                    a = subbed.Get<Vec3b>(areaOfInterest.y0, i);
                    if (a.Item0 == 255 && a.Item1 == 255 && a.Item2 == 255)
                    {
                        if (Math.Abs(lx0 - i) < 20 || framecnt1 == 10)
                        {
                            rx0 = i;
                            framecnt1 = 0;
                            break;
                        }
                        else
                        {
                            framecnt1++;
                        }
                    }
                }

                for (int i = 350; i > areaOfInterest.x0; i--)
                {
                    a = subbed.Get<Vec3b>(areaOfInterest.y0, i);

                    if (a.Item0 == 255 && a.Item1 == 255 && a.Item2 == 255)
                    {
                        if (Math.Abs(lx0 - i) < 20 || framecnt2 == 10)
                        {
                            lx0 = i;
                            framecnt2 = 0;
                            break;
                        }
                        else
                        {
                            framecnt2++;
                        }
                    }
                }

                for (int i = 350; i < areaOfInterest.x1; i++)
                {
                    a = subbed.Get<Vec3b>(areaOfInterest.y1, i);
                    if (a.Item0 == 255 && a.Item1 == 255 && a.Item2 == 255)
                    {
                        if (Math.Abs(rx1 - i) < 20 || framecnt3 == 10)
                        {
                            rx1 = i;
                            framecnt3 = 0;
                            break;
                        }
                        else
                        {
                            framecnt3++;
                        }
                    }
                }

                for (int i = 350; i > areaOfInterest.x0; i--)
                {
                    a = subbed.Get<Vec3b>(areaOfInterest.y1, i);

                    if (a.Item0 == 255 && a.Item1 == 255 && a.Item2 == 255)
                    {
                        if (Math.Abs(lx1 - i) < 20 || framecnt4 == 10)
                        {
                            lx1 = i;
                            framecnt4 = 0;
                            break;
                        }
                        else
                        {
                            framecnt4++;
                        }
                    }
                }
            }
        }

        static void DisplayResults(Mat frame, int lx0, int rx0, int lx1, int rx1)
        {
            Cv2.Line(frame, lx1, areaOfInterest.y1, lx0, areaOfInterest.y0, new Scalar(0, 0, 255), 3);
            Cv2.Line(frame, rx1, areaOfInterest.y1, rx0, areaOfInterest.y0, new Scalar(0, 0, 255), 3);

            Cv2.Rectangle(frame, new Point(areaOfInterest.x0, areaOfInterest.y0), new Point(areaOfInterest.x1, areaOfInterest.y1), new Scalar(0, 255, 0));
            Cv2.ImShow("raw", frame);
        }

        static Mat FindLines(Mat edges)
        {
            linesegments.AddRange(Cv2.HoughLinesP(edges, 1, Math.PI / 180, 50, 180, 80));

            Mat lines = new Mat(dsize, MatType.CV_8UC3);

            for (int i = 0; i < linesegments.Count; i++)
            {
                int dx = linesegments[i].P2.X - linesegments[i].P1.X;
                int dy = linesegments[i].P2.Y - linesegments[i].P1.Y;
                double angle = Math.Atan2(dy, dx) * 180 / Math.PI;

                if (Math.Abs(angle) <= 10) continue;
                if (Math.Abs(angle) >= 50) continue;

                Cv2.Line(lines, linesegments[i].P1, linesegments[i].P2, new Scalar(255, 255, 255), 3);
            }
            Cv2.CvtColor(lines, lines, ColorConversionCodes.BGR2GRAY);
            return lines;
        }

        static Mat Combine(Mat edges, Mat lines, List<Mat> framebuffer)
        {
            framebuffer.Add(edges & lines);
            if (framebuffer.Count > 10)
            {
                framebuffer.RemoveAt(0);
            }

            Mat combined = new Mat(dsize, MatType.CV_8UC3);
            Cv2.CvtColor(combined, combined, ColorConversionCodes.BGR2GRAY);

            for (int i = 0; i < framebuffer.Count; i++)
            {
                combined += framebuffer[i];
            }

            return combined;
        }

        static void Main(string[] args)
        {
            VideoCapture cap = new VideoCapture("input.avi");
            if (!cap.IsOpened())
            {
                Console.WriteLine("Error when reading video file");
                Console.ReadKey();
            }

            int erosion_size = 2;
            Mat element = Cv2.GetStructuringElement(MorphShapes.Ellipse,
              new Size(2 * erosion_size + 1, 2 * erosion_size + 1),
              new Point(erosion_size, erosion_size));

            linesegments = new List<LineSegmentPoint>();

            int k = 10;

            areaOfInterest = new AreaOfInterest();

            Mat frame = new Mat();
            VideoWriter wrt = new VideoWriter("output.avi", "MJPG", 25, new Size(640, 360));
            List<Mat> framebuffer = new List<Mat>();
            lineScanner = new LaneScanner();

            while (true)
            {
                if (cap.Read(frame))
                {
                    if (frame.Empty()) break;

                    dsize = new Size(frame.Width, frame.Height);
                    
                    Mat edges = new Mat();
                    Cv2.Canny(frame, edges, 80, 240, 3, false);

                    Mat edges_dilated = new Mat();
                    Cv2.Dilate(edges, edges_dilated, element);

                    if (k++ == 10)
                    {
                        linesegments.Clear();
                        k = 0;

                    }

                    Mat lines = FindLines(edges);

                    Mat combined = Combine(edges_dilated, lines, framebuffer);

                    lineScanner.Scan(combined);

                    DisplayResults(frame, lineScanner.lx0, lineScanner.rx0, lineScanner.lx1, lineScanner.rx1);
                    wrt.Write(frame);

                    int c = Cv2.WaitKey(10);
                    if (c != -1) { break; }
                }
            }
        }
    }
}
