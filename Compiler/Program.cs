using Compiler.LexicalAnalyzerStateMachine;

namespace Compiler
{
    public static class Program
    {
        public static void Main()
        {
            var analyzer = new LexicalAnalyzer();
            var lexeme = analyzer.GetNextLexeme("-1234");
            Console.WriteLine(lexeme.Description);
        }  
    }
}