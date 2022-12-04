namespace Compiler.Constants;

public enum OperatorValue
{
    Plus,
    Minus,
    Div,
    Multiplication,
    Equal,
    Assignment,
    DoublePoint,
    More,
    Less,
    MoreEqual,
    LessEqual,
    Point,
    Range,
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
        {"/", OperatorValue.Div},
        {"=", OperatorValue.Equal},
        {":=", OperatorValue.Assignment},
        {":", OperatorValue.DoublePoint},
        {">", OperatorValue.More},
        {"<", OperatorValue.Less},
        {"<=", OperatorValue.LessEqual},
        {">=", OperatorValue.MoreEqual},
        {".", OperatorValue.Point},
        {"..", OperatorValue.Range}
    };
    
    private static Dictionary<OperatorValue, string> _operatorSymbols = new Dictionary<OperatorValue, string>
    {
        {OperatorValue.Plus, "+"},
        {OperatorValue.Minus, "-"},
        {OperatorValue.Multiplication, "*"},
        {OperatorValue.Div, "/"},
        {OperatorValue.Equal, "="},
        {OperatorValue.Assignment, ":="},
        {OperatorValue.DoublePoint, ":"},
        {OperatorValue.More, ":"},
        {OperatorValue.Less, "<"},
        {OperatorValue.MoreEqual, ">="},
        {OperatorValue.LessEqual, "<="},
        {OperatorValue.Point, "."},
        {OperatorValue.Range, ".."}
    };

    public static char Column => _column;
    public static char Equal => _equal;
    public static char MoreSign => _moreSign;
    public static char LessSign => _lessSign;
    public static List<char> Operators => _operators;
    public static Dictionary<string, OperatorValue> OperatorValues => _operatorValues;
    public static Dictionary<OperatorValue, string> OperatorSymbols => _operatorSymbols;
}