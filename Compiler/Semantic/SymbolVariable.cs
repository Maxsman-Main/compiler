namespace Compiler.Semantic;

public class SymbolVariable : Symbol
{
    public SymbolType Type { get; }

    public SymbolVariable(string name, SymbolType type) : base(name)
    {
        Type = type;
    }

    public override string GetPrint(int level)
    {
        var result = "";
        for (var i = 0; i < level; i++)
        {
            result += " ";
        }

        result += Name + " " + Type.GetPrint(0);
        return result;
    }
}