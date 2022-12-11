namespace Compiler.Semantic;

public class SymbolTypeAlias : SymbolType
{
    private SymbolType _original;

    public SymbolTypeAlias(string name, SymbolType original) : base(name)
    {
        _original = original;
    }
}