using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolVariableParameter : SymbolVariable
{
    public int Offset { get; set; }

    public SymbolVariableParameter(string name, SymbolType type, INodeExpression? expression) : base(name, type, expression)
    {
        Offset = 0;
    }

    public void Generate(SymbolTable table)
    {
        Offset = table.Size;
        table.Size += Type.Size;
    }
}