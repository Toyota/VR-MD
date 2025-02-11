/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System;

namespace VRMD.PureC.Math
{
    public class BoxMuller
    {
        private readonly Random Random;
        private double? Saved;
        public BoxMuller()
        {
            this.Random = new Random();
        }

        public double Get()
        {
            if (this.Saved != null)
            {
                var s = (double)this.Saved;
                this.Saved = null;
                return s;
            }
            var x = this.Random.NextDouble();
            var y = this.Random.NextDouble();
            x = System.Math.Sqrt(-2 * System.Math.Log(x));
            y = 2 * System.Math.PI * y;
            var z1 = x * System.Math.Cos(y);
            var z2 = x * System.Math.Sin(y);
            this.Saved = z1;
            return z2;
        }
    }
}