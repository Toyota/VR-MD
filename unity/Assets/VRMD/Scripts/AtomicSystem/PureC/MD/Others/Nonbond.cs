/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{
    public class Nonbond
    {
        public Atom Atom1 { get; }
        public Atom Atom2 { get; }

        public double Distance => (this.Atom1.Position - this.Atom2.Position).Magnitude;

        public PairInteraction Interaction => this.LJChargeInteraction;
        public LJInteraction LJInteraction { get; }
        public ChargeInteraction ChargeInteraction { get; }
        public LJChargeInteraction LJChargeInteraction { get; }
        public Vector GetForce() => this.LJChargeInteraction.GetForce(this.Atom1.Position, this.Atom2.Position);
        public double[] GetForceAsDoubles() => this.LJChargeInteraction.GetForce(
            this.Atom1.Position.Values, this.Atom2.Position.Values
        );
        public void GetForce(out double F1x, out double F1y, out double F1z, out double F2x, out double F2y, out double F2z)
            => this.LJChargeInteraction.GetForce(
                this.Atom1.Position.Values, this.Atom2.Position.Values,
                out F1x, out F1y, out F1z, out F2x, out F2y, out F2z
            );

        public double GetEnergy() => this.LJChargeInteraction.GetEnergy(this.Atom1.Position, this.Atom2.Position);
        public double Epsilon48 { get; }
        public double Sigma2 { get; }
        public double KQ1Q2 { get; }
        public static double KCoulomb { get; } = 8.9876e9f;

        public Nonbond(Atom atom1, Atom atom2)
        {
            this.Atom1 = atom1;
            this.Atom2 = atom2;
            var ljEpsilon = System.Math.Sqrt(atom1.LJEpsilon * atom2.LJEpsilon);
            var ljSigma = 0.5 * (atom1.LJSigma + atom2.LJSigma);
            var q1 = atom1.Charge;
            var q2 = atom2.Charge;
            this.LJInteraction = new LJInteraction(ljEpsilon, ljSigma);
            this.ChargeInteraction = new ChargeInteraction(q1, q2);
            this.LJChargeInteraction = new LJChargeInteraction(ljEpsilon, ljSigma, q1, q2);
            this.Epsilon48 = 48.0 * ljEpsilon;
            this.Sigma2 = ljSigma * ljSigma;
            this.KQ1Q2 = KCoulomb * q1 * q2;

        }

        public static List<Nonbond> AutomaticCreate(List<Atom> atomList, List<Bond> bondList, List<Angle> angleList)
        {
            var nonbondList = new List<Nonbond>();
            var atom2List = new List<Atom>(atomList);
            foreach (var atom1 in atomList)
            {
                atom2List.Remove(atom1);
                foreach (var atom2 in atom2List)
                {
                    var isInteractive = true;
                    foreach (var bond in bondList)
                    {
                        if ((bond.Atom1 == atom1 && bond.Atom2 == atom2) || (bond.Atom1 == atom2 && bond.Atom2 == atom1))
                        {
                            isInteractive = false;
                            break;
                        }
                    }
                    foreach (var angle in angleList)
                    {
                        if ((angle.Atom1 == atom1 && angle.Atom3 == atom2) || (angle.Atom1 == atom2 && angle.Atom3 == atom1))
                        {
                            isInteractive = false;
                            break;
                        }
                    }
                    if (isInteractive) nonbondList.Add(new Nonbond(atom1, atom2));

                }
            }
            return nonbondList;
        }
    }
}

