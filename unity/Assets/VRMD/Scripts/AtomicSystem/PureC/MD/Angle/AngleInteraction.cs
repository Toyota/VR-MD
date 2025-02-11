/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System;
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{

    public class AngleInteraction
    {
        public double K { get; }
        public Math.Angle Angle0 { get; }

        public AngleInteraction(double k, Math.Angle angle0)
        {
            this.K = k;
            this.Angle0 = angle0;
        }

        public double GetEnergy(
            double x1, double y1, double z1, double x2, double y2, double z2, double x3, double y3, double z3
        ) => this.GetEnergy(new Vector3(x1, y1, z1), new Vector3(x2, y2, z2), new Vector3(x3, y3, z3));
        public double GetEnergy(Vector x) => this.GetEnergy(
            new Vector3(x[0], x[1], x[2]), new Vector3(x[3], x[4], x[5]), new Vector3(x[6], x[7], x[8])
        );
        public double GetEnergy(Vector3 x1, Vector3 x2, Vector3 x3)
        {
            var v1 = x2 - x1;
            var v2 = x2 - x3;
            var angle = Math.Angle.CreateFromVectors(v1, v2);
            var angleDifference = angle - this.Angle0;
            return 0.5 * this.K * angleDifference.Radian * angleDifference.Radian;
        }

        public Vector GetForceSlow(Vector x) => this.GetForceSlow(
            new Vector3(x[0], x[1], x[2]), new Vector3(x[3], x[4], x[5]), new Vector3(x[6], x[7], x[8])
        );
        public Vector GetForceSlow(Vector3 x1, Vector3 x2, Vector3 x3)
        {
            var v1 = x2 - x1;
            var v2 = x2 - x3;
            var angle = Math.Angle.CreateFromVectors(v1, v2);
            var angleDifference = angle - this.Angle0;
            var s = v1.Magnitude * v2.Magnitude;
            var y = v1 * v2 / s;
            var dydx1 = v1 * y / (v1.Magnitude * v1.Magnitude) - v2 / s;
            var dydx3 = v2 * y / (v2.Magnitude * v2.Magnitude) - v1 / s;
            var dydx2 = -dydx1 - dydx3;
            var c = this.K * angleDifference.Radian / System.Math.Sqrt(1.0 - y * y);
            return c * new Vector(new double[]{
                dydx1[0],dydx1[1],dydx1[2],dydx2[0],dydx2[1],dydx2[2],dydx3[0],dydx3[1],dydx3[2],
            });
        }

        public void GetForce(
            double x1, double y1, double z1, double x2, double y2, double z2, double x3, double y3, double z3,
            out double Fx1, out double Fy1, out double Fz1,
            out double Fx2, out double Fy2, out double Fz2,
            out double Fx3, out double Fy3, out double Fz3
        )
        {
            var vx1 = x2 - x1;
            var vy1 = y2 - y1;
            var vz1 = z2 - z1;
            var vx2 = x2 - x3;
            var vy2 = y2 - y3;
            var vz2 = z2 - z3;
            var innerProduct = vx1 * vx2 + vy1 * vy2 + vz1 * vz2;
            var r1Square = vx1 * vx1 + vy1 * vy1 + vz1 * vz1;
            var r2Square = vx2 * vx2 + vy2 * vy2 + vz2 * vz2;
            var r1 = System.Math.Sqrt(r1Square);
            var r2 = System.Math.Sqrt(r2Square);
            var s = r1 * r2;
            var sInv = 1.0 / s;
            var w = innerProduct * sInv;
            w = w > 1.0 ? 1.0 : w;
            w = w < -1.0 ? -1.0 : w;
            var wR1SquareInv = w / r1Square;
            var wR2SquareInv = w / r2Square;
            var angle = System.Math.Acos(w);
            var angleDifference = angle - this.Angle0.Radian;
            var dwdx1 = vx1 * wR1SquareInv - vx2 * sInv;
            var dwdy1 = vy1 * wR1SquareInv - vy2 * sInv;
            var dwdz1 = vz1 * wR1SquareInv - vz2 * sInv;
            var dwdx3 = vx2 * wR2SquareInv - vx1 * sInv;
            var dwdy3 = vy2 * wR2SquareInv - vy1 * sInv;
            var dwdz3 = vz2 * wR2SquareInv - vz1 * sInv;
            var dwdx2 = -dwdx1 - dwdx3;
            var dwdy2 = -dwdy1 - dwdy3;
            var dwdz2 = -dwdz1 - dwdz3;
            var sin2 = 1.0 - w * w;
            var sinInv = sin2 > 0 ? 1.0/System.Math.Sqrt(sin2) : 1.0;
            var c = this.K * angleDifference * sinInv;
            Fx1 = c * dwdx1;
            Fy1 = c * dwdy1;
            Fz1 = c * dwdz1;
            Fx2 = c * dwdx2;
            Fy2 = c * dwdy2;
            Fz2 = c * dwdz2;
            Fx3 = c * dwdx3;
            Fy3 = c * dwdy3;
            Fz3 = c * dwdz3;
        }

    }

}
