using OpenCvSharp;
using System;

namespace OpenCV_06
{
    class Program
    {
        class Brush
        {
            public Scalar colour = new Scalar(0, 0, 255);
            public bool active = false;

            int prevx;
            int prevy;

            public void Paint(int x, int y)
            {
                if (active)
                    canvas.Line(x, y, prevx, prevy, colour, Math.Abs(x-prevx) + Math.Abs(y-prevy) + 1);

                prevx = x;
                prevy = y;
            }
        }

        static void MouseTrack(MouseEvent e, int x, int y, MouseEvent e2)
        {
            if (e == MouseEvent.LButtonDown)
                brush.active = true;

            if (e == MouseEvent.LButtonUp)
                brush.active = false;

            brush.Paint(x, y);
        }

        static Brush brush = new Brush();
        static Mat canvas = new Mat(500, 500, MatType.CV_8UC3);

        static void Main(string[] args)
        {
            Window CVpaint = new Window("CVPaint", canvas);
            CVpaint.OnMouseCallback += new CvMouseCallback(MouseTrack);

            int PressedKey;
            while (true)
            {
                PressedKey = Cv2.WaitKey(10);
                if (PressedKey == 27) break;
                else if (PressedKey == 114) brush.colour = new Scalar(0, 0, 255); 
                else if (PressedKey == 103) brush.colour = new Scalar(0, 255, 0);
                else if (PressedKey == 98) brush.colour = new Scalar(255, 0, 0);
                else if (PressedKey == 99) canvas = new Mat(500, 500, MatType.CV_8UC3);

                CVpaint.ShowImage(canvas);
            } 
            CVpaint.Dispose();
        }
    }
}
