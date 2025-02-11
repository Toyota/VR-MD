/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandMR
{
    public class Synchronize : MonoBehaviour
    {
        [SerializeField] HandVRSphereHand _handVRSphereHand;
        [SerializeField] GameObject HandMrtarget;

        [SerializeField] 
        private int LengthOfFrame = 5;
        [SerializeField] 
        private int NumberOfCorrect = 1;
        
        private List<int> Judgment = new List<int>();

        void Start(){
            for(int i = 0; i < LengthOfFrame;i++) Judgment.Add(0);
        }
         private void JudgmentUpdate()
        {
            if (_handVRSphereHand.IsTrackingHand) Judgment.Add(1);
            else Judgment.Add(0);
            Judgment.RemoveAt(0);
        }
        private bool JudgeState()
        {
            int sum_dis = 0;
            for (int i = 0; i < LengthOfFrame; i++) sum_dis += Judgment[i];
            if (sum_dis > NumberOfCorrect - 1) return true;
            else return false;
        }
        void LateUpdate()
        {
            JudgmentUpdate();
            HandMrtarget.SetActive(JudgeState());
        }
    }

}