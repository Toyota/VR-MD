/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using VRMD.PureC.Math;

namespace VRMD.PureC.MD
{
    public class PairInteraction
    {
        private readonly List<PairInteraction> InteractionList;

        protected PairInteraction() { }
        private PairInteraction(List<PairInteraction> interactionList)
        {
            this.InteractionList = interactionList;
        }
        public double GetEnergy(Vector x) => this.GetEnergy(new Vector3(x[0], x[1], x[2]), new Vector3(x[3], x[4], x[5]));
        public double GetEnergy(double x1, double y1, double z1, double x2, double y2, double z2)
            => this.GetEnergy(new Vector3(x1, y1, z1), new Vector3(x2, y2, z2));
        public virtual double GetEnergy(Vector3 x1, Vector3 x2)
        {
            var E = 0.0;
            foreach (var interaction in this.InteractionList)
            {
                E += interaction.GetEnergy(x1, x2);
            }
            return E;
        }

        public Vector GetForce(Vector x) => this.GetForce(new Vector3(x[0], x[1], x[2]), new Vector3(x[3], x[4], x[5]));
        public Vector GetForce(double x1, double y1, double z1, double x2, double y2, double z2)
            => this.GetForce(new Vector3(x1, y1, z1), new Vector3(x2, y2, z2));
        public virtual Vector GetForce(Vector3 x1, Vector3 x2)
        {
            var F = Vector.Zero(6);
            foreach (var interaction in this.InteractionList)
            {
                F += interaction.GetForce(x1, x2);
            }
            return F;
        }

        public static PairInteraction operator +(PairInteraction interaction1, PairInteraction interaction2)
        {
            var interactionList = new List<PairInteraction>();
            if (interaction1.InteractionList == null)
            {
                interactionList.Add(interaction1);
            }
            else
            {
                interactionList.AddRange(interaction1.InteractionList);
            }
            if (interaction2.InteractionList == null)
            {
                interactionList.Add(interaction2);
            }
            else
            {
                interactionList.AddRange(interaction2.InteractionList);
            }
            return new PairInteraction(interactionList);
        }
    }

}
