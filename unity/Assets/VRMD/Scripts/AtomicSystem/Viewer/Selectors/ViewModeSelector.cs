/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRMD.MolecularViewer
{
    [Serializable]
    public class ViewModeSelector
    {
        [SerializeField] private ViewMode viewMode = ViewMode.BallsAndSticks;

        public enum ViewMode { VDW, BallsAndSticks };
        public void Apply(List<DisplayedAtom> atomList)
        {
            switch (this.viewMode)
            {
                case ViewMode.VDW:
                    foreach (var atom in atomList) atom.SetViewMode(DisplayedAtom.ViewMode.VDW);
                    break;
                case ViewMode.BallsAndSticks:
                    foreach (var atom in atomList) atom.SetViewMode(DisplayedAtom.ViewMode.CovalentBall);
                    break;
            }
        }

    }
}