using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public abstract class Parameter
{
    private readonly Variable _identifier;
    private readonly SymbolType _type;

    public Variable Identifier => _identifier;
    public SymbolType Type => _type;

    protected Parameter(Variable identifier, SymbolType type)
    {
        _identifier = identifier;
        _type = type;
    }
}

public class ValueParameter : Parameter
{
    public ValueParameter(Variable identifier, SymbolType type) : base(identifier, type)
    {
    }
}

public class VarParameter : Parameter
{
    public VarParameter(Variable identifier, SymbolType type) : base(identifier, type)
    {
    }
}

public class FunctionDeclaration : INodeDeclaration
{
    private const string Name = "FunctionDeclaration";
    
    private readonly Variable _identifier;
    private readonly List<Parameter> _parameters;
    private readonly SymbolType _returnType;
    private readonly List<INodeDeclaration> _declarations;
    private readonly INodeStatement _block;
    
    public FunctionDeclaration(Variable identifier, List<Parameter> parameters, SymbolType returnType, List<INodeDeclaration> declarations, INodeStatement statements)
    {
        _identifier = identifier;
        _parameters = parameters;
        _returnType = returnType;
        _declarations = declarations;
        _block = statements;
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
            if (_parameters[i] is VarParameter)
            {
                result += "var ";
            }
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

        result += "\n";

        result += _block.GetPrint(level + 2);
        return result;
    }
}