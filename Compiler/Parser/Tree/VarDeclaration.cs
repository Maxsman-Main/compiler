namespace Compiler.Parser.Tree;

public struct VarDeclarationData
{
    public Variable Identifier;
    public INodeType Type;
    public INodeExpression? Expression;
}

public class VarDeclaration : INodeDeclaration
{
    private const string Name = "VarDeclaration";
    
    private readonly List<VarDeclarationData> _varDeclarations;

    public VarDeclaration(List<VarDeclarationData> varDeclarations)
    {
        _varDeclarations = varDeclarations;
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
        
        for(int i = 0; i < _varDeclarations.Count; i++)
        {
            result += _varDeclarations[i].Identifier.GetPrint(level + 1);
            result += "\n";
            result += _varDeclarations[i].Type.GetPrint(level + 2);
            if (_varDeclarations[i].Expression is not null)
            {
                result += "\n";
            }
            result += _varDeclarations[i].Expression?.GetPrint(level + 3);
            if (i != _varDeclarations.Count - 1)
            {
                result += "\n";
            }
        }

        return result;
    }
}