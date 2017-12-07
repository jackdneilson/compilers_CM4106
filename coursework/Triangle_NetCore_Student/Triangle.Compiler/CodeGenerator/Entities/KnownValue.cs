using Triangle.AbstractMachine;
using Triangle.Compiler.SyntaxTrees.Vnames;

namespace Triangle.Compiler.CodeGenerator.Entities
{
    public class KnownValue : RuntimeEntity, IFetchableEntity
    {

        readonly int _value;

        public KnownValue(int size, int value)
            : base(size)
        {
            _value = value;
        }

        public void EncodeFetch(Emitter emitter, Frame frame, int size, Vname vname)
        {
          
        }
    }
}