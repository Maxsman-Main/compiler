using System.Globalization;
using System.Text;
using Compiler.Constants;
using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine.States;
using Compiler.Structs;

namespace Compiler.LexicalAnalyzerStateMachine
{
    public class LexemeFactory
    {
        public ILexeme CreateLexemeByState(IState state, Coordinate coordinate, string source)
        {
            switch (state)
            {
                case DecimalInteger:
                {
                    var value = Convert.ToInt32(source, 10);
                    return new IntegerLexeme(coordinate, source, value);
                }
                case HexEndState:
                {
                    var valueForConvert = ReplaceString(source, "$", "0x");
                    var value = Convert.ToInt32(valueForConvert, 16);
                    return new IntegerLexeme(coordinate, source, value);
                }
                case OctEndState:
                {
                    var valueForConvert = ReplaceString(source, "&", "");
                    var value = Convert.ToInt32(valueForConvert, 8);
                    return new IntegerLexeme(coordinate, source, value);
                }
                case BinEndState:
                {
                    var valueForConvert = ReplaceString(source, "%", "");
                    var value = Convert.ToInt32(valueForConvert, 2);
                    return new IntegerLexeme(coordinate, source, value);
                }
                case FloatEndState:
                {
                    var tempCulture = Thread.CurrentThread.CurrentCulture;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                    var value = double.Parse(source);
                    Thread.CurrentThread.CurrentCulture = tempCulture;
                    return new FloatLexeme(coordinate, source, value);
                }
                case StringEndState:
                {
                    var value = ReplaceString(source, "\'", "");
                    return new StringLexeme(coordinate, source, value);
                }
                case CharEndState:
                {
                    var value = ReplaceString(source, "\'", "");
                    return new CharLexeme(coordinate, source, value);
                }
                case EndOfFileState:
                {
                    return new EndOfFileLexeme(coordinate);
                }
                case IdentifierEndState:
                {
                    if (KeyWordsConstants.KeyWords.Contains(source))
                    {
                        return new KeyWordLexeme(coordinate, source);
                    }
                    return new IdentifierLexeme(coordinate, source);
                }
                case OperatorEndState:
                {
                    return new OperatorLexeme(coordinate, source);
                }
                case SeparatorEndState:
                {
                    return new SeparatorLexeme(coordinate, source);   
                }
                default:
                    return new ErrorLexeme(coordinate, "IncorrectLexeme", source);
            }
        }

        private string ReplaceString(string word, string oldString, string newString)
        {
            var sb = new StringBuilder(word);
            sb.Replace(oldString, newString);
            sb.Replace("+", "");
            sb.Replace("-", "");
            return sb.ToString();
        }
    }
}