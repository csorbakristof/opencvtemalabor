using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace HF_6
{
    class Program
    {
        private static Mat drawingBoard = new Mat(768, 1024, MatType.CV_8UC3);
        private static Boolean lmbd = false; //LeftMouseButtonDown
        private static Scalar color = new Scalar(255, 0, 0);
        static Point p = new Point();

        static void Main(string[] args)
        {
            int key = 0;
            Window drawingWindow = new Window("Drawing Board");
            drawingWindow.OnMouseCallback += MouseEventHandler;

            while (key != 27)
            {
                drawingWindow.ShowImage(drawingBoard);
                key = Cv2.WaitKey(15);

                switch (key)
                {
                    case 'r':
                        color = Scalar.Red;
                        break;
                    case 'g':
                        color = Scalar.Green;
                        break;
                    case 'b':
                        color = Scalar.Blue;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void MouseEventHandler(MouseEvent @event, int x, int y, MouseEvent flags)
        {
            switch (@event)
            {
                case MouseEvent.MouseMove:
                    drawLine(x, y);
                    break;
                case MouseEvent.LButtonDown:
                    lmbd = true;
                    p = new Point(x, y);
                    break;
                case MouseEvent.LButtonUp:
                    lmbd = false;
                    break;
                default:
                    break;
            }
        }

        private static void drawLine(int x, int y)
        {
            if (lmbd)
            {
                Cv2.Line(drawingBoard, p, new Point(x, y), color, 3);
                p = new Point(x, y);
            }
        }
    }
}
