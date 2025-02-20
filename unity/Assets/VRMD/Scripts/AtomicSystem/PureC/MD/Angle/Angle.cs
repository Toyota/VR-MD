/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{
    public class Angle
    {
        public Atom Atom1 { get; }
        public Atom Atom2 { get; }
        public Atom Atom3 { get; }

        public string FFSymbol
        {
            get => string.Compare(Atom1.FFSymbol, Atom3.FFSymbol) < 0 ?
                Atom1.FFSymbol + "-" + Atom2.FFSymbol + "-" + Atom3.FFSymbol : Atom3.FFSymbol + "-" + Atom2.FFSymbol + "-" + Atom1.FFSymbol;
        }

        private AngleInteraction Interaction { get; }
        public void GetForce(
            out double Fx1, out double Fy1, out double Fz1,
            out double Fx2, out double Fy2, out double Fz2,
            out double Fx3, out double Fy3, out double Fz3
        ) => this.Interaction.GetForce(
            this.Atom1.Position.X, this.Atom1.Position.Y, this.Atom1.Position.Z,
            this.Atom2.Position.X, this.Atom2.Position.Y, this.Atom2.Position.Z,
            this.Atom3.Position.X, this.Atom3.Position.Y, this.Atom3.Position.Z,
            out Fx1, out Fy1, out Fz1,
            out Fx2, out Fy2, out Fz2,
            out Fx3, out Fy3, out Fz3
        );
        public double GetEnergy() => this.Interaction.GetEnergy(this.Atom1.Position, this.Atom2.Position, this.Atom3.Position);

        private readonly double k = -1.0;
        public double K
        {
            get => this.k >= 0 ? this.k : (AngleFFTables.AngleConstantTable.TryGetValue(this.FFSymbol, out var k_) ?
            k_.TK : throw new System.Exception(this.FFSymbol));
        }

        private readonly double theta0 = -1.0;
        public Math.Angle Theta0
        {
            get => this.theta0 >= 0 ? Math.Angle.CreateFromDegree(this.theta0) :
                (AngleFFTables.AngleConstantTable.TryGetValue(this.FFSymbol, out var theta0_) ?
                Math.Angle.CreateFromRadian(theta0_.TEQ) : throw new System.Exception(this.FFSymbol));
        }

        public Angle(Atom atom1, Atom atom2, Atom atom3, double k = -1.0, double theta0 = -1.0)
        {
            this.Atom1 = atom1;
            this.Atom2 = atom2;
            this.Atom3 = atom3;
            this.k = k;
            this.theta0 = theta0;
            this.Interaction = new AngleInteraction(this.K, this.Theta0);
        }

        public static List<Angle> AutomaticCreate(List<Atom> atomList, List<Bond> bondList)
        {
            var adjacencyList = new Dictionary<Atom, List<Atom>>();
            var angleList = new List<Angle>();
            foreach (var atom in atomList) adjacencyList[atom] = new List<Atom>();
            foreach (var bond in bondList)
            {
                adjacencyList[bond.Atom1].Add(bond.Atom2);
                adjacencyList[bond.Atom2].Add(bond.Atom1);
            }
            foreach (var atom in atomList)
            {
                if (adjacencyList[atom].Count >= 2)
                {
                    foreach (var atom1 in adjacencyList[atom])
                    {
                        foreach (var atom2 in adjacencyList[atom])
                        {
                            if (atom1.GetHashCode() < atom2.GetHashCode())
                            {
                                angleList.Add(new Angle(atom1, atom, atom2));
                            }
                        }
                    }
                }
            }
            return angleList;
        }
    }
}