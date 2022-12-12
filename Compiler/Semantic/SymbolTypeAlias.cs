namespace Compiler.Semantic;

public class SymbolTypeAlias : SymbolType, IAlias
{
    public SymbolType Original { get; }

    public SymbolTypeAlias(string name, SymbolType original) : base(name)
    {
        Original = original;
    }
}