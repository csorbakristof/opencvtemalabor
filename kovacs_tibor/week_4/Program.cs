using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kovacs_tibor
{
    public class Program
    {
        private static readonly Scalar BORDER_COLOR = new Scalar(0.0d, 255.0d, 255.0d);
        private static readonly int BORDER_WIDTH = 3;

        public static void Main(string[] args)
        {
            Mat sourceImage = new Mat(args[0], ImreadModes.Color);
            Rect imageBorder = new Rect(0, 0, sourceImage.Width, sourceImage.Height);

            Cv2.Rectangle(sourceImage, imageBorder, BORDER_COLOR, BORDER_WIDTH);

            Cv2.ImWrite(args[1], sourceImage);
        }

    }
}
