/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using UnityEngine;

namespace VRMD.Controls
{
    public class Pointer : MonoBehaviour
    {
        public GameObject AsistantPlane;
        private Vector3 Reference;
        private Quaternion ReferenceRotation;

        private int AsistantLayer;

        private Vector3 ViewportPoint
        {
            get
            {
                return Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }
        }

        private Vector3 ViewportPointDifference
        {
            get
            {
                return this.ViewportPoint - this.Reference;
            }
        }

        private Ray ScreenPointRay
        {
            get
            {
                return Camera.main.ScreenPointToRay(Input.mousePosition);
            }
        }

        void Awake()
        {
            this.AsistantLayer = LayerMask.NameToLayer("MappingAsistant");
            this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            this.AsistantPlane.layer = LayerMask.NameToLayer("Ignore Raycast");
            Pointer.main = this;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                this.Reference = this.ViewportPoint;
                this.ReferenceRotation = this.transform.rotation;
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                this.Reference = this.ViewportPoint;
                this.ReferenceRotation = this.AsistantPlane.transform.rotation;
                this.AsistantPlane.transform.position = this.transform.position;
                this.AsistantPlane.transform.rotation = Camera.main.transform.rotation;
                this.AsistantPlane.transform.rotation *= Quaternion.AngleAxis(-90, Camera.main.transform.right);
            }

            if (Input.GetKey(KeyCode.LeftAlt))
            {
                var rotation = this.ViewportToRotation();
                this.transform.rotation = rotation * this.ReferenceRotation;
            }
            else
            {

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    var rotation = this.ViewportToRotation();
                    this.AsistantPlane.transform.rotation = rotation * this.ReferenceRotation;
                }
                UpdatePosition();
            }
        }

        Quaternion ViewportToRotation()
        {
            var d = this.ViewportPointDifference;
            var HRotation = Quaternion.AngleAxis(-180f / 0.2f * d.x, Camera.main.transform.up);
            var VRotation = Quaternion.AngleAxis(180f / 0.2f * d.y, Camera.main.transform.right);
            return HRotation * VRotation;
        }

        public void UpdateAsistantPlanePosition()
        {
            if (Physics.Raycast(this.ScreenPointRay, out RaycastHit hit))
            {
                this.AsistantPlane.transform.position = hit.point;
                this.transform.position = hit.point;
            }
        }

        public void UpdatePosition()
        {
            this.AsistantPlane.layer = this.AsistantLayer;
            var layerMask = 1 << this.AsistantLayer;
            var isHit = Physics.Raycast(this.ScreenPointRay, out RaycastHit hit, Mathf.Infinity, layerMask);
            this.AsistantPlane.layer = LayerMask.NameToLayer("Ignore Raycast");
            this.transform.position = isHit ? hit.point : this.transform.position;
        }

        static private Pointer main;
        static public Pointer Main
        {
            get
            {
                return main;
            }
        }
    }
}