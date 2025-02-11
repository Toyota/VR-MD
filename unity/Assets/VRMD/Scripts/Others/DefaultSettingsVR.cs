/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using UnityEngine;

public class DefaultSettingsVR : MonoBehaviour
{
    public int GoggleMode = 0;
    public int HandDetectionMode = 0;

    void Awake()
    {

#if UNITY_IOS || UNITY_ANDROID
        PlayerPrefs.SetInt("HandMR_GoogleMode", GoggleMode);
        PlayerPrefs.SetInt("HandMR_HandDetectionMode", HandDetectionMode);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

#elif UNITY_EDITOR

#endif
    }
}
