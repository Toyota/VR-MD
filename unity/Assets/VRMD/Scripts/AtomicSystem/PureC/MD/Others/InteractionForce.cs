/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
namespace VRMD.PureC.MD
{
    public class InteractionForce
    {
        private readonly Topology Topology;

        public InteractionForce(Topology topology)
        {
            this.Topology = topology;
        }

        public void ResetForce()
        {
            foreach (var atom in this.Topology.AtomList)
            {
                atom.SetForce(0.0, 0.0, 0.0);
            }
        }

        public void CalculateBondForce()
        {
            foreach (var bond in this.Topology.BondList)
            {
                bond.GetForce(
                    out double Fx1, out double Fy1, out double Fz1,
                    out double Fx2, out double Fy2, out double Fz2
                );
                bond.Atom1.AddForce(Fx1, Fy1, Fz1);
                bond.Atom2.AddForce(Fx2, Fy2, Fz2);
            }
        }

        public void CalculateAngleForce()
        {
            foreach (var angle in this.Topology.AngleList)
            {
                angle.GetForce(
                    out var Fx1, out var Fy1, out var Fz1,
                    out var Fx2, out var Fy2, out var Fz2,
                    out var Fx3, out var Fy3, out var Fz3
                );
                angle.Atom1.AddForce(Fx1, Fy1, Fz1);
                angle.Atom2.AddForce(Fx2, Fy2, Fz2);
                angle.Atom3.AddForce(Fx3, Fy3, Fz3);
            }
        }
        public void CalculateNonbondForce()
        {
            foreach (var nonbond in this.Topology.NonbondList)
            {
                var atom1 = nonbond.Atom1;
                var atom2 = nonbond.Atom2;
                var x1 = atom1.Position.Values;
                var x2 = atom2.Position.Values;
                var x = x1[0] - x2[0];
                var y = x1[1] - x2[1];
                var z = x1[2] - x2[2];
                var rInv2 = 1.0 / (x * x + y * y + z * z);
                var rInv = System.Math.Sqrt(rInv2);
                var sInv2 = nonbond.Sigma2 * rInv2;
                var sInv6 = sInv2 * sInv2 * sInv2;
                var FMagnitudeRInv = rInv2 * (nonbond.Epsilon48 * sInv6 * (sInv6 - 0.5) + nonbond.KQ1Q2 * rInv);
                var F1x = FMagnitudeRInv * x;
                var F1y = FMagnitudeRInv * y;
                var F1z = FMagnitudeRInv * z;
                atom1.Fx += F1x;
                atom1.Fy += F1y;
                atom1.Fz += F1z;
                atom2.Fx -= F1x;
                atom2.Fy -= F1y;
                atom2.Fz -= F1z;
            }
        }
        public void CalculateNonbondForceSlow()
        {
            foreach (var nonbond in this.Topology.NonbondList)
            {
                nonbond.GetForce(
                    out double F1x, out double F1y, out double F1z,
                    out double F2x, out double F2y, out double F2z
                );
                nonbond.Atom1.AddForce(F1x, F1y, F1z);
                nonbond.Atom2.AddForce(F2x, F2y, F2z);
            }
        }
        public void CalculateDihedralForce()
        {
            foreach (var dihedral in Topology.DihedralList)
            {
                var atom1 = dihedral.Atomi;
                var atom2 = dihedral.Atomj;
                var atom3 = dihedral.Atomk;
                var atom4 = dihedral.Atoml;
                var ri = atom1.Position.Values;
                var rj = atom2.Position.Values;
                var rk = atom3.Position.Values;
                var rl = atom4.Position.Values;
                var ri_x = ri[0];
                var ri_y = ri[1];
                var ri_z = ri[2];
                var rj_x = rj[0];
                var rj_y = rj[1];
                var rj_z = rj[2];
                var rk_x = rk[0];
                var rk_y = rk[1];
                var rk_z = rk[2];
                var rl_x = rl[0];
                var rl_y = rl[1];
                var rl_z = rl[2];
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
                var sign = -System.Math.Sign(r_kj_x * n12_x + r_kj_y * n12_y + r_kj_z * n12_z);
                var phi = sign * System.Math.Acos(cosphi);
                var sinphi = sign * System.Math.Sqrt(1 - cosphi * cosphi);
                foreach (var interaction in dihedral.Interactions)
                {
                    var pn = interaction.PN;
                    var phase = interaction.Phase;
                    var cosPhase = interaction.CosPhase;
                    var minusPnPkPerIdivf = interaction.MinusPnPkPerIdivf;
                    var fa = (sinphi > 0.0001) ? minusPnPkPerIdivf * System.Math.Sin(pn * phi - phase) / sinphi :
                    (pn == 2) ? 2.0 * minusPnPkPerIdivf * cosphi * cosPhase :
                    (pn == 3) ? 4.0 * minusPnPkPerIdivf * (-sinphi * sinphi + 0.75) * cosphi :
                    (pn == 4) ? 8.0 * minusPnPkPerIdivf * (cosphi * cosphi - 0.5) * cosphi * cosPhase :
                                     minusPnPkPerIdivf * cosPhase;
                    var fb_x = fa * (n2_x * n1n2 - cosphi * n1_x * n1Square);
                    var fb_y = fa * (n2_y * n1n2 - cosphi * n1_y * n1Square);
                    var fb_z = fa * (n2_z * n1n2 - cosphi * n1_z * n1Square);
                    var fc_x = fa * (n1_x * n1n2 - cosphi * n2_x * n2Square);
                    var fc_y = fa * (n1_y * n1n2 - cosphi * n2_y * n2Square);
                    var fc_z = fa * (n1_z * n1n2 - cosphi * n2_z * n2Square);
                    var Fi_x = fb_y * r_kj_z - r_kj_y * fb_z;
                    var Fi_y = fb_z * r_kj_x - r_kj_z * fb_x;
                    var Fi_z = fb_x * r_kj_y - r_kj_x * fb_y;
                    var Fj_x = -(fc_y * r_kl_z - r_kl_y * fc_z) + (fb_y * r_ik_z - r_ik_y * fb_z);
                    var Fj_y = -(fc_z * r_kl_x - r_kl_z * fc_x) + (fb_z * r_ik_x - r_ik_z * fb_x);
                    var Fj_z = -(fc_x * r_kl_y - r_kl_x * fc_y) + (fb_x * r_ik_y - r_ik_x * fb_y);
                    var Fk_x = -(fb_y * r_ij_z - r_ij_y * fb_z) + (fc_y * r_jl_z - r_jl_y * fc_z);
                    var Fk_y = -(fb_z * r_ij_x - r_ij_z * fb_x) + (fc_z * r_jl_x - r_jl_z * fc_x);
                    var Fk_z = -(fb_x * r_ij_y - r_ij_x * fb_y) + (fc_x * r_jl_y - r_jl_x * fc_y);
                    var Fl_x = fc_y * r_kj_z - r_kj_y * fc_z;
                    var Fl_y = fc_z * r_kj_x - r_kj_z * fc_x;
                    var Fl_z = fc_x * r_kj_y - r_kj_x * fc_y;
                    atom1.Fx += Fi_x;
                    atom1.Fy += Fi_y;
                    atom1.Fz += Fi_z;
                    atom2.Fx += Fj_x;
                    atom2.Fy += Fj_y;
                    atom2.Fz += Fj_z;
                    atom3.Fx += Fk_x;
                    atom3.Fy += Fk_y;
                    atom3.Fz += Fk_z;
                    atom4.Fx += Fl_x;
                    atom4.Fy += Fl_y;
                    atom4.Fz += Fl_z;
                }
            }
        }
        public void CalculateDihedralForceSlow()
        {
            foreach (var dihedral in this.Topology.DihedralList)
            {
                dihedral.GetForce(
                        out double Fi_x, out double Fi_y, out double Fi_z,
                        out double Fj_x, out double Fj_y, out double Fj_z,
                        out double Fk_x, out double Fk_y, out double Fk_z,
                        out double Fl_x, out double Fl_y, out double Fl_z
                        );
                dihedral.Atomi.AddForce(Fi_x, Fi_y, Fi_z);
                dihedral.Atomj.AddForce(Fj_x, Fj_y, Fj_z);
                dihedral.Atomk.AddForce(Fk_x, Fk_y, Fk_z);
                dihedral.Atoml.AddForce(Fl_x, Fl_y, Fl_z);
            }
        }
        public void CalculateImproperDihedralForce()
        {
            foreach (var improperdihedral in this.Topology.ImproperDihedralList)
            {
                improperdihedral.GetForce(
                        out double Fi_x, out double Fi_y, out double Fi_z,
                        out double Fj_x, out double Fj_y, out double Fj_z,
                        out double Fk_x, out double Fk_y, out double Fk_z,
                        out double Fl_x, out double Fl_y, out double Fl_z
                        );
                improperdihedral.Atomi.AddForce(Fi_x, Fi_y, Fi_z);
                improperdihedral.Atomj.AddForce(Fj_x, Fj_y, Fj_z);
                improperdihedral.Atomk.AddForce(Fk_x, Fk_y, Fk_z);
                improperdihedral.Atoml.AddForce(Fl_x, Fl_y, Fl_z);
            }
        }
        public void CalculateTotalForce()
        {
            this.ResetForce();
            this.CalculateBondForce();
            this.CalculateAngleForce();
            this.CalculateNonbondForce();
            this.CalculateDihedralForce();
            this.CalculateImproperDihedralForce();
        }
    }
}
