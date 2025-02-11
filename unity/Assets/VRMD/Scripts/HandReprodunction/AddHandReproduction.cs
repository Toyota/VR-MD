/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace HandMR
{
    public class AddHandReproduction : MonoBehaviour
    {
        [SerializeField] GameObject targetobj;
        [SerializeField] GameObject reference;

        void Start()
        {
            Reproduction();
        }

        void Reproduction()
        {
            for (int i = 0; i < reference.transform.childCount; i++)
            {
                if (reference.transform.GetChild(i).gameObject.name.Contains("Finger"))
                {
                    HandVRPosition2 handVRPosition2 = targetobj.transform.GetChild(i).gameObject.AddComponent<HandVRPosition2>();
                    handVRPosition2.Initialize(reference.transform.GetChild(i).gameObject, targetobj.transform.GetChild(i).gameObject);
                }
            }
        }
    }
}