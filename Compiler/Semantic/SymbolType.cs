namespace Compiler.Semantic;

public class SymbolType : Symbol
{
    public int Size { get; set; }

    protected SymbolType(string name) : base(name)
    {
    }

    public override void Generate(Generator.Generator generator)
    {
        throw new NotImplementedException();
    }

    public override string GetPrint(int level)
    {
        var result = "";
        for (var i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += Name;

        return result;
    }
}