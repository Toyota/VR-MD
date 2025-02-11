/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using VRMD.ChaseBehaviour;
using VRMD.Controls;
using UnityEngine;

public class Carriable : MonoBehaviour
{

    public Pointer Pointer;
    public float ChaseVelocityConstant = 5.0f;

    private ChaseBehaviour ChaseBehaviour;
    private EmissionBehaviour EmissionBehaviour;

    void Awake()
    {
        if (this.Pointer == null)
        {
            this.Pointer = Pointer.Main;
        }
        if (this.Pointer != null)
        {
            this.ChaseBehaviour = this.gameObject.AddComponent<ChaseBehaviour>();
            this.ChaseBehaviour.Target = this.Pointer.gameObject;
            this.ChaseBehaviour.enabled = false;
            var carryEventIgnitor = this.gameObject.AddComponent<CarryEventIgnitor>();
            carryEventIgnitor.OnEventStart.AddListener(this.OnCarryStart);
            carryEventIgnitor.OnEventEnd.AddListener(this.OnCarryEnd);
            carryEventIgnitor.OnEvent.AddListener(this.OnCarry);
            this.EmissionBehaviour = this.gameObject.AddComponent<EmissionBehaviour>();
            this.EmissionBehaviour.enabled = false;
        }

        this.gameObject.AddComponent<FocusEmittable>();
    }

    void OnCarryStart()
    {
        this.ChaseBehaviour.ChaseVelocityConstant = this.ChaseVelocityConstant;
        this.Pointer.UpdateAsistantPlanePosition();
        this.ChaseBehaviour.SetAbberation();
        this.Pointer.UpdatePosition();
    }

    void OnCarryEnd()
    {

        this.ChaseBehaviour.enabled = false;
        this.EmissionBehaviour.enabled = false;
    }

    void OnCarry()
    {
        this.EmissionBehaviour.enabled = true;
        this.ChaseBehaviour.enabled = true;

    }
}
