/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{
    public class Dihedral
    {
        public Atom Atomi { get; }
        public Atom Atomj { get; }
        public Atom Atomk { get; }
        public Atom Atoml { get; }
        public List<DihedralInteraction> Interactions { get; }

        public Dihedral(Atom atomi, Atom atomj, Atom atomk, Atom atoml)
        {
            if (IsNormalOrdered(atomj.FFSymbol, atomk.FFSymbol))
            {
                this.Atomi = atomi;
                this.Atomj = atomj;
                this.Atomk = atomk;
                this.Atoml = atoml;
            }
            else
            {
                this.Atomi = atoml;
                this.Atomj = atomk;
                this.Atomk = atomj;
                this.Atoml = atomi;
            }
            this.Interactions = new List<DihedralInteraction>() { };
            foreach (string label in DihedralFFtables.DihedralParameterSearch(this.FFSymbol))
            {
                var interaction = new DihedralInteraction(label);
                this.Interactions.Add(interaction);
            }
        }
        private static bool IsNormalOrdered(string symbol1, string symbol2)
        {
            if (symbol1 == "n" && (symbol2 == "cc" || symbol2 == "cd"))
            {
                return true;
            }
            if ((symbol1 == "cc" || symbol1 == "cd") && symbol2 == "n")
            {
                return false;
            }
            if (string.Compare(symbol1, symbol2) <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string FFSymbol
        {
            get => Atomi.FFSymbol + "-" + Atomj.FFSymbol + "-" + Atomk.FFSymbol + "-" + Atoml.FFSymbol;
        }
        public bool IncludedinDihedralFFTables()
        {
            var nX = DihedralFFtables.DihedralParameterSearch(this.FFSymbol);
            return nX != null;
        }
        public double GetEnergy()
        {
            var energy = 0.0;
            foreach (var Interaction in Interactions)
            {
                energy += Interaction.GetEnergy(Atomi.Position, Atomj.Position, Atomk.Position, Atoml.Position);
            }
            return energy;
        }
        public Vector GetForceOld()
        {
            var force = new Vector(new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
            foreach (var Interaction in Interactions)
            {
                force += Interaction.GetForceOld(Atomi.Position, Atomj.Position, Atomk.Position, Atoml.Position);
            }
            return force;
        }
        public void GetForce(
            out double Fi_x, out double Fi_y, out double Fi_z,
            out double Fj_x, out double Fj_y, out double Fj_z,
            out double Fk_x, out double Fk_y, out double Fk_z,
            out double Fl_x, out double Fl_y, out double Fl_z
        )
        {
            Fi_x = 0.0;
            Fi_y = 0.0;
            Fi_z = 0.0;
            Fj_x = 0.0;
            Fj_y = 0.0;
            Fj_z = 0.0;
            Fk_x = 0.0;
            Fk_y = 0.0;
            Fk_z = 0.0;
            Fl_x = 0.0;
            Fl_y = 0.0;
            Fl_z = 0.0;
            foreach (var Interaction in Interactions)
            {
                Interaction.GetForce(
                    Atomi.Position.X, Atomi.Position.Y, Atomi.Position.Z,
                    Atomj.Position.X, Atomj.Position.Y, Atomj.Position.Z,
                    Atomk.Position.X, Atomk.Position.Y, Atomk.Position.Z,
                    Atoml.Position.X, Atoml.Position.Y, Atoml.Position.Z,
                    out double Fi_x_, out double Fi_y_, out double Fi_z_,
                    out double Fj_x_, out double Fj_y_, out double Fj_z_,
                    out double Fk_x_, out double Fk_y_, out double Fk_z_,
                    out double Fl_x_, out double Fl_y_, out double Fl_z_
                );
                Fi_x += Fi_x_;
                Fi_y += Fi_y_;
                Fi_z += Fi_z_;
                Fj_x += Fj_x_;
                Fj_y += Fj_y_;
                Fj_z += Fj_z_;
                Fk_x += Fk_x_;
                Fk_y += Fk_y_;
                Fk_z += Fk_z_;
                Fl_x += Fl_x_;
                Fl_y += Fl_y_;
                Fl_z += Fl_z_;
            }
        }

        public static void Phi(
            double ri_x, double ri_y, double ri_z,
            double rj_x, double rj_y, double rj_z,
            double rk_x, double rk_y, double rk_z,
            double rl_x, double rl_y, double rl_z,
            out Math.Angle angle
        )
        {
            var r_ij_x = rj_x - ri_x;
            var r_ij_y = rj_y - ri_y;
            var r_ij_z = rj_z - ri_z;
            var r_kj_x = rj_x - rk_x;
            var r_kj_y = rj_y - rk_y;
            var r_kj_z = rj_z - rk_z;
            var r_kl_x = rl_x - rk_x;
            var r_kl_y = rl_y - rk_y;
            var r_kl_z = rl_z - rk_z;
            var n1_x = r_ij_y * r_kj_z - r_kj_y * r_ij_z;
            var n1_y = r_ij_z * r_kj_x - r_kj_z * r_ij_x;
            var n1_z = r_ij_x * r_kj_y - r_kj_x * r_ij_y;
            var n2_x = r_kj_y * r_kl_z - r_kl_y * r_kj_z;
            var n2_y = r_kj_z * r_kl_x - r_kl_z * r_kj_x;
            var n2_z = r_kj_x * r_kl_y - r_kl_x * r_kj_y;
            var cosphi = (n1_x * n2_x + n1_y * n2_y + n1_z * n2_z) / System.Math.Sqrt(n1_x * n1_x + n1_y * n1_y + n1_z * n1_z) / System.Math.Sqrt(n2_x * n2_x + n2_y * n2_y + n2_z * n2_z);
            var n12_x = n1_y * n2_z - n2_y * n1_z;
            var n12_y = n1_z * n2_x - n2_z * n1_x;
            var n12_z = n1_x * n2_y - n2_x * n1_y;
            var phi = (-1) * System.Math.Sign(r_kj_x * n12_x + r_kj_y * n12_y + r_kj_z * n12_z) * System.Math.Acos(cosphi);
            angle = Math.Angle.CreateFromRadian(phi);
        }
        public static Math.Angle PhiOld(Vector3 ri, Vector3 rj, Vector3 rk, Vector3 rl)
        {
            var r_ij = rj - ri;
            var r_kj = rj - rk;
            var r_kl = rl - rk;
            var n1 = Vector3.Product(r_ij, r_kj);
            var n2 = Vector3.Product(r_kj, r_kl);
            var cosphi = n1 * n2 / n1.Magnitude / n2.Magnitude;
            double phi = (-1) * System.Math.Sign(r_kj * Vector3.Product(n1, n2)) * System.Math.Acos(cosphi);
            return Math.Angle.CreateFromRadian(phi);
        }
        public static Math.Angle PhiOld(Vector x) => PhiOld(
        new Vector3(x[0], x[1], x[2]), new Vector3(x[3], x[4], x[5]), new Vector3(x[6], x[7], x[8]), new Vector3(x[9], x[10], x[11])
        );
        public Math.Angle GetAngle()
        {
            Phi(
            Atomi.Position.X, Atomi.Position.Y, Atomi.Position.Z,
            Atomj.Position.X, Atomj.Position.Y, Atomj.Position.Z,
            Atomk.Position.X, Atomk.Position.Y, Atomk.Position.Z,
            Atoml.Position.X, Atoml.Position.Y, Atoml.Position.Z,
            out Math.Angle angle
            );
            return angle;
        }

        public Math.Angle AngleOld =>
        PhiOld(Atomi.Position, Atomj.Position, Atomk.Position, Atoml.Position);
        public PureC.Math.Angle Angle
        {
            get => GetAngle();
        }

        public static List<Dihedral> AutomaticCreate(List<Atom> atomList, List<Bond> bondList, List<Angle> angleList)
        {
            var _adjacencyList = new Dictionary<Atom, List<Atom>>();
            var dihedralList = new List<Dihedral>();
            foreach (var atom in atomList) _adjacencyList[atom] = new List<Atom>();
            foreach (var bond in bondList)
            {
                _adjacencyList[bond.Atom1].Add(bond.Atom2);
            }
            foreach (var angle in angleList)
            {
                foreach (var atom in _adjacencyList[angle.Atom1])
                {
                    if (atom.GetHashCode() != angle.Atom2.GetHashCode())
                    {
                        if (new Dihedral(atom, angle.Atom1, angle.Atom2, angle.Atom3).IncludedinDihedralFFTables())
                        {
                            dihedralList.Add(new Dihedral(atom, angle.Atom1, angle.Atom2, angle.Atom3));
                        }
                        else
                        {
                            dihedralList.Add(new Dihedral(angle.Atom3, angle.Atom2, angle.Atom1, atom));
                        }
                    }
                }
                foreach (var atom in _adjacencyList[angle.Atom3])
                {
                    if (atom.GetHashCode() != angle.Atom2.GetHashCode())
                    {
                        if (new Dihedral(angle.Atom1, angle.Atom2, angle.Atom3, atom).IncludedinDihedralFFTables())
                        {
                            dihedralList.Add(new Dihedral(angle.Atom1, angle.Atom2, angle.Atom3, atom));
                        }
                        else
                        {
                            dihedralList.Add(new Dihedral(atom, angle.Atom3, angle.Atom2, angle.Atom1));
                        }
                    }
                }
            }
            return dihedralList;
        }
    }
}
