/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using UnityEngine;

namespace VRMD.MolecularViewer
{
    public static class AtomColorTable
    {
        public static readonly Dictionary<string, Color> Color = new Dictionary<string, Color>()
        {
            {"H",UnityEngine.Color.white},
            {"He",UnityEngine.Color.white},
            {"Li",UnityEngine.Color.white},
            {"Be",UnityEngine.Color.white},
            {"B",UnityEngine.Color.white},
            {"C",UnityEngine.Color.cyan},
            {"N",UnityEngine.Color.blue},
            {"O",UnityEngine.Color.red},
            {"F",UnityEngine.Color.white},
            {"Ne",UnityEngine.Color.white},
            {"Na",UnityEngine.Color.yellow},
            {"Cl",UnityEngine.Color.green},
        };
    }
}
