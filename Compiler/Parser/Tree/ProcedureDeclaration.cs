namespace Compiler.Parser.Tree;

public class ProcedureDeclaration : INodeDeclaration
{
    private const string Name = "ProcedureDeclaration";
    
    private readonly Variable _identifier;
    private readonly List<Parameter> _parameters;
    private readonly List<INodeDeclaration> _declarations;
    private readonly INodeStatement _statements;

    public ProcedureDeclaration(Variable identifier, List<Parameter> parameters, List<INodeDeclaration> declarations, INodeStatement statements)
    {
        _identifier = identifier;
        _parameters = parameters;
        _declarations = declarations;
        _statements = statements;
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
        result += "\n";

        for (var index = 0; index < _declarations.Count; index++)
        {
            result += _declarations[index].GetPrint(level + 2);
            if (index != _declarations.Count)
            {
                result += "\n";
            }
        }

        result += _statements.GetPrint(level + 2);
        return result;
    }
}