using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine;

namespace Compiler
{
    // Исправить:
    // Строки +
    // Дестячные целые +
    // Hex целые 
    // Oct целые 
    // Bin целые
    // Char
    public static class Program
    {
        public static void Main()
        {
            var file = "test.txt";
            var analyzer = new LexicalAnalyzer();
            analyzer.SetFile(file);
            ILexeme lexeme;
            do
            {
                lexeme = analyzer.GetLexeme();
                Console.WriteLine(lexeme.Description);
            } while (lexeme is not EndOfFileLexeme);
        }  
    }
}