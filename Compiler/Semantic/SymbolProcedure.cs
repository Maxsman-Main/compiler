using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolProcedure : Symbol
{
    protected readonly SymbolTable Locals;
    protected readonly INodeStatement Body;

    public SymbolTable Parameters { get; }

    public SymbolProcedure(string name, SymbolTable parameters, SymbolTable locals, INodeStatement body) : base(name)
    {
        Parameters = parameters;
        Locals = locals;
        Body = body;
    }

    public override string GetPrint(int level)
    {
        var result = "";
        result += Name;
        result += "\n";
        result += Parameters.GetPrint(level + 1);
        result += Locals.GetPrint(level + 1);
        result += Body.GetPrint(level + 1);
        return result;
    }
}