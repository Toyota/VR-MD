/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
namespace VRMD.PureC.MD
{

    public class WaterSampleTopology : EditableTopology
    {
        public WaterSampleTopology()
        {
            var r = 1.0e-10f;
            var x = -r * (float)System.Math.Cos(Math.Angle.CreateFromDegree(113.0).Radian);
            var y = r * (float)System.Math.Sin(Math.Angle.CreateFromDegree(113.0).Radian);
            var d = 3.2e-10f;
            var nx = 2;
            for (var i = -1; i < nx; i++)
            {
                for (var j = -1; j < nx; j++)
                {
                    for (var k = -1; k < nx; k++)
                    {
                        var atom1 = new Atom("", "H", "hw", i * d, j * d, k * d);
                        var atom2 = new Atom("", "O", "ow", i * d + r, j * d, k * d);
                        var atom3 = new Atom("", "H", "hw", i * d + r + x, j * d + y, k * d);
                        this.AtomList.Add(atom1);
                        this.AtomList.Add(atom2);
                        this.AtomList.Add(atom3);
                        var kBond = 1059.162 * 4184 / 6.02214e23 * 1e20;    // J/m^2
                        var r0 = 1.012e-10;    // m
                        this.BondList.Add(new Bond(atom1, atom2, kBond, r0));
                        this.BondList.Add(new Bond(atom2, atom3, kBond, r0));
                        var kAngle = 75.90 * 4184 / 6.02214e23;    // J/rad^2
                        var theta0 = 113.24;    // degree
                        this.AngleList.Add(new Angle(atom1, atom2, atom3, kAngle, theta0));
                    }
                }
            }
            this.NonbondList.AddRange(Nonbond.AutomaticCreate(this.AtomList, this.BondList, this.AngleList));
        }
    }
}
