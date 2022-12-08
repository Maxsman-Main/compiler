namespace Compiler.Parser.Tree;

public class RecordSection : INode
{
    private readonly List<Variable> _identifiers;
    private readonly INodeType _type;

    public RecordSection(List<Variable> identifiers, INodeType type)
    {
        _identifiers = identifiers;
        _type = type;
    }

    public string GetPrint(int level)
    {
        var result = "";
        
        for (int i = 0; i < _identifiers.Count; i++)
        {
            result += _identifiers[i].GetPrint(level);
            result += "\n";
            result += _type.GetPrint(level + 1);

            if (i != _identifiers.Count - 1)
            {
                result += '\n';
            }
        }
        
        return result;
    }
}