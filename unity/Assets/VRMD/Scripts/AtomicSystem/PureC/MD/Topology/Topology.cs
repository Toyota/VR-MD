/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using VRMD.IO;
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{
    public struct Topology
    {
        public Atom[] AtomList { get; }
        public Bond[] BondList { get; }
        public Angle[] AngleList { get; }
        public Nonbond[] NonbondList { get; }
        public Dihedral[] DihedralList { get; }
        public ImproperDihedral[] ImproperDihedralList { get; }
        public Atom[] NonHydrogenAtomList { get; }

        public Topology(Atom[] atomList, Bond[] bondList, Angle[] angleList, Nonbond[] nonbondList, Dihedral[] dihedralList, ImproperDihedral[] improperDihedralList)
        {
            this.AtomList = atomList;
            this.BondList = bondList;
            this.AngleList = angleList;
            this.NonbondList = nonbondList;
            this.DihedralList = dihedralList;
            this.ImproperDihedralList = improperDihedralList;
            this.Temperature = new Temperature(this.AtomList);
            var nonHydrgenAtomList = new List<Atom>();
            foreach (var atom in this.AtomList)
            {
                if (!atom.IsHydrogen)
                {
                    nonHydrgenAtomList.Add(atom);
                }
            }
            this.NonHydrogenAtomList = nonHydrgenAtomList.ToArray();
        }

        public void ReadVelocities(string filename)
        {
            if (filename == "") return;
            var path = FileReader.GetUnityStreamingAssetPath(filename);
            var xyz = new XyzFile(path);
            for (int i = 0; i < xyz.NAtoms; i++)
            {
                this.AtomList[i].SetVelocity(xyz.Values[0, i], xyz.Values[1, i], xyz.Values[2, i]);
            }
        }

        public Vector3 CenterOfMass
        {
            get
            {
                var positions = new Vector3[this.NAtoms];
                var weights = new double[this.NAtoms];
                for (var i = 0; i < this.NAtoms; i++)
                {
                    positions[i] = this.AtomList[i].Position;
                    weights[i] = this.AtomList[i].Mass;
                }
                var center = Vector3.GetCenterOf(positions, weights);
                return center;
            }
        }

        public double TotalMass
        {
            get
            {
                var results = 0.0;
                foreach (var atom in this.AtomList)
                {
                    results += atom.Mass;
                }
                return results;
            }
        }

        public Temperature Temperature { get; }
        public int NAtoms => this.AtomList.Length;
    }
}
