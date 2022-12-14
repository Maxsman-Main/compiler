using Compiler.Constants;
using Compiler.Exceptions;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class RecordAccess : INodeExpression
{
    private const OperatorValue Operation = OperatorValue.Point;

    private readonly INodeExpression _left;
    private readonly string? _right;
    private readonly SymbolVariable? _field;

    public RecordAccess(INodeExpression left, string right)
    {
        _left = left;
        _right = right;
    }

    public RecordAccess(INodeExpression left, SymbolVariable field)
    {
        _left = left;
        _field = field;
    }

    public SymbolType GetExpressionType()
    {
        if (_field != null) return _field.Type;
        throw new CompilerException("can't get type for field");
    }
    
    public string GetPrint(int level)
    {
        var value = "";
        for (int i = 0; i < level * 4; i++)
        {
            value += " ";
        }

        value += OperatorConstants.OperatorSymbols[Operation];
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