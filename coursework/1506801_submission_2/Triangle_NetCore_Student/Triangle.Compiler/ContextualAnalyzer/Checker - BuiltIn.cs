using Triangle.Compiler.SyntaxTrees.Declarations;
using Triangle.Compiler.SyntaxTrees.Formals;
using Triangle.Compiler.SyntaxTrees.Visitors;

namespace Triangle.Compiler.ContextualAnalyzer
{
	public partial class Checker : IDeclarationVisitor, IFormalParameterVisitor,
			IFormalParameterSequenceVisitor
    {
        
	    //Visitor function for a function declaration
        public Void VisitFuncDeclaration(FuncDeclaration ast, Void arg)
        {
            ast.Type = ast.Type.Visit(this);
            // permits recursion
            _idTable.Enter(ast.Identifier, ast);
            CheckAndReportError(!ast.Duplicated, "identifier \"%\" already declared",
                ast.Identifier, ast);
            _idTable.OpenScope();
            ast.Formals.Visit(this);
            var expressionType = ast.Expression.Visit(this);
            _idTable.CloseScope();
            CheckAndReportError(ast.Type.Equals(expressionType),
                "body of function \"%\" has wrong type", ast.Identifier, ast.Expression);
            return null;
        }

	    //Visitor function for a procedure declaration
        public Void VisitProcDeclaration(ProcDeclaration ast, Void arg)
        {
            // permits recursion
            _idTable.Enter(ast.Identifier, ast);
            CheckAndReportError(!ast.Duplicated, "identifier \"%\" already declared",
                ast.Identifier, ast);
            _idTable.OpenScope();
            ast.Formals.Visit(this);
            ast.Command.Visit(this);
            _idTable.CloseScope();
            return null;
        }

	    //Visitor function for a formal constant parameter declaration
		public Void VisitConstFormalParameter(ConstFormalParameter ast, Void arg)
		{
			ast.Type = ast.Type.Visit(this);
			_idTable.Enter(ast.Identifier, ast);
			CheckAndReportError(!ast.Duplicated, "duplicated formal parameter \"%\"",
				ast.Identifier, ast);
			return null;
		}

	    //Visitor function for a formal function parameter declaration
		public Void VisitFuncFormalParameter(FuncFormalParameter ast, Void arg)
		{
			_idTable.OpenScope();
			ast.Formals.Visit(this);
			_idTable.CloseScope();
			ast.Type = ast.Type.Visit(this);
			_idTable.Enter(ast.Identifier, ast);
			CheckAndReportError(!ast.Duplicated, "duplicated formal parameter \"%\"",
				ast.Identifier, ast);
			return null;
		}
		
	    //Visitor function for a formal procedure parameter declaration
		public Void VisitProcFormalParameter(ProcFormalParameter ast, Void arg)
		{
			_idTable.OpenScope();
			ast.Formals.Visit(this);
			_idTable.CloseScope();
			_idTable.Enter(ast.Identifier, ast);
			CheckAndReportError(!ast.Duplicated, "duplicated formal parameter \"%\"",
				ast.Identifier, ast);
			return null;
		}

	    //Visitor function for a formal variable parameter declaration
		public Void VisitVarFormalParameter(VarFormalParameter ast, Void arg)
		{
			ast.Type = ast.Type.Visit(this);
			_idTable.Enter(ast.Identifier, ast);
			CheckAndReportError(!ast.Duplicated, "duplicated formal parameter \"%\"",
				ast.Identifier, ast);
			return null;
		}

	    //Visitor function for an empty formal parameter sequence
		public Void VisitEmptyFormalParameterSequence(EmptyFormalParameterSequence ast, Void arg)
		{
			return null;
		}

	    //Visitor function for a a formal parameter sequence containing multiple formal parameters
		public Void VisitMultipleFormalParameterSequence(MultipleFormalParameterSequence ast, Void arg)
		{
			ast.Formal.Visit(this);
			ast.Formals.Visit(this);
			return null;
		}

		public Void VisitSingleFormalParameterSequence(SingleFormalParameterSequence ast, Void arg)
		{
			ast.Formal.Visit(this);
			return null;
		}

    }
}