/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using UnityEngine;

public class PlatformChanger : MonoBehaviour
{
    public DisplayModes DisplayMode;
    public List<GameObject> XROnly;
    public List<GameObject> EditorOnly;

    public InformationPanel2 InformationPanel;
    public Camera XRInformationCamera;
    public Camera EditorInformationCamera;

    public enum DisplayModes
    {
        Auto,
        XR,
        Editor,
    };

    void Awake()
    {
        if (this.DisplayMode == DisplayModes.Auto)
        {
#if UNITY_EDITOR
            this.DisplayMode = DisplayModes.Editor;
#elif UNITY_IOS
            this.DisplayMode = DisplayModes.XR;
#elif UNITY_ANDROID   
            this.DisplayMode = DisplayModes.XR;
#else
            this.DisplayMode = DisplayModes.Editor;
#endif
        }
        foreach (var obj in XROnly)
        {
            obj.SetActive(this.DisplayMode == DisplayModes.XR);
        }
        foreach (var obj in EditorOnly)
        {
            obj.SetActive(this.DisplayMode == DisplayModes.Editor);
        }
        var camera = new Dictionary<DisplayModes, Camera>()
        {
            {DisplayModes.XR,XRInformationCamera},
            {DisplayModes.Editor,EditorInformationCamera},
        };
        InformationPanel.Camera = camera[this.DisplayMode];
    }
}
