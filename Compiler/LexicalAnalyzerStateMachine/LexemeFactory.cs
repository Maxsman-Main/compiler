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
            if (state is DecimalEndState)
            {
                return new IntegerLexeme(coordinate, source, source, 10, 1);
            }

            if (state is HexEndState)
            {
                var valueForConvert = ReplaceString(source, "$", "0x");
                var sign = GetSign(source);
                return new IntegerLexeme(coordinate, source, valueForConvert, 16, sign);
            }

            if (state is OctEndState)
            {
                var valueForConvert = ReplaceString(source, "&", "");
                var sign = GetSign(source);
                return new IntegerLexeme(coordinate, source, valueForConvert, 8, sign);
            }

            if (state is BinEndState)
            {
                var valueForConvert = ReplaceString(source, "%", "");
                var sign = GetSign(source);
                return new IntegerLexeme(coordinate, source, valueForConvert, 2, sign);
            }

            if (state is FloatEndState)
            {
                return new FloatLexeme(coordinate, source);
            }
            
            if (state is StringEndState)
            {
                var value = ReplaceString(source, "\'", "");
                return new StringLexeme(coordinate, source, value);
            }

            if (state is CharEndState)
            {
                var value = ReplaceString(source, "\'", "");
                return new CharLexeme(coordinate, source, value);
            }

            if (state is EndOfFileState)
            {
                return new EndOfFileLexeme(coordinate);
            }
            
            return new ErrorLexeme(coordinate, "Uncorrected lexeme", source);
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