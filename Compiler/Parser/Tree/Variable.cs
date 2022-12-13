using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class Variable : INodeExpression, IVariable
{
    private readonly SymbolVariable? _symbol;
    private readonly List<INodeExpression> _expressions;
    
    public string Name { get; }

    public Variable(string name)
    {
        Name = name;
        _expressions = new List<INodeExpression>();
    }
    
    public Variable(string name, List<INodeExpression> expressions)
    {
        Name = name;
        _expressions = expressions;
    }

    public Variable(SymbolVariable symbol)
    {
        _symbol = symbol;
        _expressions = new List<INodeExpression>();
        Name = symbol.Name;
    }
    
    public Variable(SymbolVariable symbol, List<INodeExpression> expressions)
    {
        _symbol = symbol;
        _expressions = expressions;
        Name = symbol.Name;
    }

    public string GetPrint(int level)
    {
        var value = "";
        for (int i = 0; i < level * 4; i++)
        {
            value += " ";
        }

        value += Name;
        if (_expressions.Count != 0)
        {
            value += "[]";
            value += "\n";
            for (int i = 0; i < (level + 1) * 4; i++)
            {
                value += " ";
            }

            value += "indexes";
            value += "\n";
        }
        for (int i = 0; i < _expressions.Count; i++)
        {
            value += _expressions[i].GetPrint(level + 2);
            
            if (i != _expressions.Count - 1)
            {
                value += "\n";
            }
        }
        return value;
    }
}