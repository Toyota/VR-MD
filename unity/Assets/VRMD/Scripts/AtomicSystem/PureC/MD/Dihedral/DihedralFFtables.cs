/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VRMD.IO;

namespace VRMD.PureC.MD
{
    public class DihedralFFtables
    {
        private const double kcalPerMol = 6.94782e-21;
        private const double degree = System.Math.PI / 180;
        public static Dictionary<string, DihedralParameter> DihedralConstantTable = new Dictionary<string, DihedralParameter>()
        {
            {"X-cp-cq-X_2",new DihedralParameter(4, 4 * kcalPerMol, 180 * degree, 2)},//from X-cp-cp-X
            {"c3d-c3d-c3d-c3d_-3",new DihedralParameter(1, 0.18 * kcalPerMol, 0 * degree, -3)},
            {"c3d-c3d-c3d-c3d_-2",new DihedralParameter(1, 0.25 * kcalPerMol, 180 * degree, -2)},
            {"c3d-c3d-c3d-c3d_1",new DihedralParameter(1, 0.20 * kcalPerMol, 180 * degree, 1)},
        };
        public static void DihedralTableComplie()
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
                    bool check = double.TryParse(array[k], out double IDIVF);
                    if (!check)
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
                        if (hyphen.Length - 1 == 3 && array.Length > k + 3)
                        {
                            var PK_check = double.TryParse(array[k + 1], out var PK);
                            var PHAZE_check = double.TryParse(array[k + 2], out var PHAZE);
                            var PN_check = double.TryParse(array[k + 3], out var PN);
                            if (PK_check && PHAZE_check && PN_check)
                            {
                                int PN_ = (int)System.Math.Round(PN);
                                string nPN = n + "_" + PN_.ToString();
                                if (!DihedralConstantTable.ContainsKey(nPN))
                                {
                                    DihedralConstantTable.Add(
                                        nPN.ToString(), new DihedralParameter(IDIVF, PK * kcalPerMol, PHAZE * degree, PN_)
                                    );
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }
        public static List<string> DihedralParameterSearch(string label)
        {
            var atomLabels = label.Split('-');
            var labels = new List<string>();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        for (int l = 0; l < 2; l++)
                        {
                            var n_X = new string[atomLabels.Length];
                            n_X[0] = (i == 0) ? atomLabels[0] : "X";
                            n_X[1] = (j == 0) ? atomLabels[1] : "X";
                            n_X[2] = (k == 0) ? atomLabels[2] : "X";
                            n_X[3] = (l == 0) ? atomLabels[3] : "X";
                            var nX = n_X[0] + "-" + n_X[1] + "-" + n_X[2] + "-" + n_X[3];
                            foreach (string key in DihedralConstantTable.Keys)
                            {
                                if (key.StartsWith(nX))
                                {
                                    labels.Add(key);
                                }
                            }
                        }
                    }
                }
            }
            if (labels.Count == 0)
            {
                UnityEngine.Debug.LogError(label);
                return null;
            }
            return labels;
        }
    }
}

