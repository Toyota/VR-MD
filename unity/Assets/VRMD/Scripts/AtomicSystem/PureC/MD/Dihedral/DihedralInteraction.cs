/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{
    public class DihedralInteraction
    {
        public int PN { get; }
        public double PK { get; }
        public double IDIVF { get; }
        public double Phase { get; }
        public double MinusPnPkPerIdivf { get; }
        public double CosPhase { get; }

        public DihedralInteraction(string label)
        {
            var n_V_n = new KeyValuePair<string, DihedralParameter>(label, DihedralFFtables.DihedralConstantTable[label]);
            this.PN = n_V_n.Value.PN;
            this.PK = n_V_n.Value.PK;
            this.IDIVF = n_V_n.Value.IDIVF;
            this.Phase = n_V_n.Value.PHASE;
            this.MinusPnPkPerIdivf = -this.PN * this.PK / this.IDIVF;
            this.CosPhase = System.Math.Cos(this.Phase);

        }

        public double GetEnergy(Math.Angle angle)
        {
            return this.PK / this.IDIVF * (1 + System.Math.Cos(this.PN * angle.Radian - this.Phase));
        }
        public double GetEnergy(Vector3 ri, Vector3 rj, Vector3 rk, Vector3 rl) =>
        this.GetEnergy(GetPhi(ri, rj, rk, rl));
        public PureC.Math.Angle GetPhi(Vector3 ri, Vector3 rj, Vector3 rk, Vector3 rl)
        {
            Dihedral.Phi(
            ri.X, ri.Y, ri.Z,
            rj.X, rj.Y, rj.Z,
            rk.X, rk.Y, rk.Z,
            rl.X, rl.Y, rl.Z, out Math.Angle _phi);
            return _phi;
        }
        public double GetEnergy(Vector x) => this.GetEnergy(
            new Vector3(x[0], x[1], x[2]), new Vector3(x[3], x[4], x[5]), new Vector3(x[6], x[7], x[8]), new Vector3(x[9], x[10], x[11])
        );
        public void GetForce(
            double ri_x, double ri_y, double ri_z,
            double rj_x, double rj_y, double rj_z,
            double rk_x, double rk_y, double rk_z,
            double rl_x, double rl_y, double rl_z,
            out double Fi_x, out double Fi_y, out double Fi_z,
            out double Fj_x, out double Fj_y, out double Fj_z,
            out double Fk_x, out double Fk_y, out double Fk_z,
            out double Fl_x, out double Fl_y, out double Fl_z
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
            var r_jl_x = rl_x - rj_x;
            var r_jl_y = rl_y - rj_y;
            var r_jl_z = rl_z - rj_z;
            var n1_x = r_ij_y * r_kj_z - r_kj_y * r_ij_z;
            var n1_y = r_ij_z * r_kj_x - r_kj_z * r_ij_x;
            var n1_z = r_ij_x * r_kj_y - r_kj_x * r_ij_y;
            var n2_x = r_kj_y * r_kl_z - r_kl_y * r_kj_z;
            var n2_y = r_kj_z * r_kl_x - r_kl_z * r_kj_x;
            var n2_z = r_kj_x * r_kl_y - r_kl_x * r_kj_y;
            var n1Square = 1.0 / (n1_x * n1_x + n1_y * n1_y + n1_z * n1_z);
            var n2Square = 1.0 / (n2_x * n2_x + n2_y * n2_y + n2_z * n2_z);
            var n1_magnitude = System.Math.Sqrt(n1Square);
            var n2_magnitude = System.Math.Sqrt(n2Square);
            var n1n2 = n1_magnitude * n2_magnitude;
            var cosphi = (n1_x * n2_x + n1_y * n2_y + n1_z * n2_z) * n1n2;
            var r_ik_x = rk_x - ri_x;
            var r_ik_y = rk_y - ri_y;
            var r_ik_z = rk_z - ri_z;
            var n12_x = n1_y * n2_z - n2_y * n1_z;
            var n12_y = n1_z * n2_x - n2_z * n1_x;
            var n12_z = n1_x * n2_y - n2_x * n1_y;
            var phi = -System.Math.Sign(r_kj_x * n12_x + r_kj_y * n12_y + r_kj_z * n12_z) * System.Math.Acos(cosphi);
            var sinphi = System.Math.Sin(phi);
            var fa = (sinphi > 0.0001) ? this.MinusPnPkPerIdivf * System.Math.Sin(this.PN * phi - this.Phase) / sinphi :
            (this.PN == 2) ? 2.0 * this.MinusPnPkPerIdivf * cosphi * this.CosPhase :
            (this.PN == 3) ? 4.0 * this.MinusPnPkPerIdivf * (-sinphi * sinphi + 0.75) * cosphi :
            (this.PN == 4) ? 8.0 * this.MinusPnPkPerIdivf * (cosphi * cosphi - 0.5) * cosphi * this.CosPhase :
                             this.MinusPnPkPerIdivf * this.CosPhase;
            var fb_x = fa * (n2_x * n1n2 - cosphi * n1_x * n1Square);
            var fb_y = fa * (n2_y * n1n2 - cosphi * n1_y * n1Square);
            var fb_z = fa * (n2_z * n1n2 - cosphi * n1_z * n1Square);
            var fc_x = fa * (n1_x * n1n2 - cosphi * n2_x * n2Square);
            var fc_y = fa * (n1_y * n1n2 - cosphi * n2_y * n2Square);
            var fc_z = fa * (n1_z * n1n2 - cosphi * n2_z * n2Square);
            Fi_x = fb_y * r_kj_z - r_kj_y * fb_z;
            Fi_y = fb_z * r_kj_x - r_kj_z * fb_x;
            Fi_z = fb_x * r_kj_y - r_kj_x * fb_y;
            Fj_x = -(fc_y * r_kl_z - r_kl_y * fc_z) + (fb_y * r_ik_z - r_ik_y * fb_z);
            Fj_y = -(fc_z * r_kl_x - r_kl_z * fc_x) + (fb_z * r_ik_x - r_ik_z * fb_x);
            Fj_z = -(fc_x * r_kl_y - r_kl_x * fc_y) + (fb_x * r_ik_y - r_ik_x * fb_y);
            Fk_x = -(fb_y * r_ij_z - r_ij_y * fb_z) + (fc_y * r_jl_z - r_jl_y * fc_z);
            Fk_y = -(fb_z * r_ij_x - r_ij_z * fb_x) + (fc_z * r_jl_x - r_jl_z * fc_x);
            Fk_z = -(fb_x * r_ij_y - r_ij_x * fb_y) + (fc_x * r_jl_y - r_jl_x * fc_y);
            Fl_x = fc_y * r_kj_z - r_kj_y * fc_z;
            Fl_y = fc_z * r_kj_x - r_kj_z * fc_x;
            Fl_z = fc_x * r_kj_y - r_kj_x * fc_y;
        }
        public Vector GetForceOld(Vector3 ri, Vector3 rj, Vector3 rk, Vector3 rl)
        {
            var _phi = Dihedral.PhiOld(ri, rj, rk, rl);
            var r_kj = rj - rk;
            var r_ik = rk - ri;
            var r_ij = rj - ri;
            var r_jl = rl - rj;
            var r_kl = rl - rk;
            var n1 = Vector3.Product(r_ij, r_kj);
            var n2 = Vector3.Product(r_kj, r_kl);
            double fa = this.FA(_phi);
            Vector3 fb = (n2 / n2.Magnitude - System.Math.Cos(_phi.Radian) * n1 / n1.Magnitude) / n1.Magnitude;
            Vector3 fc = (n1 / n1.Magnitude - System.Math.Cos(_phi.Radian) * n2 / n2.Magnitude) / n2.Magnitude;
            Vector3 F_i = fa * Vector3.Product(fb, r_kj);
            Vector3 F_j = fa * (-1 * Vector3.Product(fc, r_kl) + Vector3.Product(fb, r_ik));
            Vector3 F_k = fa * (-1 * Vector3.Product(fb, r_ij) + Vector3.Product(fc, r_jl));
            Vector3 F_l = fa * Vector3.Product(fc, r_kj);
            double[] answer = { F_i.X, F_i.Y, F_i.Z, F_j.X, F_j.Y, F_j.Z, F_k.X, F_k.Y, F_k.Z, F_l.X, F_l.Y, F_l.Z };
            return new Vector(answer);
        }
        public Vector GetForceOld(Vector x) => this.GetForceOld(
            new Vector3(x[0], x[1], x[2]), new Vector3(x[3], x[4], x[5]), new Vector3(x[6], x[7], x[8]), new Vector3(x[9], x[10], x[11])
        );
        private double FA(Math.Angle Angle)
        {
            if (System.Math.Sin(Angle.Radian) > 0.001)
            {
                return (-1) * this.PK * this.PN / this.IDIVF * System.Math.Sin(this.PN * Angle.Radian - this.Phase) / System.Math.Sin(Angle.Radian);
            }
            else
            {
                return this.PN switch
                {
                    (int)1 => (-1) * this.PK * this.PN / this.IDIVF * System.Math.Cos(this.Phase),
                    (int)2 => (-1) * 2 * this.PK / this.IDIVF * this.PN * System.Math.Cos(Angle.Radian) * System.Math.Cos(this.Phase),
                    (int)3 => (-1) * this.PK * this.PN / this.IDIVF * (-4 * System.Math.Sin(Angle.Radian) * System.Math.Sin(Angle.Radian) + 3) * System.Math.Cos(Angle.Radian),
                    (int)4 => (-1) * this.PK / this.IDIVF * this.PN * 4 * (2 * System.Math.Cos(Angle.Radian) * System.Math.Cos(Angle.Radian) - 1) * System.Math.Cos(Angle.Radian) * System.Math.Cos(this.Phase),
                    _ => (-1) * 2 * this.PK / this.IDIVF * this.PN * System.Math.Cos(Angle.Radian) * System.Math.Cos(this.Phase),//double.NaN;
                };
            }
        }
    }
}