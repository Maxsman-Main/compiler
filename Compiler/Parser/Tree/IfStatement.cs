﻿namespace Compiler.Parser.Tree;

public class IfStatement : INodeStatement
{
    private readonly INodeExpression _condition;
    private readonly INodeStatement _body;
    private readonly INodeStatement? _elsePart;
    
    public IfStatement(INodeExpression condition, INodeStatement body, INodeStatement? elsePart)
    {
        _condition = condition;
        _body = body;
        _elsePart = elsePart;
    }

    public string GetPrint(int level)
    {
        var result = "";
        for (int i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += "IfStatement";
        result += '\n';
        result += _condition.GetPrint(level + 1);
        result += '\n';
        result += _body.GetPrint(level + 1);
        result += '\n';
        result += _elsePart?.GetPrint(level + 1);
        return result;
    }
}