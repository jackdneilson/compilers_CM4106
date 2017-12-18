using Triangle.Compiler.SyntaxTrees.Declarations;
using Triangle.Compiler.SyntaxTrees.Expressions;
using Triangle.Compiler.SyntaxTrees.Terminals;
using Triangle.Compiler.SyntaxTrees.Types;
using Triangle.Compiler.SyntaxTrees.Visitors;

namespace Triangle.Compiler.ContextualAnalyzer
{
    public partial class Checker : IExpressionVisitor<Void, TypeDenoter>
    {
        // Expressions

        // Returns the TypeDenoter denoting the type of the expression. Does
        // not use the given object.

        //Visitor method for a binary expression. Visits both sides of the expression, then the operator.
        //Reverse Polish notation is used in expression resolution. Also checks types for errors
        public TypeDenoter VisitBinaryExpression(BinaryExpression ast, Void arg)
        {
            var e1Type = ast.LeftExpression.Visit(this);
            var e2Type = ast.RightExpression.Visit(this);
            var binding = ast.Operator.Visit(this);

            var bbinding = binding as BinaryOperatorDeclaration;
            if (bbinding != null)
            {
                if (bbinding.FirstArgument == StandardEnvironment.AnyType)
                {
                    // this operator must be "=" or "\="
                    CheckAndReportError(e1Type.Equals(e2Type), "incompatible argument types for \"%\"",
                        ast.Operator, ast);
                }
                else
                {
                    CheckAndReportError(e1Type.Equals(bbinding.FirstArgument),
                        "wrong argument type for \"%\"", ast.Operator, ast.LeftExpression);
                    CheckAndReportError(e2Type.Equals(bbinding.SecondArgument),
                        "wrong argument type for \"%\"", ast.Operator, ast.RightExpression);
                }
                return ast.Type = bbinding.Result;
            }

            ReportUndeclaredOrError(binding, ast.Operator, "\"%\" is not a binary operator");
            return ast.Type = StandardEnvironment.ErrorType;
        }

        //Visitor method for a call expression. Visits the identifier (retrieving from the symbol table), then all of 
        //the formal parameters.
        public TypeDenoter VisitCallExpression(CallExpression ast, Void arg)
        {
            var binding = ast.Identifier.Visit(this);
            var function = binding as IFunctionDeclaration;
            if (function != null)
            {
                ast.Actuals.Visit(this, function.Formals);
                return ast.Type = function.Type;
            }

            ReportUndeclaredOrError(binding, ast.Identifier, "\"%\" is not a function identifier");
            return ast.Type = StandardEnvironment.ErrorType;
        }

        //Visitor method for a character expression. Simply returns the type of the expression (char).
        public TypeDenoter VisitCharacterExpression(CharacterExpression ast, Void arg)
        {
            return ast.Type = StandardEnvironment.CharType;
        }

        //Visitor method for an empty expression
        public TypeDenoter VisitEmptyExpression(EmptyExpression ast, Void arg)
        {
            return ast.Type = null;
        }

        //Visitor method for an if expression. Visits the evaluated expression as well as both true and false
        //expressions and performs type checking.
        public TypeDenoter VisitIfExpression(IfExpression ast, Void arg)
        {
            var e1Type = ast.TestExpression.Visit(this);
            CheckAndReportError(e1Type == StandardEnvironment.BooleanType,
                "Boolean expression expected here", ast.TestExpression);
            var e2Type = ast.TrueExpression.Visit(this);
            var e3Type = ast.FalseExpression.Visit(this);
            CheckAndReportError(e2Type.Equals(e3Type), "incompatible limbs in if-expression", ast);
            return ast.Type = e2Type;
        }

        //Visitor method for an integer expression. Simple returns the type (integer).
        public TypeDenoter VisitIntegerExpression(IntegerExpression ast, Void arg)
        {
            return ast.Type = StandardEnvironment.IntegerType;
        }

        //Visitor method for a let expression. Opens a new scope, then visits the declaration and expression.
        //Finally, closes the opened scope and returns the type of the expression.
        public TypeDenoter VisitLetExpression(LetExpression ast, Void arg)
        {
            _idTable.OpenScope();
            ast.Declaration.Visit(this);
            ast.Expression.Visit(this);
            _idTable.CloseScope();
            return ast.Type;
        }


        //Visitor method for a unary expression. Performs type checking on the given expression and operator
        public TypeDenoter VisitUnaryExpression(UnaryExpression ast, Void arg)
        {
            var expressionType = ast.Expression.Visit(this);
            var binding = ast.Operator.Visit(this);
            var ubinding = binding as UnaryOperatorDeclaration;
            if (ubinding != null)
            {
                CheckAndReportError(expressionType.Equals(ubinding.Argument),
                    "wrong argument type for \"%\"", ast.Operator);
                return ast.Type = ubinding.Result;
            }

            ReportUndeclaredOrError(binding, ast.Operator, "\"%\" is not a unary operator");
            return ast.Type = StandardEnvironment.ErrorType;
        }

        //Visitor method for a variable name expression. Visits the variablle name, retrieving it's type from the 
        //symbol table then returns its type.
        public TypeDenoter VisitVnameExpression(VnameExpression ast, Void arg)
        {
            var vnameType = ast.Vname.Visit(this);
            return ast.Type = vnameType;
        }
    }
}