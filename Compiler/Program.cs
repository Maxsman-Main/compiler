using Compiler.Tests;

namespace Compiler
{
    public static class Program
    {
        public static void Main()
        {
            var file = "test.txt";
            var testSystem = new TestSystem();
            testSystem.TestLexicalAnalyze(file);
        }  
    }
}