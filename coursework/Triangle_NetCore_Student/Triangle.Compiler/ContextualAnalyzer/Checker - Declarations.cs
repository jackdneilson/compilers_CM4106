using Triangle.Compiler.SyntaxTrees.Declarations;
using Triangle.Compiler.SyntaxTrees.Visitors;

namespace Triangle.Compiler.ContextualAnalyzer
{
    public partial class Checker : IDeclarationVisitor
    {
        
		//Visitor method for a constant declaration, visits the expression then adds to the symbol table
        public Void VisitConstDeclaration(ConstDeclaration ast, Void arg)
        {
	        ast.Expression.Visit(this);
	        _idTable.Enter(ast.Identifier, ast);
	        CheckAndReportError(!ast.Duplicated, "identifier \"%\" already declared",
		        ast.Identifier, ast);
            return null;
        }

	    //Visitor method for a variable declaration, gets the type of the variable then adds it to the symbol table
		public Void VisitVarDeclaration(VarDeclaration ast, Void arg)
		{
			ast.Type = ast.Type.Visit(this);
			_idTable.Enter(ast.Identifier, ast);
			CheckAndReportError(!ast.Duplicated, "identifier \"%\" already declared",
				ast.Identifier, ast);
			return null;
		}

	    //Visitor method for a sequential declaration, visits the first declaration then visits the second declaration
	    //(the second declaration may be another sequential declaration to allow for chaining of multiple declarations)
        public Void VisitSequentialDeclaration(SequentialDeclaration ast, Void arg)
        {
	        ast.FirstDeclaration.Visit(this);
	        ast.SecondDeclaration.Visit(this);
            return null;
        }

	    //Visitor method for a type declaration, visits the type then adds to the symbol table
		public Void VisitTypeDeclaration(TypeDeclaration ast, Void arg)
		{
			ast.Type = ast.Type.Visit(this);
			_idTable.Enter(ast.Identifier, ast);
			CheckAndReportError(!ast.Duplicated, "identifier \"%\" already declared",
				ast.Identifier, ast);
			return null;
		}

	    //Visitor method for a unary operator declaration
        public Void VisitUnaryOperatorDeclaration(UnaryOperatorDeclaration ast, Void arg)
        {
            return null;
        }

	    //Visitor method for a binary operator declaration
		public Void VisitBinaryOperatorDeclaration(BinaryOperatorDeclaration ast, Void arg)
		{
			return null;
		}



    }
}