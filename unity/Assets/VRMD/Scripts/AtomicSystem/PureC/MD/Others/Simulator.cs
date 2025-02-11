/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
namespace VRMD.PureC.MD
{
    public class Simulator
    {
        public Topology Topology { get; }
        private IntegratorInterface Integrator { get; }
        private InteractionEnergy InteractionEnergy { get; }
        private InteractionForce InteractionForce { get; }

        public double ElapsedTime = 0.0;

        public virtual void Update(double dt)
        {
            this.ElapsedTime += dt;
            this.Integrator.Update(dt, this.InteractionForce);
        }

        public Simulator(Topology topology, IntegratorInterface integrator)
        {
            this.Topology = topology;
            this.Integrator = integrator;
            this.InteractionEnergy = new InteractionEnergy(this.Topology);
            this.InteractionForce = new InteractionForce(this.Topology);
        }

        public Temperature Temperature => this.Topology.Temperature;
        public void ResetForce() => this.InteractionForce.ResetForce();
        public void CalculateBondForce() => this.InteractionForce.CalculateBondForce();
        public void CalculateAngleForce() => this.InteractionForce.CalculateAngleForce();
        public void CalculateDihedralForce() => this.InteractionForce.CalculateDihedralForce();
        public void CalculateDihedralForceSlow() => this.InteractionForce.CalculateDihedralForceSlow();
        public void CalculateImproperDihedralForce() => this.InteractionForce.CalculateImproperDihedralForce();
        public void CalculateNonbondForce() => this.InteractionForce.CalculateNonbondForce();
        public void CalculateNonbondForceSlow() => this.InteractionForce.CalculateNonbondForceSlow();
        public void CalculateForce() => this.InteractionForce.CalculateTotalForce();
        public double CalculateInteractionEnergy() => this.InteractionEnergy.CalculateTotalEnergy();
        public double CalculateKineticEnergy() => this.Topology.Temperature.CalculateKineticEnergy();
    }
}
