using Triangle.Compiler.SyntaxTrees.Declarations;
using Triangle.Compiler.SyntaxTrees.Terminals;
using Triangle.Compiler.SyntaxTrees.Expressions;
using Triangle.Compiler.SyntaxTrees.Types;

namespace Triangle.Compiler.SyntacticAnalyzer
{
    public partial class Parser
    {

        ///////////////////////////////////////////////////////////////////////////////
        //
        // DECLARATIONS
        //
        ///////////////////////////////////////////////////////////////////////////////

        /**
         * Parses the declaration, and constructs an AST to represent its phrase
         * structure.
         * 
         * @return a {@link triangle.compiler.syntax.trees.declarations.Declaration}
         * 
         * @throws SyntaxError
         *           a syntactic error
         * 
         */
        Declaration ParseDeclaration()
        {
            var startLocation = _currentToken.Start;
            Declaration dec = ParseSingleDeclaration();
            while (_currentToken.Kind == TokenKind.Semicolon)
            {
                AcceptIt();
                Declaration dec2 = ParseSingleDeclaration();
                var pos = new SourcePosition(startLocation, _currentToken.Finish);
                dec = new SequentialDeclaration(dec, dec2, pos);
            }
            return dec;
        }

        /**
         * Parses the single declaration, and constructs an AST to represent its
         * phrase structure.
         * 
         * @return a {@link triangle.compiler.syntax.trees.declarations.Declaration}
         * 
         * @throws SyntaxError
         *           a syntactic error
         * 
         */
        Declaration ParseSingleDeclaration()
        {
            var startLocation = _currentToken.Start;
            switch (_currentToken.Kind)
            {

                case TokenKind.Const:
                    {
                        AcceptIt();
                        Identifier ident = ParseIdentifier();
                        Accept(TokenKind.Is);
                        Expression exp = ParseExpression();
                        var pos = new SourcePosition(startLocation, _currentToken.Finish);
                        return new ConstDeclaration(ident, exp, pos);
                    }

                case TokenKind.Var:
                    {
                        AcceptIt();
                        Identifier ident = ParseIdentifier();
                        Accept(TokenKind.Colon);
                        TypeDenoter type = ParseTypeDenoter();
                        var pos = new SourcePosition(startLocation, _currentToken.Finish);
                        return new VarDeclaration(ident, type, pos);
                    }

                case TokenKind.Type:
                    {
                        AcceptIt();
                        var identifier = ParseIdentifier();
                        Accept(TokenKind.Is);
                        var typeDenoter = ParseTypeDenoter();
                        var pos = new SourcePosition(startLocation, _currentToken.Finish);
                        return new TypeDeclaration(identifier, typeDenoter, pos);
                    }

                default:
                    {
                        RaiseSyntacticError("\"%\" cannot start a declaration", _currentToken);
                        return null;
                    }
            }
        }
    }
}