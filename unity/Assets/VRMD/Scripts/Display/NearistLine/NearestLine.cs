/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using VRMD.MolecularViewer;
using UnityEngine;

public class NearestLine : MonoBehaviour
{
    List<DisplayedAtom> Atoms = new List<DisplayedAtom>();
    DisplayedAtom Target;
    LineRenderer LineRenderer;

    void Start()
    {
        var atoms = Object.FindObjectsOfType<DisplayedAtom>();
        foreach (var atom in atoms)
        {
            if (!atom.IsHydrogen)
            {
                this.Atoms.Add(atom);
            }
        }
        this.LineRenderer = this.gameObject.AddComponent<LineRenderer>();
        this.LineRenderer.material = (Material)Resources.Load("Materials/WhiteDashLine");
        this.LineRenderer.startWidth = 0.005f;
        this.LineRenderer.endWidth = 0.005f;
    }

    void Update()
    {
        var rMin = float.PositiveInfinity;
        this.Target = this.Atoms[0];
        foreach (var touchable in this.Atoms)
        {
            var r = Vector3.Magnitude(this.transform.position - touchable.transform.position);
            if (r < rMin)
            {
                this.Target = touchable;
                rMin = r;
            }
        }
        this.UpdateLine();
    }

    void UpdateLine()
    {
        this.LineRenderer.SetPositions(new Vector3[]{
            this.transform.position,this.Target.transform.position
        });
    }
}
