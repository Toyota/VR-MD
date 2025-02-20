/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using VRMD.PureC.MD;
using UnityEngine;

namespace VRMD.MolecularViewer
{
    public class Viewer : MonoBehaviour
    {
        public Simulator Simulator { get; private set; }
        [SerializeField] private IntegratorSelector Integrator = new IntegratorSelector();
        [SerializeField] private ViewModeSelector ViewMode = new ViewModeSelector();
        [SerializeField] private FileSelector File = new FileSelector();
        [SerializeField] private HydrogenBondList HydrogenBonds = new HydrogenBondList();

        public DisplayedAtomList AtomList;
        public DisplayedBondList BondList;
        public UnityUnit UnityUnit { get; private set; }

        public void Awake()
        {
            var topology = this.File.Read();
            this.Initialize(topology);
        }

        public void Initialize(Topology topology)
        {
            var integrator = this.Integrator.Generate(topology);
            this.UnityUnit = new UnityUnit(this.transform.lossyScale.x, this.transform.position);
            this.AtomList = new DisplayedAtomList(this.gameObject, topology.AtomList, this.UnityUnit);
            this.BondList = new DisplayedBondList(this.gameObject, topology.BondList);
            this.HydrogenBonds.Initialize(this.AtomList, this.BondList, this.transform);
            var interactives = new InteractiveObjectList(topology, this.UnityUnit);
            this.Simulator = new InteractiveSimulator(topology, integrator, interactives);
            this.ViewMode.Apply(this.AtomList);
        }

        public void FixedUpdate()
        {
            var dt = 0.5e-15;
            foreach (var atom in this.AtomList) atom.SetExternalForce(0.0, 0.0, 0.0);
            foreach (var atom in this.AtomList) atom.UpdateUnityForce();
            this.Simulator.Update(dt);
            foreach (var atom in this.AtomList) atom.Synchronize();
        }

        public void Update()
        {
            this.HydrogenBonds.Update(this.ElapsedTime);
            this.BondList.Update();
        }

        public float ElapsedTime => (float)this.Simulator.ElapsedTime;
        public Temperature Temperature => this.Simulator.Temperature;
        public Topology Topology => this.Simulator.Topology;
        public List<HBond> ApparentHBondList => this.HydrogenBonds.ApparentBondList;
        public List<HBond> LatentHBondList => this.HydrogenBonds.LatentBondList;
    }
}