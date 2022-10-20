using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine;

namespace Compiler.Tests;

public class TestSystem
{
    private FileWriter _writer = new FileWriter();

    public void TestLexicalAnalyze(string testFile)
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
    }
}