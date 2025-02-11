/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{
    public class NVTIntegrator : IntegratorInterface
    {
        private readonly Topology Topology;
        public double RelaxTime { get; }
        public BoxMuller BoxMuller = new BoxMuller();
        private const double kB = 1.380649e-23;

        public NVTIntegrator(Topology topology, double T0, double relaxTime)
        {
            this.Topology = topology;
            this.Topology.Temperature.Preset(T0);
            this.RelaxTime = relaxTime;
        }

        public void Update(double dt, InteractionForce force)
        {
            var dt2 = 0.5 * dt;
            var T0 = this.Topology.Temperature.PresetValue;
            foreach (var atom in this.Topology.AtomList) atom.UpdateVelocity(dt2);
            foreach (var atom in this.Topology.AtomList) atom.UpdatePosition(dt2);
            foreach (var atom in this.Topology.AtomList)
            {
                var m = atom.Mass;
                var p = m * atom.Velocity;
                var gamma = 1.0 / this.RelaxTime;
                var attenuation = System.Math.Exp(-gamma * dt);
                var R = new Vector3(BoxMuller.Get(), BoxMuller.Get(), BoxMuller.Get());
                p = attenuation * p + System.Math.Sqrt(kB * T0 * (1.0 - attenuation * attenuation) * m) * R;
                atom.SetVelocity(p / m);
            }
            foreach (var atom in this.Topology.AtomList) atom.UpdatePosition(dt2);
            force.CalculateTotalForce();
            foreach (var atom in this.Topology.AtomList) atom.AddForce(atom.ExternalForce);
            foreach (var atom in this.Topology.AtomList) atom.UpdateVelocity(dt2);
        }
    }
}
