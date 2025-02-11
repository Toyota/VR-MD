/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System;
using VRMD.MolecularViewer;
using VRMD.PureC.MD;
using UnityEngine;

[RequireComponent(typeof(Midpoint))]
public class AttractiveMidpoint : InteractiveObjectBase
{
    private Transform Reference1;
    private Transform Reference2;

    public override void Initialize(Topology topology,UnityUnit unityUnit)
    {
        base.Initialize(topology,unityUnit);
        var midpoint = this.GetComponent<Midpoint>();
        this.Reference1 = midpoint.Reference1;
        this.Reference2 = midpoint.Reference2;
    }

    public override void CalculateForce(Topology topology)
    {
        var unityUnit = UnityUnitDictionary[topology];
        var x1 = unityUnit.ConvertToAtomicPosition(this.Reference1.position).Values;
        var x2 = unityUnit.ConvertToAtomicPosition(this.Reference2.position).Values;
        var x = unityUnit.ConvertToAtomicPosition(this.transform.position).Values;
        var objectSize1 = unityUnit.ConvertToAtomicLength(this.Reference1.lossyScale.x);
        var objectSize2 = unityUnit.ConvertToAtomicLength(this.Reference2.lossyScale.x);
        var r0_ = 0.25*(objectSize1+objectSize2);
        var k = 2e-7;
        var gamma = 0.8e-11;
        foreach (var atom in topology.NonHydrogenAtomList)
        {
            var y = atom.Position.Values;
            var v = atom.Velocity.Values;
            var rx = x[0] - y[0];
            var ry = x[1] - y[1];
            var rz = x[2] - y[2];
            var r1x = x1[0] - y[0];
            var r1y = x1[1] - y[1];
            var r1z = x1[2] - y[2];
            var r2x = x2[0] - y[0];
            var r2y = x2[1] - y[1];
            var r2z = x2[2] - y[2];
            var rSquare = rx * rx + ry * ry + rz * rz;
            var r = Math.Sqrt(rSquare);
            var r0 = atom.CovalentRadius+r0_;
            var s0 = atom.CovalentRadius+2*r0_;
            var rr0Inv = 1.0 / (r0 * r);
            var d = rSquare * rr0Inv;
            var rInv = r0 * rr0Inv;
            var ex = rx * rInv;
            var ey = ry * rInv;
            var ez = rz * rInv;
            var r1Square = r1x * r1x + r1y * r1y + r1z * r1z;
            var r2Square = r2x * r2x + r2y * r2y + r2z * r2z;
            var isAttractive = r < r0 && r1Square < s0 * s0 && r2Square < s0 * s0;
            var coeff = isAttractive ? k * (2*d*d*d - 3*d*d +1) : 0.0;
            var gamma_ = isAttractive ? gamma : 0.0;
            var Fx0 = coeff * ex;
            var Fy0 = coeff * ey;
            var Fz0 = coeff * ez;
            var Fx = Fx0 - gamma_ * v[0];
            var Fy = Fy0 - gamma_ * v[1];
            var Fz = Fz0 - gamma_ * v[2];
            atom.AddExternalForce(Fx, Fy, Fz);
        }
    }
}
