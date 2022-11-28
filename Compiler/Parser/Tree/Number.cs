namespace Compiler.Parser.Tree;

public class Number : INodeExpression
{
    private readonly int _value;

    public Number(int value)
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

        value += _value.ToString();
        return value;
    }
}