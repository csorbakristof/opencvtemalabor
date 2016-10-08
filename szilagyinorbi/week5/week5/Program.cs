using OpenCvSharp;
using System;

namespace week5
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            //Beállítások
            const int fps = 25;
            const string output = "ellipse.avi";
            Size resolution = new Size(640,480);

            try
            {

                //Webcamera megnyitása
                VideoCapture vc = new VideoCapture(0);
                if (!vc.IsOpened()) throw new Exception("Could not open the input video!");
                vc.Fps = fps;

                //Kimeneti fájl
                VideoWriter vw = new VideoWriter(output, "MJPG", 25, new Size(640,480));
                if (!vw.IsOpened()) throw new Exception("Invalid file format!");

                Mat frame = new Mat();
                Random r = new Random();

                int key = -1;

                //Amíg nem nyomunk le egy gombot és tudja olvasni a képkockákat
                while (vc.Read(frame) && key == -1)
                {

                    //Random távolságra a középponttól
                    RotatedRect rr = new RotatedRect(new Point2f(vc.FrameWidth / 2 + r.Next(-vc.FrameWidth / 4, vc.FrameWidth / 4), vc.FrameHeight / 2 + r.Next(-vc.FrameHeight / 4, vc.FrameHeight / 4)), new Size2f(30.0, 40.0), 45.0f);

                    //Random színnel
                    frame.Ellipse(rr, new Scalar(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)), -1);

                    //Kép megjelenítése
                    Cv2.ImShow("Video - Press any key to exit", frame);
                    vw.Write(frame);

                    //Várakozás gomblenyomásra
                    key = Cv2.WaitKey((int)(1000.0 / fps));
                }

                vc.Release();
                vw.Release();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }
    }
}