namespace Compiler.Semantic;

public class SymbolVariable : Symbol
{
    private SymbolType _type;

    public SymbolVariable(string name, SymbolType type) : base(name)
    {
        _type = type;
    }
}