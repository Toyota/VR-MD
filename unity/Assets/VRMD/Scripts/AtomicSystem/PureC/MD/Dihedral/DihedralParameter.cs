/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
namespace VRMD.PureC.MD
{
    public class DihedralParameter
    {
        public double IDIVF { get; }
        public double PK { get; }
        public double PHASE { get; }//radian
        public int PN { get; }

        public DihedralParameter(double IDIVF, double PK, double PHASE, int PN)
        {
            this.IDIVF = IDIVF;
            this.PK = PK;
            this.PHASE = PHASE;
            this.PN = PN;
        }
    }
}