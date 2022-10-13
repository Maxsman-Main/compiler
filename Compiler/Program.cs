using Compiler.LexicalAnalyzerStateMachine;

namespace Compiler
{
    public static class Program
    {
        public static void Main()
        {
            var analyzer = new LexicalAnalyzer();
            var lexeme = analyzer.GetNextLexeme("11");
            Console.WriteLine(lexeme.Description);
            lexeme = analyzer.GetNextLexeme("$11");
            Console.WriteLine(lexeme.Description);
            lexeme = analyzer.GetNextLexeme("&11");
            Console.WriteLine(lexeme.Description);
            lexeme = analyzer.GetNextLexeme("%11");
            Console.WriteLine(lexeme.Description);
            lexeme = analyzer.GetNextLexeme("-$0011");
            Console.WriteLine(lexeme.Description);
            lexeme = analyzer.GetNextLexeme("-&0011");
            Console.WriteLine(lexeme.Description);
            lexeme = analyzer.GetNextLexeme("-%0011");
            Console.WriteLine(lexeme.Description);
        }  
    }
}