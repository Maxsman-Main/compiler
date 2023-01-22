namespace Compiler.Semantic;

public class SymbolChar : SymbolType
{
    public SymbolChar(string name) : base(name)
    {
        Size = 4;
    }
}