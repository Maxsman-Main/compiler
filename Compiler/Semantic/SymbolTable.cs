using System.Collections.Specialized;
using Compiler.Exceptions;

namespace Compiler.Semantic;

public struct Pair
{
    public string Key;
    public Symbol Value;
}

public class SymbolTable
{
    public OrderedDictionary Data { get; }
    public int Count => Data.Count;
    
    public int Size { get; set; }

    public SymbolTable()
    {
        Data = new OrderedDictionary();
        Size = 0;
    }
    
    public SymbolTable(OrderedDictionary data)
    {
        Data = data;
    }

    public void GenerateForVariables(Generator.Generator generator)
    {
        foreach (var symbol in Data.Values)
        {
            var sym = ((Symbol)symbol);
            if (sym is SymbolVariable)
            {
                sym.Generate(generator);
            }
        }
    }

    public void GenerateForProcedures(Generator.Generator generator)
    {
        foreach (var symbol in Data.Values)
        {
            var sym = ((Symbol)symbol);
            if (sym is SymbolProcedure)
            {
                sym.Generate(generator);
            }
        }
    }

    public void Add(string name, Symbol value)
    {
        try
        {
            Data.Add(name, value);
        }
        catch (ArgumentException)
        {
            
        }
    }

    public SymbolTable Merge(SymbolTable table)
    {
        var dictionary = new OrderedDictionary();
        foreach (var symbol in Data.Values.Cast<Symbol>())
        {
            try
            {
                if (symbol != null) dictionary.Add(symbol.Name, symbol);
            }
            catch (ArgumentException)
            {
                throw new CompilerException(symbol?.Name + " was defined twice");
            }
        }
        
        foreach (var symbol in table.Data.Values.Cast<Symbol?>())
        {
            try
            {
                if (symbol != null) dictionary.Add(symbol.Name, symbol);
            }
            catch (ArgumentException)
            {
                throw new CompilerException(symbol?.Name + " was defined twice");
            }
        }

        return new SymbolTable(dictionary);
    }

    public Symbol? Get(string name)
    {
        return Data[name] as Symbol;
    }

    public string? CompareTablesData(SymbolTable table)
    {
        foreach (var item in Data.Values)
        {
            var name = ((Symbol) item).Name;
            if (table.Get(name) is not null)
            {
                return name;
            }
        }

        return null;
    }

    public string GetPrint(int level)
    {
        var result = "";
        foreach (var item in Data.Values)
        {
            var symbol = (Symbol) item;
            result += symbol.GetPrint(level);
            result += "\n";
        }

        return result;
    }
}