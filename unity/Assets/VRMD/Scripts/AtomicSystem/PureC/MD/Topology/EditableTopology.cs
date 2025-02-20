/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;

namespace VRMD.PureC.MD
{

    public class EditableTopology
    {
        public List<Atom> AtomList { get; }
        public List<Bond> BondList { get; }
        public List<Angle> AngleList { get; }
        public List<Nonbond> NonbondList { get; }
        public List<Dihedral> DihedralList { get; }
        public List<ImproperDihedral> ImproperDihedralList { get; }

        public EditableTopology()
        {
            this.AtomList = new List<Atom>();
            this.BondList = new List<Bond>();
            this.AngleList = new List<Angle>();
            this.NonbondList = new List<Nonbond>();
            this.DihedralList = new List<Dihedral>();
            this.ImproperDihedralList = new List<ImproperDihedral>();
        }

        public Topology Compile()
        {
            var atomArray = this.AtomList.ToArray();
            var bondArray = this.BondList.ToArray();
            var angleArray = this.AngleList.ToArray();
            var nonbondArray = this.NonbondList.ToArray();
            var dihedralArray = this.DihedralList.ToArray();
            var improperDihedralArray = this.ImproperDihedralList.ToArray();
            var topology = new Topology(atomArray, bondArray, angleArray, nonbondArray, dihedralArray, improperDihedralArray);
            return topology;
        }
    }
}
