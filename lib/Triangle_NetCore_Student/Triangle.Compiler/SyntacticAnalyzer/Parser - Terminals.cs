/**
 * @Author: John Isaacs <john>
 * @Date:   10-Oct-172017
 * @Filename: Parser - Terminals.cs
 * @Last modified by:   john
 * @Last modified time: 19-Oct-172017
 */



namespace Triangle.Compiler.SyntacticAnalyzer
{
    public partial class Parser
    {

        ///////////////////////////////////////////////////////////////////////////////
        //
        // TERMINALS
        //
        ///////////////////////////////////////////////////////////////////////////////

        /**
         * Parses an identifier, and constructs a leaf AST to represent it.
         */
        void ParseIdentifier()
        {
            System.Console.WriteLine("Parsing identifier line: " 
                                     + _currentToken.getLine()
                                     + " index: "
                                     + _currentToken.getIndex());
            Accept(TokenKind.Identifier);
        }

        void ParseIntLiteral() {
            System.Console.WriteLine("Parsing int literal line: " 
                                     + _currentToken.getLine()
                                     + " index: "
                                     + _currentToken.getIndex());
            Accept(TokenKind.IntLiteral);
        }

        void ParseCharLiteral() {
            System.Console.WriteLine("Parsing char literal line: " 
                                     + _currentToken.getLine()
                                     + " index: "
                                     + _currentToken.getIndex());
            Accept(TokenKind.CharLiteral);
        }

        void ParseOperator() {
            System.Console.WriteLine("Parsing operator line: " 
                                     + _currentToken.getLine()
                                     + " index: "
                                     + _currentToken.getIndex());
            Accept(TokenKind.Operator);
        }
    }
}
