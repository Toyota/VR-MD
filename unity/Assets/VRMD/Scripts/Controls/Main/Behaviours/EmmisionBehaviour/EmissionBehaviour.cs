/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using UnityEngine;

public class EmissionBehaviour : MonoBehaviour
{
    private Material Material;
    private static Material EmissionResorce;

    void OnEnable()
    {
        if (this.Material == null)
        {
            this.Material = this.GetComponent<MeshRenderer>().material;
            this.Material.EnableKeyword("_EMISSION");
        }
        if (EmissionResorce == null)
        {
            EmissionResorce = Resources.Load<Material>("VRMD/emission");
        }
    }

    void OnDisable()
    {
        Material.SetColor("_EmissionColor", Color.white * 0.0f);
    }
    
    void Update()
    {
        Material.SetColor("_EmissionColor", Color.white * 0.5f);
    }

}
