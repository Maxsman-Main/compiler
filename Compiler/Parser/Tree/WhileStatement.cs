using Compiler.Exceptions;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class WhileStatement : INodeStatement
{
    private readonly INodeExpression _condition;
    private readonly INodeStatement? _body;

    public WhileStatement(INodeExpression condition, INodeStatement body)
    {
        _condition = condition;
        if (_body is NullStatement)
        {
            _body = null;
        }
        _body = body;
    }

    public string GetPrint(int level)
    {
        var result = "";
        for (int i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += "WhileStatement";
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
        return result;
    }
}