namespace Triangle.Compiler.SyntacticAnalyzer {
    public partial class Parser {
        void ParseActualParameterSequence() {
            if (_currentToken.Kind == TokenKind.RightParen) {
                return;
            }
            ParseActualParameter();
            while (_currentToken.Kind == TokenKind.Comma) {
                ParseActualParameter();
            }
        }

        void ParseActualParameter() {
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