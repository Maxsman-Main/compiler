using Compiler.Parser.Tree;

namespace Compiler.Parser.Semantic;

public class SymbolProcedure : Symbol
{
    private SymbolTable _parameters;
    private SymbolTable _locals;
    private CompoundStatement _body;

    public SymbolProcedure(string name, SymbolTable parameters, SymbolTable locals, CompoundStatement body) : base(name)
    {
        _parameters = parameters;
        _locals = locals;
        _body = body;
    }
}