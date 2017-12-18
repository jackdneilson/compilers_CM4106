using Triangle.Compiler.SyntaxTrees.Declarations;
using Triangle.Compiler.SyntaxTrees.Terminals;
using Triangle.Compiler.SyntaxTrees.Types;
using Triangle.Compiler.SyntaxTrees.Visitors;

namespace Triangle.Compiler.ContextualAnalyzer
{
    public partial class Checker : ILiteralVisitor<Void, TypeDenoter>,
            IIdentifierVisitor<Void, Declaration>,
            IOperatorVisitor<Void, Declaration>
    {
        // Literals, Identifiers and Operators

        //Visitor for a char literal. Type can be inferred so is returned without lookup to symbol table
        public TypeDenoter VisitCharacterLiteral(CharacterLiteral literal, Void arg)
        {
            return StandardEnvironment.CharType;
        }

        //Visitor for an identifier. Retrieves entry from the symbol table then returns
        public Declaration VisitIdentifier(Identifier identifier, Void arg)
        {
            var binding = _idTable.Retrieve(identifier.Spelling);
            if (binding != null)
            {
                identifier.Declaration = binding;
            }
            return binding;
        }

        //Visitor for an int literal. Type can be inferred so is returned without lookup to symbol table
        public TypeDenoter VisitIntegerLiteral(IntegerLiteral literal, Void arg)
        {
            return StandardEnvironment.IntegerType;
        }

        //Visitor for an operation. Looks up the operator in the symbol table then returns
        public Declaration VisitOperator(Operator op, Void arg)
        {
            var binding = _idTable.Retrieve(op.Spelling);
            if (binding != null)
            {
                op.Declaration = binding;
            }
            return binding;
        }

    }
}