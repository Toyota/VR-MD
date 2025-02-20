/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
public class FocusEventIgnitor : EventIgnitorBase
{

    public void OnMouseEnter()
    {
        OnEventStart?.Invoke();
    }

    public void OnMouseExit()
    {
        OnEventEnd?.Invoke();
    }
}