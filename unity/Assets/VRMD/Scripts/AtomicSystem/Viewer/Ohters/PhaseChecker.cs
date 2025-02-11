/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using UnityEngine;

namespace VRMD.MolecularViewer
{
    [RequireComponent(typeof(Viewer))]
    public class PhaseChecker : MonoBehaviour
    {

        private Viewer Viewer;
        private int nWater;
        void Start()
        {
            this.Viewer = this.GetComponent<Viewer>();
            this.nWater = this.Viewer.AtomList.Count / 3;
        }

        void Update()
        {
            var nHB = this.Viewer.ApparentHBondList.Count;
            if (nHB < 0.2 * 2 * this.nWater)
            {
                Debug.Log("gas");
            }
            else
            {
                var t = this.Viewer.ElapsedTime;
                var nDissociated = this.GetNumberOfDissociation(t - 0.5e-12);
                if (nDissociated > 0)
                {
                    Debug.Log("Liquid");
                }
                else
                {
                    Debug.Log("Solid");
                }
            }
        }

        private int GetNumberOfDissociation(double t0)
        {
            var result = 0;
            foreach (var HBond in this.Viewer.LatentHBondList)
            {
                var tBirth = HBond.DateOfBirth;
                var tDeath = HBond.DateOfDeath;
                if (tBirth < t0 && t0 < tDeath)
                {
                    result++;
                }
            }
            return result;
        }
    }
}