namespace Compiler.Constants;

public static class LexemesSeparators
{
    private static char _carrigeReturn = (char)13;
    private static char _newLine = (char)10;

    private static readonly List<char> _separators = new()
    {
        ' ', _carrigeReturn, _newLine
    };

    public static char SpaceSymbol => ' ';
    public static char LineBreakSymbol => (char) 13; // it is \n

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
}