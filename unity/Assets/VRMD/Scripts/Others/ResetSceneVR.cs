/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSceneVR : MonoBehaviour
{
    void Update()
    {
#if UNITY_IOS || UNITY_ANDROID
        if (Input.GetMouseButtonDown(0))
            SceneManager.LoadScene (SceneManager.GetActiveScene().name);
#else

#endif
    }
}
