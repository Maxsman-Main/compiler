using System.Collections.Specialized;
using Compiler.Constants;
using Compiler.Exceptions;
using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolTableStack
{
    private List<SymbolTable> Tables { get; }
    private int _head = -1;
    
    public int Offset { get; set; }

    public SymbolTableStack()
    {
        Tables = new List<SymbolTable>();
        Offset = 0;
        InitializeStack();
    }

    public void GenerateForVariables(Generator.Generator generator)
    {
        for (var i = 1; i < Tables.Count; i++)
        {
            Tables[i].GenerateForVariables(generator);
        }
    }

    public void GenerateForProcedures(Generator.Generator generator)
    {
        for (var i = 1; i < Tables.Count; i++)
        {
            Tables[i].GenerateForProcedures(generator);
        }
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

    public void Push(SymbolTable table)
    {
        Tables.Add(table);
        _head++;
    }

    public SymbolTable Pop()
    {
        var item = Tables[_head];
        Tables.RemoveAt(_head);
        _head -= 1;
        return item;
    }

    private void InitializeStack()
    {
        var dictionary = new OrderedDictionary();
        var nullBlock = new CompoundStatement(new List<INodeStatement>());
        var writeProcedure = new SymbolProcedure("write", new SymbolTable(new OrderedDictionary()),
            new SymbolTable(new OrderedDictionary()), nullBlock, this);
        var readProcedure = new SymbolProcedure("read", new SymbolTable(new OrderedDictionary()),
            new SymbolTable(new OrderedDictionary()), nullBlock, this);
        dictionary.Add("write", writeProcedure);
        dictionary.Add("read", readProcedure);
        Tables.Add(new SymbolTable(dictionary));
        _head++;
        Tables.Add(new SymbolTable(new OrderedDictionary()));
        _head++;
    }

    public string GetPrint()
    {
        var result = "";
        foreach (var table in Tables)
        {
            result += table.GetPrint(0);
            result += "\n";
        }

        return result;
    }
}