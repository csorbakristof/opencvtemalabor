using OpenCvSharp;

namespace temalab
{

    public class HSVColor
    {
        private double h, s, v;

        public double H
        {
            get { return h; }
            private set { h = value < 0 ? 0 : h > 180 ? 180 : value; }
        }
        public double S
        {
            get { return s; }
            private set { s = value < 0 ? 0 : s > 255 ? 255 : value; }
        }

        public double V
        {
            get { return v; }
            private set { v = value < 0 ? 0 : v > 255 ? 255 : value; }
        }

        public Scalar LowThreshold { get; private set; }
        public Scalar HighThreshold { get; private set; }

        private int threshold;
        public int Threshold
        {
            get { return threshold; }
            set
            {
                threshold = value;
                LowThreshold = new Scalar(
                    (H - 5 < 0 ? 0 : H - 5),
                    (S - value < 0 ? 0 : S - value),
                    (V - value < 0 ? 0 : V - value));
                HighThreshold = new Scalar(
                    (H + 5 > 180 ? 180 : H + 5),
                    (S + value > 255 ? 255 : S + value),
                    (V + value > 255 ? 255 : V + value));
            }
        }

        public HSVColor(double h, double s, double v, int th = 25)
        {
            H = h;
            S = s;
            V = v;
            Threshold = th;
        }
    }
}
