/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System;
using VRMD.PureC.MD;
using UnityEngine;

namespace VRMD.MolecularViewer
{

    [Serializable]
    public class FileSelector
    {
        [SerializeField] private string MolecularFile = "";
        [SerializeField] private string VelocityFile = "";

        public Topology Read()
        {
            var topology = TopologyBuilder.Build(this.MolecularFile);
            topology.ReadVelocities(VelocityFile);
            return topology;
        }
    }
}