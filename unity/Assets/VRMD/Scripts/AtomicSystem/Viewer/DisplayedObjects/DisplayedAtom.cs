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
    public class DisplayedAtom : MonoBehaviour
    {
        public enum ViewMode { VDW, CovalentBall, };
        [SerializeField] private ViewMode viewMode = ViewMode.VDW;
        public void SetViewMode(ViewMode mode)
        {
            this.viewMode = mode;
            this.transform.localScale = mode switch
            {
                ViewMode.VDW => 2 * new Vector3(this.VdwRadius, this.VdwRadius, this.VdwRadius),
                ViewMode.CovalentBall => 2 * new Vector3(this.CovalentRadius, this.CovalentRadius, this.CovalentRadius),
                _ => throw new Exception(),
            };
        }

        public void Initialize(Atom atom, UnityUnit unityUnit)
        {
            this.Atom = atom;
            this.UnityUnit = unityUnit;
            atomToDisplayedAtomTable[atom] = this;
            this.transform.position = this.UnityUnit.ConvertFromAtomicPosition(this.Atom.Position);
            this.SetViewMode(this.viewMode);
            this.Rigidbody = this.gameObject.AddComponent<Rigidbody>();
            this.Rigidbody.mass = this.UnityUnit.ConvertFromAtomicMass(this.Atom.Mass);
            this.Rigidbody.velocity = this.UnityUnit.ConvertFromAtomicVelocity(this.Atom.Velocity);
            this.VOld = this.Rigidbody.velocity;
            this.Rigidbody.drag = 0.1f;
            this.Rigidbody.useGravity = false;
#if UNITY_EDITOR || !UNITY_IOS || !UNITY_ANDROID
            this.Carriable = this.gameObject.AddComponent<Carriable>();
            this.Carriable.ChaseVelocityConstant = 0.01f;
#endif
            this.GetComponent<Renderer>().material.color = AtomColorTable.Color[this.Atom.Symbol];
        }

        public void Synchronize()
        {
            this.transform.position = this.UnityUnit.ConvertFromAtomicPosition(this.Atom.Position);
        }

        private Vector3 VOld;
        public void UpdateUnityForce()
        {
            var v = this.Rigidbody.velocity;
            var m = this.Rigidbody.mass;
            var FUnity = m * (v - this.VOld) / Time.fixedDeltaTime;
            var F = this.UnityUnit.ConvertToAtomicForce(FUnity);
            this.Atom.AddExternalForce(F.X, F.Y, F.Z);
            this.VOld = v;
        }

        public Atom Atom { get; private set; }
        public float CovalentRadius => this.UnityUnit.ConvertAtomicScaleToUnityLocalScale(this.Atom.CovalentRadius);
        public float VdwRadius => this.UnityUnit.ConvertAtomicScaleToUnityLocalScale(this.Atom.VdwRadius);
        public UnityUnit UnityUnit { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
#if UNITY_EDITOR || !UNITY_IOS || !UNITY_ANDROID
        public Carriable Carriable { get; private set; }
#endif
        private static Dictionary<Atom, DisplayedAtom> atomToDisplayedAtomTable = new Dictionary<Atom, DisplayedAtom>();
        public static DisplayedAtom GetFromAtom(Atom atom) => atomToDisplayedAtomTable[atom];
        public Vector3 Force => this.UnityUnit.ConvertFromAtomicForce(this.Atom.Force);
        public bool IsProtonAcceptor => this.Atom.IsProtonAcceptor;
        public bool IsHydrogen => this.Atom.IsHydrogen;
        public float Mass => this.Rigidbody.mass;
        public void SetExternalForce(double Fx, double Fy, double Fz) => this.Atom.SetExternalForce(Fx, Fy, Fz);
    }
}
