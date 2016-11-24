using OpenCvSharp;
using System;
using System.Numerics;

namespace week10_mandelbrot_set
{
    public class Program
    {
        private const int KEY_ESCAPE = 27;
        private const int KEY_UP_DIRECTION = 119;
        private const int KEY_RIGHT_DIRECTION = 100;
        private const int KEY_DOWN_DIRECTION = 115;
        private const int KEY_LEFT_DIRECTION = 97;
        private const int KEY_ZOOM_IN = 101;
        private const int KEY_ZOOM_OUT = 113;
        private const int KEY_ITERATION_INCREMENT = 114;
        private const int KEY_ITERATION_DECREMENT = 102;

        private const string WINDOW_NAME = "Mandelbrot-set Calculator";
        private const int WINDOW_WIDTH = 640;
        private const int WINDOW_HEIGHT = 480;

        private const double MAGNITUDE_THRESHOLD = 4.0d;
        private static readonly Scalar DEFAULT_COLOR = new Scalar(0.0d, 0.0d, 0.0d);

        private const double DELTA_Y_FACTOR = 1.0d / 25.0d;
        private const double DELTA_X_FACTOR = ((double)WINDOW_WIDTH / WINDOW_HEIGHT) * DELTA_Y_FACTOR;
        private const float ZOOM_IN_FACTOR = 0.85f;
        private const float ZOOM_OUT_FACTOR = 1.0f / ZOOM_IN_FACTOR;

        private static Mat canvas = new Mat(new Size(WINDOW_WIDTH, WINDOW_HEIGHT), MatType.CV_8UC3);
        private static Window mainWindow = new Window(WINDOW_NAME, WindowMode.AutoSize, canvas);
        private static Point2d modelSpaceOrigo = new Point2d(0.0d, 0.0d);
        private static Size2f modelSpaceSize = new Size2f(8.0d, 6.0d);
        private static int maxIterationCount = 25;

        public static void Main(string[] args)
        {
            int typedCharacter = 0;
            while (typedCharacter != KEY_ESCAPE)
            {
                RenderMandelbrotSetToCanvas();
                mainWindow.ShowImage(canvas);
                typedCharacter = OnKeyEventHandler(Cv2.WaitKey(0));
            }
        }

        private static void RenderMandelbrotSetToCanvas()
        {
            for (int y = 0; y < WINDOW_HEIGHT; ++y)
            {
                for (int x = 0; x < WINDOW_WIDTH; ++x)
                {
                    Point screenCoordinate = new Point(x, y);
                    DrawPoint(screenCoordinate, GetColorOfMandelbrotSetAtPoint(GetModelCoordinate(screenCoordinate)));
                }
            }
        }

        private static void DrawPoint(Point screenCoordinate, Scalar color)
        {
            canvas.Line(screenCoordinate, screenCoordinate, color);
        }

        private static Scalar GetColorOfMandelbrotSetAtPoint(Point2d modelCoordinate)
        {
            Complex z = new Complex(0.0d, 0.0d);
            Complex c = new Complex(modelCoordinate.X, modelCoordinate.Y);
            bool converges = true;
            int iterationCount = 0;

            for (int i = 0; i < maxIterationCount; ++i)
            {
                z = Complex.Add(Complex.Pow(z, 2.0d), c);
                if (z.Magnitude > MAGNITUDE_THRESHOLD)
                {
                    converges = false;
                    iterationCount = i;
                    break;
                }
            }

            return converges ? DEFAULT_COLOR : 
                new Scalar((iterationCount * 31) % 256, (iterationCount * 47) % 256, (iterationCount * 37) % 256);
        }

        private static Point2d GetModelCoordinate(Point screenCoordinate)
        {
            return new Point2d(
                modelSpaceOrigo.X - modelSpaceSize.Width / 2.0f + ((double)screenCoordinate.X / WINDOW_WIDTH) * modelSpaceSize.Width,
                modelSpaceOrigo.Y - modelSpaceSize.Height / 2.0f + ((double)screenCoordinate.Y / WINDOW_HEIGHT) * modelSpaceSize.Height
            );
        }

        private static int OnKeyEventHandler(int eventKeyCode)
        {
            switch (eventKeyCode)
            {
                case KEY_UP_DIRECTION:
                    modelSpaceOrigo.Y -= modelSpaceSize.Height * DELTA_Y_FACTOR;
                    break;
                case KEY_RIGHT_DIRECTION:
                    modelSpaceOrigo.X += modelSpaceSize.Width * DELTA_X_FACTOR;
                    break;
                case KEY_DOWN_DIRECTION:
                    modelSpaceOrigo.Y += modelSpaceSize.Height * DELTA_Y_FACTOR;
                    break;
                case KEY_LEFT_DIRECTION:
                    modelSpaceOrigo.X -= modelSpaceSize.Width * DELTA_X_FACTOR;
                    break;
                case KEY_ZOOM_IN:
                    modelSpaceSize.Width *= ZOOM_IN_FACTOR;
                    modelSpaceSize.Height *= ZOOM_IN_FACTOR;
                    break;
                case KEY_ZOOM_OUT:
                    modelSpaceSize.Width *= ZOOM_OUT_FACTOR;
                    modelSpaceSize.Height *= ZOOM_OUT_FACTOR;
                    break;
                case KEY_ITERATION_INCREMENT:
                    ++maxIterationCount;
                    break;
                case KEY_ITERATION_DECREMENT:
                    --maxIterationCount;
                    break; 
            }
            return eventKeyCode;
        }
    }
}
