/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using VRMD.MolecularViewer;
using UnityEngine;
using UnityEngine.UI;

public class ActualTemperatureText : MonoBehaviour
{
    [SerializeField] private List<Viewer> Viewers;
    private Text Text;

    void Start()
    {
        this.Text = this.gameObject.GetComponent<Text>();
    }

    void Update()
    {
        this.SetInstantaneousTemperature();
        this.Text.text = this.CurrentTemperatureString;
    }

    private string CurrentTemperatureString => "Current Temp: " + (this.movingAverageTemperature - 273.15f).ToString("F2") + " ℃";
    private float movingAverageTemperature = 273.15f;
    private void SetInstantaneousTemperature()
    {
        var T = 0f;
        foreach (var viewer in this.Viewers)
        {
            T += (float)viewer.Temperature.Kelvin;
        }
        T /= this.Viewers.Count;
        this.movingAverageTemperature *= 0.999f;
        this.movingAverageTemperature += 0.001f * T;
    }



}
