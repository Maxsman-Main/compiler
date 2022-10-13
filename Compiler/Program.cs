using Compiler.LexicalAnalyzerStateMachine;

namespace Compiler
{
    public static class Program
    {
        public static void Main()
        {
            var analyzer = new LexicalAnalyzer();
            var lexeme = analyzer.GetNextLexeme("$11F");
            Console.WriteLine(lexeme.Description);
        }  
    }
}