namespace Compiler.Parser.Tree;

public class ForStatement : INodeStatement
{
    private readonly Variable _identifier;
    private readonly INodeExpression _startExpression;
    private readonly INodeExpression _endExpression;
    private readonly INodeStatement _statement;

    public ForStatement(Variable identifier, INodeExpression startExpression, INodeExpression endExpression,
        INodeStatement statement)
    {
        _identifier = identifier;
        _startExpression = startExpression;
        _endExpression = endExpression;
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
        result += _identifier.GetPrint(level + 1);
        result += '\n';
        result += _startExpression.GetPrint(level + 1);
        result += '\n';
        result += _endExpression.GetPrint(level + 1);
        result += '\n';
        result += _statement.GetPrint(level + 1);
        return result;
    }
}