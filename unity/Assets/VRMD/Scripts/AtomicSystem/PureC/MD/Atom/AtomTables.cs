/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;

namespace VRMD.PureC.MD
{
    public static class AtomTables
    {
        public const double angs = 1e-10f;
        public const double amu = 1.66054e-27f;

        public static Dictionary<string, double> Mass { get; } = new Dictionary<string, double>()
        {
           {"H",1.008*amu},
            {"He",4.003*amu},
            {"Li",6.941*amu},
            {"Be",9.012*amu},
            {"B",10.81*amu},
            {"C",12.01*amu},
            {"N",14.01*amu},
            {"O",16.00*amu},
            {"F",19.00*amu},
            {"Ne",20.18*amu},
            {"Na",22.989769*amu},
            {"Cl",35.453*amu},
            // http://www.inv.co.jp/~yoshi/kigou/gensi.html
        };

        public static Dictionary<string, double> VdwRadius { get; } = new Dictionary<string, double>()
        {
            {"H",1.20*angs},
            {"He",1.40*angs},
            {"Li",1.82*angs},
            {"C",1.70*angs},
            {"N",1.55*angs},
            {"O",1.52*angs},
            {"F",1.47*angs},
            {"Ne",1.54*angs},
            {"Na",2.27*angs},
            {"Cl",1.75*angs},
            // https://www.hulinks.co.jp/support/c-maker/qa_05.html
        };

        public static Dictionary<string, double> CovalentRadius { get; } = new Dictionary<string, double>()
         {
            {"H",0.32*angs},
            {"He",0.46*angs},
            {"Li",1.33*angs},
            {"Be",1.02*angs},
            {"B",0.85*angs},
            {"C",0.75*angs},
            {"N",0.71*angs},
            {"O",0.63*angs},
            {"F",0.64*angs},
            {"Ne",0.67*angs},
            {"Na",0.95*angs},
            {"Cl",0.99*angs},
            // https://ja.wikipedia.org/wiki/%E5%85%B1%E6%9C%89%E7%B5%90%E5%90%88%E5%8D%8A%E5%BE%84
        };
    }
}