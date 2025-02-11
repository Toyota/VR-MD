/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{
    public class LJChargeInteraction : PairInteraction
    {
        public double Epsilon { get; }
        public double Sigma { get; }
        private readonly double Epsilon48;
        private readonly double Sigma2;
        public static double KCoulomb { get; } = 8.9876e9f;
        public double Q1 { get; }
        public double Q2 { get; }
        private readonly double KQ1Q2;

        public LJChargeInteraction(double epsilon, double sigma, double q1, double q2)
        {
            this.Epsilon = epsilon;
            this.Sigma = sigma;
            this.Epsilon48 = 48.0 * epsilon;
            this.Sigma2 = sigma * sigma;
            this.Q1 = q1;
            this.Q2 = q2;
            this.KQ1Q2 = KCoulomb * Q1 * Q2;
        }

        public override double GetEnergy(Vector3 x, Vector3 y)
        {
            var r = x - y;
            var rInv = 1.0 / r.Magnitude;
            var sInv = this.Sigma * rInv;
            return 4.0 * this.Epsilon * (System.Math.Pow(sInv, 12) - System.Math.Pow(sInv, 6)) + KCoulomb * this.Q1 * this.Q2 * rInv;
        }

        public override Vector GetForce(Vector3 x1, Vector3 x2) => new Vector(this.GetForce(x1.Values, x2.Values));
        public double[] GetForce(double[] x1, double[] x2)
        {
            var x = x1[0] - x2[0];
            var y = x1[1] - x2[1];
            var z = x1[2] - x2[2];
            var rInv2 = 1.0 / (x * x + y * y + z * z);
            var rInv = System.Math.Sqrt(rInv2);
            var sInv2 = this.Sigma2 * rInv2;
            var sInv6 = sInv2 * sInv2 * sInv2;
            var FMagnitudeRInv = rInv2 * (this.Epsilon48 * sInv6 * (sInv6 - 0.5) + this.KQ1Q2 * rInv);
            var F1 = FMagnitudeRInv * x;
            var F2 = FMagnitudeRInv * y;
            var F3 = FMagnitudeRInv * z;
            return new double[] { F1, F2, F3, -F1, -F2, -F3 };
        }
        public void GetForce(
            double[] x1, double[] x2,
            out double F1x, out double F1y, out double F1z,
            out double F2x, out double F2y, out double F2z
        )
        {
            var x = x1[0] - x2[0];
            var y = x1[1] - x2[1];
            var z = x1[2] - x2[2];
            var rInv2 = 1.0 / (x * x + y * y + z * z);
            var rInv = System.Math.Sqrt(rInv2);
            var sInv2 = this.Sigma2 * rInv2;
            var sInv6 = sInv2 * sInv2 * sInv2;
            var FMagnitudeRInv = rInv2 * (this.Epsilon48 * sInv6 * (sInv6 - 0.5) + this.KQ1Q2 * rInv);
            F1x = FMagnitudeRInv * x;
            F1y = FMagnitudeRInv * y;
            F1z = FMagnitudeRInv * z;
            F2x = -F1x;
            F2y = -F1y;
            F2z = -F1z;
        }

    }
}
