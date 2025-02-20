/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tips : MonoBehaviour
{

    [SerializeField] private float TipsTime = 10f;
    private float DeltaTime;
    private Text Text;

    private List<string> TipsMessages = new List<string>()
    {
        // If you enter the precautions here, they will be displayed on the VR panel.　
        "Molecules can be observed freely.",
        "Your hand within the yellow frame \ntriggers VR mode.",
        "The hand model allows for \ntouching and holding molecules.",
        "Please adjust the left bar \nto set the temperature.",

    };

    void Start()
    {
        this.Text = this.gameObject.GetComponent<Text>();
        this.Text.text = this.SelectTips();
    }

    void Update()
    {
        this.DeltaTime += Time.deltaTime;
        if (this.DeltaTime > this.TipsTime)
        {
            this.Text.text = this.SelectTips();
            this.DeltaTime = 0f;
        }
    }

    private string SelectTips() => TipsMessages[Random.Range(0, this.TipsMessages.Count)];
}
