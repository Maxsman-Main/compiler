namespace Compiler.Constants;

public static class LexemesSeparators
{
    private static readonly List<char> _separators = new()
    {
        ' ', (char)13 // it is \n
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