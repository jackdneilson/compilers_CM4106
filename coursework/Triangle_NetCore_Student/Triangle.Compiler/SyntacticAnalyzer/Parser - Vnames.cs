using Triangle.Compiler.SyntaxTrees.Terminals;
using Triangle.Compiler.SyntaxTrees.Vnames;

namespace Triangle.Compiler.SyntacticAnalyzer
{
    public partial class Parser
    {

        // /////////////////////////////////////////////////////////////////////////////
        //
        // VALUE-OR-VARIABLE NAMES
        //
        // /////////////////////////////////////////////////////////////////////////////

        /**
         * Parses the v-name, and constructs an AST to represent its phrase structure.
         * 
         * @return a {@link triangle.compiler.syntax.trees.vnames.Vname}
         * 
         * @throws SyntaxError
         *           a syntactic error
         * 
         */
        Vname ParseVname()
        {
            Identifier ident = ParseIdentifier();
            return ParseRestOfVname(ident);
        }

        Vname ParseRestOfVname(Identifier ident)
        {
            var pos = new SourcePosition(ident.Start, ident.Finish);
            return new SimpleVname(ident, pos);
        }
    }
}