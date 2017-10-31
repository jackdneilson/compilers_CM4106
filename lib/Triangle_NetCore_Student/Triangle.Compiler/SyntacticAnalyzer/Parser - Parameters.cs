namespace Triangle.Compiler.SyntacticAnalyzer {
    public partial class Parser {
        void ParseActualParameterSequence() {
            System.Console.WriteLine("Parsing parameter sequence line: " 
                                     + _currentToken.getLine()
                                     + " index: "
                                     + _currentToken.getIndex());
            if (_currentToken.Kind == TokenKind.RightParen) {
                return;
            }
            ParseActualParameter();
            while (_currentToken.Kind == TokenKind.Comma) {
                ParseActualParameter();
            }
        }

        void ParseActualParameter() {
            System.Console.WriteLine("Parsing actual parameter line: " 
                                     + _currentToken.getLine()
                                     + " index: "
                                     + _currentToken.getIndex());
            switch (_currentToken.Kind) {
                case TokenKind.Var: {
                    AcceptIt();
                    ParseVname();
                    break;
                }

                default: {
                    ParseExpression();
                    break;
                }
            }
        }
    }
}