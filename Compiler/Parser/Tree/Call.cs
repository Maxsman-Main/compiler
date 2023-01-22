using Compiler.Constants;
using Compiler.Exceptions;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class Call : INodeExpression
{
    private readonly string _name;
    private readonly SymbolProcedure? _function;
    private readonly List<INodeExpression> _arguments;

    public Call(string name, List<INodeExpression> arguments)
    {
        _name = name;
        _arguments = arguments;
    }

    public Call(SymbolProcedure function, List<INodeExpression> arguments)
    {
        _function = function;
        _arguments = arguments;
        _name = function.Name;
    }

    public SymbolType GetExpressionType()
    {
        if (_function is null)
            throw new CompilerException("can't find function");
        var function = _function as SymbolFunction ?? throw new CompilerException(_function.Name + " procedure can't return type");
        return function.ReturnType;
    }

    public void Generate(Generator.Generator generator)
    {
        generator.Add(AssemblerCommand.Push, AssemblerRegisters.Ecx);
        foreach (var argument in _arguments)
        {
            argument.Generate(generator);
        }
        generator.Add(AssemblerCommand.Call, $"_{_name}");
        generator.Add(AssemblerCommand.Add, $"{GeneratorConstants.Registers[AssemblerRegisters.Esp]}, {_arguments.Count * 4}");
        generator.Add(AssemblerCommand.Pop, AssemblerRegisters.Ecx);
        if (GetExpressionType() is SymbolDouble)
        {
            generator.Add(AssemblerCommand.Sub, AssemblerRegisters.Esp, 8);
            generator.Add("movsd qword [esp], xmm2");
        }
        else
        {
            generator.Add(AssemblerCommand.Push, AssemblerRegisters.Edx);
        }
    }

    public string GetPrint(int level)
    {
        var value = "";
        for (int i = 0; i < level * 4; i++)
        {
            value += " ";
        }

        value += _name + "()";
        value += "\n";
        for (int i = 0; i < (level + 1) * 4; i++)
        {
            value += " ";
        }
        value += "arguments";
        if (_arguments.Count != 0)
        {
            value += "\n";
        }

        for(int i = 0; i < _arguments.Count; i++)
        {
            value += _arguments[i].GetPrint(level + 2);
            if (i != _arguments.Count - 1)
            {
                value += "\n";
            }
        }
        return value;
    }
}