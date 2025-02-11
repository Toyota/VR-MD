/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using VRMD.PureC.MD;
using UnityEngine;

namespace VRMD.MolecularViewer
{
    public class InteractiveObjectList : List<IInteractive>
    {
        public InteractiveObjectList(Topology topology,UnityUnit unityUnit)
        {
            var interactives = Object.FindObjectsOfType<InteractiveObjectBase>(true);
            foreach (var interactive in interactives)
            {
                interactive.Initialize(topology,unityUnit);
                this.Add(interactive);
            }
        }
    }
}