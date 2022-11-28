namespace Compiler.Parser.Tree;

public class Variable : INodeExpression
{
    private readonly string _name;

    public Variable(string name)
    {
        _name = name;
    }

    public string GetPrint(int level)
    {
        var value = "";
        for (int i = 0; i < level * 4; i++)
        {
            value += " ";
        }

        value += _name;
        return value;
    }
}