namespace Compiler.Constants;

public static class KeyWordsConstants
{
    private static List<string> _keyWords = new()
    {
        "and", "end", "nil", "set",
        "array", "file", "not", "then",
        "begin", "for", "of", "to",
        "case", "function", "or", "type",
        "const", "goto", "packed", "until",
        "div", "if", "procedure", "var",
        "do", "in", "program", "while",
        "downto", "label", "record", "with",
        "else", "mod", "repeat",
        "AND", "END", "NIL", "SET",
        "ARRAY", "FILE", "NOT", "THEN",
        "BEGIN", "FOR", "OF", "TO",
        "CASE", "FUNCTION", "OR", "TYPE",
        "CONST", "GOTO", "PACKED", "UNTIL",
        "DIV", "IF", "PROCEDURE", "VAR",
        "DO", "IN", "PROGRAM", "WHILE",
        "DOWNTO", "LABEL", "RECORD", "WITH",
        "ELSE", "MOD", "REPEAT"
    };

    public static List<string> KeyWords => _keyWords;
}