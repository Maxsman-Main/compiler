using System.Collections.Specialized;

namespace Compiler.Semantic;

public class SymbolTable
{
    private readonly OrderedDictionary _data;

    public SymbolTable(OrderedDictionary data)
    {
        _data = data;
    }

    public void Add(string name, Symbol value)
    {
        _data.Add(name, value);
    }

    public Symbol? Get(string name)
    {
        return _data[name] as Symbol;
    }
}