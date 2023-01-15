using Compiler.Constants;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class Number : INodeExpression
{
    public int Value { get; }

    public Number(int value)
    {
        Value = value;
    }

    public SymbolType GetExpressionType()
    {
        return new SymbolInteger("integer");
    }

    public void Generate(Generator.Generator generator)
    {
        generator.Add(AssemblerCommand.Push, Value);
    }
    
    public string GetPrint(int level)
    {
        var value = "";
        for (int i = 0; i < level * 4; i++)
        {
            value += " ";
        }

        value += Value.ToString();
        return value;
    }
}