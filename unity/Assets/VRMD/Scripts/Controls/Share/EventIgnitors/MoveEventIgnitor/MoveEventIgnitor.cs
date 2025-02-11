/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using UnityEngine;

public class MoveEventIgnitor : EventIgnitorBase
{
    public float dz;

    void Update()
    {
        this.dz = Input.GetAxis("Mouse ScrollWheel");
        if (this.dz != 0f)
        {
            this.OnEventStart.Invoke();
            this.OnEventEnd.Invoke();
        }
    }
}
