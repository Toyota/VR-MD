/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VRMD.IO;
using UnityEngine;

namespace VRMD.PureC.MD
{
    public class ImproperDihedralFFtables
    {
        private const double kcalPerMol = 6.94782e-21;
        private const double degree = System.Math.PI / 180;
        public static Dictionary<string, ImproperDihedralParameter> ImproperDihedralConstantTable = new Dictionary<string, ImproperDihedralParameter>()
        {

        };
        public static void ImproperDihedralTableComplie()
        {
            var ReachEND = false;
            var path = FileReader.GetUnityStreamingAssetPath("VRMD/FFTables/gaff.dat");
            var reader = new FileReader(path);
            while (!ReachEND)
            {
                var line = reader.ReadLine();
                var array = Regex.Split(line.Trim(), " +");
                string n = "";
                double PK;
                for (int k = 0; k < array.Length; k++)
                {
                    bool check = double.TryParse(array[k], out PK);
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
                        if (hyphen.Length - 1 == 3)
                        {
                            if (array.Length == k + 3 || (array.Length > k + 3  && !double.TryParse(array[k+3], out var _check)))
                            {
                                var PHAZE_check = double.TryParse(array[k + 1], out var PHAZE);
                                var PN_check = double.TryParse(array[k + 2], out double PN);
                                if (PHAZE_check && PN_check && !ImproperDihedralConstantTable.ContainsKey(n))
                                {
                                    ImproperDihedralConstantTable.Add(n, new ImproperDihedralParameter(PK * kcalPerMol, PHAZE * degree, (int)PN));
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }
        public static string ImproperDihedralParameterSearch(string n)
        {
            var n_child = n.Split('-');
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        for (int l = 0; l < 2; l++)
                        {
                            var n_X = new string[n_child.Length];
                            n_X[0] = (i == 0) ? n_child[0] : "X";
                            n_X[1] = (j == 0) ? n_child[1] : "X";
                            n_X[2] = (k == 0) ? n_child[2] : "X";
                            n_X[3] = (l == 0) ? n_child[3] : "X";
                            if (isContain(n_X, out string nX))
                            {
                                return nX;
                            }
                        }
                    }
                }
            }
            return null;
        }
        private static bool isContain(string[] n_X, out string nX)
        {
            nX = n_X[0] + "-" + n_X[1] + "-" + n_X[2] + "-" + n_X[3];
            if (ImproperDihedralFFtables.ImproperDihedralConstantTable.ContainsKey(nX))
            {
                return true;
            }
            nX = n_X[0] + "-" + n_X[3] + "-" + n_X[2] + "-" + n_X[1];
            if (ImproperDihedralFFtables.ImproperDihedralConstantTable.ContainsKey(nX))
            {
                return true;
            }
            nX = n_X[1] + "-" + n_X[0] + "-" + n_X[2] + "-" + n_X[3];
            if (ImproperDihedralFFtables.ImproperDihedralConstantTable.ContainsKey(nX))
            {
                return true;
            }
            nX = n_X[1] + "-" + n_X[3] + "-" + n_X[2] + "-" + n_X[0];
            if (ImproperDihedralFFtables.ImproperDihedralConstantTable.ContainsKey(nX))
            {
                return true;
            }
            nX = n_X[3] + "-" + n_X[1] + "-" + n_X[2] + "-" + n_X[0];
            if (ImproperDihedralFFtables.ImproperDihedralConstantTable.ContainsKey(nX))
            {
                return true;
            }
            nX = n_X[3] + "-" + n_X[0] + "-" + n_X[2] + "-" + n_X[1];
            if (ImproperDihedralFFtables.ImproperDihedralConstantTable.ContainsKey(nX))
            {
                return true;
            }
            return false;
        }
    }
}

