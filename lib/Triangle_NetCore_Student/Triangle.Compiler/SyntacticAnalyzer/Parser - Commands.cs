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
        //TODO: Review if should look for identifier or should parse vname
        void ParseSingleCommand()
        {
          System.Console.WriteLine("parsing single command");
            switch (_currentToken.Kind)
            {   
                case TokenKind.If: {
                    AcceptIt();
                    ParseExpression();
                    Accept(TokenKind.Then);
                    ParseSingleCommand();
                    Accept(TokenKind.Else);
                    ParseSingleCommand();
                    break;
                }
                    
                case TokenKind.While: {
                    AcceptIt();
                    ParseExpression();
                    Accept(TokenKind.Do);
                    ParseSingleCommand();
                    break;
                }
                
                case TokenKind.Let: {
                    AcceptIt();
                    ParseDeclaration();
                    Accept(TokenKind.In);
                    ParseSingleCommand();
                    break;
                }    
                    
                case TokenKind.Begin:
                    {
                        AcceptIt();
                        ParseCommand();
                        break;
                    }

                default:
                    ParseVname();
                    if (_currentToken.Kind == TokenKind.Becomes) {
                        AcceptIt();
                        ParseExpression();
                    } else if (_currentToken.Kind == TokenKind.LeftParen) {
                        AcceptIt();
                        ParseExpression();
                        Accept(TokenKind.RightParen);
                    }
                    break;
            }
        }
    }
}
