/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VRMD.IO;

namespace VRMD.PureC.MD
{

    public class Mol2Topology : EditableTopology
    {
        public Mol2Topology(string filename)
        {
            var path = FileReader.GetUnityStreamingAssetPath(filename);
            var reader = new FileReader(path);
            var angs = 1e-10f;
            var e = 1.602176634e-19f;
            reader.ReadLine();
            reader.ReadLine();
            var line = reader.ReadLine();
            var array = Regex.Split(line.Trim(), " +");
            var nAtoms = int.Parse(array[0]);
            var nBonds = int.Parse(array[1]);
            while (line == "" || line[0].ToString() != "@")
            {
                line = reader.ReadLine();
            }
            var atomIdTable = new Dictionary<string, Atom>();
            for (var i = 0; i < nAtoms; i++)
            {
                line = reader.ReadLine();
                array = Regex.Split(line.Trim(), " +");
                var atomId = array[0];
                var label = array[1];
                var ffSymbol = array[5];
                var x = float.Parse(array[2]) * angs;
                var y = float.Parse(array[3]) * angs;
                var z = float.Parse(array[4]) * angs;
                var charge = float.Parse(array[8]) * e;
                var atom = new Atom(label, "", ffSymbol, x, y, z, charge);
                this.AtomList.Add(atom);
                atomIdTable[atomId] = atom;
            }
            reader.ReadLine();
            for (var i = 0; i < nBonds; i++)
            {
                line = reader.ReadLine();
                array = Regex.Split(line.Trim(), " +");
                var atomId1 = array[1];
                var atomId2 = array[2];
                var atom1 = atomIdTable[atomId1];
                var atom2 = atomIdTable[atomId2];
                var bond = new Bond(atom1, atom2);
                this.BondList.Add(bond);
            }
            this.AngleList.AddRange(Angle.AutomaticCreate(this.AtomList, this.BondList));
            this.NonbondList.AddRange(Nonbond.AutomaticCreate(this.AtomList, this.BondList, this.AngleList));
            this.DihedralList.AddRange(Dihedral.AutomaticCreate(this.AtomList, this.BondList, this.AngleList));
            this.ImproperDihedralList.AddRange(ImproperDihedral.AutomaticCreate(this.AtomList, this.BondList));
        }
    }
}