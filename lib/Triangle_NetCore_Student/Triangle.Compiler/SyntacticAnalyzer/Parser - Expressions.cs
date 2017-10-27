namespace Triangle.Compiler.SyntacticAnalyzer {
    public partial class Parser {
        void ParseExpression() {
            switch (_currentToken.Kind) {
                case TokenKind.Let: {
                    AcceptIt();
                    ParseDeclaration();
                    Accept(TokenKind.In);
                    ParseExpression();
                    break;
                }

                case TokenKind.If: {
                    AcceptIt();
                    ParseExpression();
                    Accept(TokenKind.Then);
                    ParseExpression();
                    Accept(TokenKind.Else);
                    ParseExpression();
                    break;
                }

                default: {
                    ParseSecondaryExpression();
                    break; 
                }
            }
        }

        void ParseSecondaryExpression() {
            ParsePrimaryExpression();
            if (_currentToken.Kind == TokenKind.Operator) {
                AcceptIt();
                ParsePrimaryExpression();
            }
        }

        void ParsePrimaryExpression() {
            switch (_currentToken.Kind) {
                case TokenKind.IntLiteral: {
                    AcceptIt();
                    break; 
                }
                    
                case TokenKind.CharLiteral: {
                    AcceptIt();
                    break;
                }

                case TokenKind.Identifier: {
                    ParseVname();
                    if (_currentToken.Kind == TokenKind.LeftParen) {
                        AcceptIt();
                        ParseActualParameterSequence();
                        Accept(TokenKind.RightParen);
                    }
                    break;
                }

                case TokenKind.Operator: {
                    AcceptIt();
                    ParsePrimaryExpression();
                    break;
                }

                case TokenKind.LeftParen: {
                    AcceptIt();
                    ParseExpression();
                    Accept(TokenKind.RightParen);
                    break;
                }
            }
        }
    }
}