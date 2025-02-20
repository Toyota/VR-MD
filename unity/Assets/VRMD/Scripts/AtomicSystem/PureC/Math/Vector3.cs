/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;

namespace VRMD.PureC.Math
{
    public class Vector3 : Vector
    {
        public double X => this[0];
        public double Y => this[1];
        public double Z => this[2];

        public Vector3(double x, double y, double z) : base(new double[] { x, y, z }) { }
        public Vector3(Vector v) : base(new double[] { v[0], v[1], v[2] }) { }

        public static Vector3 operator +(Vector3 v1, Vector3 v2) => new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static Vector3 operator -(Vector3 v1, Vector3 v2) => new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public static Vector3 operator *(double a, Vector3 v) => new Vector3(a * v.X, a * v.Y, a * v.Z);
        public static Vector3 operator /(Vector3 v, double a) => 1.0 / a * v;

        public static Vector3 Zero() => new Vector3(0.0, 0.0, 0.0);

        public static Vector3 Product(Vector3 v1, Vector3 v2) => new Vector3(v1.Y * v2.Z - v2.Y * v1.Z,
                                                                             v1.Z * v2.X - v2.Z * v1.X,
                                                                             v1.X * v2.Y - v2.X * v1.Y);
        public new Vector3 Direction => new Vector3(base.Direction);
        public static Vector3 GetCenterOf(IReadOnlyCollection<Vector3> positions, IReadOnlyCollection<double> weights)
            => new Vector3(Vector.GetCenterOf(positions, weights));
    }
}