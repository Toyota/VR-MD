/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using VRMD.PureC.MD;
using UnityEngine;

namespace VRMD.MolecularViewer
{
    public class DisplayedAtomList : List<DisplayedAtom>
    {
        public DisplayedAtomList(GameObject parent, IReadOnlyCollection<Atom> atomList, UnityUnit unityUnit)
        {
            this.Clear();
            foreach (var atom in atomList)
            {
                var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                obj.transform.parent = parent.transform;
                obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                var displayedAtom = obj.AddComponent<DisplayedAtom>();
                displayedAtom.gameObject.layer = LayerMask.NameToLayer("Atoms");
                if (displayedAtom.gameObject.layer < 0) Debug.LogError("error");
                displayedAtom.Initialize(atom, unityUnit);
                this.Add(displayedAtom);
            }
        }
    }
}
