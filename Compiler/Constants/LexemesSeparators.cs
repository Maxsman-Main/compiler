namespace Compiler.Constants;

public static class LexemesSeparators
{
    private static char _carriageReturn = (char)13;
    private static char _newLine = (char)10;
    private static int _endOfFile = -1;
    
    private static readonly List<char> _separators = new()
    {
        ' ', _carriageReturn, _newLine
    };

    private static readonly List<char> _newLineSeparator = new()
    {
        _carriageReturn, _newLine
    };

    private static readonly List<char> _visibleSeparators = new()
    {
        '.', ';', ',', '(', ')'
    };

    public static char EndOfLine => _newLine;
    public static List<char> NewLineSeparators => _newLineSeparator;
    public static int EndOfFile => _endOfFile;
    public static List<char> VisibleSeparators => _visibleSeparators;
    
    public static bool ContainSymbol(char symbol)
    {
        foreach(var separator in _separators)
        {
            if (separator == symbol)
            {
                return true;
            }
        }

        return false;
    }
    
    public static bool ContainSymbol(int symbol)
    {
        foreach(var separator in _separators)
        {
            if (separator == (char)symbol)
            {
                return true;
            }
        }

        return false;
    }
}