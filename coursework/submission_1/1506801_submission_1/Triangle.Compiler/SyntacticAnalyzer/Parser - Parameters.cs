namespace Triangle.Compiler.SyntacticAnalyzer {
    public partial class Parser {
        
        ///Parses a sequence of actual parameters, or returns if parameter
        /// sequence is empty
        void ParseActualParameterSequence() {
            System.Console.WriteLine("parsing actual parameter sequence");
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

        //Parses a single actual parameter
        void ParseActualParameter() {
            System.Console.WriteLine("parsing actual parameter");
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