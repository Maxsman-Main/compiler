using Compiler.Exceptions;
using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine;

namespace Compiler.Tests;

enum TestCompareKey
{
    Analyzer,
    Parser
}

public class TestSystem
{
    private string[] _files = Directory.GetFiles("C:/Users/NITRO/RiderProjects/Compiler/Compiler/Tests/In");
    private FileWriter _writer = new();
    private List<string> _tests = new();
    
    public void TestLexicalAnalyze(string testFile, string exceptedResult)
    {
        if (_writer.IsOpened == false)
        {
            _writer.OpenFile();
        }
        try
        {
            var analyzer = new LexicalAnalyzer();
            analyzer.SetFile(testFile);

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
            CompareFilesAnalyzer(exceptedResult);
        }
        catch (CompilerException exception)
        {
            _writer.WriteLine(exception.Message);
            _writer.CloseFile();
            CompareFilesAnalyzer(exceptedResult);
        }
    }

    public void TestParser(string testFile, string exceptedResult)
    {
        if (_writer.IsOpened == false)
        {
            _writer.OpenFile();
        }

        try
        {
            var analyzer = new LexicalAnalyzer();
            analyzer.SetFile(testFile);
            var parser = new Parser.Parser(analyzer);
            while (analyzer.CurrentLexeme is not EndOfFileLexeme)
            {
                _writer.WriteLine(parser.ParseProgram().GetPrint(0));
            }
            _writer.CloseFile();
            CompareFilesParser(exceptedResult);
        }
        catch (CompilerException exception)
        {
            _writer.WriteLine(exception.Message);
            _writer.CloseFile();
            CompareFilesParser(exceptedResult);
        }
    }

    private void CompareFilesAnalyzer(string firstFile)
    {
        var secondFileReader = new StreamReader("../../../Tests/result.txt");
        var testCounter = 0;
        var correctTestCounter = 0;
        using (var firstFileReader = new StreamReader("../../../Tests/Out/" + firstFile))
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
                    
                    _tests.Add("Test number " + testCounter + " is FALSE");
                    correctTestCounter -= 1;
                }
                else
                {
                    _tests.Add("Test number " + testCounter + " is OK");
                }
            }
        }

        if (correctTestCounter == testCounter)
        { 
            Console.WriteLine("Tests successful!" + " tests count: " + correctTestCounter.ToString() + "/" + testCounter.ToString());
        }
        else
        {
            Console.WriteLine("Tests failed!" + " tests count: " + correctTestCounter.ToString() + "/" + testCounter.ToString());
        }

        foreach (var testResult in _tests)
        {
            Console.WriteLine(testResult);
        }
    }

    private void CompareFilesParser(string firstFile)
    {
        var secondFileReader = new StreamReader("../../../Tests/result.txt");
        var testCounter = 0;
        var correctTestCounter = 0;
        using (var firstFileReader = new StreamReader("../../../Tests/Out/" + firstFile))
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

        Console.WriteLine(correctTestCounter == testCounter ? "Tests successful!" : "Tests failed!");
    }
}