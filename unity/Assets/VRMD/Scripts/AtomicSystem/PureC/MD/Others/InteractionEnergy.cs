/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;

namespace VRMD.PureC.MD
{
    public class InteractionEnergy
    {
        private Topology Topology { get; }
        public InteractionEnergy(Topology topology)
        {
            this.Topology = topology;
        }

        public double CalculateBondEnergy()
        {
            var E = 0.0;
            foreach (var bond in this.Topology.BondList)
            {
                E += bond.GetEnergy();
            }
            return E;
        }

        public double CalculateAngleEnergy()
        {
            var E = 0.0;
            foreach (var angle in this.Topology.AngleList)
            {
                E += angle.GetEnergy();
            }
            return E;
        }

        public double CalculateNonbondEnergy()
        {
            var E = 0.0;
            foreach (var nonbond in this.Topology.NonbondList)
            {
                E += nonbond.GetEnergy();
            }
            return E;
        }
        public double CalculateDihedralEnergy()
        {
            var E = 0.0;
            foreach (var dihedralbond in this.Topology.DihedralList)
            {
                E += dihedralbond.GetEnergy();
            }
            return E;
        }
        public double CalculateImproperDihedralEnergy()
        {
            var E = 0.0;
            foreach (var improperdihedralbond in this.Topology.ImproperDihedralList)
            {
                E += improperdihedralbond.GetEnergy();
            }
            return E;
        }

        public double CalculateTotalEnergy()
        {
            var E = this.CalculateBondEnergy();
            E += this.CalculateAngleEnergy();
            E += this.CalculateNonbondEnergy();
            E += this.CalculateDihedralEnergy();
            E += this.CalculateImproperDihedralEnergy();
            return E;
        }

    }
}
