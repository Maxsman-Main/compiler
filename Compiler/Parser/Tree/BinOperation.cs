namespace Compiler.Parser.Tree;

public class BinOperation : Node
{
    private string _operation;
    private Node _left;
    private Node _right;

    public BinOperation(string operation, Node left, Node right)
    {
        _operation = operation;
        _left = left;
        _right = right;
    }

    public override int Calc()
    {
        switch (_operation)
        {
            case "+":
                return _left.Calc() + _right.Calc();
            case "-":
                return _left.Calc() - _right.Calc();
            case "*":
                return _left.Calc() * _right.Calc();
            case "/":
                return _left.Calc() / _right.Calc();
            default:
                return 0;
        }
    }

    public override string GetPrint(int level)
    {
        var value = "";
        for (int i = 0; i < level; i++)
        {
            value += "\t";
        }

        value += _operation;
        value += "\n";
        value += _left.GetPrint(level + 1);
        value += "\n";
        value += _right.GetPrint(level + 1);
        return value;
    }
}