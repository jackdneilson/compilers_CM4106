namespace Triangle.Compiler.SyntacticAnalyzer {
    public class SourcePosition {
        public Location startPos { get; }
        public Location endPos { get; }

        public SourcePosition(Location startPos, Location endPos) {
            this.startPos = startPos;
            this.endPos = endPos;
        }
    }
}