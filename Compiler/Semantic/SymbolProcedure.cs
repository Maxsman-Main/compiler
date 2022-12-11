using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolProcedure : Symbol
{
    private SymbolTable _parameters;
    private SymbolTable _locals;
    private CompoundStatement _body;

    protected SymbolProcedure(string name, SymbolTable parameters, SymbolTable locals, CompoundStatement body) : base(name)
    {
        _parameters = parameters;
        _locals = locals;
        _body = body;
    }
}