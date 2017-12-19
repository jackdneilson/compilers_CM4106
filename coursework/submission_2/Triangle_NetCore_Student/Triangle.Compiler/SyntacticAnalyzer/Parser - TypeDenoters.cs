using Triangle.Compiler.SyntaxTrees.Types;
using Triangle.Compiler.SyntaxTrees.Terminals;

namespace Triangle.Compiler.SyntacticAnalyzer
{
    public partial class Parser
    {

        // /////////////////////////////////////////////////////////////////////////////
        //
        // TYPE-DENOTERS
        //
        // /////////////////////////////////////////////////////////////////////////////

        /**
         * Parses the type denoter, and constructs an AST to represent its phrase
         * structure.
         * 
         * @return a {@link triangle.compiler.syntax.trees.types.TypeDenoter}
         * 
         * @throws SyntaxError
         *           a syntactic error
         * 
         */
        TypeDenoter ParseTypeDenoter()
        {
            var startLocation = _currentToken.Start;

            switch (_currentToken.Kind)
            {
                case TokenKind.Identifier:
                    {
                        Identifier ident = ParseIdentifier();
                        var pos = new SourcePosition(startLocation, _currentToken.Finish);
                        return new SimpleTypeDenoter(ident, pos);
                    }

                default:
                    {
                        RaiseSyntacticError("\"%\" cannot start a type denoter", _currentToken);
                        return null;
                    }
            }
        }
    }
}