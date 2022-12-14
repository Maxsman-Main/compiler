using Compiler.Constants;
using Compiler.Exceptions;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class BinOperation : INodeExpression
{
    private readonly OperatorValue _operation;
    
    private SymbolType? _type;
    private INodeExpression _left;
    private INodeExpression _right;

    public BinOperation(OperatorValue operation, INodeExpression left, INodeExpression right)
    {
        _operation = operation;
        _left = left;
        _right = right;
    }
    
    public SymbolType GetExpressionType()
    {
        if (_type is not null)
        {
            return _type;
        }
        
        var leftType = _left.GetExpressionType();
        var rightType = _right.GetExpressionType();
        switch (leftType)
        {
            case SymbolInteger when rightType is SymbolInteger:
                _type = new SymbolInteger("Integer");
                break;
            case SymbolDouble when rightType is SymbolDouble:
                _type = new SymbolDouble("Double");
                break;
            case SymbolDouble when rightType is SymbolInteger:
                _right = new CastToDouble(_right);
                _type = new SymbolDouble("Double");
                break;
            case SymbolInteger when rightType is SymbolDouble:
                _left = new CastToDouble(_left);
                _type = new SymbolDouble("Double");
                break;
            default:
                throw new CompilerException("can't use bin operation for " + _left.GetExpressionType().Name + " and " +
                                            _right.GetExpressionType().Name);
        }
        return _type;
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