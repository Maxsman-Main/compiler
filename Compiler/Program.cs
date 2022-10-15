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
            var lexeme = analyzer.GetLexeme();
            Console.WriteLine(lexeme.Description);
            lexeme = analyzer.GetLexeme();
            Console.WriteLine(lexeme.Description);
            lexeme = analyzer.GetLexeme();
            Console.WriteLine(lexeme.Description);
            lexeme = analyzer.GetLexeme();
            Console.WriteLine(lexeme.Description);
        }  
    }
}