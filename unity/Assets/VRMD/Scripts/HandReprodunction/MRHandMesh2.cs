/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandMR
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MRHandMesh2 : MonoBehaviour
    {
        [SerializeField] List<Transform> Fingerpos;
        MeshRenderer meshRenderer_;
        int[] meshtriangles;
        MeshFilter filter;
        void Start()
        {
            meshRenderer_ = GetComponent<MeshRenderer>();
            meshRenderer_.enabled = false;
            meshtriangles = new int[] {
                0, 5, 4,
                0, 4, 3,
                0, 3, 2,
                0, 2, 1,
                0, 1, 2,
                0, 2, 3,
                0, 3, 4,
                0, 4, 5
            };
            filter = this.gameObject.GetComponent<MeshFilter>();
        }

        void LateUpdate()
        {
                Mesh mesh = new Mesh();
                mesh.vertices = new Vector3[] {
                Fingerpos[0].position,
                Fingerpos[1].position,
                Fingerpos[2].position,
                Fingerpos[3].position,
                Fingerpos[4].position,
                Fingerpos[5].position
            };
                mesh.triangles = meshtriangles;
                mesh.RecalculateNormals();
                filter.mesh = mesh;
                meshRenderer_.enabled = true;
        }
    }
}
