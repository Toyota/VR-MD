/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System;
using System.Collections.Generic;
using VRMD.MolecularViewer;
using UnityEngine;
using UnityEngine.UI;

public class TemperatureBar : MonoBehaviour
{
    [SerializeField] private List<Viewer> Viewers;

    private Slider Slider;

    void Start()
    {
        this.Slider = this.transform.GetComponent<Slider>();
        if (this.Viewers[0] == null) throw new Exception();
        this.Slider.value = (float)this.Viewers[0].Temperature.PresetValue;
    }

    void Update()
    {
        foreach (var viewer in this.Viewers)
        {
            viewer.Temperature.Preset(this.Slider.value);
        }
    }
}
