using OpenCvSharp;

namespace week9_drawing
{
    public class Program
    {
        private const string WINDOW = "Simple Paint";
        private const int WINDOW_WIDTH = 640;
        private const int WINDOW_HEIGHT = 480;

        private const int R_CHARACTER_KEY = 114;
        private const int G_CHARACTER_KEY = 103;
        private const int B_CHARACTER_KEY = 98;
        private const int ESCAPE = 27;

        private static readonly Scalar RED_COLOR = new Scalar(0.0d, 0.0d, 255.0d);
        private static readonly Scalar GREEN_COLOR = new Scalar(0.0d, 255.0d, 0.0d);
        private static readonly Scalar BLUE_COLOR = new Scalar(255.0d, 0.0d, 0.0d);

        private static Mat canvas = new Mat(new Size(WINDOW_WIDTH, WINDOW_HEIGHT), MatType.CV_8UC3);
        private static bool isInDraw;
        private static Scalar drawingColor = RED_COLOR;
        private static Point previousPoint;

        public static void Main(string[] args)
        {
            Window window = new Window(WINDOW);
            window.OnMouseCallback += MouseEventHandler;

            int pressedCharacter = -1;
            while (pressedCharacter != ESCAPE)
            {
                window.ShowImage(canvas);
                pressedCharacter = Cv2.WaitKey(1);
                KeyEventListener(pressedCharacter);
            }

            window.Dispose();
        }

        private static void KeyEventListener(int pressedChar)
        {
            switch (pressedChar)
            {
                case R_CHARACTER_KEY:
                    OnRKeyPress();
                    break;
                case G_CHARACTER_KEY:
                    OnGKeyPress();
                    break;
                case B_CHARACTER_KEY:
                    OnBKeyPress();
                    break;
            }
        }

        private static void OnRKeyPress()
        {
            drawingColor = RED_COLOR;
        }

        private static void OnGKeyPress()
        {
            drawingColor = GREEN_COLOR;
        }

        private static void OnBKeyPress()
        {
            drawingColor = BLUE_COLOR;
        }

        private static void MouseEventHandler(MouseEvent eventType, int x, int y, MouseEvent flags)
        {
            Point eventPosition = new Point(x, y);
            switch (eventType)
            {
                case MouseEvent.LButtonDown:
                    OnLeftMouseButtonDown(eventPosition);
                    break;
                case MouseEvent.LButtonUp:
                    OnLeftMouseButtonUp(eventPosition);
                    break;
                case MouseEvent.MouseMove:
                    OnMouseMove(eventPosition);
                    break;
                case MouseEvent.RButtonDown:
                    OnRightMouseButtonDown(eventPosition);
                    break; 
            }
        }

        private static void OnLeftMouseButtonDown(Point eventPosition)
        {
            isInDraw = true;
            previousPoint = eventPosition;
        }

        private static void OnLeftMouseButtonUp(Point eventPosition)
        {
            isInDraw = false;
        }

        private static void OnMouseMove(Point eventPosition)
        {
            if (isInDraw)
            {
                canvas.Line(previousPoint, eventPosition, drawingColor);
                previousPoint = eventPosition;
            }
        }

        private static void OnRightMouseButtonDown(Point eventPosition)
        {
            canvas.FloodFill(eventPosition, drawingColor);
        }
    }
}
