namespace Compiler.Semantic;

public class SymbolType : Symbol
{
    protected SymbolType(string name) : base(name)
    {
    }

    public override string GetPrint(int level)
    {
        var result = "";
        for (var i = 0; i < level; i++)
        {
            result += " ";
        }

        result += Name;

        return result;
    }
}