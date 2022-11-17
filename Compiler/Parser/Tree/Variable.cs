namespace Compiler.Parser.Tree;

public class Variable : Node
{
    private string _name;

    public Variable(string name)
    {
        _name = name;
    }
    
    public override int Calc()
    {
        return 0;
    }

    public override string GetPrint(int level)
    {
        var value = "";
        for (int i = 0; i < level; i++)
        {
            value += "\t";
        }

        value += _name;
        return value;
    }
}