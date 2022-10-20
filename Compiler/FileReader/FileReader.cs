namespace Compiler.FileReader;

public class FileReader
{
    private StreamReader? _reader;
    private string _path = "";
    private bool _isOpened = false;

    public bool IsOpened => _isOpened;

    public int MoveToNextPosition()
    {
        if (_reader is null)
        {
            throw new FileNotFoundException();
        }
        
        return _reader.Read();
    }

    public int ReadSymbol()
    {
        if (_reader is null)
        {
            throw new FileNotFoundException();
        }
        
        return _reader.Peek();
    }

    public void SetFile(string fileName)
    {
        _path = fileName;
    }
    
    public void CloseFile()
    {
        _isOpened = false;
        _reader?.Close();
    }
    
    public void OpenFile()
    {
        _isOpened = true;
        _reader = File.OpenText("../../../Tests/" + _path);
    }
}