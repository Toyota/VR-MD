/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
namespace VRMD.PureC.MD
{
    public class AngleParameter
    {
        public double TK { get; }
        public double TEQ { get; }

        public AngleParameter(double TK, double TEQ)
        {
            this.TK = TK;
            this.TEQ = TEQ;
        }
    }
}