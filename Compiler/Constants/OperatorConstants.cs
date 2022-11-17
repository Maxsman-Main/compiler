namespace Compiler.Constants;

public enum OperatorValue
{
    Plus,
    Minus,
    Div,
    Multiplication,
    Null
}

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

    private static Dictionary<string, OperatorValue> _operatorValues = new Dictionary<string, OperatorValue>
    {
        {"+", OperatorValue.Plus},
        {"-", OperatorValue.Minus},
        {"*", OperatorValue.Multiplication},
        {"/", OperatorValue.Div}
    };
    
    private static Dictionary<OperatorValue, string> _operatorSymbols = new Dictionary<OperatorValue, string>
    {
        {OperatorValue.Plus, "+"},
        {OperatorValue.Minus, "-"},
        {OperatorValue.Multiplication, "*"},
        {OperatorValue.Div, "/"}
    };

    public static char Column => _column;
    public static char Equal => _equal;
    public static char MoreSign => _moreSign;
    public static char LessSign => _lessSign;
    public static List<char> Operators => _operators;
    public static Dictionary<string, OperatorValue> OperatorValues => _operatorValues;
    public static Dictionary<OperatorValue, string> OperatorSymbols => _operatorSymbols;
}