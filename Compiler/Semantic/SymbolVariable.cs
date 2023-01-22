using System.Drawing;
using Compiler.Constants;
using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolVariable : Symbol, IVariable
{
    public SymbolType Type { get; }
    public INodeExpression? Value { get; }
    
    public SymbolVariable(string name, SymbolType type, INodeExpression? expression) : base(name)
    {
        Type = type;
        Value = expression;
    }

    public override void Generate(Generator.Generator generator)
    {
        generator.Add(this, Type is not SymbolDouble ? AssemblerCommand.Resd : AssemblerCommand.Resq);
    }


    public SymbolVariableLocal ConvertToLocal()
    {
        return new SymbolVariableLocal(Name, Type, Value);
    }

    public SymbolVariableParameter ConvertToParameter()
    {
        return new SymbolVariableParameter(Name, Type, Value);
    }

    public override string GetPrint(int level)
    {
        var result = "";
        for (var i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += Name + " " + Type.GetPrint(0);
        if (Value is null) return result;
        result += "\n";
        result += Value.GetPrint(level + 1);
        return result;
    }
}