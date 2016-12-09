using OpenCvSharp;

namespace temalab
{

    public class HSVColor
    {
        private double h, s, v;

        public double H
        {
            get { return h; }
            private set { h = value < 0 ? 0 : h > 180.0 ? 180.0 : value; }
        }
        public double S
        {
            get { return s; }
            private set { s = value < 0 ? 0 : s > 255.0 ? 255.0 : value; }
        }

        public double V
        {
            get { return v; }
            private set { v = value < 0 ? 0 : v > 255.0 ? 255.0 : value; }
        }

        public Scalar LowThreshold => new Scalar((H - 3 < 0 ? 0 : H - 3), (S - Threshold < 0 ? 0 : S - Threshold), (V - Threshold < 0 ? 0 : V - Threshold));
        public Scalar HighThreshold => new Scalar((H + 3 > 180 ? 180 : H + 3), (S + Threshold > 255 ? 255 : S + Threshold), (V + Threshold > 255 ? 255 : V + Threshold));
        public static int Threshold = 20;

        public HSVColor(double h, double s, double v)
        {
            H = h;
            S = s;
            V = v;
        }
    }
}
