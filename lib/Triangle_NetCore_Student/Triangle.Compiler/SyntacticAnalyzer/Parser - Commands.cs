/**
 * @Author: John Isaacs <john>
 * @Date:   10-Oct-172017
 * @Filename: Parser - Commands.cs
 * @Last modified by:   john
 * @Last modified time: 19-Oct-172017
 */



namespace Triangle.Compiler.SyntacticAnalyzer
{
    public partial class Parser
    {
        ///////////////////////////////////////////////////////////////////////////////
        //
        // COMMANDS
        //
        ///////////////////////////////////////////////////////////////////////////////


        /// Parses the command error
        void ParseCommand()
        {
          System.Console.WriteLine("parsing command");
            ParseSingleCommand();
            while (_currentToken.Kind == TokenKind.Semicolon)
            {
                AcceptIt();
                ParseSingleCommand();
            }
        }

        /// Parses the single command

        void ParseSingleCommand()
        {
          System.Console.WriteLine("parsing single command");
            switch (_currentToken.Kind)
            {

                case TokenKind.Identifier:
                    {
                        ParseIdentifier();
                        //Accept(TokenKind.Becomes);
                        //ParseExpression();
                        break;

                    }

                case TokenKind.Begin:
                    {
                        AcceptIt();
                        ParseCommand();
                        break;

                    }

                case TokenKind.If: {
                    AcceptIt();
                    Accept(TokenKind.LeftParen);
                    ParseExpression();
                    Accept(TokenKind.RightParen);
                    Accept(TokenKind.Then);
                    ParseSingleCommand();
                    Accept(TokenKind.Else);
                    ParseSingleCommand();
                    break;
                }

                case TokenKind.While: {
                    AcceptIt();
                    Accept(TokenKind.LeftParen);
                    ParseExpression();
                    Accept(TokenKind.RightParen);
                    Accept(TokenKind.Do);
                    ParseSingleCommand();
                    break;
                }

                case TokenKind.Let: {
                    Accept(TokenKind.Identifier);
                    Accept(TokenKind.In);
                    ParseSingleCommand();
                    break;
                }


                default:
                    System.Console.WriteLine("error");
                    break;

            }
        }
        
        /// Parses an expression
        void ParseExpression() {
            System.Console.WriteLine("parsing expression");
            ParsePrimaryExpression();
            while (_currentToken.Kind == TokenKind.Operator) {
                AcceptIt();
                Accept(TokenKind.Operator);
                ParsePrimaryExpression();
            }
        }

        /// Parses the single expression
        void ParsePrimaryExpression() {
            System.Console.WriteLine("parsing primary expression");
            if (_currentToken.Kind == TokenKind.Identifier) {
                AcceptIt();
            } else if (_currentToken.Kind == TokenKind.IntLiteral) {
                AcceptIt();
                if (_currentToken.Kind == TokenKind.Operator) {
                    AcceptIt();
                    ParsePrimaryExpression();
                }
            }
        }

        void ParseDeclaration() {
            
        }

        void ParseSingleDeclaration() {
            
        }
    }
}
