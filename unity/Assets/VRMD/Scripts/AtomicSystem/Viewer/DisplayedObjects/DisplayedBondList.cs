/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using VRMD.PureC.MD;
using UnityEngine;

namespace VRMD.MolecularViewer
{
    public class DisplayedBondList : List<DisplayedBond>
    {
        public DisplayedBondList(GameObject parent, IReadOnlyCollection<Bond> bondList)
        {
            this.Clear();
            foreach (var bond in bondList)
            {
                var obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                obj.transform.parent = parent.transform;
                obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                var displayedBond = obj.AddComponent<DisplayedBond>();
                displayedBond.gameObject.layer = LayerMask.NameToLayer("Atoms");
                if (displayedBond.gameObject.layer < 0) Debug.LogError("error");
                var atom1 = DisplayedAtom.GetFromAtom(bond.Atom1);
                var atom2 = DisplayedAtom.GetFromAtom(bond.Atom2);
                displayedBond.Initialize(bond, atom1, atom2);
                this.Add(displayedBond);
            }
        }

        public void Update()
        {
            foreach (var bond in this) bond.Refresh();
        }
    }
}
