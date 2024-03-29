﻿using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public struct TypeDeclarationPair
{
    public Variable Identifier;
    public SymbolType Type;
}

public class TypeDeclaration : INodeDeclaration
{
    private const string Name = "TypeDeclaration";
    
    private readonly List<TypeDeclarationPair> _typeDeclarations;

    public TypeDeclaration(List<TypeDeclarationPair> typeDeclarations)
    {
        _typeDeclarations = typeDeclarations;
    }

    public string GetPrint(int level)
    {
        var result = "";
        for (int i = 0; i < level * 4; i++)
        {
            result += " ";
        }
        
        result += Name;
        result += "\n";
        
        for(int i = 0; i < _typeDeclarations.Count; i++)
        {
            result += _typeDeclarations[i].Identifier.GetPrint(level + 1);
            result += "\n";
            result += _typeDeclarations[i].Type.GetPrint(level + 2);
            if (i != _typeDeclarations.Count - 1)
            {
                result += "\n";
            }
        }

        return result;
    }
}