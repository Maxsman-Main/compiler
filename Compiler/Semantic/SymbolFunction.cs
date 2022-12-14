using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolFunction : SymbolProcedure
{
    public SymbolType ReturnType { get; }

    public SymbolFunction(string name, SymbolTable parameters, SymbolTable locals, INodeStatement body, SymbolType returnType) : base(name, parameters, locals, body)
    {
        ReturnType = returnType;
    }
}