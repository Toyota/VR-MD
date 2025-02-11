/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System;
using VRMD.PureC.MD;
using UnityEngine;

namespace VRMD.MolecularViewer
{
    public class DisplayedBond : MonoBehaviour
    {
        public void Initialize(Bond bond, DisplayedAtom atom1, DisplayedAtom atom2)
        {
            this.Bond = bond;
            this.Atom1 = atom1;
            this.Atom2 = atom2;
            var collider = this.gameObject.GetComponent<Collider>();
            collider.isTrigger = true;
            this.GetComponent<Renderer>().material.color = Color.gray;
        }

        public void Refresh()
        {
            var x1 = this.Atom1.transform.position;
            var x2 = this.Atom2.transform.position;
            this.transform.position = 0.5f * (x1 + x2);
            this.transform.up = x1 - x2;
            var rScale = Math.Min(this.Atom1.CovalentRadius, this.Atom2.CovalentRadius);
            var yLossyScale = 0.5f * Vector3.Distance(x1, x2);
            var parentLossyScale = this.transform.parent != null ? this.transform.parent.lossyScale.x : 1.0f;
            this.transform.localScale = new Vector3(rScale, yLossyScale / parentLossyScale, rScale);
        }

        public Bond Bond { get; private set; }
        public UnityUnit UnityUnit { get; private set; }
        public DisplayedAtom Atom1;
        public DisplayedAtom Atom2;

        public bool IsProtonDonor => this.Bond.IsProtonDonor;
        public bool Contains(DisplayedAtom atom) => this.Bond.Contains(atom.Atom);
        public DisplayedAtom GetHeavierAtom() => this.Atom1.Mass > this.Atom2.Mass ? this.Atom1 : this.Atom2;
        public DisplayedAtom GetLighterAtom() => this.Atom1.Mass > this.Atom2.Mass ? this.Atom2 : this.Atom1;
    }
}
