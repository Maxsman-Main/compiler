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
                return new Integer(coordinate, source, source, 10, 1);
            }

            if (state is HexInteger)
            {
                var valueForConvert = ReplaceString(source, "$", "0x");
                var sign = GetSign(source);
                return new Integer(coordinate, source, valueForConvert, 16, sign);
            }

            if (state is OctInteger)
            {
                var valueForConvert = ReplaceString(source, "&", "");
                var sign = GetSign(source);
                return new Integer(coordinate, source, valueForConvert, 8, sign);
            }

            if (state is BinInteger)
            {
                var valueForConvert = ReplaceString(source, "%", "");
                var sign = GetSign(source);
                return new Integer(coordinate, source, valueForConvert, 2, sign);
            }

            if (state is FloatState)
            {
                return new Float(coordinate, source);
            }
            
            return new Error(coordinate, "Uncorrected lexeme", source);
        }

        private string ReplaceString(string word, string oldString, string newString)
        {
            var sb = new StringBuilder(word);
            sb.Replace(oldString, newString);
            sb.Replace("+", "");
            sb.Replace("-", "");
            return sb.ToString();
        }

        private int GetSign(string word)
        {
            var sign = word[0] == '-' ? -1 : 1;
            return sign;
        }
    }
}