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
    public class IntegratorSelector
    {
        public enum IntegratorMode { NVE, NVT, }
        [SerializeField] private IntegratorMode integratorMode = IntegratorMode.NVT;
        [SerializeField] private double T0 = 100.0;
        [SerializeField] private double RelaxTime = 1e-13;

        public IntegratorInterface Generate(Topology topology) => this.integratorMode switch
        {
            IntegratorMode.NVT => this.CreateNVTIntegrator(topology),
            _ => new NVEIntegrator(topology),
        };

        private NVTIntegrator CreateNVTIntegrator(Topology topology) => new NVTIntegrator(topology, this.T0, this.RelaxTime);
    }
}