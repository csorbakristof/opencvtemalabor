using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace week5_draw_on_video_frames
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read the path of the source video file
            Console.Write("Enter the path of the source video file: ");
            string sourceFile = Console.ReadLine();
            
            // Read the path of the target video file
            Console.Write("Enter the path of the target file: ");
            string targetFile = Console.ReadLine();
             
            // Open source video file for reading
            VideoCapture videoReader = new VideoCapture(sourceFile);

            // Create and open target video file for writing
            VideoWriter videoWriter = new VideoWriter(targetFile, videoReader.FourCC, videoReader.Fps, new Size(videoReader.FrameWidth, videoReader.FrameHeight));

            // Calculate the number of milliseconds to wait between displaying the
            // frames of the read video 
            int delay = (int)( 1000.0d / videoReader.Fps);

            // Read the source video frame-by-frame
            for (int i = 0; i < videoReader.FrameCount; ++i)
            {
                // Create a new Mat for the frame
                Mat videoFrame = new Mat();
                // Read the next frame of the source video
                videoReader.Read(videoFrame);

                // Draw a yellow ellipse to the center of the image
                videoFrame.Ellipse(new RotatedRect(new Point2f(videoReader.FrameWidth / 2, videoReader.FrameHeight / 2), new Size2f(10.0d, 30.0d), 20.0f), new Scalar(0.0d, 255.0d, 255.0d), -1);

                // Show the current frame in a window
                Cv2.ImShow("Video", videoFrame);
                // Wait to achive the proper FPS at display
                Cv2.WaitKey(delay);

                // Write the modified frame to the target video file
                videoWriter.Write(videoFrame);
            }

            // Close the video files
            videoReader.Release();
            videoWriter.Release();
            // Close the opened window
            Cv2.DestroyAllWindows();
        }
    }
}
