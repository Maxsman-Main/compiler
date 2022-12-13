using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolProcedure : Symbol
{
    private SymbolTable _parameters;
    private SymbolTable _locals;
    private INodeStatement _body;

    public SymbolProcedure(string name, SymbolTable parameters, SymbolTable locals, INodeStatement body) : base(name)
    {
        _parameters = parameters;
        _locals = locals;
        _body = body;
    }

    public override string GetPrint(int level)
    {
        var result = "";
        result += _parameters.GetPrint(level);
        result += "\n";
        result += _locals.GetPrint(level);
        result += "\n";
        result += _body.GetPrint(level);
        return result;
    }
}