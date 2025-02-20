/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private MoveEventIgnitor MoveEventIgnitor;

    void Start()
    {
        this.MoveEventIgnitor = this.gameObject.AddComponent<MoveEventIgnitor>();
        this.MoveEventIgnitor.OnEventStart.AddListener(OnEventStart);
    }

    void OnEventStart()
    {
        var dz = this.MoveEventIgnitor.dz;
        var forward = this.transform.forward;
        this.transform.position += forward * dz;
    }
}

