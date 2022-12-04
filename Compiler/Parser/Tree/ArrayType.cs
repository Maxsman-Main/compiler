namespace Compiler.Parser.Tree;

public class ArrayType : INodeType
{
    private readonly INodeType _type;
    private readonly List<INode> _bounds;

    public ArrayType(INodeType type, List<INode> bounds)
    {
        _type = type;
        _bounds = bounds;
    }

    public string GetPrint(int level)
    {
        var result = "";
        for (int i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += "array[";
        for (int i = 0; i < _bounds.Count; i++)
        {
            result += _bounds[i].GetPrint(0);
            if (i != _bounds.Count - 1)
            {
                result += ",";
            }
        }
        result += "] of " + _type.GetPrint(0);

        return result;
    }
}