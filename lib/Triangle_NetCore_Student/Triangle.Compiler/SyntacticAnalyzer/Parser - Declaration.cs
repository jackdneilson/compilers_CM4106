namespace Triangle.Compiler.SyntacticAnalyzer {
    public partial class Parser {
        void ParseDeclaration() {
            ParseSingleDeclaration();
            while (_currentToken.Kind == TokenKind.Semicolon) {
                AcceptIt();
                ParseSingleDeclaration();
            }
        }

        void ParseSingleDeclaration() {
            switch (_currentToken.Kind) {
                case TokenKind.Const: {
                    AcceptIt();
                    ParseIdentifier();
                    Accept(TokenKind.Is);
                    ParseExpression();
                    break;
                }

                case TokenKind.Var: {
                    AcceptIt();
                    ParseIdentifier();
                    Accept(TokenKind.Colon);
                    ParseTypeDenoter();
                    break;
                }
            }
        }

        void ParseTypeDenoter() {
            ParseIdentifier();
        }
    }
}