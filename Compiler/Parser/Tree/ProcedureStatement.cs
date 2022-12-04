namespace Compiler.Parser.Tree;

public class ProcedureStatement : INodeStatement
{
    private readonly Variable _identifier;
    private readonly List<INodeExpression>? _parameters;

    public ProcedureStatement(Variable identifier, List<INodeExpression>? parameters)
    {
        _identifier = identifier;
        _parameters = parameters;
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
        result += _identifier.GetPrint(level + 1);
        result += '\n';
        if (_parameters == null) return result;
        foreach (var parameter in _parameters)
        {
            result += parameter.GetPrint(level + 2);
            result += '\n';
        }

        return result;
    }
}