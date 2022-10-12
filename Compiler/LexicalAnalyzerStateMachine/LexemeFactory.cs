using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine.States;
using Compiler.Structs;

namespace Compiler.LexicalAnalyzerStateMachine
{
    public static class LexemeFactory
    {
        public static ILexeme CreateLexemeByState(IState state, Coordinate coordinate, string source)
        {
            if (state is DecimalInteger)
            {
                return new Integer(coordinate, source);
            }

            return new Error();
        }
    }
}