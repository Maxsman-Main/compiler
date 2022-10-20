using Compiler.LexicalAnalyzerStateMachine;
using Compiler.Tests;

namespace Compiler
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                if (args[1] == "t")
                {
                    var testSystem = new TestSystem();
                    testSystem.TestLexicalAnalyze(args[0]);
                }

                if (args[1] == "a")
                {
                    var analyzer = new LexicalAnalyzer();
                    analyzer.AnalyzeFile(args[0]);
                }
            }
        }  
    }
}