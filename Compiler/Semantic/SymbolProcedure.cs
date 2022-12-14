using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolProcedure : Symbol
{
    private SymbolTable _locals;
    private INodeStatement _body;

    public SymbolTable Parameters { get; }

    public SymbolProcedure(string name, SymbolTable parameters, SymbolTable locals, INodeStatement body) : base(name)
    {
        Parameters = parameters;
        _locals = locals;
        _body = body;
    }

    public override string GetPrint(int level)
    {
        var result = "";
        result += Parameters.GetPrint(level);
        result += "\n";
        result += _locals.GetPrint(level);
        result += "\n";
        result += _body.GetPrint(level);
        return result;
    }
}