using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolVariable : Symbol, IVariable
{
    public SymbolType Type { get; }
    public INodeExpression? Value { get; }

    public SymbolVariable(string name, SymbolType type) : base(name)
    {
        Type = type;
        Value = null;
    }

    public SymbolVariable(string name, SymbolType type, INodeExpression expression) : base(name)
    {
        Type = type;
        Value = expression;
    }

    public override string GetPrint(int level)
    {
        var result = "";
        for (var i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += Name + " " + Type.GetPrint(0);
        return result;
    }
}