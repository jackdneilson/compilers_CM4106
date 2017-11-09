namespace Triangle.Compiler.SyntacticAnalyzer {
    public class SourcePosition {
        public static readonly SourcePosition Empty =
            new SourcePosition(Location.Empty, Location.Empty);

        public readonly Location Start;

        public readonly Location Finish;

        public SourcePosition(Location start, Location finish) {
            Start = start;
            Finish = finish;
        }

        public override string ToString() {
            return string.Format("{0}..{1}", Start, Finish);
        }
    }
}