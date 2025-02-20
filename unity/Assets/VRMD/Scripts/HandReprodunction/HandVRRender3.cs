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
    public class HandVRRender3 : MonoBehaviour
    {
        [SerializeField] List<Transform> ids;

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
            Vector3[] positions = new Vector3[ids.Count];
                for (int loop = 0; loop < ids.Count; loop++)
                {
                    positions[loop] = ids[loop].localPosition;
                }
            this.GetComponent<LineRenderer>().SetPositions(positions);
        }
    }
}
