using System;

namespace Triangle.Compiler.SyntacticAnalyzer {
    public class ErrorReporter {
        private Boolean errorFound;
        private int numberErrors;

        public ErrorReporter() {
            errorFound = false;
            numberErrors = 0;
        }
        
        public void ReportError(string msg, Token token) {
            errorFound = true;
            numberErrors++;
            Console.WriteLine(msg + token.Kind);
        }

        public Boolean HasErrors() {
            return errorFound;
        }

        public int ErrorCount() {
            return numberErrors;
        }
    }
}