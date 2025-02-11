/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using VRMD.MolecularViewer;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{

    private Text Text;
    [SerializeField] private Viewer Viewer;

    void Start()
    {
        this.Text = this.gameObject.GetComponent<Text>();
    }

    void Update()
    {
        this.Text.text = this.ElapsedTimeString;
    }

    private string ElapsedTimeString => "Elapsed Time: " + (this.Viewer.ElapsedTime * 1e12f).ToString("F2") + " ps";
}
