using Compiler.Exceptions;
using Compiler.Generator;
using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine;
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
                case "-c" when args[1] == "-t":
                {
                    var testSystemForParser = new TestSystem();
                    testSystemForParser.TestCompile();
                    break;
                }
                case "-c" when args[1] == "-w":
                {
                    var lexer = new LexicalAnalyzer();
                    lexer.SetFile("../../../Files/" + args[2]);
                    var parser = new Parser.Parser(lexer);
                    try
                    {
                        var program = parser.ParseProgram();
                        var generator = new Generator.Generator();
                        var directoryMaker = new DirectoryMaker();
                        var assemblerMaker = new AssemblerFileMaker();
                        var assemblerCodeExecutor = new AssemblerCodeExecutor();
                        generator.AddSectionBss();
                        program.Stack.GenerateForVariables(generator);
                        generator.AddSectionText();
                        program.Stack.GenerateForProcedures(generator);
                        generator.AddMain();
                        program.MainBlock.Generate(generator);
                        generator.AddSectionData();
                        directoryMaker.MakeDirectory(args[2]);
                        assemblerMaker.MakeFile(args[2], generator.Commands);
                        assemblerCodeExecutor.GenerateFiles(args[2]);
                        assemblerCodeExecutor.RunAssemblerCode(args[2]);
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
                        generator.AddSectionBss();
                        program.Stack.GenerateForVariables(generator);
                        generator.AddSectionText();
                        program.Stack.GenerateForProcedures(generator);
                        generator.AddMain();
                        program.MainBlock.Generate(generator);
                        generator.AddSectionData();
                        directoryMaker.MakeDirectory(args[1]);
                        assemblerMaker.MakeFile(args[1], generator.Commands);
                        assemblerCodeExecutor.GenerateFiles(args[1]);
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