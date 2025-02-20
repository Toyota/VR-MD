/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using UnityEngine;

public class InformationPanel2 : MonoBehaviour
{
    public Camera Camera;

    private Vector3 DefaultPosition;
    private Quaternion DefaultRotation;
    private Transform CameraTransform;

    void Start()
    {
        this.DefaultPosition = this.transform.position;
        this.DefaultRotation = this.transform.rotation;
        this.CameraTransform = this.Camera.transform;
    }

    void Update()
    {
        var up = Vector3.up;
        var cross1 = Vector3.Cross(this.CameraTransform.right, up);
        var cross2 = Vector3.Cross(this.CameraTransform.up, up);
        var forward = cross1.magnitude > cross2.magnitude ? cross1.normalized : cross2.normalized;
        var rotation = Quaternion.FromToRotation(Vector3.forward, forward);
        var x = this.DefaultPosition.z * forward.x;
        var y = this.DefaultPosition.y;
        var z = this.DefaultPosition.z * forward.z;
        var defaultPosition = new Vector3(x, y, z);
        this.transform.position = defaultPosition + this.CameraTransform.position;
        this.transform.rotation = rotation * this.DefaultRotation;
    }
}
