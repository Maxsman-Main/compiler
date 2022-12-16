using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolVarVariable : SymbolVariable
{
    public SymbolVarVariable(string name, SymbolType type) : base(name, type)
    {
    }

    public SymbolVarVariable(string name, SymbolType type, INodeExpression expression) : base(name, type, expression)
    {
    }
}