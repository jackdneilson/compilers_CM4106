using System;

namespace Triangle.Compiler.SyntacticAnalyzer {
    public partial class Parser {
        void ParseExpression() {
            System.Console.WriteLine("Parsing expression");
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
            System.Console.WriteLine("Parsing secondary expression");
            ParsePrimaryExpression();
            if (_currentToken.Kind == TokenKind.Operator) {
                ParseOperator();
                ParsePrimaryExpression();
            }
        }

        void ParsePrimaryExpression() {
            System.Console.WriteLine("Parsing primary expression");
            switch (_currentToken.Kind) {
                case TokenKind.IntLiteral: {
                    ParseIntLiteral();
                    break; 
                }
                    
                case TokenKind.CharLiteral: {
                    ParseCharLiteral();
                    break;
                }

                case TokenKind.Identifier: {
                    ParseVname();
                    if (_currentToken.Kind == TokenKind.LeftParen) {
                        ParseActualParameterSequence();
                    }
                    break;
                }

                case TokenKind.Operator: {
                    ParseOperator();
                    ParsePrimaryExpression();
                    break;
                }

                case TokenKind.LeftParen: {
                    AcceptIt();
                    ParseExpression();
                    Accept(TokenKind.RightParen);
                    break;
                }
                    
                default: {
                    Console.WriteLine(_currentToken.Spelling);
                    _reporter.ReportError("Error while parsing expresion line: "
                                          + _currentToken.getLine()
                                          + " index: "
                                          + _currentToken.getIndex()
                                          + " got " + _currentToken.Spelling,
                        _currentToken);
                    break;
                }
            }
        }
    }
}