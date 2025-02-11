/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{
    public class ChargeInteraction : PairInteraction
    {
        public static double KCoulomb { get; } = 8.9876e9f;
        public double Q1 { get; }
        public double Q2 { get; }
        private readonly double KQ1Q2;

        public ChargeInteraction(double q1, double q2)
        {
            this.Q1 = q1;
            this.Q2 = q2;
            this.KQ1Q2 = KCoulomb * Q1 * Q2;
        }

        public override double GetEnergy(Vector3 x, Vector3 y)
        {
            var r = x - y;
            var rInv = 1.0 / r.Magnitude;
            return KCoulomb * this.Q1 * this.Q2 * rInv;
        }

        public Vector GetForceSlow(Vector3 x, Vector3 y)
        {
            var r = x - y;
            var rInv = 1.0 / r.Magnitude;
            var FMagnitude = KCoulomb * this.Q1 * this.Q2 * rInv * rInv;
            var F = FMagnitude * r.Direction;
            return new Vector(new double[] { F[0], F[1], F[2], -F[0], -F[1], -F[2] });
        }
        public override Vector GetForce(Vector3 x1, Vector3 x2)
        {
            var x = x1.Values[0] - x2.Values[0];
            var y = x1.Values[1] - x2.Values[1];
            var z = x1.Values[2] - x2.Values[2];
            var rInv = 1.0 / System.Math.Sqrt(x * x + y * y + z * z);
            var FMagnitudeRInv = KQ1Q2 * rInv * rInv * rInv;
            var Fx = FMagnitudeRInv * x;
            var Fy = FMagnitudeRInv * y;
            var Fz = FMagnitudeRInv * z;
            return new Vector(new double[] { Fx, Fy, Fz, -Fx, -Fy, -Fz });
        }
    }
}
