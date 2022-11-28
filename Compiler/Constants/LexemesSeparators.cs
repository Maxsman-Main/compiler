namespace Compiler.Constants;

public enum SeparatorValue
{
    LeftBracket,
    RightBracket,
    Point,
    Semicolon,
    Comma,
    SquareLeftBracket,
    SquareRightBracket
}

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
        '.', ';', ',', '(', ')', '[', ']'
    };

    private static Dictionary<string, SeparatorValue> _separatorValues = new()
    {
        {".", SeparatorValue.Point},
        {";", SeparatorValue.Semicolon},
        {",", SeparatorValue.Comma},
        {"(", SeparatorValue.LeftBracket},
        {")", SeparatorValue.RightBracket},
        {"[", SeparatorValue.SquareLeftBracket},
        {"]", SeparatorValue.SquareRightBracket}
    };

    private static Dictionary<SeparatorValue, string> _separatorSymbols = new()
    {
        {SeparatorValue.Point, "."},
        {SeparatorValue.Semicolon, ";"},
        {SeparatorValue.Comma, ","},
        {SeparatorValue.LeftBracket, "("},
        {SeparatorValue.RightBracket, ")"},
        {SeparatorValue.SquareLeftBracket, "["},
        {SeparatorValue.SquareRightBracket, "]"}
    };

    public static char EndOfLine => _newLine;
    public static List<char> NewLineSeparators => _newLineSeparator;
    public static int EndOfFile => _endOfFile;
    public static List<char> VisibleSeparators => _visibleSeparators;
    public static List<char> InvisibleSeparators => _separators;
    public static Dictionary<string, SeparatorValue> SeparatorValues => _separatorValues;
    public static Dictionary<SeparatorValue, string> SeparatorSymbols => _separatorSymbols;

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