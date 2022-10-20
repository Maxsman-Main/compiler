namespace Compiler.Constants;

public static class OperatorConstants
{
    private static char _column = ':';
    private static char _equal = '=';
    private static char _moreSign = '>';
    private static char _lessSign = '<';
    private static List<char> _operators = new List<char>
    {
        '+', '-', '=', '*', '/', '=', '>', '<', ':'
    };

    public static char Column => _column;
    public static char Equal => _equal;
    public static char MoreSign => _moreSign;
    public static char LessSign => _lessSign;
    public static List<char> Operators => _operators;
}