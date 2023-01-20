using Compiler.Constants;
using Compiler.Lexeme;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class ForStatement : INodeStatement
{
    private readonly SymbolVariable _identifier;
    private readonly INodeExpression _startExpression;
    private readonly INodeExpression _endExpression;
    private readonly INodeStatement? _statement;
    private readonly KeyWordValue _direction;

    public ForStatement(SymbolVariable identifier, INodeExpression startExpression, INodeExpression endExpression,
        INodeStatement statement, KeyWordValue direction)
    {
        _identifier = identifier;
        _startExpression = startExpression;
        _endExpression = endExpression;
        if (statement is NullStatement)
        {
            _statement = null;
        }

        _statement = statement;
        _direction = direction;
    }

    public void Generate(Generator.Generator generator)
    {
        generator.ForCounter += 1;
        _startExpression.Generate(generator);
        switch (_identifier)
        {
            case SymbolVariableParameter parameter:
                generator.AddRight(AssemblerCommand.Pop, IndirectAssemblerRegisters.Ebp, 8 + parameter.Offset);
                break;
            case SymbolVariableLocal local:
                generator.AddLeft(AssemblerCommand.Pop, IndirectAssemblerRegisters.Ebp, 4 + local.Offset);
                var offset = 0;
                if (local.Offset - generator.EspHead >= 0)
                {
                    offset = (local.Offset - generator.EspHead) + 4;
                    generator.EspHead = local.Offset - generator.EspHead;
                }
                generator.Add(AssemblerCommand.Sub, AssemblerRegisters.Esp, offset);
                break;
            default:
                generator.Add(AssemblerCommand.Pop, _identifier);
                break;
        }
        _endExpression.Generate(generator);
        generator.Add(AssemblerCommand.Pop, AssemblerRegisters.Ecx);
        var command = AssemblerCommand.Jg;
        var updateValue = 1;
        switch (_direction)
        {
            case KeyWordValue.To:
                command = AssemblerCommand.Jg;
                updateValue = 1;
                break;
            case KeyWordValue.DownTo:
                command = AssemblerCommand.Jl;
                updateValue = -1;
                break;
        }
        generator.Add($"for{generator.ForCounter}:");

        switch (_identifier)
        {
            case SymbolVariableParameter parameter:
                break;
            case SymbolVariableLocal local:
                break;
            default:
                generator.Add(AssemblerCommand.Cmp, _identifier, AssemblerRegisters.Ecx);
                generator.Add(command, $"endOfFor{generator.ForCounter}");
                _statement?.Generate(generator);
                generator.Add(AssemblerCommand.Add, _identifier, updateValue);
                generator.Add(AssemblerCommand.Jmp, $"for{generator.ForCounter}");
                generator.Add($"endOfFor{generator.ForCounter}:");
                break;
        }
    }

    public string GetPrint(int level)
    {
        var result = "";
        for (int i = 0; i < level * 4; i++)
        {
            result += " ";
        }
        result += "ForStatement";
        result += '\n';
        
        for (int i = 0; i < (level + 1) * 4; i++)
        {
            result += " ";
        }
        result += "Identifier";
        result += '\n';

        result += _identifier.GetPrint(level + 2);
        result += '\n';
        
        for (int i = 0; i < (level + 1) * 4; i++)
        {
            result += " ";
        }
        result += "LeftBound";
        result += '\n';

        result += _startExpression.GetPrint(level + 2);
        result += '\n';

        for (int i = 0; i < (level + 1) * 4; i++)
        {
            result += " ";
        }
        result += "RightBound";
        result += '\n';
        
        result += _endExpression.GetPrint(level + 2);
        result += '\n';
        
        for (int i = 0; i < (level + 1) * 4; i++)
        {
            result += " ";
        }
        result += "Body";
        result += _statement is not null ? '\n' + _statement.GetPrint(level + 2) : "";
        return result;
    }
}