namespace Compiler.Semantic;

public class SymbolRecord : SymbolType
{
    public SymbolTable Fields { get; }

    public SymbolRecord(string name, SymbolTable fields) : base(name)
    {
        Fields = fields;
    }
}