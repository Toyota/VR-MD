/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
namespace VRMD.PureC.Math
{
    public class Angle
    {
        public double Radian { get; }
        public double Degree { get => RadianToDegree(Radian); }

        public static Angle CreateFromRadian(double radian) => new Angle(radian);
        public static Angle CreateFromDegree(double degree) => CreateFromRadian(DegreeToRadian(degree));
        public static Angle CreateFromVectors(Vector3 v1, Vector3 v2) => CreateFromRadian(System.Math.Acos(v1 * v2 / (v1.Magnitude * v2.Magnitude)));
        public static Angle CreateFromVectors(Vector3 x1, Vector3 x2, Vector3 x3) => CreateFromVectors(x2 - x1, x2 - x3);


        public static Angle operator -(Angle a) => new Angle(-a.Radian);
        public static Angle operator +(Angle a1, Angle a2) => new Angle(a1.Radian + a2.Radian);
        public static Angle operator -(Angle a1, Angle a2) => new Angle(a1.Radian - a2.Radian);

        private Angle(double radian)
        {
            this.Radian = radian;
        }

        private static double DegreeToRadian(double degree) => System.Math.PI * degree / 180.0;
        private static double RadianToDegree(double radian) => 180.0 * radian / System.Math.PI;

    }
}
