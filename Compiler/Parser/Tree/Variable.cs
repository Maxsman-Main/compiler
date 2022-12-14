using System.Reflection.PortableExecutable;
using Compiler.Exceptions;
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
    
    
    public SymbolType GetExpressionType()
    {
        if (_expressions.Count != 0)
        {
            var type = _symbol?.Type as SymbolArray ?? throw new CompilerException(_symbol?.Name + " isn't array");
            var itemsType = type.ItemsType;
            for (var i = 1; i < _expressions.Count; i++)
            {
                if (itemsType is not SymbolArray arrayType)
                {
                    throw new CompilerException(_symbol.Name +  " array has " + i + " indexes, but " + _expressions.Count + " received");
                }

                itemsType = arrayType.ItemsType;
            }

            return itemsType;
        }

        if (_symbol == null) throw new CompilerException("can't get type for variable");
        if(_symbol.Value is null)
        {
            throw new CompilerException(_symbol.Name + " isn't initialize");
        }
        return _symbol.Type;
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