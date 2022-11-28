using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine;
using Compiler.Parser;
using Compiler.Parser.Tree;
using Compiler.Tests;

namespace Compiler
{
    public static class Program
    {
        public static void Main(string[] args)
        {
           switch (args[0])
            {
                case "-a" when args[1] == "-t":
                {
                    var testSystemForLexer = new TestSystem();
                    testSystemForLexer.TestLexicalAnalyze(args[2], args[3]);
                    break;
                }
                case "-a":
                {
                    var analyzer = new LexicalAnalyzer();
                    analyzer.SetFile(args[1]);
                    var lexeme = analyzer.GetLexeme();
                    Console.WriteLine(lexeme.Description);
                    while (lexeme is not EndOfFileLexeme)
                    {
                        lexeme = analyzer.GetLexeme();
                        Console.WriteLine(lexeme.Description);
                    }

                    break;
                }
                case "-p" when args[1] == "-t":
                    var testSystemForParser = new TestSystem();
                    testSystemForParser.TestParser(args[2], args[3]);
                    break;
                case "-p":
                    var lexer = new LexicalAnalyzer();
                    lexer.SetFile(args[1]);
                    var parser = new Parser.Parser(lexer);
                    Console.WriteLine(parser.ParseProgram().GetPrint(0));
                    break;
                default:
                    Console.WriteLine("No keys");
                    break;
            }
        }  
    }
}