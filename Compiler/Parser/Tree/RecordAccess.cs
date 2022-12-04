using Compiler.Constants;

namespace Compiler.Parser.Tree;

public class RecordAccess : INodeExpression
{
    private readonly INode _left;
    private readonly OperatorValue _operation;
    private readonly string _right;

    public RecordAccess(INode left, string right)
    {
        _left = left;
        _operation = OperatorValue.Point;
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
        
        for (int i = 0; i < (level + 1) * 4; i++)
        {
            value += " ";
        }
        value += _right;
        return value;
    }
}