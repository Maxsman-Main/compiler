using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolVariableLocal : SymbolVariable
{
    private SymbolTableStack _stack;
    
    public int Offset { get; }

    public SymbolVariableLocal(string name, SymbolType type, INodeExpression? expression, SymbolTableStack stack) : base(name, type, expression)
    {
        Offset = 0;
        _stack = stack;
    }

    public override void Generate(Generator.Generator generator)
    {
        base.Generate(generator);
    }
}