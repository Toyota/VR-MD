/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Text.RegularExpressions;
using VRMD.IO;

namespace VRMD.PureC.MD
{
    public class XyzTopology : EditableTopology
    {
        public XyzTopology(string filename)
        {
            var path = FileReader.GetUnityStreamingAssetPath(filename);
            var xyz = new XyzFile(path);
            var angs = 1e-10f;
            for (int i = 0; i < xyz.NAtoms; i++)
            {
                var label = xyz.Labels[i];
                var x = xyz.Values[0, i] * angs;
                var y = xyz.Values[1, i] * angs;
                var z = xyz.Values[2, i] * angs;
                var atom = new Atom("", "", label, x, y, z);
                this.AtomList.Add(atom);
            }
            this.BondList.AddRange(Bond.AutomaticCreate(this.AtomList));
            this.AngleList.AddRange(Angle.AutomaticCreate(this.AtomList, this.BondList));
            this.NonbondList.AddRange(Nonbond.AutomaticCreate(this.AtomList, this.BondList, this.AngleList));
        }
    }

    public class XyzFile
    {
        public int NAtoms;
        public string Title;
        public double[,] Values;
        public string[] Labels;

        public XyzFile(string path)
        {
            var reader = new FileReader(path);
            this.NAtoms = int.Parse(reader.ReadLine());
            this.Title = reader.ReadLine();
            this.Labels = new string[this.NAtoms];
            this.Values = new double[3, this.NAtoms];
            for (int i = 0; i < this.NAtoms; i++)
            {
                var line = reader.ReadLine();
                var array = Regex.Split(line.Trim(), " +");
                this.Labels[i] = array[0];
                this.Values[0, i] = double.Parse(array[1]);
                this.Values[1, i] = double.Parse(array[2]);
                this.Values[2, i] = double.Parse(array[3]);
            }
        }
    }
}