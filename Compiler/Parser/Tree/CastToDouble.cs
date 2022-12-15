using Compiler.Exceptions;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class CastToDouble : INodeExpression
{
    private INodeExpression _expression;
    
    public CastToDouble(INodeExpression expression)
    {
        if (expression.GetExpressionType() is not SymbolInteger)
        {
            throw new CompilerException("can't cast " + expression.GetExpressionType() + " to int");
        }
        _expression = expression;
    }

    public SymbolType GetExpressionType()
    {
        return new SymbolDouble("Double");
    }
    
    public string GetPrint(int level)
    {
        
        var result = "";
        for (var i = 0; i < level * 4; i++)
        {
            result += " ";
        }
        result += "CastToDouble";
        result += "\n";
        return result + _expression.GetPrint(level + 1);
    }
}