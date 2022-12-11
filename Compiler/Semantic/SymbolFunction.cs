using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolFunction : SymbolProcedure
{
    private SymbolType _returnType;
    
    public SymbolFunction(string name, SymbolTable parameters, SymbolTable locals, CompoundStatement body, SymbolType returnType) : base(name, parameters, locals, body)
    {
        _returnType = returnType;
    }
}