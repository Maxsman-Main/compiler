using Compiler.Exceptions;
using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine;

namespace Compiler.Tests;

public class TestSystem
{
    private const string LexerTestsPath = "../../../Tests/In/LexerTests";
    private const string ParserExpressionTestsPath = "../../../Tests/In/ParserExpressionTests";
    private const string ParserTestsPath = "../../../Tests/In/ParserTests";

    private const string LexerOutPath = "../../../Tests/Out/LexerTests";
    private const string ParserExpressionOutPath = "../../../Tests/Out/ParserExpressionTests";
    private const string ParserOutPath = "../../../Tests/Out/ParserTests";

    private readonly FileWriter _writer = new();
    
    public void TestLexicalAnalyze()
    {
        var files = GetFiles(LexerTestsPath);
        var outFiles = GetFiles(LexerOutPath);
        List<bool> results = new();
        for (int i = 0; i < files.Count; i++)
        {
            if (_writer.IsOpened == false)
            {
                _writer.OpenFile();
            }

            try
            {
                var analyzer = new LexicalAnalyzer();
                analyzer.SetFile(files[i]);

                do
                {
                    var lexeme = analyzer.GetLexeme();
                    if (lexeme is EndOfFileLexeme)
                    {
                        break;
                    }

                    _writer.WriteLine(lexeme.Description);
                } while (true);

                _writer.CloseFile();
                results.Add(CompareFiles(outFiles[i]));
            }
            catch (CompilerException exception)
            {
                _writer.WriteLine(exception.Message);
                _writer.CloseFile();
                results.Add(CompareFiles(outFiles[i]));
            }
        }
        
        PrintResults(results, files);
    }

    public void TestParserExpression()
    {
        var files = GetFiles(ParserExpressionTestsPath);
        var outFiles = GetFiles(ParserExpressionOutPath);
        List<bool> results = new();
        for (int i = 0; i < files.Count; i++)
        {
            if (_writer.IsOpened == false)
            {
                _writer.OpenFile();
            }

            try
            {
                var analyzer = new LexicalAnalyzer();
                analyzer.SetFile(files[i]);
                var parser = new Parser.Parser(analyzer);
                _writer.WriteLine(parser.ParseExpression().GetPrint(0));
                _writer.CloseFile();
                results.Add(CompareFiles(outFiles[i]));
            }
            catch (CompilerException exception)
            {
                _writer.WriteLine(exception.Message);
                _writer.CloseFile();
                results.Add(CompareFiles(outFiles[i]));
            }
        }

        PrintResults(results, files);
    }

    public void TestParser()
    {
        var files = GetFiles(ParserTestsPath);
        var outFiles = GetFiles(ParserOutPath);
        List<bool> results = new();
        for (int i = 0; i < files.Count; i++)
        {
            if (_writer.IsOpened == false)
            {
                _writer.OpenFile();
            }

            try
            {
                var analyzer = new LexicalAnalyzer();
                analyzer.SetFile(files[i]);
                var parser = new Parser.Parser(analyzer);
                _writer.WriteLine(parser.ParseProgram().GetPrint(0));

                _writer.CloseFile();
                results.Add(CompareFiles(outFiles[i]));
            }
            catch (CompilerException exception)
            {
                _writer.WriteLine(exception.Message);
                _writer.CloseFile();
                results.Add(CompareFiles(outFiles[i]));
            }
        }
        
        PrintResults(results, files);
    }

    private bool CompareFiles(string firstFile)
    {
        _writer.CloseFile();
        var secondFileReader = new StreamReader("../../../Tests/result.txt");
        var testCounter = 0;
        var correctTestCounter = 0;
        using (var firstFileReader = new StreamReader(firstFile))
        {
            while (true)
            {
                var firstFileString = firstFileReader.ReadLine();
                var secondFileString = secondFileReader.ReadLine();
                if (firstFileString == null && secondFileString == null)
                {
                    break;
                }
                testCounter += 1;
                correctTestCounter += 1;

                if (firstFileString == null || secondFileString == null || !firstFileString.Equals(secondFileString))
                {
                    correctTestCounter -= 1;
                }
            }
        }

        secondFileReader.Close();
        return correctTestCounter == testCounter;
    }

    private List<string> GetFiles(string path)
    {
        List<string> names = new();
        var files = Directory.GetFiles(path);
        foreach (var file in files)
        {
            var name = file;
            names.Add(name);
        }

        names.Sort();
        return names;
    }

    private void PrintResults(List<bool> results, List<string> files)
    {
        var counter = 0;
        foreach (var result in results)
        {
            if (result)
            {
                counter += 1;
            }
        }
        
        Console.WriteLine("Right tests: " + counter + "/" + results.Count);
        for (int i = 0; i < results.Count; i++)
        {
            Console.WriteLine(results[i] ? "Test " + files[i].Split("\\")[1] + " OK" : "Test " + files[i].Split("\\")[1] + " FALSE");
        }
    }
}