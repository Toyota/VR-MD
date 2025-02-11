/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VRMD.IO;

namespace VRMD.PureC.MD
{
    public static class BondFFTables
    {
        private const double kcalPerMol = 6.94782e-21;
        private const double angs = 1e-10;
        private const double perAngs2 = 1e20;

        public static Dictionary<string, double> StretchingConstantTable = new Dictionary<string, double>()
        {
            {"c-cc",377.4*kcalPerMol*perAngs2},
            {"c-n",478.2*kcalPerMol*perAngs2},
            {"c-nc",438.8*kcalPerMol*perAngs2},
            {"c-o",648.0*kcalPerMol*perAngs2},
            {"c2-c3",328.3*kcalPerMol*perAngs2},
            {"c2-n3",486.3*kcalPerMol*perAngs2},
            {"c3-c3",303.1*kcalPerMol*perAngs2},
            {"c3-ca",323.5*kcalPerMol*perAngs2},
            {"c3-cc",337.3*kcalPerMol*perAngs2},
            {"c3-h1",335.9*kcalPerMol*perAngs2},
            {"c3-h3",335.9*kcalPerMol*perAngs2},
            {"c3-hc",337.3*kcalPerMol*perAngs2},
            {"c3-n",330.6*kcalPerMol*perAngs2},
            {"c3-n2",313.8*kcalPerMol*perAngs2},
            {"c3-n3",320.6*kcalPerMol*perAngs2},
            {"c3-na",426.7*kcalPerMol*perAngs2},
            {"c3-os",301.5*kcalPerMol*perAngs2},
            {"c3d-c3d",303.1*kcalPerMol*perAngs2},
            {"ca-ca",478.4*kcalPerMol*perAngs2},
            {"ca-cp",466.1*kcalPerMol*perAngs2},
            {"ca-cq",450.2*kcalPerMol*perAngs2},    // from ambermini/gaff.dat
            {"ca-h5",347.2*kcalPerMol*perAngs2},
            {"ca-ha",344.3*kcalPerMol*perAngs2},
            {"ca-n",372.3*kcalPerMol*perAngs2},
            {"ca-n2",320.6*kcalPerMol*perAngs2},
            {"ca-na",470.3*kcalPerMol*perAngs2},
            {"ca-nb",483.1*kcalPerMol*perAngs2},
            {"ca-nc",492.9*kcalPerMol*perAngs2},
            {"ca-nh",449.0*kcalPerMol*perAngs2},
            {"ca-o",519.7*kcalPerMol*perAngs2},
            {"cc-cd",504.0*kcalPerMol*perAngs2},
            {"cc-h4",350.1*kcalPerMol*perAngs2},
            {"cc-n",426.0*kcalPerMol*perAngs2},
            {"cc-nc",431.6*kcalPerMol*perAngs2},
            {"cc-nd",494.6*kcalPerMol*perAngs2},
            {"cc-nh",449.0*kcalPerMol*perAngs2},
            {"cd-cd",504.0*kcalPerMol*perAngs2},
            {"cd-h4",350.1*kcalPerMol*perAngs2},
            {"cd-h5",356.0*kcalPerMol*perAngs2},
            {"cd-ha",347.2*kcalPerMol*perAngs2},
            {"cd-n",426.0*kcalPerMol*perAngs2},
            {"cd-na",438.8*kcalPerMol*perAngs2},
            {"cd-nd",431.6*kcalPerMol*perAngs2},
            {"cd-nc",494.6*kcalPerMol*perAngs2},
            {"cd-nh",449.0*kcalPerMol*perAngs2},
            {"cp-cp",346.5*kcalPerMol*perAngs2},
            {"cp-cq",386.6*kcalPerMol*perAngs2},   // from ambermini/gaff.dat
            {"hn-n",410.2*kcalPerMol*perAngs2},
            {"hn-n2",375.5*kcalPerMol*perAngs2},
            {"hn-n3",394.1*kcalPerMol*perAngs2},
            {"hn-nh",401.2*kcalPerMol*perAngs2},
            {"hw-ow",553.0*kcalPerMol*perAngs2},
            {"CO-OC",648.0*kcalPerMol*perAngs2},
            {"O-O",1640.4*kcalPerMol*perAngs2/2},
        };

