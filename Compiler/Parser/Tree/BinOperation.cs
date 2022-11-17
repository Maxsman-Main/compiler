using Compiler.Constants;

namespace Compiler.Parser.Tree;

public class BinOperation : Node
{
    private OperatorValue _operation;
    private Node _left;
    private Node _right;

    public BinOperation(OperatorValue operation, Node left, Node right)
    {
        _operation = operation;
        _left = left;
        _right = right;
    }

    public override int Calc()
    {
        switch (_operation)
        {
            case OperatorValue.Plus:
                return _left.Calc() + _right.Calc();
            case OperatorValue.Minus:
                return _left.Calc() - _right.Calc();
            case OperatorValue.Multiplication:
                return _left.Calc() * _right.Calc();
            case OperatorValue.Div:
                return _left.Calc() / _right.Calc();
            default:
                return 0;
        }
    }

    public override string GetPrint(int level)
    {
        var value = "";
        for (int i = 0; i < level * 4; i++)
        {
            value += " ";
        }

        value += OperatorConstants.OperatorSymbols[_operation];
        value += "\n";
        value += _left.GetPrint(level + 1);
        value += "\n";
        value += _right.GetPrint(level + 1);
        return value;
    }
}