using Compiler.LexicalAnalyzerStateMachine;

namespace Compiler
{
    public static class Program
    {
        public static void Main()
        {
            var analyzer = new LexicalAnalyzer();
            var lexeme = analyzer.GetNextLexeme();
            Console.WriteLine(lexeme.Description);
        }  
    }
}