        public static Dictionary<string, double> BondDistanceTable = new Dictionary<string, double>()
        {
            {"c-cc",1.462*angs},
            {"c-o",1.214*angs},
            {"c-n",1.345*angs},
            {"c-nc",1.371*angs},
            {"c2-c3",1.508*angs},
            {"c2-n3",1.340*angs},
            {"c3-c3",1.535*angs},
            {"c3-ca",1.513*angs},
            {"c3-cc",1.499*angs},
            {"c3-h1",1.093*angs},
            {"c3-h3",1.093*angs},
            {"c3-hc",1.092*angs},
            {"c3-n",1.460*angs},
            {"c3-n2",1.477*angs},
            {"c3-n3",1.470*angs},
            {"c3-na",1.420*angs},
            {"c3-os",1.439*angs},
            {"c3d-c3d",1.535*angs},
            {"ca-ca",1.387*angs},
            {"ca-cp",1.395*angs},    // from ambermini/gaff.dat
            {"ca-cq",1.4058*angs},
            {"ca-h5",1.085*angs},
            {"ca-ha",1.087*angs},
            {"ca-n",1.422*angs},
            {"ca-n2",1.470*angs},
            {"ca-na",1.350*angs},
            {"ca-nb",1.342*angs},
            {"ca-nc",1.336*angs},
            {"ca-nh",1.364*angs},
            {"ca-o",1.275*angs},
            {"cc-cd",1.371*angs},
            {"cc-h4",1.083*angs},
            {"cc-n",1.380*angs},
            {"cc-nc",1.376*angs},
            {"cc-nd",1.335*angs},
            {"cc-nh",1.364*angs},
            {"cd-cd",1.371*angs},
            {"cd-h4",1.083*angs},
            {"cd-h5",1.079*angs},
            {"cd-ha",1.085*angs},
            {"cd-n",1.380*angs},
            {"cd-na",1.371*angs},
            {"cd-nc",1.335*angs},
            {"cd-nd",1.376*angs},
            {"cd-nh",1.364*angs},
            {"cp-cp",1.490*angs},
            {"cp-cq",1.4542*angs},    // from ambermini/gaff.dat
            {"hn-n",1.009*angs},
            {"hn-n2",1.029*angs},
            {"hn-n3",1.018*angs},
            {"hn-nh",1.014*angs},
            {"hw-ow",0.9572*angs},
            {"CO-OC",1.149*angs},
            {"O-O",1.2074*angs},
        };

        public static Dictionary<string,BondParameter> BondConstantTable = new Dictionary<string, BondParameter>()
        {

        };
        public static void BondTableComplie()
        {
            var ReachEND = false;
            var path = FileReader.GetUnityStreamingAssetPath("VRMD/FFTables/gaff.dat");
            var reader = new FileReader(path);
            while (!ReachEND)
            {
                var line = reader.ReadLine();
                var array = Regex.Split(line.Trim(), " +");
                string n = "";
                for (int k = 0; k < array.Length; k++)
                {
                    bool RK_check = double.TryParse(array[k], out var RK);
                    if (!RK_check)
                    {
                        n += array[k];
                        if (n.Contains("END"))
                        {
                            ReachEND = true;
                            break;
                        }
                    }
                    else
                    {
                        var hyphen = n.Split('-');
                        if (hyphen.Length - 1 == 1 && array.Length > k + 1)
                        {
                            var REQ_check = double.TryParse(array[k + 1], out var REQ);
                            if (REQ_check && !BondConstantTable.ContainsKey(n))
                            {
                                BondConstantTable.Add(n, new BondParameter(RK * kcalPerMol * perAngs2, REQ * angs));
                            }
                        }
                        break;
                    }
                }
            }
            foreach(var key in StretchingConstantTable.Keys)
            {
                if(BondFFTables.BondDistanceTable.TryGetValue(key, out double k_) && !BondConstantTable.ContainsKey(key))
                {
                        BondConstantTable.Add(key, new BondParameter(StretchingConstantTable[key], k_));
                }
            }
        }
    }
}
