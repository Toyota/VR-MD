/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using UnityEngine;

public class FocusEmittable : MonoBehaviour
{

    private EmissionBehaviour EmissionBehaviour;

    void Start()
    {
        this.EmissionBehaviour = this.gameObject.AddComponent<EmissionBehaviour>();
        this.EmissionBehaviour.enabled = false;
        var ignitor = this.gameObject.AddComponent<FocusEventIgnitor>();
        ignitor.OnEventStart.AddListener(this.OnFocusStart);
        ignitor.OnEventEnd.AddListener(this.OnFocusEnd);
    }

    void OnFocusStart()
    {
        this.EmissionBehaviour.enabled = true;
    }

    void OnFocusEnd()
    {
        this.EmissionBehaviour.enabled = false;
    }
}
