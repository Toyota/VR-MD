/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;

namespace VRMD.PureC.MD
{
    public class ImproperDihedralParameter
    {
        public double PK { get; }
        public double PHASE { get; }//radian
        public int PN { get; }

        public ImproperDihedralParameter(double PK,double PHASE,int PN)
        {   
            this.PK = PK;
            this.PHASE = PHASE;
            this.PN = PN;
        }
    }
}