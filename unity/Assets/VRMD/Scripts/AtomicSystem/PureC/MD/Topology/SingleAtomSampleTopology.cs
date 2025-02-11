/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
namespace VRMD.PureC.MD
{

    public class SingleAtomSampleTopology : EditableTopology
    {
        public SingleAtomSampleTopology()
        {
            var atom = new Atom("", "C", "c3", 0, 0, 0);
            this.AtomList.Add(atom);
            this.NonbondList.AddRange(Nonbond.AutomaticCreate(this.AtomList, this.BondList, this.AngleList));
        }
    }
}
