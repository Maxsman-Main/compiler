using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public interface IVariable
{
    public SymbolType Type { get; }
    public INodeExpression? Value { get; }
}