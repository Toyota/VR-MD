/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using VRMD.MolecularViewer;
using UnityEngine;
using UnityEngine.UI;

public class PresetTemperatureText : MonoBehaviour
{
    private Text Text;
    [SerializeField] private Viewer Viewer;

    void Start()
    {
        this.Text = this.gameObject.GetComponent<Text>();
    }

    void Update()
    {
        this.Text.text = this.PresetTemperatureString;
    }

    private string PresetTemperatureString => "Set Temp: " + (this.Viewer.Temperature.PresetValue - 273.15f).ToString("F2") + " ℃";

}
