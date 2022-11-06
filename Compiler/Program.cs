using Compiler.LexicalAnalyzerStateMachine;
using Compiler.Tests;

namespace Compiler
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args[0] == "a")
            {
                var analyzer = new LexicalAnalyzer();
            }
            
            else if (args[0] == "t")
            {
                var testSystem = new TestSystem();
                testSystem.TestLexicalAnalyze(args[1], args[2]);
            }
        }  
    }
}