using System.Text;
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
                return new Integer(coordinate, source, 10);
            }

            if (state is HexInteger)
            {
                var sb = new StringBuilder(source);
                sb.Replace("$", "0x");
                source = sb.ToString();
                return new Integer(coordinate, source, 16);
            }

            return new Error();
        }
    }
}