/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{
    public class ImproperDihedral
    {
        public Atom Atomi { get; }
        public Atom Atomj { get; }
        public Atom Atomk { get; }
        public Atom Atoml { get; }
        public ImproperDihedralInteraction Interaction { get; }

        public ImproperDihedral(Atom atomi, Atom atomj, Atom atomk, Atom atoml)
        {
            this.Atomi = atomi;
            this.Atomj = atomj;
            this.Atomk = atomk;
            this.Atoml = atoml;
            this.Interaction = new ImproperDihedralInteraction(this.FFSymbol);
        }
        public string FFSymbol
        {
            get => Atomi.FFSymbol + "-" + Atomj.FFSymbol + "-" + Atomk.FFSymbol + "-" + Atoml.FFSymbol;
        }
        public static bool IncludedinDihedralFFTables(Atom atomi, Atom atomj, Atom atomk, Atom atoml)
        {
            var FFSymbol = atomi.FFSymbol + "-" + atomj.FFSymbol + "-" + atomk.FFSymbol + "-" + atoml.FFSymbol;
            var nX = ImproperDihedralFFtables.ImproperDihedralParameterSearch(FFSymbol);
            return nX != null;
        }
        public double GetEnergy() => this.Interaction.GetEnergy(this.Atomi.Position, this.Atomj.Position, this.Atomk.Position, this.Atoml.Position);
        public Vector GetForceOld() => this.Interaction.GetForceOld(this.Atomi.Position, this.Atomj.Position, this.Atomk.Position, this.Atoml.Position);
        public void GetForce(
            out double Fi_x, out double Fi_y, out double Fi_z,
            out double Fj_x, out double Fj_y, out double Fj_z,
            out double Fk_x, out double Fk_y, out double Fk_z,
            out double Fl_x, out double Fl_y, out double Fl_z
        ) => this.Interaction.GetForce(
            this.Atomi.Position.X, this.Atomi.Position.Y, this.Atomi.Position.Z,
            this.Atomj.Position.X, this.Atomj.Position.Y, this.Atomj.Position.Z,
            this.Atomk.Position.X, this.Atomk.Position.Y, this.Atomk.Position.Z,
            this.Atoml.Position.X, this.Atoml.Position.Y, this.Atoml.Position.Z,
            out Fi_x, out Fi_y, out Fi_z,
            out Fj_x, out Fj_y, out Fj_z,
            out Fk_x, out Fk_y, out Fk_z,
            out Fl_x, out Fl_y, out Fl_z
        );

        public static void phi(
            double ri_x, double ri_y, double ri_z,
            double rj_x, double rj_y, double rj_z,
            double rk_x, double rk_y, double rk_z,
            double rl_x, double rl_y, double rl_z,
            out PureC.Math.Angle angle
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
            var r_ki_x = ri_x - rk_x;
            var r_ki_y = ri_y - rk_y;
            var r_ki_z = ri_z - rk_z;
            var r_jl_x = rl_x - rj_x;
            var r_jl_y = rl_y - rj_y;
            var r_jl_z = rl_z - rj_z;
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
            var _sign = r_kj_x * n12_x + r_kj_y * n12_y + r_kj_z * n12_z;
            var phi = (-1) * System.Math.Sign(r_kj_x * n12_x + r_kj_y * n12_y + r_kj_z * n12_z) * System.Math.Acos(cosphi);
            angle = Math.Angle.CreateFromRadian(phi);
        }
        public static PureC.Math.Angle phiOld(Vector3 ri, Vector3 rj, Vector3 rk, Vector3 rl)
        {
            var r_ij = rj - ri;
            var r_kj = rj - rk;
            var r_kl = rl - rk;
            var r_ki = ri - rk;
            var r_jl = rl - rj;
            var n1 = Vector3.Product(r_ij, r_kj);
            var n2 = Vector3.Product(r_kj, r_kl);
            var cosphi = n1 * n2 / n1.Magnitude / n2.Magnitude;
            double phi = (-1) * System.Math.Sign(r_kj * (Vector3.Product(n1, n2))) * System.Math.Acos(cosphi);
            return Math.Angle.CreateFromRadian(phi);
        }
        public static PureC.Math.Angle phiOld(Vector x) => Dihedral.PhiOld(
        new Vector3(x[0], x[1], x[2]), new Vector3(x[3], x[4], x[5]), new Vector3(x[6], x[7], x[8]), new Vector3(x[9], x[10], x[11])
        );
        public PureC.Math.Angle GetAngle()
        {
            ImproperDihedral.phi(
            this.Atomi.Position.X, this.Atomi.Position.Y, this.Atomi.Position.Z,
            this.Atomj.Position.X, this.Atomj.Position.Y, this.Atomj.Position.Z,
            this.Atomk.Position.X, this.Atomk.Position.Y, this.Atomk.Position.Z,
            this.Atoml.Position.X, this.Atoml.Position.Y, this.Atoml.Position.Z,
            out PureC.Math.Angle angle
            );
            return angle;
        }

        public PureC.Math.Angle AngleOld =>
        Dihedral.PhiOld(this.Atomi.Position, this.Atomj.Position, this.Atomk.Position, this.Atoml.Position);
        public PureC.Math.Angle Angle
        {
            get => GetAngle();
        }

        public static List<ImproperDihedral> AutomaticCreate(List<Atom> atomList, List<Bond> bondList)
        {
            var _adjacencyList = new Dictionary<Atom, List<Atom>>();
            var ImproperdihedralList = new List<ImproperDihedral>();
            foreach (var atom in atomList) _adjacencyList[atom] = new List<Atom>();
            foreach (var bond in bondList)
            {
                _adjacencyList[bond.Atom1].Add(bond.Atom2);
                _adjacencyList[bond.Atom2].Add(bond.Atom1);
            }
            foreach (var atom in atomList)
            {
                if (_adjacencyList[atom].Count > 2)
                {
                    for (int i = 0; i < _adjacencyList[atom].Count - 2; i++)
                    {
                        var atomi = _adjacencyList[atom][i];
                        for (int j = i + 1; j < _adjacencyList[atom].Count - 1; j++)
                        {
                            var atomj = _adjacencyList[atom][j];
                            for (int k = j + 1; k < _adjacencyList[atom].Count; k++)
                            {
                                var atomk = _adjacencyList[atom][k];
                                if (ImproperDihedral.IncludedinDihedralFFTables(atomi, atomj, atom, atomk))
                                {
                                    ImproperdihedralList.Add(new ImproperDihedral(atomi, atomj, atom, atomk));
                                }
                            }
                        }
                    }
                }
            }
            return ImproperdihedralList;
        }
    }
}
