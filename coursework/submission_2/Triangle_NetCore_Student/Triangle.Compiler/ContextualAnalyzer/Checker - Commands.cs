using Triangle.Compiler.SyntaxTrees.Commands;
using Triangle.Compiler.SyntaxTrees.Declarations;
using Triangle.Compiler.SyntaxTrees.Visitors;

namespace Triangle.Compiler.ContextualAnalyzer
{
    public partial class Checker : ICommandVisitor
    {

        // Commands
        //Visitor method for an assign command, adds the declared variable to the symbol table then checks itself and
        //its children for errors
        public Void VisitAssignCommand(AssignCommand ast, Void arg)
        {
            var vnameType = ast.Vname.Visit(this);
            var expressionType = ast.Expression.Visit(this);            
            CheckAndReportError(ast.Vname.IsVariable, "LHS of assignment is not a variable",
                ast.Vname);
            CheckAndReportError(expressionType.Equals(vnameType), "assignment incompatibilty", ast);
            return null;
        }

        //Visitor method for a call command, retrieves the procedure to be called and checks the parameters given for 
        //errors. Also checks the procedure spelling exists in the symbol table
        public Void VisitCallCommand(CallCommand ast, Void arg)
        {
            var binding = ast.Identifier.Visit(this);
            var procedure = binding as IProcedureDeclaration;
            if (procedure != null)
            {
                ast.Actuals.Visit(this, procedure.Formals);
            }
            else
            {
                ReportUndeclaredOrError(binding, ast.Identifier, "\"%\" is not a procedure identifier");
            }
            return null;
        }

        //Visitor for an empty command
        public Void VisitEmptyCommand(EmptyCommand ast, Void arg)
        {
            return null;
        }

        //Visitor method for an if command, checks the type of the expression then visits the true and false commands
        public Void VisitIfCommand(IfCommand ast, Void arg)
        {
            var expressionType = ast.Expression.Visit(this);
            CheckAndReportError(expressionType == StandardEnvironment.BooleanType,
                "Boolean expression expected here", ast.Expression);
            ast.TrueCommand.Visit(this);
            ast.FalseCommand.Visit(this);
            return null;
        }

        //Visitor method for a let command, opens a new scope then adds any declarations to the symbol table, then 
        //visits the command
        public Void VisitLetCommand(LetCommand ast, Void arg)
        {
            _idTable.OpenScope();
            ast.Declaration.Visit(this);
            ast.Command.Visit(this);
            _idTable.CloseScope();
            return null;
        }

        //Visitor method for sequential commands, visits the first command then visits the second command (the second
        //command may be another sequential command to allow for chains of sequential commands)
        public Void VisitSequentialCommand(SequentialCommand ast, Void arg)
        {
            ast.FirstCommand.Visit(this);
            ast.SecondCommand.Visit(this);
            return null;
        }

        //Visitor method for while commands, checks the type of the expression given then visits the command
        public Void VisitWhileCommand(WhileCommand ast, Void arg)
        {
            var expressionType = ast.Expression.Visit(this);
            CheckAndReportError(expressionType == StandardEnvironment.BooleanType,
                "Boolean expression expected here", ast.Expression);
            ast.Command.Visit(this);
            return null;
        }
    }
}