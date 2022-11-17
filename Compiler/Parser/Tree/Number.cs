namespace Compiler.Parser.Tree;

public class Number : Node
{
    private int _value;

    public Number(int value)
    {
        _value = value;
    }

    public override int Calc()
    {
        return _value;
    }

    public override string GetPrint(int level)
    {
        var value = "";
        for (int i = 0; i < level; i++)
        {
            value += "\t";
        }

        value += _value.ToString();
        return value;
    }
}