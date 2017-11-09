using System;

namespace Triangle.Compiler.SyntacticAnalyzer {
    public partial class Parser {
        //Parses a full expression
        void ParseExpression() {
            System.Console.WriteLine("parsing expression");
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

        //Parses one or more primary expressions
        void ParseSecondaryExpression() {
            System.Console.WriteLine("parsing secondary expression");
            ParsePrimaryExpression();
            if (_currentToken.Kind == TokenKind.Operator) {
                ParseOperator();
                ParsePrimaryExpression();
            }
        }

        //Parses a single part of an expression
        void ParsePrimaryExpression() {
            System.Console.WriteLine("parsing primary expression");
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
                    ParseIdentifier();
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
            }
        }
    }
}