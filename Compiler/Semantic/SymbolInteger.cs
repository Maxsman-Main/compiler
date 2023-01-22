namespace Compiler.Semantic;

public class SymbolInteger : SymbolType
{
    public SymbolInteger(string name) : base(name)
    {
        Size = 4;
    }
}