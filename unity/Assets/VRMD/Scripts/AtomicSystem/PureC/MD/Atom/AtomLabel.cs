/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
namespace VRMD.PureC.MD
{
    public class AtomLabel
    {
        public const string UnknownLabel = "X";

        public AtomLabel(string label, string symbol, string ffSymbol)
        {
            this.label = label;
            this.symbol = symbol;
            this.ffSymbol = ffSymbol;
        }

        private readonly string label;
        public string Label =>
            this.HasLabel() ? this.label :
            this.HasFFSymbol() ? this.ffSymbol :
            this.HasSymbol() ? this.symbol : UnknownLabel;

        private readonly string symbol = "";
        public string Symbol =>
            this.HasSymbol() ? this.symbol :
            this.HasFFSymbol() ?
                (FFTables.Symbol.TryGetValue(this.ffSymbol, out string symbol_) ? symbol_ : throw new System.Exception(this.ffSymbol))
                : UnknownLabel;

        private readonly string ffSymbol = "";
        public string FFSymbol => this.HasFFSymbol() ? this.ffSymbol : UnknownLabel;

        private bool HasLabel() => this.label != "";
        private bool HasSymbol() => this.symbol != "";
        private bool HasFFSymbol() => this.ffSymbol != "";

        public static implicit operator string(AtomLabel label) => label.Label;
    }
}
