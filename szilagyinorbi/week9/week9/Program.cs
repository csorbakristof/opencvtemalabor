using System;
using OpenCvSharp;

namespace week9
{
    class Program
    {

        private static Mat drawable = new Mat(600, 600, MatType.CV_8UC3);
        private static bool mousepressed = false;
        private static Scalar color = new Scalar(255.0, 255.0, 255.0);
        private static Point prev;

        static void Main(string[] args)
        {
            Window window = new Window("Drawable Window");
            window.OnMouseCallback += MouseEventHandler;

            int key;
            while ((key = Cv2.WaitKey(30)) != 27)
            {
                window.ShowImage(drawable);
                switch (key)
                {
                    case 'r': color = new Scalar(0.0d, 0.0d, 255.0d); break;
                    case 'g': color = new Scalar(0.0d, 255.0d, 0.0d); break;
                    case 'b': color = new Scalar(255.0d, 0.0d, 0.0d); break;
                }
            }
            Cv2.DestroyAllWindows();
        }

        private static void MouseEventHandler(MouseEvent eventType, int x, int y, MouseEvent flags)
        {
            switch (eventType)
            {
                case MouseEvent.LButtonDown:
                    mousepressed = true;
                    prev = new Point(x,y);
                    break;
                case MouseEvent.LButtonUp:
                    mousepressed = false;
                    break;
                case MouseEvent.MouseMove:
                    if (mousepressed) {
                        drawable.Line(prev.X, prev.Y, x, y, color);
                        prev = new Point(x, y);
                    }
                    break;
            }
        }
    }
}
