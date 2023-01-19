using Compiler.Constants;

namespace Compiler.Parser.Tree;

public class IfStatement : INodeStatement
{
    private readonly INodeExpression _condition;
    private readonly INodeStatement? _body;
    private readonly INodeStatement? _elsePart;

    private int _ifCount;
    
    public IfStatement(INodeExpression condition, INodeStatement body, INodeStatement? elsePart)
    {
        _condition = condition;
        _body = body is NullStatement ? null : body;
        _elsePart = elsePart is NullStatement ? null : elsePart;
        _ifCount = 0;
    }

    public void Generate(Generator.Generator generator)
    {
        generator.IfCounter += 1;
        _ifCount = generator.IfCounter;
        _condition.Generate(generator);
        generator.Add(AssemblerCommand.Pop, AssemblerRegisters.Eax);
        generator.Add(AssemblerCommand.Cmp, AssemblerRegisters.Eax, 0);
        generator.Add(AssemblerCommand.Je, $"elseOfIf{_ifCount}");
        _body?.Generate(generator);
        generator.Add(AssemblerCommand.Jmp, $"endOfIf{_ifCount}");
        generator.Add($"elseOfIf{_ifCount}:");
        _elsePart?.Generate(generator);
        generator.Add($"endOfIf{_ifCount}:");
    }

    public string GetPrint(int level)
    {
        var result = "";
        for (int i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += "IfStatement";
        result += '\n';
        for (int i = 0; i < (level + 1) * 4; i++)
        {
            result += " ";
        }
        result += "Condition";
        result += '\n';
        result += _condition.GetPrint(level + 2);
        result += '\n';
        for (int i = 0; i < (level + 1) * 4; i++)
        {
            result += " ";
        }
        result += "Body";
        result += _body is not null ? '\n' + _body.GetPrint(level + 2) : "";
        if (_elsePart is not null)
        {
            result += '\n'; 
            for (int i = 0; i < (level + 1) * 4; i++)
            {
                result += " ";
            }

            result += "ElsePart";
            result += '\n';
            result += _elsePart.GetPrint(level + 2);
        }
        return result;
    }
}