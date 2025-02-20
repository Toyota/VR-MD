/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandMR;

public class ScreenType : MonoBehaviour
{
    public enum VRMode { AR0, AR1, VR0, VR1, AR0VR1 }
    [SerializeField] private VRMode mode_ = VRMode.VR0;
    public Transform hologlaCamera;
    public Camera SingleEyeCamera;
    public GameObject screen;
    public GameObject mask;
    public GameObject mrHand;
    public bool isFadeOut = false;
    public bool isFadeIn = false;
    public float arscale_x = 1.0f;
    public float arscale_y = 1.0f;
    public float arscale_z = 1.0f;
    public float vrscale_x = 1.8f;
    public float vrscale_y = 1.0f;
    public float vrscale_z = 1.0f;

    [SerializeField] 
    private int LengthOfFrame = 5;
    [SerializeField] 
    private int NumberOfCorrect = 1;

    private List<int> Judgment = new List<int>();

    private HandVRSphereHand sphereHand_;
    private bool tf_ar = false;
    private bool tf_vr = false;

    void Awake()
    {
        screen.transform.parent = hologlaCamera;
        sphereHand_ = mrHand.GetComponent<HandVRSphereHand>();

        Vector3 arscale = new Vector3(arscale_x, arscale_y, arscale_z);
        Vector3 vrscale = new Vector3(vrscale_x, vrscale_y, vrscale_z);

        switch (mode_)
        {
            case VRMode.AR0:
                screen.transform.position = new Vector3(0.0f, 0.5f, 5.0f);
                screen.transform.localScale = arscale;
                mask.SetActive(false);
                tf_ar = false;
                tf_vr = false;
                break;

            case VRMode.AR1:
                screen.transform.position = new Vector3(0.0f, 0.5f, 5.0f);
                screen.transform.localScale = arscale;
                mask.SetActive(true);
                tf_ar = true;
                tf_vr = false;
                break;

            case VRMode.VR0:
                screen.transform.position = new Vector3(0.0f, 0.5f, 17.5f);
                screen.transform.localScale = vrscale;
                mask.SetActive(false);
                tf_ar = false;
                tf_vr = false;
                break;

            case VRMode.VR1:
                screen.transform.position = new Vector3(0.0f, 0.5f, 17.5f);
                screen.transform.localScale = vrscale;
                mask.SetActive(false);
                tf_ar = false;
                tf_vr = true;
                break;

            case VRMode.AR0VR1:
                screen.transform.position = new Vector3(0.0f, 0.5f, 5.0f);
                screen.transform.localScale = arscale;
                mask.SetActive(false);
                tf_ar = true;
                tf_vr = true;
                break;
        }
    }

    void Start()
    {
        for(int i = 0; i < LengthOfFrame;i++){
            Judgment.Add(0);
        }

    }

    void Update()
    {
        JudgmentUpdate(sphereHand_.IsTrackingHand,Judgment);
        if (JudgeState(Judgment))
        {
            if (tf_ar && !tf_vr && !mask.activeSelf) mask.SetActive(true);
            if (tf_vr && screen.activeSelf)
            {
                if ( isFadeIn ) isFadeIn  = false;
                if (!isFadeOut) isFadeOut = true;
            } 
        }
        else
        {
            if (tf_ar && !tf_vr && mask.activeSelf) mask.SetActive(false);
            if (tf_vr && !screen.activeSelf)
            {
                if (!isFadeIn ) isFadeIn  = true;
                if ( isFadeOut) isFadeOut = false;
            }
        }

        if (SingleEyeCamera.backgroundColor == Color.black)
        {
            SingleEyeCamera.backgroundColor = Color.grey;
        }
    }

    private void JudgmentUpdate(bool tf, List<int> Judgment)
    {
        if (tf) Judgment.Add(1);
        else Judgment.Add(0);
        Judgment.RemoveAt(0);
    }

    private bool JudgeState(List<int> Judgment)
    {
        int sum_tf = 0;
        for (int i = 0; i < LengthOfFrame; i++) sum_tf += Judgment[i];
        if (sum_tf > NumberOfCorrect - 1) return true;
        else return false;
    }
}
