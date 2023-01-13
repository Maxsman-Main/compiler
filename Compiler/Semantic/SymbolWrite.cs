using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolWrite : SymbolProcedure
{
    public SymbolWrite(string name, SymbolTable parameters, SymbolTable locals, INodeStatement body) : base(name, parameters, locals, body)
    {
    }
}