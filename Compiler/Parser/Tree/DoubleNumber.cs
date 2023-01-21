using System.Globalization;
using Compiler.Constants;
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
        generator.DoubleCounter += 1;
        var valueString = _value.ToString(CultureInfo.CurrentCulture);
        var replace = valueString.Replace(",", ".");
        generator.DoubleUsingCommand.Add($"doubleValue{generator.DoubleCounter}: dq {replace}");
        generator.Add($"movsd xmm0, qword [doubleValue{generator.DoubleCounter}]");
        generator.Add(AssemblerCommand.Sub, AssemblerRegisters.Esp, 8);
        generator.Add("movsd qword [esp], xmm0");
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