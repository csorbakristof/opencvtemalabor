using OpenCvSharp;

namespace OpenCVTest
{
    class Program
    {
        static void Main(string[] args)
        {

            string filein = args[0];
            string fileout = args[1];

            Mat picin = new Mat(filein, ImreadModes.Color);
            Mat picout = new Mat();
            picin.CopyTo(picout);

            Cv2.Rectangle(picout, new Rect(0, 0, picout.Width, picout.Height), new Scalar(0, 255, 255), 10);
            Cv2.ImWrite(fileout, picout);

            Cv2.ImShow("image", picin);
            Cv2.ImShow("framed", picout);

            Cv2.WaitKey();

        }
    }
}
