namespace Compiler.Parser.Tree;

public class Variable : INodeExpression
{
    private readonly string _name;
    private readonly List<INodeExpression> _expressions;

    public Variable(string name)
    {
        _name = name;
        _expressions = new List<INodeExpression>();
    }

    public Variable(string name, List<INodeExpression> expressions)
    {
        _name = name;
        _expressions = expressions;
    }

    public string GetPrint(int level)
    {
        var value = "";
        for (int i = 0; i < level * 4; i++)
        {
            value += " ";
        }

        value += _name;
        if (_expressions.Count != 0)
        {
            value += "[]";
            value += "\n";
            for (int i = 0; i < (level + 1) * 4; i++)
            {
                value += " ";
            }

            value += "indexes";
            value += "\n";
        }
        for (int i = 0; i < _expressions.Count; i++)
        {
            value += _expressions[i].GetPrint(level + 2);
            
            if (i != _expressions.Count - 1)
            {
                value += "\n";
            }
        }
        return value;
    }
}