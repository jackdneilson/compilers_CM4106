namespace Triangle.Compiler.SyntacticAnalyzer {
    public partial class Parser {
        void ParseActualParameterSequence() {
            System.Console.WriteLine("Parsing actual parameter sequence");
            Accept(TokenKind.LeftParen);
            if (_currentToken.Kind == TokenKind.RightParen) {
                AcceptIt();
                return;
            }
            ParseActualParameter();
            while (_currentToken.Kind == TokenKind.Comma) {
                AcceptIt();
                ParseActualParameter();
            }
            Accept(TokenKind.RightParen);
        }

        void ParseActualParameter() {
            System.Console.WriteLine("Parsing actual parameter");
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