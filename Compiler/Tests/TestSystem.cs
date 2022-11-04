using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine;

namespace Compiler.Tests;

public class TestSystem
{
    private FileWriter _writer = new FileWriter();

    public void TestLexicalAnalyze(string testFile, string exceptedResult)
    {
        if (_writer.IsOpened == false)
        {
            _writer.OpenFile();
        }

        var analyzer = new LexicalAnalyzer();
        analyzer.SetFile(testFile);

        ILexeme lexeme;
        do
        {
            lexeme = analyzer.GetLexeme();
            _writer.WriteLine(lexeme.Description);
        } while (lexeme is not EndOfFileLexeme);
        _writer.CloseFile();
        CompareFiles(exceptedResult);
    }

    private void CompareFiles(string firstFile)
    {
        StreamReader secondFileReader = new StreamReader("../../../Tests/result.txt");
        int testCounter = 0;
        int correctTestCounter = 0;
        using (StreamReader firstFileReader = new StreamReader("../../../Tests/Out/" + firstFile))
        {
            while (true)
            {
                string? firstFileString = firstFileReader.ReadLine();
                string? secondFileString = secondFileReader.ReadLine();
                testCounter += 1;
                correctTestCounter += 1;
                if (firstFileString == null && secondFileString == null)
                {
                    break;
                }

                if (firstFileString == null || secondFileString == null || !firstFileString.Equals(secondFileString))
                {
                    correctTestCounter -= 1;
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
    }
}