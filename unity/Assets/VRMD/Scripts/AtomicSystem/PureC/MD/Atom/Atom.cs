/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{

    public class Atom
    {
        public AtomLabel Label { get; }
        public string Symbol => this.Label.Symbol;
        public string FFSymbol => this.Label.FFSymbol;

        public Vector3 Position { get; private set; }
        public Vector3 Velocity { get; private set; }
        public Vector3 Force { get => new Vector3(this.Fx, this.Fy, this.Fz); }
        public Vector3 ExternalForce { get; private set; }

        private readonly double mass = -1.0;
        public double Mass => this.mass > 0.0 ? this.mass : AtomTables.Mass[this.Symbol];

        private readonly double vdwRadius = -1.0;
        public double VdwRadius => this.vdwRadius > 0.0 ? this.vdwRadius : AtomTables.VdwRadius[this.Symbol];

        private readonly double covalentRadius = -1.0;
        public double CovalentRadius => this.covalentRadius > 0.0 ? this.covalentRadius : AtomTables.CovalentRadius[this.Symbol];

        private readonly double ljSigma = -1.0;
        public double LJSigma => this.ljSigma >= 0.0 ? this.ljSigma :
            (FFTables.LJSigma.TryGetValue(this.FFSymbol, out double sigma) ? sigma : throw new System.Exception(this.FFSymbol));

        private readonly double ljEpsilon = -1.0;
        public double LJEpsilon => this.ljEpsilon >= 0.0 ? this.ljEpsilon :
            (FFTables.LJEpsilon.TryGetValue(this.FFSymbol, out double epsilon) ? epsilon : throw new System.Exception(this.FFSymbol));

        private readonly double? charge = null;
        public double Charge => this.charge != null ? (double)this.charge : FFTables.Charge[this.FFSymbol];

        public Atom(string label, string symbol, string ffSymbol, double x, double y, double z, double? charge = null)
        {
            this.Label = new AtomLabel(label, symbol, ffSymbol);
            this.SetPosition(x, y, z);
            this.SetVelocity(0.0, 0.0, 0.0);
            this.SetForce(0.0, 0.0, 0.0);
            this.SetExternalForce(0.0, 0.0, 0.0);
            this.charge = charge;
        }

        public void SetPosition(Vector3 x) => this.SetPosition(x[0], x[1], x[2]);
        public void SetPosition(double x, double y, double z) => this.Position = new Vector3(x, y, z);
        public void SetVelocity(Vector3 v) => this.SetVelocity(v[0], v[1], v[2]);
        public void SetVelocity(double vx, double vy, double vz) => this.Velocity = new Vector3(vx, vy, vz);
        public void SetExternalForce(double Fx, double Fy, double Fz) => this.ExternalForce = new Vector3(Fx, Fy, Fz);
        public void AddExternalForce(double Fx, double Fy, double Fz) => this.ExternalForce += new Vector3(Fx, Fy, Fz);
        public void SetForce(Vector3 F) => this.SetForce(F[0], F[1], F[2]);
        public void SetForce(double Fx, double Fy, double Fz)
        {
            this.Fx = Fx;
            this.Fy = Fy;
            this.Fz = Fz;
        }

        public void AddForce(Vector3 F) => this.AddForce(F[0], F[1], F[2]);
        public void AddForce(double Fx, double Fy, double Fz)
        {
            this.Fx += Fx;
            this.Fy += Fy;
            this.Fz += Fz;
        }

        public double Fx;
        public double Fy;
        public double Fz;

        public void UpdatePosition(double dt)
        {
            var x = this.Position;
            var v = this.Velocity;
            x += dt * v;
            this.Position = x;
        }
        public void UpdateVelocity(double dt)
        {
            var v = this.Velocity;
            var F = this.Force;
            var m = this.Mass;
            v += dt / m * F;
            this.Velocity = v;
        }

        public bool IsProtonAcceptor
        {
            get
            {
                if (this.Symbol == "O" || this.Symbol == "N") return true;
                else return false;
            }
        }

        public bool IsHydrogen => this.Symbol == "H";
    }
}