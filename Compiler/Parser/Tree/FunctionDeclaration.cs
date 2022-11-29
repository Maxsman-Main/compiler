﻿namespace Compiler.Parser.Tree;

public struct Parameter
{
    public Variable Identifier;
    public INodeType Type;
}

public class FunctionDeclaration : INodeDeclaration
{
    private const string Name = "FunctionDeclaration";
    
    private readonly Variable _identifier;
    private readonly List<Parameter> _parameters;
    private readonly INodeType _returnType;
    private readonly List<INodeDeclaration> _declarations;
    private readonly INodeStatement _statement;

    public FunctionDeclaration(Variable identifier, List<Parameter> parameters, INodeType returnType, List<INodeDeclaration> declarations, INodeStatement statement)
    {
        _identifier = identifier;
        _parameters = parameters;
        _returnType = returnType;
        _declarations = declarations;
        _statement = statement;
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

        result += _identifier.GetPrint(level + 1);
        result += "(";
        for(int i = 0; i < _parameters.Count; i++)
        {
            result += _parameters[i].Identifier.GetPrint(0) + ":" + _parameters[i].Type.GetPrint(0);
            if (i != _parameters.Count - 1)
            {
                result += ", ";
            }
        }
        result += ")";
        result += " : " + _returnType.GetPrint(0);
        if (_declarations.Count != 0)
        {
            result += "\n";
        }

        foreach (var declaration in _declarations)
        {
            result += declaration.GetPrint(level + 2);
        }

        //result += _statement.GetPrint(level + 1);
        return result;
    }
}