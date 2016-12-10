using System;
using OpenCvSharp;

namespace temalab
{
    class Program
    {

        static void Main(string[] args)
        {
            //Webcamera megnyitása 30 fps-sel
            /*
            VideoCapture video = new VideoCapture(0);
            video.Fps = 30;
            */

            Console.Write("Video file: ");
            VideoCapture video = new VideoCapture(Console.ReadLine());
            ColorTracker c = new ColorTracker();
            c.Open(video,true);
        }
    }
}

