namespace Compiler.Constants;

public enum KeyWordValue
{
    And,
    End,
    Nil,
    Set,
    Array,
    File,
    Not,
    Then,
    Begin,
    For,
    Of,
    To,
    Case,
    Function,
    Or,
    Type,
    Const,
    GoTo,
    Packed,
    Until,
    Div,
    If,
    Procedure,
    Var,
    Do,
    In,
    Program,
    While,
    DownTo,
    Label,
    Record,
    With,
    Else,
    Mod,
    Repeat,
    Integer,
    Double,
    String,
    Char
}

public static class KeyWordsConstants
{
    private static List<string> _keyWords = new()
    {
        "and", "end", "nil", "set", "integer", "double", "string", "char",
        "array", "file", "not", "then",
        "begin", "for", "of", "to",
        "case", "function", "or", "type",
        "const", "goto", "packed", "until",
        "div", "if", "procedure", "var",
        "do", "in", "program", "while",
        "downto", "label", "record", "with",
        "else", "mod", "repeat",
        "AND", "END", "NIL", "SET", "INTEGER", "DOUBLE", "STRING", "CHAR",
        "ARRAY", "FILE", "NOT", "THEN",
        "BEGIN", "FOR", "OF", "TO",
        "CASE", "FUNCTION", "OR", "TYPE",
        "CONST", "GOTO", "PACKED", "UNTIL",
        "DIV", "IF", "PROCEDURE", "VAR",
        "DO", "IN", "PROGRAM", "WHILE",
        "DOWNTO", "LABEL", "RECORD", "WITH",
        "ELSE", "MOD", "REPEAT"
    };

    private static Dictionary<string, KeyWordValue> _keyWordValues = new()
    {
        {"and", KeyWordValue.And},
        {"end", KeyWordValue.End},
        {"nil", KeyWordValue.Nil},
        {"set", KeyWordValue.Set},
        {"integer", KeyWordValue.Integer},
        {"double", KeyWordValue.Double},
        {"string", KeyWordValue.String},
        {"char", KeyWordValue.Char},
        {"array", KeyWordValue.Array},
        {"file", KeyWordValue.File},
        {"not", KeyWordValue.Not},
        {"then", KeyWordValue.Then},
        {"begin", KeyWordValue.Begin},
        {"for", KeyWordValue.For},
        {"of", KeyWordValue.Of},
        {"to", KeyWordValue.To},
        {"case", KeyWordValue.Case},
        {"function", KeyWordValue.Function},
        {"or", KeyWordValue.Or},
        {"type", KeyWordValue.Type},
        {"const", KeyWordValue.Const},
        {"goto", KeyWordValue.GoTo},
        {"packed", KeyWordValue.Packed},
        {"until", KeyWordValue.Until},
        {"div", KeyWordValue.Div},
        {"if", KeyWordValue.If},
        {"procedure", KeyWordValue.Procedure},
        {"var", KeyWordValue.Var},
        {"do", KeyWordValue.Do},
        {"in", KeyWordValue.In},
        {"program", KeyWordValue.Program},
        {"while", KeyWordValue.While},
        {"downto", KeyWordValue.DownTo},
        {"label", KeyWordValue.Label},
        {"record", KeyWordValue.Record},
        {"with", KeyWordValue.With},
        {"else", KeyWordValue.Else},
        {"mod", KeyWordValue.Mod},
        {"repeat", KeyWordValue.Repeat},
        {"AND", KeyWordValue.And},
        {"END", KeyWordValue.End},
        {"NIL", KeyWordValue.Nil},
        {"SET", KeyWordValue.Set},
        {"INTEGER", KeyWordValue.Integer},
        {"DOUBLE", KeyWordValue.Double},
        {"STRING", KeyWordValue.String},
        {"CHAR", KeyWordValue.Char},
        {"ARRAY", KeyWordValue.Array},
        {"FILE", KeyWordValue.File},
        {"NOT", KeyWordValue.Not},
        {"THEN", KeyWordValue.Then},
        {"BEGIN", KeyWordValue.Begin},
        {"FOR", KeyWordValue.For},
        {"OF", KeyWordValue.Of},
        {"TO", KeyWordValue.To},
        {"CASE", KeyWordValue.Case},
        {"FUNCTION", KeyWordValue.Function},
        {"OR", KeyWordValue.Or},
        {"TYPE", KeyWordValue.Type},
        {"CONST", KeyWordValue.Const},
        {"GOTO", KeyWordValue.GoTo},
        {"PACKED", KeyWordValue.Packed},
        {"UNTIL", KeyWordValue.Until},
        {"DIV", KeyWordValue.Div},
        {"IF", KeyWordValue.If},
        {"PROCEDURE", KeyWordValue.Procedure},
        {"VAR", KeyWordValue.Var},
        {"DO", KeyWordValue.Do},
        {"IN", KeyWordValue.In},
        {"PROGRAM", KeyWordValue.Program},
        {"WHILE", KeyWordValue.While},
        {"DOWNTO", KeyWordValue.DownTo},
        {"LABEL", KeyWordValue.Label},
        {"RECORD", KeyWordValue.Record},
        {"WITH", KeyWordValue.With},
        {"ELSE", KeyWordValue.Else},
        {"MOD", KeyWordValue.Mod},
        {"REPEAT", KeyWordValue.Repeat}
    };

    public static Dictionary<string, KeyWordValue> KeyWordValues => _keyWordValues;
    public static Dictionary<KeyWordValue, string> KeyWordStrings => SwapDictionary(_keyWordValues);
    public static List<string> KeyWords => _keyWords;

    private static Dictionary<KeyWordValue, string> SwapDictionary(Dictionary<string, KeyWordValue> dictionary)
    {
        Dictionary<KeyWordValue, string> newDictionary = new();
        
        foreach(var item in dictionary)
        {
            KeyValuePair<KeyWordValue, string> newItem = new(item.Value, item.Key);
            try
            {
                newDictionary.Add(newItem.Key, newItem.Value);
            }
            catch(ArgumentException)
            {
                
            }
        }

        return newDictionary;
    }
}