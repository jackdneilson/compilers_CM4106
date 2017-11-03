namespace Triangle.Compiler.SyntacticAnalyzer {
    public partial class Parser {
        //Parses one or more declarations
        void ParseDeclaration() {
            System.Console.WriteLine("Parsing declaration");
            ParseSingleDeclaration();
            while (_currentToken.Kind == TokenKind.Semicolon) {
                AcceptIt();
                ParseSingleDeclaration();
            }
        }
        
        //Parses a single declaration
        void ParseSingleDeclaration() {
            System.Console.WriteLine("Parsing single declaration");
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

                default: {
                    _reporter.ReportError("Error while parsing single" +
                                          "declaration line: "
                                          + _currentToken.getLine()
                                          + " index: "
                                          + _currentToken.getIndex()
                                          + " expected var or const, got ",
                        _currentToken);
                    break;
                }
            }
        }

        //Parses a variable type denoter
        void ParseTypeDenoter() {
            System.Console.WriteLine("Parsing type denoter");
            ParseIdentifier();
        }
    }
}