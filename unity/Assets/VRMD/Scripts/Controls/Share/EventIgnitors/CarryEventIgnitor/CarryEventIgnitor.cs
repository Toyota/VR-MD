/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using UnityEngine;

public class CarryEventIgnitor : EventIgnitorBase
{
    void Start()
    {
        this.OnEvent.AddListener(this.OnCarry);
    }

    void OnCarry()
    {
        if (Input.GetMouseButtonUp(0))
        {
            this.OnEventEnd.Invoke();
        }
    }

    public void OnMouseDown()
    {
        this.OnEventStart.Invoke();
    }
}

