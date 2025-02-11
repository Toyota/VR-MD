/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
namespace VRMD.PureC.MD
{
    public class BondParameter
    {
        public double RK { get; }
        public double REQ { get; }

        public BondParameter(double RK,double REQ)
        {   
            this.RK = RK;
            this.REQ = REQ;
        }
    }
}