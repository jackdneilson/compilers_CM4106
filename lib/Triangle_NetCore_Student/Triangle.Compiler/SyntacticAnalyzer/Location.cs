namespace Triangle.Compiler.SyntacticAnalyzer {
    public class Location {
        public int lineNumber { get; }
        public int lineIndex { get; }

        public Location(int lineNumber, int lineIndex) {
            this.lineNumber = lineNumber;
            this.lineIndex = lineIndex;
        }
    }
}