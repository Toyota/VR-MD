/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;

namespace VRMD.PureC.MD
{
    public class FFTables
    {
        private static readonly double c0 = System.Math.Pow(2f, (5f / 6f));
        private const double angs = 1e-10f;
        private const double nm = 1e-9f;
        private const double kcalPerMol = 6.94782e-21f;
        private const double kJPerMol = 1.660e-21;
        private const double K = 1.38066E-23f;
        private const double e = 1.602176634e-19f;
        public static Dictionary<string, double> LJSigma = new Dictionary<string, double>()
        {
            {"h1",1.387*c0*angs},
            {"h2",1.287*c0*angs},
            {"h3",1.187*c0*angs},
            {"h4",1.409*c0*angs},
            {"h5",1.359*c0*angs},
            {"ha",1.459*c0*angs},
            {"hc",1.487*c0*angs},
            {"hn",0.600*c0*angs},
            {"ho",0.000},
            {"hw",0.000},
            {"hx",1.100*c0*angs},
            {"c",1.908*c0*angs},
            {"c1",1.908*c0*angs},
            {"c2",1.908*c0*angs},
            {"c3",1.908*c0*angs},
            {"c3d",1.908*c0*angs},    // for diamond
            {"c3oh",1.908*c0*angs},
            {"ca",1.908*c0*angs},
            {"cc",1.908*c0*angs},
            {"cd",1.908*c0*angs},
            {"cg",1.908*c0*angs},
            {"ch",1.908*c0*angs},
            {"cp",1.908*c0*angs},
            {"cq",1.908*c0*angs},
            {"n",1.824*c0*angs},
            {"n2",1.824*c0*angs},
            {"n3",1.824*c0*angs},
            {"n4",1.824*c0*angs},
            {"na",1.824*c0*angs},
            {"nb",1.824*c0*angs},
            {"nc",1.824*c0*angs},
            {"nd",1.824*c0*angs},
            {"nh",1.824*c0*angs},
            {"o",1.6612*c0*angs},
            {"oh",1.721*c0*angs},
            {"os",1.6837*c0*angs},
            {"ow",1.7683*c0*angs},
            //GAFF
            {"CO",2.757*angs},
            {"O",3.297*angs},
            {"OC",3.033*angs},
            {"Na",0.2160*nm},
            {"Cl",0.4830*nm},
            {"X",0},
        };

        public static Dictionary<string, double> LJEpsilon = new Dictionary<string, double>()
        {
            {"h1",0.0157*kcalPerMol},
            {"h2",0.0157*kcalPerMol},
            {"h3",0.0157*kcalPerMol},
            {"h4",0.0150*kcalPerMol},
            {"h5",0.0150*kcalPerMol},
            {"ha",0.0150*kcalPerMol},
            {"hc",0.0157*kcalPerMol},
            {"hn",0.0157*kcalPerMol},
            {"ho",0.000},
            {"hw",0.000},
            {"hx",0.0157*kcalPerMol},
            {"c",0.0860*kcalPerMol},
            {"c1",0.0860*kcalPerMol},
            {"c2",0.0860*kcalPerMol},
            {"c3",0.1094*kcalPerMol},
            {"c3d",0.1094*kcalPerMol},
            {"c3oh",0.0860*kcalPerMol},
            {"ca",0.0860*kcalPerMol},
            {"cc",0.0860*kcalPerMol},
            {"cd",0.0860*kcalPerMol},
            {"cg",0.0860*kcalPerMol},
            {"ch",0.0860*kcalPerMol},
            {"cp",0.0860*kcalPerMol},
            {"cq",0.0860*kcalPerMol},
            {"n",0.1700*kcalPerMol},
            {"n2",0.1700*kcalPerMol},
            {"n3",0.1700*kcalPerMol},
            {"n4",0.1700*kcalPerMol},
            {"na",0.1700*kcalPerMol},
            {"nb",0.1700*kcalPerMol},
            {"nc",0.1700*kcalPerMol},
            {"nd",0.1700*kcalPerMol},
            {"nh",0.1700*kcalPerMol},
            {"o",0.2100*kcalPerMol},
            {"oh",0.2104*kcalPerMol},
            {"os",0.1700*kcalPerMol},
            {"ow",0.1520*kcalPerMol},
            // GAFF
            {"CO",28.129*K},
            {"O",0.1047*kcalPerMol},
            {"OC",80.507*K},
            {"Na",1.4754533*kJPerMol},
            {"Cl",0.0534924*kJPerMol},
            {"X",0},
        };

        public static Dictionary<string, double> Charge = new Dictionary<string, double>()
        {
            {"h1",0.040*e},    // http://theochemlab.asu.edu/teaching/chm543/OPLS.pdf
            {"hc",0.074*e},   // https://pubs.acs.org/doi/pdf/10.1021/ct200908r
            {"ho",0.418*e},    // http://theochemlab.asu.edu/teaching/chm543/OPLS.pdf
            {"hw",0.417*e},    // TIP3P
            {"c3",-0.222*e},    // https://pubs.acs.org/doi/pdf/10.1021/ct200908r
            {"c3d",0},
            {"c3oh",0.145*e},    // http://theochemlab.asu.edu/teaching/chm543/OPLS.pdf
            {"ca",0},
            {"oh",-0.683*e},    // http://theochemlab.asu.edu/teaching/chm543/OPLS.pdf
            {"ow",-0.834*e},    // TIP3P
            {"CO",0.6512*e},
            {"O",0},
            {"OC",-0.3256*e},
            {"Na",e},
            {"Cl",-e},
            {"X",0},
        };

        public static Dictionary<string, string> Symbol = new Dictionary<string, string>()
        {
            {"h","H"},
            {"h1","H"},
            {"h2","H"},
            {"h3","H"},
            {"h4","H"},
            {"h5","H"},
            {"ha","H"},
            {"hc","H"},
            {"hn","H"},
            {"ho","H"},
            {"hw","H"},
            {"hx","H"},
            {"c","C"},
            {"c1","C"},
            {"c2","C"},
            {"c3","C"},
            {"c3d","C"},
            {"ca","C"},
            {"cc","C"},
            {"cd","C"},
            {"cg","C"},
            {"ch","C"},
            {"cp","C"},
            {"cq","C"},
            {"n","N"},
            {"n2","N"},
            {"n3","N"},
            {"n4","N"},
            {"na","N"},
            {"nb","N"},
            {"nc","N"},
            {"nd","N"},
            {"nh","N"},
            {"o","O"},
            {"oh","O"},
            {"os","O"},
            {"ow","O"},
            {"CO","C"},
            {"O","O"},
            {"OC","O"},
            {"Na","Na"},
            {"Cl","Cl"},
        };
    }
}
