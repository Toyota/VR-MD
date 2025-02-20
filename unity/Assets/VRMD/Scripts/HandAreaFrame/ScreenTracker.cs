/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using VRMD.MolecularViewer;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTracker : MonoBehaviour
{
    [SerializeField] private float DistanceFromCamera = 0.4f;
    public Camera Camera;

    float DefaultWidth;
    float DefaultHeight;
    Vector3 DefaultPosition;

    void Start()
    {
        var defaultScale = this.transform.localScale;
        this.DefaultWidth = defaultScale.x;
        this.DefaultHeight = defaultScale.y;
        this.DefaultPosition = this.transform.position;
    }

    void Update()
    {
        this.UpdatePosition();
    }

    void UpdatePosition()
    {
        var viewLeftTop = this.Camera.ViewportToWorldPoint(new Vector3(0f, 0f, DistanceFromCamera));
        var viewRightTop = this.Camera.ViewportToWorldPoint(new Vector3(1f, 0f, DistanceFromCamera));
        var viewRightBottom = this.Camera.ViewportToWorldPoint(new Vector3(1f, 1f, DistanceFromCamera));
        var width = Vector3.Magnitude(viewRightTop - viewLeftTop);
        var height = Vector3.Magnitude(viewRightBottom - viewRightTop);
        var localScale = new Vector3(width*this.DefaultWidth, height*this.DefaultHeight, 1.0f);
        this.transform.localScale = localScale;

        var cameraTransform = this.Camera.transform;
        var cameraPosition = cameraTransform.position;
        var position = cameraPosition + DistanceFromCamera * cameraTransform.forward;
        this.transform.position = new Vector3(
            position.x+localScale.x*this.DefaultPosition.x,
            position.y+localScale.y*this.DefaultPosition.y,
            position.z
        );
        var direction = position-cameraPosition;
        this.transform.rotation = Quaternion.LookRotation(direction,cameraTransform.up);
    }
}
