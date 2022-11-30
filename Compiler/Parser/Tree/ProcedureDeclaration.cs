namespace Compiler.Parser.Tree;

public class ProcedureDeclaration : INodeDeclaration
{
    private const string Name = "ProcedureDeclaration";
    
    private readonly Variable _identifier;
    private readonly List<Parameter> _parameters;
    private readonly List<INodeDeclaration> _declarations;
    private readonly INodeStatement _statement;

    public ProcedureDeclaration(Variable identifier, List<Parameter> parameters, List<INodeDeclaration> declarations, INodeStatement statement)
    {
        _identifier = identifier;
        _parameters = parameters;
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