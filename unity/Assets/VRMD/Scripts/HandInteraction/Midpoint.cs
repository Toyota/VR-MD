/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using UnityEngine;

public class Midpoint : MonoBehaviour
{

    public Transform Reference1;
    public Transform Reference2;

    void FixedUpdate()
    {
        this.transform.position = 0.5f * (this.Reference1.position + this.Reference2.position);
    }
}
