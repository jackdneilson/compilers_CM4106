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


        /// Parses a command 
        void ParseCommand()
        {
          System.Console.WriteLine("Parsing command");
            ParseSingleCommand();
            while (_currentToken.Kind == TokenKind.Semicolon)
            {
                AcceptIt();
                ParseSingleCommand();
            }
        }

        /// Parses a single command
        void ParseSingleCommand()
        {
          System.Console.WriteLine("Parsing single command");
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
                        Accept(TokenKind.End);
                        break;
                    }

                //In the case of trailing else or hitting end, simply break
                case TokenKind.End:
                case TokenKind.Else:
                case TokenKind.EndOfText: {
                    break;
                }

                default:
                    ParseVname();
                    if (_currentToken.Kind == TokenKind.Becomes) {
                        AcceptIt();
                        ParseExpression();
                    } else if (_currentToken.Kind == TokenKind.LeftParen) {
                        ParseActualParameterSequence();
                    }
                    break;
            }
        }
    }
}
