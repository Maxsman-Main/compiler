namespace Compiler.Parser.Tree;

public class IdentifierType : INodeType
{
    private readonly Variable _identifier;

    public IdentifierType(Variable identifier)
    {
        _identifier = identifier;
    }

    public string GetPrint(int level)
    {
        var result = "";
        for (int i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += _identifier.GetPrint(0);
        return result;
    }
}