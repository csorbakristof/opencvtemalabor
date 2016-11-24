﻿using System;
using OpenCvSharp;

namespace temalab
{

    public class ColorTracker
    {
        private readonly ColorStack colorStack = new ColorStack();
        private Mat hsv = new Mat();
        private Mat original = new Mat();
        private int threshold = 20;
        private VideoCapture video;

        private readonly Window hsvWindow = new Window("HSV");
        private readonly Window originalWindow = new Window("Original");
        private readonly Window maskWindow = new Window("Mask");

        public ColorTracker(VideoCapture video)
        {

            this.video = video;

            //Kattintásra új szín felvétele
            hsvWindow.OnMouseCallback += GetColor;
            maskWindow.OnMouseCallback += GetColor;
            originalWindow.OnMouseCallback += GetColor;

            //Threshold mértékének változtatása csúszka segítésével
            new CvTrackbar("Threshold", "Original", threshold, 50, ThresholdChange);
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
            Mat mask = colorStack.GetMat(hsv);

            /*
            Cv2.Erode(mask,mask,new Mat(),null,3);
            Cv2.Dilate(mask, mask, new Mat(),null,3);
            */

            //Kis foltok eltűntetése a maszkból
            //MorphologyEx Closing = Erode + Dilate
            Cv2.MorphologyEx(mask, mask, MorphTypes.Close, null, null, 3);

            //Maszkban lévő fehér pixeleket bezáró négyzet rajzolása
            var rect = Cv2.BoundingRect(mask);
            Cv2.Rectangle(original, rect, Scalar.LightGreen);

            //Ablakok megjelenítése
            Cv2.ImShow(hsvWindow.Name, hsv);
            Cv2.ImShow(maskWindow.Name, mask);
            Cv2.ImShow(originalWindow.Name, original);
        }

        //Threshold csúszka értékének változtatása
        private void ThresholdChange(int pos, object userdata)
        {
            threshold = pos;

            //Összes szín threshold értékének változtatása
            colorStack.ChangeThreshold(pos);
        }

        //Kattintás esetén az új szín felvétele a HSV ablakból
        private void GetColor(MouseEvent eventype, int x, int y, MouseEvent flags) {
            if (eventype == MouseEvent.LButtonDown) {
                Vec3b p = hsv.Get<Vec3b>(y, x);
                Console.WriteLine($"H: {p.Item0} S: {p.Item1} V: {p.Item2} added to the stack!");
                colorStack.Add(new HSVColor(p.Item0, p.Item1, p.Item2, threshold));
                Track();
            }
        }
    }
}