/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using UnityEngine;

namespace HandMR
{
    public class AddFilter : MonoBehaviour
    {
        [SerializeField] GameObject targetobj;
        [SerializeField] GameObject referenceobj;
        [SerializeField] Material transparent;
        void Start()
        {
            int childCount = referenceobj.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Transform childTransform = referenceobj.transform.GetChild(i);
                MeshRenderer childMeshRenderer = childTransform.gameObject.GetComponent<MeshRenderer>();
                LineRenderer childLineRenderer = childTransform.gameObject.GetComponent<LineRenderer>();

                if (childMeshRenderer != null)
                {
                    childMeshRenderer.material = transparent;
                }

                if (childLineRenderer != null)
                {
                    childLineRenderer.material = transparent;
                }
            }
            addFilter();
        }

        void addFilter()
        {
            int n = (int)Mathf.Min(referenceobj.transform.childCount, targetobj.transform.childCount);
            for (int i = 0; i < n; i++)
            {
                KalmanFilter.AddKalmanFilter(referenceobj, targetobj, i);
            }
        }
    }
}