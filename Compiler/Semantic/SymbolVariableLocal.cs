using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolVariableLocal : SymbolVariable
{
    public int Offset { get; set; }

    public SymbolVariableLocal(string name, SymbolType type, INodeExpression? expression) : base(name, type, expression)
    {
        Offset = 0;
    }

    public void Generate(SymbolTable table)
    {
        Offset = table.Size;
        table.Size += 4;
    }
}