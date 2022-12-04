using Compiler.Constants;

namespace Compiler.Parser.Tree;

public class Type : INodeType
{
    private readonly KeyWordValue _type;

    public Type(KeyWordValue type)
    {
        _type = type;
    }
    
    public virtual string GetPrint(int level)
    {
        var result = "";
        for (int i = 0; i < level * 4; i++)
        {
            result += " ";
        }
        
        return result + _type;
    }
}