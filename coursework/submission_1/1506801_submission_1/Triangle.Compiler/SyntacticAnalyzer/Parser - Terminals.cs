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
            System.Console.WriteLine("parsing identifier");
            Accept(TokenKind.Identifier);
        }

        //Parses a single integer literal
        void ParseIntLiteral() {
            System.Console.WriteLine("parsing integer");
            Accept(TokenKind.IntLiteral);
        }

        //Parses a single character literal
        void ParseCharLiteral() {
            System.Console.WriteLine("parsing char literal");
            Accept(TokenKind.CharLiteral);
        }

        //Parses a single operator
        void ParseOperator() {
            System.Console.WriteLine("parsing operator");
            Accept(TokenKind.Operator);
        }
    }
}
