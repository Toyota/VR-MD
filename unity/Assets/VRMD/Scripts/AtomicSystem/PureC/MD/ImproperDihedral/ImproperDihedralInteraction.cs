/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections;
using System.Collections.Generic;
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{
    public class ImproperDihedralInteraction
    {
        public KeyValuePair<string, ImproperDihedralParameter> n_V_n { get; }

        public ImproperDihedralInteraction(string n)
        {
            var nX = ImproperDihedralFFtables.ImproperDihedralParameterSearch(n);
            this.n_V_n = new KeyValuePair<string, ImproperDihedralParameter>(nX,ImproperDihedralFFtables.ImproperDihedralConstantTable[nX]);
        }
        
        public double GetEnergy(Math.Angle angle)
        {
            return n_V_n.Value.PK  * (1 + System.Math.Cos(n_V_n.Value.PN * angle.Radian - n_V_n.Value.PHASE));
        }
        public double GetEnergy(Vector3 ri, Vector3 rj, Vector3 rk, Vector3 rl) =>
        this.GetEnergy(GetPhi(ri, rj, rk, rl));
        public PureC.Math.Angle GetPhi(Vector3 ri, Vector3 rj, Vector3 rk, Vector3 rl)
        {
            PureC.Math.Angle _phi;
            ImproperDihedral.phi(
            ri.X, ri.Y, ri.Z,
            rj.X, rj.Y, rj.Z,
            rk.X, rk.Y, rk.Z,
            rl.X, rl.Y, rl.Z, out _phi);
            return _phi;
        }
        public double GetEnergy(Vector x) => this.GetEnergy(
            new Vector3(x[0], x[1], x[2]), new Vector3(x[3], x[4], x[5]), new Vector3(x[6], x[7], x[8]), new Vector3(x[9], x[10], x[11])
        );
        public void GetForce(
            double ri_x,double ri_y,double ri_z, 
            double rj_x,double rj_y,double rj_z, 
            double rk_x,double rk_y,double rk_z, 
            double rl_x,double rl_y,double rl_z,
            out double Fi_x,out double Fi_y,out double Fi_z, 
            out double Fj_x,out double Fj_y,out double Fj_z, 
            out double Fk_x,out double Fk_y,out double Fk_z, 
            out double Fl_x,out double Fl_y,out double Fl_z
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
            var n1_magnitude = 1 / System.Math.Sqrt(n1_x * n1_x + n1_y * n1_y + n1_z * n1_z);
            var n2_magnitude = 1 / System.Math.Sqrt(n2_x * n2_x + n2_y * n2_y + n2_z * n2_z);
            var cosphi = (n1_x * n2_x + n1_y * n2_y + n1_z * n2_z) * n1_magnitude * n2_magnitude;
            var r_ik_x = rk_x - ri_x;
            var r_ik_y = rk_y - ri_y;
            var r_ik_z = rk_z - ri_z;
            var n12_x = n1_y * n2_z - n2_y * n1_z;
            var n12_y = n1_z * n2_x - n2_z * n1_x;
            var n12_z = n1_x * n2_y - n2_x * n1_y;
            var phi = (-1) * System.Math.Sign(r_kj_x * n12_x + r_kj_y * n12_y + r_kj_z * n12_z) * System.Math.Acos(cosphi);
            var sinphi = System.Math.Sin(phi);
            double fa = (sinphi > 0.001) ? (-1) * n_V_n.Value.PK * n_V_n.Value.PN * System.Math.Sin(n_V_n.Value.PN * phi - n_V_n.Value.PHASE) / sinphi :
            (n_V_n.Value.PN == 1) ? (-1) * n_V_n.Value.PK * n_V_n.Value.PN * System.Math.Cos(n_V_n.Value.PHASE) :
            (n_V_n.Value.PN == 2) ? (-1) * 2 * n_V_n.Value.PK * n_V_n.Value.PN * cosphi * System.Math.Cos(n_V_n.Value.PHASE) :
            (n_V_n.Value.PN == 3) ? (-1) * n_V_n.Value.PK * n_V_n.Value.PN * (-4 * sinphi * sinphi + 3) * cosphi :
            (n_V_n.Value.PN == 4) ? (-1) * n_V_n.Value.PK * n_V_n.Value.PN * 4 * (2 * cosphi * cosphi - 1) * cosphi * System.Math.Cos(n_V_n.Value.PHASE) :
            (-1) * n_V_n.Value.PK * n_V_n.Value.PN * System.Math.Cos(n_V_n.Value.PHASE);
            var fb_x = (n2_x * n2_magnitude - cosphi * n1_x * n1_magnitude) * n1_magnitude;
            var fb_y = (n2_y * n2_magnitude - cosphi * n1_y * n1_magnitude) * n1_magnitude;
            var fb_z = (n2_z * n2_magnitude - cosphi * n1_z * n1_magnitude) * n1_magnitude;
            var fc_x = (n1_x * n1_magnitude - cosphi * n2_x * n2_magnitude) * n2_magnitude;
            var fc_y = (n1_y * n1_magnitude - cosphi * n2_y * n2_magnitude) * n2_magnitude;
            var fc_z = (n1_z * n1_magnitude - cosphi * n2_z * n2_magnitude) * n2_magnitude;
            Fi_x = fa * (fb_y * r_kj_z - r_kj_y * fb_z);
            Fi_y = fa * (fb_z * r_kj_x - r_kj_z * fb_x);
            Fi_z = fa * (fb_x * r_kj_y - r_kj_x * fb_y);
            Fj_x = fa * ((-1) * (fc_y * r_kl_z - r_kl_y * fc_z) + (fb_y * r_ik_z - r_ik_y * fb_z));
            Fj_y = fa * ((-1) * (fc_z * r_kl_x - r_kl_z * fc_x) + (fb_z * r_ik_x - r_ik_z * fb_x));
            Fj_z = fa * ((-1) * (fc_x * r_kl_y - r_kl_x * fc_y) + (fb_x * r_ik_y - r_ik_x * fb_y));
            Fk_x = fa * ((-1) * (fb_y * r_ij_z - r_ij_y * fb_z) + (fc_y * r_jl_z - r_jl_y * fc_z));
            Fk_y = fa * ((-1) * (fb_z * r_ij_x - r_ij_z * fb_x) + (fc_z * r_jl_x - r_jl_z * fc_x));
            Fk_z = fa * ((-1) * (fb_x * r_ij_y - r_ij_x * fb_y) + (fc_x * r_jl_y - r_jl_x * fc_y));
            Fl_x = fa * (fc_y * r_kj_z - r_kj_y * fc_z);
            Fl_y = fa * (fc_z * r_kj_x - r_kj_z * fc_x);
            Fl_z = fa * (fc_x * r_kj_y - r_kj_x * fc_y);
        }
        public Vector GetForceOld(Vector3 ri, Vector3 rj, Vector3 rk, Vector3 rl)
        {
            var _phi = ImproperDihedral.phiOld(ri, rj, rk, rl);
            var r_kj = rj - rk;
            var r_ik = rk - ri;
            var r_ij = rj - ri;
            var r_jl = rl - rj;
            var r_kl = rl - rk;
            var n1 = Vector3.Product(r_ij, r_kj);
            var n2 = Vector3.Product(r_kj, r_kl);
            double fa = this.fa(_phi);
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
            new Vector3(x[0],x[1],x[2]),new Vector3(x[3],x[4],x[5]),new Vector3(x[6],x[7],x[8]),new Vector3(x[9],x[10],x[11])
        );
        private double fa(Math.Angle Angle)
        {
            if (System.Math.Sin(Angle.Radian) > 0.001)
            {
                return (-1) * n_V_n.Value.PK * n_V_n.Value.PN * System.Math.Sin(n_V_n.Value.PN * Angle.Radian - n_V_n.Value.PHASE) / System.Math.Sin(Angle.Radian);
            }
            else
            {
                switch (n_V_n.Value.PN)
                {
                    case (int)1:
                        return (-1) * n_V_n.Value.PK * n_V_n.Value.PN  * System.Math.Cos(n_V_n.Value.PHASE);
                    case (int)2:
                        return (-1) * 2 * n_V_n.Value.PK  * n_V_n.Value.PN * System.Math.Cos(Angle.Radian) * System.Math.Cos(n_V_n.Value.PHASE);
                    case (int)3:
                        return (-1) * n_V_n.Value.PK * n_V_n.Value.PN  * (-4 * System.Math.Sin(Angle.Radian) * System.Math.Sin(Angle.Radian) + 3) * System.Math.Cos(Angle.Radian);
                    case (int)4:
                        return (-1) * n_V_n.Value.PK  * n_V_n.Value.PN * 4 * (2 * System.Math.Cos(Angle.Radian) * System.Math.Cos(Angle.Radian) - 1) * System.Math.Cos(Angle.Radian) * System.Math.Cos(n_V_n.Value.PHASE);
                    default:
                        return (-1) * 2 * n_V_n.Value.PK  * n_V_n.Value.PN * System.Math.Cos(Angle.Radian) * System.Math.Cos(n_V_n.Value.PHASE);//double.NaN;
                }
            }
        }
    }
}