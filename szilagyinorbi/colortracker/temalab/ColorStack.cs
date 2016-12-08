using System;
using System.Collections;
using System.Collections.Generic;
using OpenCvSharp;

namespace temalab
{

    class ColorStack
    {

        private List<HSVColor> list = new List<HSVColor>();

        public void Add(HSVColor c) {
            list.Add(c);
        }

        public void Pop() {
            if (list.Count > 0)
                list.RemoveAt(list.Count - 1);
        }

        public void Clear()
        {
            list.Clear();
        }

        public void ChangeThreshold(int threshold)
        {
            foreach (var c in list)
                c.Threshold = threshold;
        }

        public Mat GetMat(Mat m)
        {
            if (list.Count == 0)
                return new Mat(m.Size(),MatType.CV_8UC1,0);

            Mat mask = new Mat();
            Cv2.InRange(m, list[0].LowThreshold, list[0].HighThreshold, mask);

            if (list.Count > 1) {
                Mat range = new Mat();
                for (int i = 1; i < list.Count; i++)
                {
                    Cv2.InRange(m, list[i].LowThreshold, list[i].HighThreshold, range);
                    Cv2.BitwiseOr(mask, range, mask);
                }
            }
            return mask;
        } 
    }
}
