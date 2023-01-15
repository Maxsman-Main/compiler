using System.Collections.ObjectModel;
using System.Diagnostics;
using Compiler.Exceptions;
using Compiler.Generator;
using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine;
using Compiler.Parser;
using Compiler.Tests;

namespace Compiler
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            args = new string[10];
            args[0] = "-c";
            args[1] = "test.txt";
            switch (args[0])
            {
                case "-a" when args[1] == "-t":
                {
                    var testSystemForLexer = new TestSystem();
                    testSystemForLexer.TestLexicalAnalyze();
                    break;
                }
                case "-a":
                {
                    var analyzer = new LexicalAnalyzer();
                    analyzer.SetFile("../../../Files/" + args[1]);
                    var lexeme = analyzer.GetLexeme();
                    Console.WriteLine(lexeme.Description);
                    while (lexeme is not EndOfFileLexeme)
                    {
                        lexeme = analyzer.GetLexeme();
                        Console.WriteLine(lexeme.Description);
                    }

                    break;
                }
                case "-e" when args[1] == "-t":
                {
                    var testSystemForParser = new TestSystem();
                    testSystemForParser.TestParserExpression();
                    break;
                }
                case "-e":
                {
                    var lexer = new LexicalAnalyzer();
                    lexer.SetFile("../../../Files/" + args[1]);
                    var parser = new Parser.Parser(lexer);
                    Console.WriteLine(parser.ParseExpression().GetPrint(0));
                    break;
                }
                case "-p" when args[1] == "-t":
                {
                    var testSystemForParser = new TestSystem();
                    testSystemForParser.TestParser();
                    break;
                }
                case "-p":
                {
                    var lexer = new LexicalAnalyzer();
                    lexer.SetFile("../../../Files/" + args[1]);
                    var parser = new Parser.Parser(lexer);
                    try
                    {
                        Console.WriteLine(parser.ParseProgram().SyntaxTree.GetPrint(0));
                    }
                    catch (CompilerException exception)
                    {
                        Console.WriteLine(exception.Message);
                    }
                    break;
                }
                case "-s" when args[1] == "-t":
                {
                    var testSystemForParser = new TestSystem();
                    testSystemForParser.TestSemantic();
                    break;
                }
                case "-s":
                {
                    var lexer = new LexicalAnalyzer();
                    lexer.SetFile("../../../Files/" + args[1]);
                    var parser = new Parser.Parser(lexer);
                    try
                    {
                        var program = parser.ParseProgram();
                        Console.Write(program.Stack.GetPrint());
                        Console.WriteLine(program.MainBlock.GetPrint(0));
                    }
                    catch (CompilerException exception)
                    {
                        Console.WriteLine(exception.Message);
                    }
                    break;
                }
                case "-c":
                {
                    var lexer = new LexicalAnalyzer();
                    lexer.SetFile("../../../Files/" + args[1]);
                    var parser = new Parser.Parser(lexer);
                    try
                    {
                        var program = parser.ParseProgram();
                        var generator = new Generator.Generator();
                        var directoryMaker = new DirectoryMaker();
                        var assemblerMaker = new AssemblerFileMaker();
                        var assemblerCodeExecutor = new AssemblerCodeExecutor();
                        directoryMaker.MakeDirectory(args[1]);
                        assemblerMaker.MakeFile(args[1], generator.Commands);
                        assemblerCodeExecutor.RunAssemblerCode(args[1]);
                    }
                    catch (CompilerException exception)
                    {
                        Console.WriteLine(exception.Message);
                    }
                    break;
                }    
                default:
                {
                    Console.WriteLine("No right keys");
                    break;
                }
            }
        }  
    }
}