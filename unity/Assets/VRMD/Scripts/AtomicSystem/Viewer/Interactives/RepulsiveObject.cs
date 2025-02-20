/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System;
using VRMD.PureC.MD;

namespace VRMD.MolecularViewer
{
    public class RepulsiveObject : InteractiveObjectBase
    {
        private VRMD.PureC.Math.Vector3 xOld;
        public override void Initialize(Topology topology, UnityUnit unityUnit)
        {
            base.Initialize(topology,unityUnit);
            this.xOld = UnityUnitDictionary[topology].ConvertToAtomicPosition(this.transform.position);
        }
        public override void CalculateForce(Topology topology)
        {
            var unityUnit = UnityUnitDictionary[topology];
            var xObject = unityUnit.ConvertToAtomicPosition(this.transform.position);
            var objectSize = unityUnit.ConvertToAtomicLength(this.transform.lossyScale.x);
            var s = (xObject - this.xOld).Magnitude;
            var kMin = 3e-8;
            var kMax = 1e-5;
            var sMax = 0.005f;
            var u = s / sMax;
            var k = (kMax - kMin) * (s > sMax ? 1.0 : -4 * u * u * u * u * u + 5 * u * u * u * u) + kMin;
            this.xOld = xObject;
            var x1 = xObject.Values[0];
            var y1 = xObject.Values[1];
            var z1 = xObject.Values[2];
            foreach (var atom in topology.AtomList)
            {
                var atomPosition = atom.Position;
                var x2 = atomPosition.Values[0];
                var y2 = atomPosition.Values[1];
                var z2 = atomPosition.Values[2];
                var x = x1 - x2;
                var y = y1 - y2;
                var z = z1 - z2;
                var r0 = 0.5 * objectSize + atom.CovalentRadius;
                var rMagnitude = Math.Sqrt(x * x + y * y + z * z);
                var d = rMagnitude / r0;
                var withinThreshold = rMagnitude < r0;
                var FDividedByR = -k * (2 * d * d * d - 3 * d * d + 1) / rMagnitude;
                var Fx = withinThreshold ? FDividedByR * x : 0.0;
                var Fy = withinThreshold ? FDividedByR * y : 0.0;
                var Fz = withinThreshold ? FDividedByR * z : 0.0;
                atom.AddExternalForce(Fx, Fy, Fz);
            }
        }
    }
}
