using Triangle.Compiler.SyntaxTrees.Declarations;
using Triangle.Compiler.SyntaxTrees.Types;
using Triangle.Compiler.SyntaxTrees.Visitors;

namespace Triangle.Compiler.ContextualAnalyzer
{
    public partial class Checker : ITypeDenoterVisitor<Void, TypeDenoter>
    {
        // Type Denoters

        // Returns the expanded version of the TypeDenoter. Does not
        // use the given object.

        //Visitors for all primitive type denoters
        public TypeDenoter VisitAnyTypeDenoter(AnyTypeDenoter ast, Void arg)
        {
            return StandardEnvironment.AnyType;
        }

        public TypeDenoter VisitBoolTypeDenoter(BoolTypeDenoter ast, Void arg)
        {
            return StandardEnvironment.BooleanType;
        }

        public TypeDenoter VisitCharTypeDenoter(CharTypeDenoter ast, Void arg)
        {
            return StandardEnvironment.CharType;
        }

        public TypeDenoter VisitErrorTypeDenoter(ErrorTypeDenoter ast, Void arg)
        {
            return StandardEnvironment.ErrorType;
        }

        //Visitor for a variable type denoter, retrieves variable type from symbol table
        public TypeDenoter VisitSimpleTypeDenoter(SimpleTypeDenoter ast, Void arg)
        {

            var binding = ast.Identifier.Visit(this);
            var decl = binding as TypeDeclaration;
            if (decl != null)
            {
                return decl.Type;
            }

            ReportUndeclaredOrError(binding, ast.Identifier, "\"%\" is not a type identifier");
            return StandardEnvironment.ErrorType;
        }

        public TypeDenoter VisitIntTypeDenoter(IntTypeDenoter ast, Void arg)
        {
            return StandardEnvironment.IntegerType;
        }

       

    }
}