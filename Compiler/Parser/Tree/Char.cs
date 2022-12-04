namespace Compiler.Parser.Tree;

public class Char : INodeExpression
{
    private readonly string _value;

    public Char(string value)
    {
        _value = value;
    }

    public string GetPrint(int level)
    {
        var value = "";
        for (int i = 0; i < level * 4; i++)
        {
            value += " ";
        }

        value += "\'";
        value += _value;
        value += "\'";
        return value;
    }
}