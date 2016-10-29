using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace week8
{
    class Program
    {

        private const double highThreshold = 200.0;
        private const double lowThreshold = 60.0;

        public static bool isWhite(Vec3b pixel)
        {
            return pixel.Item0 == 255 && pixel.Item1 == 255 && pixel.Item2 == 255;
        }

        static void Main(string[] args)
        {
            //Webcam 25 fps-el
            VideoCapture capture = new VideoCapture(0) { Fps = 25 };

            Mat frame = new Mat();
            Mat gray = new Mat();
            Mat edge = new Mat();

            while (capture.Read(frame))
            {
                int count = 0;

                //Szürkeárnyalatos kép előállítása
                Cv2.CvtColor(frame,gray,ColorConversionCodes.RGB2GRAY);

                //Élkeresés
                Cv2.Canny(gray, edge, lowThreshold, highThreshold);

                //Fehér pixelek száma
                for (int i = 0; i < edge.Width; i++)
                    for (int j = 0; j < edge.Height; j++)
                        if (isWhite(edge.Get<Vec3b>(j, i)))
                            count++;

                //Arány
                double ratio = (double)count/(edge.Width*edge.Height);
                string text = "Ratio: " + ratio + "%";
                Cv2.PutText(edge, text, new Point(5.0,15.0), HersheyFonts.HersheySimplex, 0.5d, new Scalar(255.0d, 255.0d, 255.0d));

                //Ablakok megjelenítése
                Cv2.ImShow("Frame",frame);
                Cv2.ImShow("Edge",edge);
                if (Cv2.WaitKey((int)capture.Fps) != -1) break;
            }

            Cv2.DestroyAllWindows();
        }
    }
}
