using Triangle.Compiler.SyntaxTrees.Actuals;
using Triangle.Compiler.SyntaxTrees.Expressions;
using Triangle.Compiler.SyntaxTrees.Vnames;


namespace Triangle.Compiler.SyntacticAnalyzer
{
    public partial class Parser
    {

        ///////////////////////////////////////////////////////////////////////////////
        //
        // PARAMETERS
        //
        ///////////////////////////////////////////////////////////////////////////////

       

       

        /**
         * Parses the actual parameter sequence, and constructs an AST to represent
         * its phrase structure.
         * 
         * @return an
         *         {@link triangle.compiler.syntax.trees.actuals.ActualParameterSequence}
         * 
         * @throws SyntaxError
         *           a syntactic error
         * 
         */
        ActualParameterSequence ParseActualParameterSequence()
        {
            var startLocation = _currentToken.Position.Start;
            if (_currentToken.Kind == TokenKind.RightParen)
            {
                var pos = new SourcePosition(startLocation, _currentToken.Position.Finish);
                return new EmptyActualParameterSequence(pos);
            } else
            {
                return ParseProperActualParameterSequence();
            }
        }

        /**
         * Parses the proper (non-empty) actual parameter sequence, and constructs an
         * AST to represent its phrase structure.
         * 
         * @return an
         *         {@link triangle.compiler.syntax.trees.actuals.ActualParameterSequence}
         * 
         * @throws SyntaxError
         *           a syntactic error
         * 
         */
        ActualParameterSequence ParseProperActualParameterSequence()
        {
            var startLocation = _currentToken.Position.Start;
            ActualParameter param = ParseActualParameter();
            if (_currentToken.Kind == TokenKind.Comma)
            {
                AcceptIt();
                ActualParameterSequence seq = ParseProperActualParameterSequence();
                var pos = new SourcePosition(startLocation, _currentToken.Position.Finish);
                return new MultipleActualParameterSequence(param, seq, pos);
            }
            else
            {
                var pos = new SourcePosition(startLocation, _currentToken.Position.Finish);
                return new SingleActualParameterSequence(param, pos);
            }
           
        }

        /**
         * Parses the actual parameter, and constructs an AST to represent its phrase
         * structure.
         * 
         * @return an {@link triangle.compiler.syntax.trees.actuals.ActualParameter}
         * 
         * @throws SyntaxError
         *           a syntactic error
         * 
         */
        ActualParameter ParseActualParameter()
        {
            var startLocation = _currentToken.Position.Start;
            switch (_currentToken.Kind)
            {
                case TokenKind.Identifier:
                case TokenKind.IntLiteral:
                case TokenKind.CharLiteral:
                case TokenKind.Operator:
                case TokenKind.Let:
                case TokenKind.If:
                case TokenKind.LeftParen:
                case TokenKind.LeftBracket:
                case TokenKind.LeftCurly:
                    {
                        Expression exp = ParseExpression();
                        var pos = new SourcePosition(startLocation, _currentToken.Position.Finish);
                        return new ConstActualParameter(exp, pos);
                    }

                case TokenKind.Var:
                    {
                        AcceptIt();
                        Vname varName = ParseVname();
                        var pos = new SourcePosition(startLocation, _currentToken.Position.Finish);
                        return new VarActualParameter(varName, pos);
                    }

                default:
                    {
                        RaiseSyntacticError("\"%\" cannot start an actual parameter", _currentToken);
                        return null;
                    }

            }

        }
    }
}