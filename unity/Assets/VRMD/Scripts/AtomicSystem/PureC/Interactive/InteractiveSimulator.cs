/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;

namespace VRMD.PureC.MD
{
    public class InteractiveSimulator : Simulator
    {
        private readonly List<IInteractive> Interactives;

        public InteractiveSimulator(
            Topology topology, IntegratorInterface integrator, List<IInteractive> interactives
        ) : base(topology, integrator)
        {
            this.Interactives = interactives;
        }

        public override void Update(double dt)
        {
            foreach (var interactive in this.Interactives) interactive.CalculateForce(this.Topology);
            base.Update(dt);
        }
    }

    public interface IInteractive
    {
        void CalculateForce(Topology topology);
    }
}
