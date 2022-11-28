using Compiler.Constants;

namespace Compiler.Parser.Tree;

public class BinOperation : INodeExpression
{
    private readonly OperatorValue _operation;
    private readonly INode _left;
    private readonly INode _right;

    public BinOperation(OperatorValue operation, INode left, INode right)
    {
        _operation = operation;
        _left = left;
        _right = right;
    }

    public string GetPrint(int level)
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