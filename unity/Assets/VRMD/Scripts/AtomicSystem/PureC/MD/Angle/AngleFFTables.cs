/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VRMD.IO;

namespace VRMD.PureC.MD
{
    public static class AngleFFTables
    {
        private const double kcalPerMol = 6.94782e-21;
        private const double kJPerMol = 1.660e-21;
        private const double degree = System.Math.PI / 180;

        public static Dictionary<string, double> BendingConstantTable = new Dictionary<string, double>()
        {
            {"hw-ow-hw",100*kcalPerMol},
        };

        public static Dictionary<string, double> EquilibriumAngleTable = new Dictionary<string, double>()
        {
            {"hw-ow-hw", 104.52*degree},
        };
     public static Dictionary<string,AngleParameter> AngleConstantTable = new Dictionary<string, AngleParameter>()
        {

        };
        public static void AngleTableComplie()
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
                    bool TK_check = double.TryParse(array[k], out var TK);
                    if (!TK_check)
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
                        if (hyphen.Length - 1 == 2 && array.Length > k + 2)
                        {
                            var TEQ_check = double.TryParse(array[k + 1], out var TEQ);
                            if (TEQ_check && !AngleConstantTable.ContainsKey(n))
                            {
                                AngleConstantTable.Add(n, new AngleParameter(TK *kcalPerMol, TEQ * degree));
                            }
                        }
                        break;
                    }
                }
            }
            foreach(var key in BendingConstantTable.Keys)
            {
                if(EquilibriumAngleTable.TryGetValue(key, out double k_) && !AngleConstantTable.ContainsKey(key))
                {
                        AngleConstantTable.Add(key, new AngleParameter(BendingConstantTable[key], k_));
                }
            }
        }
    }
}
