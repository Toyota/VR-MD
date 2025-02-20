/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{
    public class BondInteraction : PairInteraction
    {
        private double K { get; }
        private double R0 { get; }

        public BondInteraction(double k, double r0)
        {
            this.K = k;
            this.R0 = r0;
        }

        public override double GetEnergy(Vector3 x1, Vector3 x2)
        {
            var dx = x1 - x2;
            var dr = dx.Magnitude - this.R0;
            return 0.5 * this.K * dr * dr;
        }

        public override Vector GetForce(Vector3 x1, Vector3 x2)
        {
            var dx = x1 - x2;
            var r = dx.Magnitude;
            var dr = r - this.R0;
            var F1 = -this.K * dr * dx / r;
            var F2 = -F1;
            return Vector.Unify(F1, F2);
        }
        public void GetForce(
            double x1, double y1, double z1,
            double x2, double y2, double z2,
            out double Fx1, out double Fy1, out double Fz1,
            out double Fx2, out double Fy2, out double Fz2
        )
        {
            var dx = x1 - x2;
            var dy = y1 - y2;
            var dz = z1 - z2;
            var r = System.Math.Sqrt(dx * dx + dy * dy + dz * dz);
            var c = -this.K * (r - this.R0) / r;
            Fx1 = c * dx;
            Fy1 = c * dy;
            Fz1 = c * dz;
            Fx2 = -Fx1;
            Fy2 = -Fy1;
            Fz2 = -Fz1;
        }

    }
}
