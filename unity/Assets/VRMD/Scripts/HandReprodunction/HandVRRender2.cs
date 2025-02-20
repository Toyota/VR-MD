/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandMR
{
    [RequireComponent(typeof(LineRenderer))]
    public class HandVRRender2 : MonoBehaviour
    {
        [SerializeField] Transform ids0;
        [SerializeField] Transform ids1;
        [SerializeField] Transform ids2;
        [SerializeField] Transform ids3;
        [SerializeField] Transform ids4;

         private void Start()
        {
            this.GetComponent<LineRenderer>().useWorldSpace = true;  
        }

        private void LateUpdate()
        {
            SetLines();
        }

        private void SetLines()
        {
            Vector3[] positions = new Vector3[5];
            positions[0] = ids0.position;
            positions[1] = ids1.position;
            positions[2] = ids2.position;
            positions[3] = ids3.position;
            positions[4] = ids4.position;
            this.GetComponent<LineRenderer>().SetPositions(positions);
        }
    }
}
