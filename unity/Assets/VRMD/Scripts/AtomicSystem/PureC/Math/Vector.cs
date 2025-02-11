/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System;
using System.Collections.Generic;

namespace VRMD.PureC.Math
{
    public class Vector
    {
        public virtual double[] Values { get; }
        public double this[int i] => this.Values[i];
        public int Dimension => this.Values.Length;
        public double Magnitude => System.Math.Sqrt(this * this);
        public Vector Direction => this / this.Magnitude;
        public static Vector operator -(Vector x) => OperateEach(x, (xx) => -xx);
        public static Vector operator +(Vector x, Vector y) => OperateEach(x, y, (xx, yy) => xx + yy);
        public static Vector operator -(Vector x, Vector y) => OperateEach(x, y, (xx, yy) => xx - yy);
        public static Vector operator *(double a, Vector x) => OperateEach(x, (xx) => a * xx);
        public static Vector operator *(Vector x, double a) => OperateEach(x, (xx) => a * xx);

        public static double operator *(Vector x, Vector y) => ElementSum(OperateEach(x, y, (xx, yy) => xx * yy));
        public static Vector operator /(Vector x, double a) => OperateEach(x, (xx) => xx / a);
        public static Vector Zero(int n) => new Vector(new double[n]);

        public static Vector Unify(Vector x, Vector y)
        {
            var n = x.Dimension + y.Dimension;
            var values = new double[n];
            for (var i = 0; i < x.Dimension; i++)
            {
                values[i] = x[i];
            }
            for (var i = 0; i < y.Dimension; i++)
            {
                values[x.Dimension + i] = y[i];
            }
            return new Vector(values);
        }

        public static Vector GetCenterOf(IReadOnlyCollection<Vector> positions, IReadOnlyCollection<double> weights)
        {
            var n = positions.Count;
            var x = new Vector[n];
            var w = new double[n];
            var i = 0;
            foreach (var x_ in positions)
            {
                x[i] = x_;
                i++;
            }
            i = 0;
            foreach (var w_ in weights)
            {
                w[i] = w_;
                i++;
            }
            var results = Vector.Zero(x[0].Dimension);
            for (i = 0; i < n; i++)
            {
                results += w[i] * x[i];
            }
            return results;
        }

        protected Vector() { }
        public Vector(double[] values)
        {
            this.Values = values;
        }

        private static Vector OperateEach(Vector x, Func<double, double> operation)
        {
            var values = new double[x.Dimension];
            for (var i = 0; i < x.Dimension; i++)
            {
                values[i] = operation(x[i]);
            }
            return new Vector(values);
        }

        private static Vector OperateEach(Vector x, Vector y, Func<double, double, double> operation)
        {
            var values = new double[x.Dimension];
            for (var i = 0; i < x.Dimension; i++)
            {
                values[i] = operation(x[i], y[i]);
            }
            return new Vector(values);
        }

        private static double ElementSum(Vector x)
        {
            var s = 0.0;
            for (var i = 0; i < x.Dimension; i++)
            {
                s += x[i];
            }
            return s;
        }

    }
}
