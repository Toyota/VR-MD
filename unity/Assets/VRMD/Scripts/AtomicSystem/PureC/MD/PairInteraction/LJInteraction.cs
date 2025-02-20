/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{
    public class LJInteraction : PairInteraction
    {
        public double Epsilon { get; }
        public double Sigma { get; }

        private readonly double Epsilon24;
        private readonly double Sigma2;

        public LJInteraction(double epsilon, double sigma)
        {
            this.Epsilon = epsilon;
            this.Sigma = sigma;
            this.Epsilon24 = 24.0 * epsilon;
            this.Sigma2 = sigma * sigma;
        }

        public override double GetEnergy(Vector3 x, Vector3 y)
        {
            var r = x - y;
            var sInv = this.Sigma / r.Magnitude;
            return 4.0 * this.Epsilon * (System.Math.Pow(sInv, 12) - System.Math.Pow(sInv, 6));
        }

        public Vector GetForceSlow(Vector3 x, Vector3 y)
        {
            var r = x - y;
            var sInv = this.Sigma / r.Magnitude;
            var FMagnitude = 4.0 * this.Epsilon / r.Magnitude
                * (12.0 * System.Math.Pow(sInv, 12) - 6.0 * System.Math.Pow(sInv, 6));
            var F1 = FMagnitude * r.Direction;
            return new Vector(new double[] { F1[0], F1[1], F1[2], -F1[0], -F1[1], -F1[2] });
        }
        public override Vector GetForce(Vector3 x1, Vector3 x2)
        {
            var x = x1.Values[0] - x2.Values[0];
            var y = x1.Values[1] - x2.Values[1];
            var z = x1.Values[2] - x2.Values[2];
            var rInv2 = 1.0 / (x * x + y * y + z * z);
            var sInv2 = this.Sigma2 * rInv2;
            var sInv6 = sInv2 * sInv2 * sInv2;
            var FMagnitudeRInv = this.Epsilon24 * rInv2 * sInv6 * (2.0 * sInv6 - 1.0);
            var F1 = FMagnitudeRInv * x;
            var F2 = FMagnitudeRInv * y;
            var F3 = FMagnitudeRInv * z;
            return new Vector(new double[] { F1, F2, F3, -F1, -F2, -F3 });
        }
    }
}
