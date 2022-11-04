using Compiler.LexicalAnalyzerStateMachine;
using Compiler.Tests;

namespace Compiler
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args[2] == "t")
            {
                var testSystem = new TestSystem();
                testSystem.TestLexicalAnalyze(args[0], args[1]);
            }

            if (args[1] == "a")
            {
                var analyzer = new LexicalAnalyzer();
                analyzer.AnalyzeFile(args[0]);
            }
        }  
    }
}