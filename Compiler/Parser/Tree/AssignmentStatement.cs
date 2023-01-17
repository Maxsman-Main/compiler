using Compiler.Constants;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class AssignmentStatement : INodeStatement
{
    private readonly Variable _identifier;
    private readonly SymbolVariable? _variable;
    private readonly INodeExpression _expression;

    public AssignmentStatement(Variable identifier, INodeExpression expression)
    {
        _identifier = identifier;
        _expression = expression;
    }

    public AssignmentStatement(SymbolVariable variable, INodeExpression expression)
    {
        _variable = variable;
        _identifier = new Variable(variable.Name);
        _expression = expression;
    }

    public void Generate(Generator.Generator generator)
    {
        _expression.Generate(generator);
        switch (_variable)
        {
            case SymbolVariableParameter parameter:
                generator.AddRight(AssemblerCommand.Pop, IndirectAssemblerRegisters.Ebp, 8 + parameter.Offset);
                return;
            case SymbolVariableLocal local:
                generator.AddLeft(AssemblerCommand.Pop, IndirectAssemblerRegisters.Ebp, local.Offset);
                return;
            default:
                generator.Add(AssemblerCommand.Pop, _identifier);
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

        result += ":=";
        result += '\n';
        result += _identifier.GetPrint(level + 1);
        result += '\n';
        result += _expression.GetPrint(level + 1);

        return result;
    }
}