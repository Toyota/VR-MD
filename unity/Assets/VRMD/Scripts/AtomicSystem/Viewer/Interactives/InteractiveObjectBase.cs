/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using VRMD.PureC.MD;
using UnityEngine;

namespace VRMD.MolecularViewer
{
    public abstract class InteractiveObjectBase : MonoBehaviour, IInteractive
    {
        protected static Dictionary<Topology,UnityUnit> UnityUnitDictionary = new Dictionary<Topology,UnityUnit>();
        public virtual void Initialize(Topology topology, UnityUnit unityUnit)
        {
            if(!UnityUnitDictionary.ContainsKey(topology))
            {
                UnityUnitDictionary.Add(topology,unityUnit);
            }
        }
        public abstract void CalculateForce(Topology topology);
    }
}
