namespace Compiler.Parser.Semantic;

public class SymbolRecord
{
    private SymbolTable _fields;

    public SymbolRecord(SymbolTable fields)
    {
        _fields = fields;
    }
}