﻿using System.Collections.Specialized;
using Compiler.Exceptions;

namespace Compiler.Semantic;

public struct Pair
{
    public string Key;
    public Symbol Value;
}

public class SymbolTable
{
    private OrderedDictionary Data { get; }

    public SymbolTable(OrderedDictionary data)
    {
        Data = data;
    }

    public void Add(string name, Symbol value)
    {
        Data.Add(name, value);
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