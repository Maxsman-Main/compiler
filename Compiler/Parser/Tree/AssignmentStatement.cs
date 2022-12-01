namespace Compiler.Parser.Tree;

public class AssignmentStatement : INodeStatement
{
    private readonly Variable _identifier;
    private readonly INodeExpression _expression;

    public AssignmentStatement(Variable identifier, INodeExpression expression)
    {
        _identifier = identifier;
        _expression = expression;
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