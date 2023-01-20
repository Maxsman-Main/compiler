using Compiler.Constants;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class String : INodeExpression
{
    private readonly string _value;

    public String(string value)
    {
        _value = value;
    }
    
    public SymbolType GetExpressionType()
    {
        return new SymbolString("string");
    }

    public void Generate(Generator.Generator generator)
    {
        generator.Add($"_str: db \"{_value}\", 10, 0");
        generator.Add(AssemblerCommand.Push, "_str");
    }

    public string GetPrint(int level)
    {
        var value = "";
        for (int i = 0; i < level * 4; i++)
        {
            value += " ";
        }

        value += "\"";
        value += _value;
        value += "\"";
        return value;
    }
}