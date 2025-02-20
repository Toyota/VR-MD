/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandMR
{
    public class HandVRPosition2 : MonoBehaviour
    {
        private Transform reference;
        private Transform thisobject;

        public HandVRPosition2 Initialize(GameObject _reference, GameObject _thisobject)
        {
            this.reference = _reference.transform;
            this.thisobject = _thisobject.transform;
            return this;
        }

        private void Update()
        {
            var pos = this.reference.position;
            var rot = this.reference.rotation;
            this.thisobject.position = pos + new Vector3(0, 0f, 0);
            this.thisobject.rotation = rot;
        }
    }
}
