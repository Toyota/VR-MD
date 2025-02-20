/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System;
using System.IO;

namespace VRMD.PureC.MD
{

    public static class TopologyBuilder
    {
        public static Topology Build(string filename)
        {
            FFTableComplile();
            EditableTopology topology = filename switch
            {
                "" => new WaterSampleTopology(),
                "singleAtom" => new SingleAtomSampleTopology(),
                _ => Path.GetExtension(filename) switch
                {
                    ".xyz" => new XyzTopology(filename),
                    ".mol2" => new Mol2Topology(filename),
                    _ => throw new Exception()
                }
            };
            return topology.Compile();
        }
        public static void FFTableComplile()
        {
            AngleFFTables.AngleTableComplie();
            BondFFTables.BondTableComplie();
            DihedralFFtables.DihedralTableComplie();
            ImproperDihedralFFtables.ImproperDihedralTableComplie();
        }
    }

}
