using OpenCvSharp;

namespace week8_edge_detection
{
    public class Program
    {
        private static readonly string WIN_NAME_ORIGINAL = "Original";
        private static readonly string WIN_NAME_EDGE = "Edges";

        private static readonly int SPACE = 32;

        private static readonly double THRESHOLD_2 = 100.0d;
        private static readonly double THRESHOLD_1 = 20.0d;

        private static readonly Point TEXT_POSITION = new Point(10.0d, 20.0d);
        private static readonly double TEXT_SIZE = 0.5d;
        private static readonly Scalar TEXT_COLOR = new Scalar(255.0d, 0.0d, 0.0d);

        public static void Main(string[] args)
        {
            Cv2.NamedWindow(WIN_NAME_ORIGINAL);
            Cv2.NamedWindow(WIN_NAME_EDGE);

            VideoCapture videoReader = new VideoCapture(0);
            Mat videoFrame = new Mat();
            while (videoReader.Read(videoFrame))
            {
                Mat edgeFrameImage = GetEdgeImage(videoFrame);
                string ratioText = "Ratio: " + (GetEdgeRatio(edgeFrameImage)*100.0d).ToString() + "%";
                Cv2.PutText(videoFrame, ratioText, TEXT_POSITION, HersheyFonts.HersheySimplex, TEXT_SIZE, TEXT_COLOR);

                Cv2.ImShow(WIN_NAME_ORIGINAL, videoFrame);
                Cv2.ImShow(WIN_NAME_EDGE, edgeFrameImage);
                
                if (Cv2.WaitKey(1) == SPACE)
                {
                    break;
                }
            }

            Cv2.DestroyAllWindows();
        }

        private static Mat GetEdgeImage(Mat image)
        {
            Mat grayImage = new Mat();
            Cv2.CvtColor(image, grayImage, ColorConversionCodes.RGB2GRAY);
            Mat edgeImage = new Mat();
            Cv2.Canny(grayImage, edgeImage, THRESHOLD_1, THRESHOLD_2);
            return edgeImage;
        }

        private static double GetEdgeRatio(Mat image)
        {
            int edgePixelCount = image.CountNonZero();
            return  (double)edgePixelCount / (image.Width * image.Height);
        }

    }

}
