/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System;
using System.Collections.Generic;
using VRMD.PureC.MD;
using UnityEngine;

namespace VRMD.MolecularViewer
{

    [Serializable]
    public class HydrogenBondList
    {
        [SerializeField] bool Draw = true;
        [SerializeField] int UpdateFrequency = 4;

        public List<HBond> LatentBondList { get; } = new List<HBond>();
        private int updateCount = 0;

        public void Initialize(DisplayedAtomList atomList, DisplayedBondList bondList, Transform parent)
        {
            if (this.Draw)
            {
                this.LatentBondList.Clear();
                var protonAcceptors = new List<DisplayedAtom>();
                var protonDonors = new List<DisplayedBond>();
                foreach (var atom in atomList) if (atom.IsProtonAcceptor) protonAcceptors.Add(atom);
                foreach (var bond in bondList) if (bond.IsProtonDonor) protonDonors.Add(bond);
                foreach (var bond in protonDonors)
                {
                    foreach (var atom in protonAcceptors)
                    {
                        if (!bond.Contains(atom))
                        {
                            var rayObject = new GameObject("RayObject");
                            rayObject.transform.parent = parent;
                            var displayedHBond = rayObject.AddComponent<HBond>();
                            displayedHBond.Initialize(bond, atom, rayObject);
                            this.LatentBondList.Add(displayedHBond);
                        }
                    }
                }
            }
        }

        public void Update(double elapsedTime)
        {
            this.updateCount++;
            if (this.updateCount == this.UpdateFrequency)
            {
                foreach (var hBond in this.LatentBondList) hBond.Refresh(elapsedTime);
                this.updateCount = 0;
            }
        }

        public List<HBond> ApparentBondList
        {
            get
            {
                var result = new List<HBond>();
                foreach (var hBond in this.LatentBondList)
                {
                    if (hBond.IsShown) result.Add(hBond);
                }
                return result;
            }
        }
    }
}