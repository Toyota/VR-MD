/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using UnityEngine;

namespace VRMD.ChaseBehaviour
{

    [RequireComponent(typeof(Rigidbody))]
    public class ChaseBehaviour : MonoBehaviour
    {

        public GameObject Target;
        public Vector3 PositionAberration;
        public Quaternion RotationAberration = Quaternion.identity;
        public Vector3 UpAberration;
        public Vector3 RightAberration;
        private Rigidbody Rigidbody;

        public float ChaseVelocityConstant = 5.0f;

        void Start()
        {
            this.Rigidbody = this.GetComponent<Rigidbody>();
        }

        void Update()
        {
            var position = this.transform.position;
            var targetPosition = this.Target.transform.position - this.PositionAberration;
            var distance = Vector3.Distance(targetPosition, position);
            var direction = Vector3.Normalize(targetPosition - position);
            var velocity = this.ChaseVelocityConstant * distance * direction;
            this.Rigidbody.velocity = velocity;

            var targetRotation = this.Target.transform.rotation * this.RotationAberration;
            var RotationDifference = targetRotation * Quaternion.Inverse(this.transform.rotation);
            RotationDifference.ToAngleAxis(out var angle, out var axis);
            this.Rigidbody.angularVelocity = 5 * angle * axis / 180f;
        }

        public void SetAbberation()
        {
            this.PositionAberration = this.Target.transform.position - this.transform.position;
            this.RotationAberration = Quaternion.Inverse(this.Target.transform.rotation) * this.transform.rotation;
        }
    }
}