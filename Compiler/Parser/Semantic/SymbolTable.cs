using System.Collections.Specialized;

namespace Compiler.Parser.Semantic;

public class SymbolTable
{
    private readonly OrderedDictionary _data;

    public SymbolTable(OrderedDictionary data)
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