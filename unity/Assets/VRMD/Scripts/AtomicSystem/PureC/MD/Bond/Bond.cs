/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{
    public class Bond
    {
        public Atom Atom1 { get; }
        public Atom Atom2 { get; }

        public string FFSymbol
        {
            get => string.Compare(Atom1.FFSymbol, Atom2.FFSymbol) < 0 ? Atom1.FFSymbol + "-" + Atom2.FFSymbol : Atom2.FFSymbol + "-" + Atom1.FFSymbol;
        }

        private BondInteraction Interaction { get; }
        public Vector GetForce() => this.Interaction.GetForce(this.Atom1.Position, this.Atom2.Position);
        public void GetForce(
            out double Fx1, out double Fy1, out double Fz1,
            out double Fx2, out double Fy2, out double Fz2
        ) => this.Interaction.GetForce(
            this.Atom1.Position.X, this.Atom1.Position.Y, this.Atom1.Position.Z,
            this.Atom2.Position.X, this.Atom2.Position.Y, this.Atom2.Position.Z,
            out Fx1, out Fy1, out Fz1, out Fx2, out Fy2, out Fz2
        );
        public double GetEnergy() => this.Interaction.GetEnergy(this.Atom1.Position, this.Atom2.Position);

        private readonly double k = -1.0;
        public double K => k >= 0 ? k : (BondFFTables.BondConstantTable.TryGetValue(this.FFSymbol, out var k_) ? k_.RK : throw new System.Exception(this.FFSymbol));   // J/m^2

        private readonly double r0 = -1.0;
        public double R0 => r0 >= 0 ? r0 : (BondFFTables.BondConstantTable.TryGetValue(this.FFSymbol, out var r0_) ? r0_.REQ : throw new System.Exception(this.FFSymbol));   // m

        public Bond(Atom atom1, Atom atom2, double k = -1.0, double r0 = -1.0)
        {
            this.Atom1 = atom1;
            this.Atom2 = atom2;
            this.k = k;
            this.r0 = r0;
            this.Interaction = new BondInteraction(this.K, this.R0);
        }

        public static List<Bond> AutomaticCreate(List<Atom> atomList)
        {
            var bondList = new List<Bond>();
            var i = 0;
            foreach (var atom1 in atomList)
            {
                i++;
                var j = 0;
                foreach (var atom2 in atomList)
                {
                    j++;
                    if (i < j)
                    {
                        var dx = atom1.Position.X - atom2.Position.X;
                        var dy = atom1.Position.Y - atom2.Position.Y;
                        var dz = atom1.Position.Z - atom2.Position.Z;
                        var d = System.Math.Sqrt(dx * dx + dy * dy + dz * dz);
                        var d0 = atom1.CovalentRadius + atom2.CovalentRadius;
                        var d1 = atom1.VdwRadius + atom2.VdwRadius;
                        var dThresh = 0.8f * d0 + 0.2f * d1;
                        if (d < dThresh)
                        {
                            var bond = new Bond(atom1, atom2);
                            bondList.Add(bond);
                        }
                    }
                }
            }
            return bondList;
        }

        public bool IsProtonDonor
        {
            get
            {
                if (this.Atom1.Symbol == "H")
                {
                    return this.Atom2.Symbol == "O" || this.Atom2.Symbol == "N";
                }
                else if (this.Atom2.Symbol == "H")
                {
                    return this.Atom1.Symbol == "O" || this.Atom1.Symbol == "N";
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Contains(Atom atom)
        {
            if (this.Atom1 == atom || this.Atom2 == atom) return true;
            else return false;
        }

    }
}

