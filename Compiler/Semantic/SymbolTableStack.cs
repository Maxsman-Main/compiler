using System.Collections.Specialized;
using Compiler.Constants;
using Compiler.Exceptions;
using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolTableStack
{
    private List<SymbolTable> Tables { get; }
    private int _head = -1;

    public SymbolTableStack()
    {
        Tables = new List<SymbolTable>();
        InitializeStack();
    }

    public void Add(string name, Symbol value)
    {
        var result = Tables[_head].Get(name);
        if (result is not null)
        {
            throw new CompilerException(name + " identifier was defined twice");
        }
        Tables[_head].Add(name, value);
    }

    public Symbol Get(string name)
    {
        for (var i = _head; i >= 0; i--)
        {
            var result = Tables[i].Get(name);
            if (result is not null)
            {
                return result;
            }
        }

        throw new CompilerException(name + " identifier wasn't defined");
    }

    private void InitializeStack()
    {
        var dictionary = new OrderedDictionary();
        foreach (var keyWord in KeyWordsConstants.KeyWords)
        {
            var variable = new SymbolVariable(keyWord, new SymbolKeyWord(keyWord));
            dictionary.Add(keyWord, variable);
        }
        Tables.Add(new SymbolTable(dictionary));
        _head++;
    }
}