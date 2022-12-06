namespace Compiler.Parser.Tree;

public class IfStatement : INodeStatement
{
    private readonly INodeExpression _condition;
    private readonly INodeStatement? _body;
    private readonly INodeStatement? _elsePart;
    
    public IfStatement(INodeExpression condition, INodeStatement body, INodeStatement? elsePart)
    {
        _condition = condition;
        _body = body is NullStatement ? null : body;
        _elsePart = elsePart is NullStatement ? null : elsePart;
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
        result += _elsePart is not null ? '\n' + _elsePart.GetPrint(level + 2) : "";
        return result;
    }
}