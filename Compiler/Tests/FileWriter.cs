namespace Compiler.Tests;

public class FileWriter
{
    private StreamWriter? _writer;
    private bool _isOpened = false;

    public bool IsOpened => _isOpened;

    public void WriteLine(string text)
    {
        _writer?.WriteLine(text);
    }
    
    public void CloseFile()
    {
        _isOpened = false;
        _writer?.Close();
    }
    
    public void OpenFile()
    {
        _isOpened = true;
        _writer = new StreamWriter("../../../Tests/result.txt", true);
    }
}