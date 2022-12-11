using System.Collections.Specialized;

namespace Compiler.Semantic;

public abstract class SymbolTable
{
    private readonly OrderedDictionary _data;

    protected SymbolTable(OrderedDictionary data)
    {
        _data = data;
    }

    public void Add(string name)
    {
    }

    public Symbol? GetByName(string name)
    {
        return _data[name] as Symbol;
    }
}