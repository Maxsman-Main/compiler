using Compiler.Exceptions;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class ForStatement : INodeStatement
{
    private readonly Variable _identifier;
    private readonly INodeExpression _startExpression;
    private readonly INodeExpression _endExpression;
    private readonly INodeStatement? _statement;

    public ForStatement(Variable identifier, INodeExpression startExpression, INodeExpression endExpression,
        INodeStatement statement)
    {
        _identifier = identifier;
        _startExpression = startExpression;
        _endExpression = endExpression;
        if (statement is NullStatement)
        {
            _statement = null;
        }

        _statement = statement;
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