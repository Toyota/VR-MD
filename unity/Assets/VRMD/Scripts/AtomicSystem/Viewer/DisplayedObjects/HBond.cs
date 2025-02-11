/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using UnityEngine;

namespace VRMD.MolecularViewer
{
    public class HBond : MonoBehaviour
    {
        public DisplayedAtom ProtonAcceptingAtom;
        private DisplayedAtom HAtom;
        private DisplayedAtom ProtonDonatingAtom;

        private GameObject rayObject;
        private LineRenderer lineRenderer;

        public void Initialize(DisplayedBond protonDonatingBond, DisplayedAtom protonAcceptingAtom, GameObject _rayObject)
        {
            this.ProtonAcceptingAtom = protonAcceptingAtom;
            this.rayObject = _rayObject;
            this.InstantiateLine();
            this.HAtom = protonDonatingBond.GetLighterAtom();
            this.ProtonDonatingAtom = protonDonatingBond.GetHeavierAtom();
            this.lineRenderer.enabled = false;
        }

        public void InstantiateLine()
        {
            this.lineRenderer = rayObject.AddComponent<LineRenderer>();
            this.lineRenderer.material = (Material)Resources.Load("Materials/CyanDashLine");
            this.lineRenderer.startWidth = 0.005f;
            this.lineRenderer.endWidth = 0.005f;
        }

        public void Refresh(double elapsedTime)
        {
            if (this.IsBonded())
            {
                if (this.IsHidden)
                {
                    this.DateOfBirth = elapsedTime;
                }
                this.ShowLine();
            }
            else if (this.IsShown)
            {
                this.HideLine();
                this.DateOfDeath = elapsedTime;
            }
        }

        private float BondStrength()
        {
            var r = this.DistanceInAtomicUnit;
            var _degree = ((float)this.Angle.Degree - 140) / 40;
            var _distance = Mathf.Abs(((float)r - 1.93e-10f) / 0.34e-10f);
            return Mathf.Min(_degree, _distance);
        }

        public void ShowLine()
        {
            this.lineRenderer.enabled = true;
            var positions = new UnityEngine.Vector3[]{
                HAtom.transform.position,
                ProtonAcceptingAtom.transform.position,
            };
            this.lineRenderer.SetPositions(positions);
            this.lineRenderer.startWidth = 0.009f * this.BondStrength();
            this.lineRenderer.endWidth = 0.009f * this.BondStrength();
        }

        public void HideLine() => this.lineRenderer.enabled = false;
        public double DistanceInAtomicUnit => (this.ProtonAcceptingAtom.Atom.Position - this.HAtom.Atom.Position).Magnitude;
        private VRMD.PureC.Math.Angle Angle => VRMD.PureC.Math.Angle.CreateFromVectors(
            this.ProtonDonatingAtom.Atom.Position, this.HAtom.Atom.Position, this.ProtonAcceptingAtom.Atom.Position
        );
        public bool IsBonded() =>
            this.DistanceInAtomicUnit > 1.59e-10f && this.DistanceInAtomicUnit < 2.27e-10f && this.Angle.Degree >= 140;
        public bool IsShown => this.lineRenderer.enabled;
        public bool IsHidden => !this.lineRenderer.enabled;
        public double DateOfBirth = -200.0;
        public double DateOfDeath = -100.0;
    }
}
