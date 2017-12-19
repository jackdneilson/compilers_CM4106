
using Triangle.Compiler.SyntaxTrees.Expressions;
using Triangle.Compiler.SyntaxTrees.Declarations;
using Triangle.Compiler.SyntaxTrees.Terminals;
using Triangle.Compiler.SyntaxTrees.Vnames;
using Triangle.Compiler.SyntaxTrees.Actuals;

namespace Triangle.Compiler.SyntacticAnalyzer
{
    public partial class Parser
    {
        ///////////////////////////////////////////////////////////////////////////////
        //
        // EXPRESSIONS
        //
        ///////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Parses the expression, and constructs an AST to represent its phrase
        /// structure.
        /// </summary>
        /// <returns>
        /// an <link>Triangle.SyntaxTrees.Expressions.Expression</link>
        /// </returns> 
        /// <throws type="SyntaxError">
        /// a syntactic error
        /// </throws>
        Expression ParseExpression()
        {
            var startLocation = _currentToken.Start;

            switch (_currentToken.Kind)
            {
                case TokenKind.Let:
                    {
                        AcceptIt();
                        Declaration dec = ParseDeclaration();
                        Accept(TokenKind.In);
                        Expression exp = ParseExpression();
                        var pos = new SourcePosition(startLocation, _currentToken.Finish);
                        return new LetExpression(dec, exp, pos);
                    }

                case TokenKind.If:
                    {
                        AcceptIt();
                        Expression ifExp = ParseExpression();
                        Accept(TokenKind.Then);
                        Expression thenExp = ParseExpression();
                        Accept(TokenKind.Else);
                        Expression elseExp = ParseExpression();
                        var pos = new SourcePosition(startLocation, _currentToken.Finish);
                        return new IfExpression(ifExp, thenExp, elseExp, pos);
                    }

                default:
                    {
                        return ParseSecondaryExpression();
                    }
            }
        }

        /// <summary>
        // Parses the secondary expression, and constructs an AST to represent its
        /// phrase structure.
        /// </summary>
        /// <returns>
        /// an <link>Triangle.SyntaxTrees.Expressions.Expression</link>
        /// </returns>
        /// <throws type="SyntaxError">
        /// a syntactic error
        /// </throws>
        Expression ParseSecondaryExpression()
        {
            var startLocation = _currentToken.Start;
            Expression exp = ParsePrimaryExpression();
            while (_currentToken.Kind == TokenKind.Operator)
            {
                Operator op = ParseOperator();
                Expression exp2 = ParsePrimaryExpression();
                var pos = new SourcePosition(startLocation, _currentToken.Finish);
                exp = new BinaryExpression(exp, op, exp2, pos);
            }
            return exp;
        }

        /// <summary>
        /// Parses the primary expression, and constructs an AST to represent its
        /// phrase structure.
        /// </summary>
        /// <returns>
        /// an <link>Triangle.SyntaxTrees.Expressions.Expression</link>
        /// </returns>
        /// <throws type="SyntaxError">
        /// a syntactic error
        /// </throws>
        Expression ParsePrimaryExpression()
        {
            var startlocation = _currentToken.Start;
            switch (_currentToken.Kind)
            {
                case TokenKind.IntLiteral:
                    {
                        IntegerLiteral lit = ParseIntegerLiteral();
                        var pos = new SourcePosition(startlocation, _currentToken.Finish);
                        return new IntegerExpression(lit, pos);
                    }

                case TokenKind.CharLiteral:
                    {
                        CharacterLiteral character = ParseCharacterLiteral();
                        var pos = new SourcePosition(startlocation, _currentToken.Finish);
                        return new CharacterExpression(character, pos);
                    }


                case TokenKind.Identifier:
                    {
                        Identifier ident = ParseIdentifier();
                        if (_currentToken.Kind == TokenKind.LeftParen)
                        {
                            AcceptIt();
                            ActualParameterSequence seq = ParseActualParameterSequence();
                            Accept(TokenKind.RightParen);
                            var pos = new SourcePosition(startlocation, _currentToken.Finish);
                            return new CallExpression(ident, seq, pos);
                        }
                        else
                        {
                            Vname varName = ParseRestOfVname(ident);
                            var pos = new SourcePosition(startlocation, _currentToken.Finish);
                            return new VnameExpression(varName, pos);
                        }
                    }

                case TokenKind.Operator:
                    {
                        Operator op = ParseOperator();
                        Expression exp = ParsePrimaryExpression();
                        var pos = new SourcePosition(startlocation, _currentToken.Finish);
                        return new UnaryExpression(op, exp, pos);
                    }

                case TokenKind.LeftParen:
                    {
                        AcceptIt();
                        Expression exp = ParseExpression();
                        Accept(TokenKind.RightParen);
                        return exp;
                    }

                default:
                    {
                        RaiseSyntacticError("\"%\" cannot start an expression", _currentToken);
                        return null;
                    }
            }
        }

    }
}