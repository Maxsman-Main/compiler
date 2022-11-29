namespace Compiler.Parser.Tree;

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
    private INodeType _returnType;

    public FunctionDeclaration(Variable identifier, List<Parameter> parameters, INodeType returnType)
    {
        _identifier = identifier;
        _parameters = parameters;
        _returnType = returnType;
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
        foreach (var parameter in _parameters)
        {
            result += parameter.Identifier + ":" + parameter.Type + ",";
        }
        result += ")";
        result += " : " + _returnType.GetPrint(0);
        return result;
    }
}