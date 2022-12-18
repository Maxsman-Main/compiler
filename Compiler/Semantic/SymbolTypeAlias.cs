namespace Compiler.Semantic;

public class SymbolTypeAlias : SymbolType, IAlias
{
    public SymbolType Original { get; }

    public SymbolTypeAlias(string name, SymbolType original) : base(name)
    {
        Original = original;
    }

    public override string GetPrint(int level)
    {
        var result = "";
        for (var i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += Name + " alias " + Original.GetPrint(0);
        return result;
    }
}