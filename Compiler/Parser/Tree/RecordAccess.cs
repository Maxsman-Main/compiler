using Compiler.Constants;

namespace Compiler.Parser.Tree;

public class RecordAccess : INode
{
    private readonly OperatorValue _operation;
    private readonly INode _left;
    private readonly string _right;


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
        value += _right;
        return value;
    }
}