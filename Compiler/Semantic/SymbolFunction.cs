using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolFunction : SymbolProcedure
{
    public SymbolType ReturnType { get; }
    public INodeExpression? ReturnValue { get; }

    public SymbolFunction(string name, SymbolTable parameters, SymbolTable locals, INodeStatement body, SymbolType returnType, SymbolTableStack stack) : base(name, parameters, locals, body, stack)
    {
        ReturnType = returnType;
        ReturnValue = null;
    }
    
    public SymbolFunction(string name, SymbolTable parameters, SymbolTable locals, INodeStatement body, SymbolType returnType, INodeExpression returnValue, SymbolTableStack stack) : base(name, parameters, locals, body, stack)
    {
        ReturnType = returnType;
        ReturnValue = returnValue;
    }

    public override string GetPrint(int level)
    {
        var result = "";
        result += Name;
        result += "\n";
        result += Parameters.GetPrint(level + 1);
        result += Locals.GetPrint(level + 1);
        result += Body.GetPrint(level + 1);
        result += "\n";
        result += ReturnType.GetPrint(level + 1);
        return result;
    }
}