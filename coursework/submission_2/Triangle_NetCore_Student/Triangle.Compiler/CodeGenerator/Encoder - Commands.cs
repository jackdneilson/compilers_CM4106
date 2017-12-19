using Triangle.AbstractMachine;
using Triangle.Compiler.SyntaxTrees.Commands;
using Triangle.Compiler.SyntaxTrees.Visitors;

namespace Triangle.Compiler.CodeGenerator
{
    public partial class Encoder : ICommandVisitor<Frame, Void>
    {
        // Commands

        //Visitor for assign command
        public Void VisitAssignCommand(AssignCommand ast, Frame frame)
        {
            var valSize = ast.Expression.Visit(this, frame);
            EncodeAssign(ast.Vname, frame.Expand(valSize), valSize);
            return null;
        }

        //Visitor for call command
        public Void VisitCallCommand(CallCommand ast, Frame frame)
        {
            int argsSize = ast.Actuals.Visit(this, frame);
            ast.Identifier.Visit(this, frame.Replace(argsSize));
            return null;
        }

        public Void VisitEmptyCommand(EmptyCommand ast, Frame frame)
        {
            return null;
        }

        //Visitor for if command, backpatches jump addresses so only the correct visited command is executed
        public Void VisitIfCommand(IfCommand ast, Frame frame)
        {
            ast.Expression.Visit(this, frame);
            var jumpifAddr = _emitter.Emit(OpCode.JUMPIF, Machine.FalseValue, Register.CB);
            ast.TrueCommand.Visit(this, frame);
            var jumpAddr = _emitter.Emit(OpCode.JUMP, Register.CB);
            _emitter.Patch(jumpifAddr);
            ast.FalseCommand.Visit(this, frame);
            _emitter.Patch(jumpAddr);
            return null;
        }

        //Visitor for let command
        public Void VisitLetCommand(LetCommand ast, Frame frame)
        {
            var extraSize = ast.Declaration.Visit(this, frame);
            ast.Command.Visit(this, frame.Expand(extraSize));
            if (extraSize > 0)
            {
                _emitter.Emit(OpCode.POP, (short)extraSize);
            }
            return null;
        }

        //Visitor for sequential commands, note secondcommand may be another sequentialcommand to allow for chaining of
        //commands.
        public Void VisitSequentialCommand(SequentialCommand ast, Frame frame)
        {
            ast.FirstCommand.Visit(this, frame);
            ast.SecondCommand.Visit(this, frame);
            return null;
        }

        //Visitor for while command, backpatches the start addres of the loop command to the jump command
        public Void VisitWhileCommand(WhileCommand ast, Frame frame)
        {
            var jumpAddr = _emitter.Emit(OpCode.JUMP, Register.CB);
            var loopAddr = _emitter.NextInstrAddr;
            ast.Command.Visit(this, frame);
            _emitter.Patch(jumpAddr);
            ast.Expression.Visit(this, frame);
            _emitter.Emit(OpCode.JUMPIF, Machine.TrueValue, Register.CB, loopAddr);
            return null;
        }
    }
}