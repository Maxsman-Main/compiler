namespace Compiler.Parser.Semantic;

public class SymbolTypeAlias
{
    private SymbolType _original;

    public SymbolTypeAlias(SymbolType original)
    {
        _original = original;
    }
}