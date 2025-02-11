/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
namespace VRMD.PureC.MD
{
    public class NVEIntegrator : IntegratorInterface
    {
        private readonly Topology Topology;

        public NVEIntegrator(Topology topology)
        {
            this.Topology = topology;
        }

        public void Update(double dt, InteractionForce force)
        {
            var dt2 = 0.5 * dt;
            foreach (var atom in this.Topology.AtomList) atom.UpdateVelocity(dt2);
            foreach (var atom in this.Topology.AtomList) atom.UpdatePosition(dt);
            force.CalculateTotalForce();
            foreach (var atom in this.Topology.AtomList) atom.AddForce(atom.ExternalForce);
            foreach (var atom in this.Topology.AtomList) atom.UpdateVelocity(dt2);
        }
    }

}
