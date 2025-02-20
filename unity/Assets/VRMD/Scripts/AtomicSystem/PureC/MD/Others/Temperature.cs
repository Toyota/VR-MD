/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
namespace VRMD.PureC.MD
{
    public class Temperature
    {
        public double PresetValue { get; private set; } = -1.0;
        public void Preset(double T) => this.PresetValue = T;

        public Temperature(Atom[] atomList)
        {
            this.AtomList = atomList;
        }

        public double Kelvin
        {
            get
            {
                var K = this.CalculateKineticEnergy();
                var N = this.AtomList.Length;
                var T = 2 * K / (3 * N * kB);
                return T;
            }
        }

        private const double kB = 1.380649e-23;

        public double CalculateKineticEnergy()
        {
            var K = 0.0;
            foreach (var atom in this.AtomList)
            {
                var m = atom.Mass;
                var v = atom.Velocity;
                K += 0.5 * m * v * v;
            }
            return K;
        }

        private readonly Atom[] AtomList;

    }
}