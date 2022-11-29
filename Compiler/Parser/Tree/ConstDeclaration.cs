namespace Compiler.Parser.Tree;

public struct ConstDeclarationData
{
    public Variable Identifier;
    public INodeType Type;
    public INodeExpression Expression;
}

public class ConstDeclaration : INodeDeclaration
{
    private const string Name = "ConstDeclaration";
    
    private readonly List<ConstDeclarationData> _constDeclarations;

    public ConstDeclaration(List<ConstDeclarationData> constDeclarations)
    {
        _constDeclarations = constDeclarations;
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
        
        for(int i = 0; i < _constDeclarations.Count; i++)
        {
            result += _constDeclarations[i].Identifier.GetPrint(level + 1);
            result += " ";
            result += _constDeclarations[i].Type.GetPrint(0);
            result += "\n";
            result += _constDeclarations[i].Expression.GetPrint(level + 2);
            if (i != _constDeclarations.Count - 1)
            {
                result += "\n";
            }
        }

        return result;
    }
}