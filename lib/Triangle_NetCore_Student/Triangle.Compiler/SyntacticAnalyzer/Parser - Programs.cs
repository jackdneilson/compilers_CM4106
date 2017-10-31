

namespace Triangle.Compiler.SyntacticAnalyzer
{
    public partial class Parser
    {

        ///////////////////////////////////////////////////////////////////////////////
        //
        // PROGRAMS
        //
        ///////////////////////////////////////////////////////////////////////////////


        public void ParseProgram()
        {
                System.Console.WriteLine("Parsing Program");
                _tokens.MoveNext();
                _currentToken = _tokens.Current;
                //var startLocation = _currentToken.Start;
                ParseCommand();
        }
    }
}
