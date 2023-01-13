using System.Globalization;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class DoubleNumber : INodeExpression
{
    private readonly double _value;
    
    public DoubleNumber(double value)
    {
        _value = value;
    }

    public SymbolType GetExpressionType()
    {
        return new SymbolDouble("Double");
    }

    public void Generate(Generator.Generator generator)
    {
        throw new NotImplementedException();
    }

    public string GetPrint(int level)
    {
        var value = "";
        for (int i = 0; i < level * 4; i++)
        {
            value += " ";
        }

        value += _value.ToString(CultureInfo.InvariantCulture);
        return value;
    }
}