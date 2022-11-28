using Compiler.Constants;

namespace Compiler.Parser.Tree;

public class UnaryOperation : INodeExpression
{
    private readonly OperatorValue _operation;
    private readonly INode _argument;

    public UnaryOperation(OperatorValue operation, INode argument)
    {
        _operation = operation;
        _argument = argument;
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
        value += _argument.GetPrint(level + 1);
        return value;
    }
}