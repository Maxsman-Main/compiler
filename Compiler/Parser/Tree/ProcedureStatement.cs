using Compiler.Constants;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class ProcedureStatement : INodeStatement
{
    private readonly Variable _identifier;
    private readonly SymbolProcedure? _procedure;
    private readonly List<INodeExpression>? _parameters;
    
    public ProcedureStatement(Variable identifier, List<INodeExpression>? parameters)
    {
        _identifier = identifier;
        _parameters = parameters;
    }

    public ProcedureStatement(SymbolProcedure procedure, List<INodeExpression> parameters)
    {
        _identifier = new Variable(procedure.Name);
        _procedure = procedure;
        _parameters = parameters;
    }

    public void Generate(Generator.Generator generator)
    {
        var size = 0;
        foreach (var parameter in _parameters)
        {
            parameter.Generate(generator);
            var type = parameter.GetExpressionType();
            if (type is SymbolDouble)
            {
                size += 8;
            }
            else
            {
                size += 4;
            }
        }
        generator.Add(AssemblerCommand.Call, _identifier);
        generator.Add(AssemblerCommand.Add, AssemblerRegisters.Esp, size);
    }

    public string GetPrint(int level)
    {
        var result = "";
        for (int i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += "ProcedureStatement";
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
        result += "arguments";

        switch (_parameters)
        {
            case null:
                return result;
            case {Count: 0}:
                return result;
        }

        result += '\n';
        for (var index = 0; index < _parameters.Count; index++)
        {
            var parameter = _parameters[index];
            result += parameter.GetPrint(level + 2);
            if (index != _parameters.Count - 1)
            {
                result += '\n';
            }
        }

        return result;
    }
}