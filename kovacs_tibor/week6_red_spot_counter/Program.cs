using OpenCvSharp;
using System;

namespace week6_red_spot_counter
{
    public class Program
    {
        private static readonly string WINDOW_CAPTION = "Red_Spot_Counter";
        private static readonly int WAIT_TIME_MILLISECOND = 750;

        private static readonly double GENERATED_COLOR_CHANNEL_MAX = 254.0d;
        private static readonly Random RANDOM_GENERATOR = new Random();

        private static readonly Scalar BLACK = new Scalar(0.0d, 0.0d, 0.0d);

        public static void Main(string[] args)
        {
            // Read the necessary parameters
            Console.Write("Enter the path to the image file: ");
            string sourceFilePath = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Define the RGB bounds of the red color:");

            Console.Write("\t");
            Console.Write("Enter the lower bound of the R component [HINT: 200.0 < x < 255.0]: ");
            double r_component_lower_bound = Double.Parse(Console.ReadLine());

            Console.Write("\t");
            Console.Write("Enter the upper bound of the G and B component [HINT: 0.0 < x < 55.0]: ");
            double gr_component_upper_bound = Double.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.Write("Enter the number of dilate rounds to perform [HINT 1 < x < 10]: ");
            int dilateRounds = Int32.Parse(Console.ReadLine());

            // Construct the bounds for the color detection
            Scalar RED_LOWER_BOUNDS = new Scalar(0.0d, 0.0d, r_component_lower_bound);
            Scalar RED_UPPER_BOUNDS = new Scalar(gr_component_upper_bound, gr_component_upper_bound, 255.0d);

            // Open a window in which the user can see the process
            Cv2.NamedWindow(WINDOW_CAPTION);

            // Load and show the original image
            Mat originalImage = new Mat(sourceFilePath, ImreadModes.Color);

            Cv2.ImShow(WINDOW_CAPTION, originalImage);
            Cv2.WaitKey(WAIT_TIME_MILLISECOND);

            // Show the image containing only the red components of the source image 
            Mat redComponentOnlyImage = new Mat();
            Cv2.InRange(originalImage, RED_LOWER_BOUNDS, RED_UPPER_BOUNDS, redComponentOnlyImage);

            Cv2.ImShow(WINDOW_CAPTION, redComponentOnlyImage);
            Cv2.WaitKey(WAIT_TIME_MILLISECOND);

            // Dilate the red component only image and show after every dilate round
            Mat dilatedImage = new Mat();
            Cv2.Dilate(redComponentOnlyImage, dilatedImage, new Mat());

            Cv2.ImShow(WINDOW_CAPTION, dilatedImage);
            Cv2.WaitKey(WAIT_TIME_MILLISECOND / 2);

            for (int i = 1; i < dilateRounds; ++i)
            {
                Cv2.Dilate(dilatedImage, dilatedImage, new Mat());

                Cv2.ImShow(WINDOW_CAPTION, dilatedImage);
                Cv2.WaitKey(WAIT_TIME_MILLISECOND / 2);
            }

            // Count red spots and show the process after each spot detection
            int redSpotCount = 0;
            for (int x = 0; x < dilatedImage.Width; ++x)
            {
                for (int y = 0; y < dilatedImage.Height; ++y)
                {
                    Vec3b pixelColor = dilatedImage.At<Vec3b>(y, x);
                    if (isWhite(pixelColor))
                    {
                        ++redSpotCount;
                        dilatedImage.FloodFill(new Point(x, y), BLACK);

                        // Show the modified dilated image
                        Cv2.ImShow(WINDOW_CAPTION, dilatedImage);
                        Cv2.WaitKey(WAIT_TIME_MILLISECOND / 2);
                    }
                }
            }
            Cv2.DestroyWindow(WINDOW_CAPTION);

            Console.WriteLine();
            Console.WriteLine("The number of the red spots: " + redSpotCount);
            Console.ReadKey();
        }

        private static Scalar getRandomColor()
        {
            return new Scalar(
                GENERATED_COLOR_CHANNEL_MAX * RANDOM_GENERATOR.NextDouble(),
                GENERATED_COLOR_CHANNEL_MAX * RANDOM_GENERATOR.NextDouble(),
                GENERATED_COLOR_CHANNEL_MAX * RANDOM_GENERATOR.NextDouble()
            );
        }

        private static bool isWhite(Vec3b color)
        {
            return color.Item0 == 255 && color.Item1 == 255 && color.Item2 == 255;
        }

    }

}
