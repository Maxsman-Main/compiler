namespace Compiler.Semantic;

public class SymbolRecord : SymbolType
{
    private SymbolTable _fields;

    public SymbolRecord(string name, SymbolTable fields) : base(name)
    {
        _fields = fields;
    }
}