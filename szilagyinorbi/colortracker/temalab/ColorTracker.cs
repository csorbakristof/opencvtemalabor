using System;
using OpenCvSharp;

namespace temalab
{

    public class ColorTracker
    {
        private readonly ColorStack colorStack = new ColorStack();
        private Mat hsv = new Mat();
        private Mat original = new Mat();
        private VideoCapture video;

        private BackgroundSubtractorMOG2 mog;

        //private readonly Window hsvWindow = new Window("HSV");
        private readonly Window originalWindow = new Window("Original");
        private readonly Window maskColorWindow = new Window("Color Mask");
        private readonly Window mog2Window = new Window("Mog2 Mask");
        private readonly Window maskAndWindow = new Window("AND Mask");

        public ColorTracker(VideoCapture video)
        {
            this.video = video;

            //Kattintásra új szín felvétele
            //hsvWindow.OnMouseCallback += GetColor;
            maskColorWindow.OnMouseCallback += GetColor;
            originalWindow.OnMouseCallback += GetColor;

            mog = BackgroundSubtractorMOG2.Create(150);

            //Threshold mértékének változtatása csúszka segítésével
            new CvTrackbar("Threshold", "Original", HSVColor.Threshold, 50, (pos, userdata) => HSVColor.Threshold = pos);
        }

        public void Open(bool loop)
        {
            int keyPressed = -1;
            bool first = true;

            //Amíg nem nyomunk ESC-et és betudja olvasni a következő képkockát
            while (keyPressed != 27 && video.Read(original))
            {

                //Ha szeretnénk az elejéről kezdeni a videót ha vége van
                if (loop && original.Empty()) { video.Set(CaptureProperty.PosFrames, 0); video.Read(original); }
                else if (!loop && original.Empty()) break;

                Track();
                
                //Várakozás az első képkockánál a színek felvételéhez
                if (first) { Cv2.WaitKey(); first = false; }

                //Billenytű lenyomásra várás
                keyPressed = Cv2.WaitKey((int)(1000.0 / video.Fps * 3));
                switch (keyPressed)
                {
                    case 'e': colorStack.Clear(); break;    //E - Összes szín kivétele a maszkból
                    case 'r': colorStack.Pop(); break;      //R - Legutoljára hozzáadott szín kivétele a maszkból
                    case 'p': Cv2.WaitKey(); break;         //P - Várakozás gomblenyomásig
                    default: break;
                }
            }

            //Összes ablak bezárása
            video.Release();
            Cv2.DestroyAllWindows();
        }

        public void Track()
        {
            //RGB átalákítása HSV-re
            Cv2.CvtColor(original, hsv, ColorConversionCodes.BGR2HSV);

            //Kép homályosítása, hogy kevésbe legyen zajos
            Cv2.GaussianBlur(hsv, hsv, new Size(9, 9), 2, 2);

            //Adott színek kimaszkolása
            Mat maskColor = colorStack.GetMat(hsv);
            Mat maskMog2 = new Mat();
            Mat maskAnd = new Mat();

            mog.Apply(hsv, maskMog2);
            
            //Kis foltok eltűntetése a maszkból
            //MorphologyEx Closing = Erode + Dilate
            Mat kernel = new Mat(3, 3, MatType.CV_8UC1, 1.0);
            Point point = new Point(-1, -1);
            Cv2.MorphologyEx(maskColor, maskColor, MorphTypes.Close, kernel, point, 2);

            //maskColor és maskMog2 közös maszkja
            Cv2.BitwiseAnd(maskColor, maskMog2, maskAnd);
            Cv2.MorphologyEx(maskAnd, maskAnd, MorphTypes.Close, kernel, point, 2);

            kernel.Release();

            //Maszkban lévő fehér pixeleket bezáró négyzet rajzolása
            var rect = Cv2.BoundingRect(maskAnd);
            Cv2.Rectangle(original, rect, Scalar.LightGreen);

            //Ablakok megjelenítése
            //Cv2.ImShow(hsvWindow.Name, hsv);
            Cv2.ImShow(maskAndWindow.Name, maskAnd);
            Cv2.ImShow(maskColorWindow.Name, maskColor);
            Cv2.ImShow(mog2Window.Name, maskMog2);
            Cv2.ImShow(originalWindow.Name, original);

            maskAnd.Release();
            maskColor.Release();
            maskMog2.Release();
        }

        //Kattintás esetén az új szín felvétele
        private void GetColor(MouseEvent eventype, int x, int y, MouseEvent flags) {
            if (eventype == MouseEvent.LButtonDown) {
                Vec3b p = hsv.Get<Vec3b>(y, x);
                Console.WriteLine($"H: {p.Item0} S: {p.Item1} V: {p.Item2} added to the stack!");
                colorStack.Add(new HSVColor(p.Item0, p.Item1, p.Item2));
                Track();
            }
        }
    }
}
