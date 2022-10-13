using System.Text;
using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine.States;
using Compiler.Structs;

namespace Compiler.LexicalAnalyzerStateMachine
{
    public class LexemeFactory
    {
        public ILexeme CreateLexemeByState(IState state, Coordinate coordinate, string source)
        {
            if (state is DecimalInteger)
            {
                return new Integer(coordinate, source, source, 10);
            }

            if (state is HexInteger)
            {
                var valueForConvert = ReplaceString(source, "$", "0x");
                return new Integer(coordinate, source, valueForConvert, 16);
            }

            if (state is OctInteger)
            {
                var valueForConvert = ReplaceString(source, "&", "");
                return new Integer(coordinate, source, valueForConvert, 8);
            }

            return new Error(coordinate, "Uncorrected lexeme", source);
        }

        private string ReplaceString(string word, string oldString, string newString)
        {
            var sb = new StringBuilder(word);
            sb.Replace(oldString, newString);
            return sb.ToString();
        }
    }
}