using Compiler.Constants;
using Compiler.Exceptions;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class UnaryOperation : INodeExpression
{
    private readonly OperatorValue _operation;
    private readonly INodeExpression _argument;

    public UnaryOperation(OperatorValue operation, INodeExpression argument)
    {
        _operation = operation;
        _argument = argument;
    }
    
    public SymbolType GetExpressionType()
    {
        if (_argument.GetExpressionType() is SymbolInteger or SymbolDouble)
        {
            return _argument.GetExpressionType();
        }

        throw new CompilerException("argument can't use with unary " + _operation);
    }

    public void Generate(Generator.Generator generator)
    {
        _argument.Generate(generator);
        if (_operation is not OperatorValue.Minus) return;
        generator.Add(AssemblerCommand.Pop, AssemblerRegisters.Eax);
        generator.Add(AssemblerCommand.IMul, AssemblerRegisters.Eax, -1);
        generator.Add(AssemblerCommand.Push, AssemblerRegisters.Eax);
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