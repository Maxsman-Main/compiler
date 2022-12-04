namespace Compiler.Parser.Tree;

public class RecordType : INodeType
{
    private readonly List<INode> _recordSection;

    public RecordType(List<INode> recordSection)
    {
        _recordSection = recordSection;
    }

    public string GetPrint(int level)
    {
        var result = "";
        for (int i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += "record";
        result += '\n';
        for (int i = 0; i < _recordSection.Count; i++)
        {
            result += _recordSection[i].GetPrint(level + 1);
            if (i != _recordSection.Count)
            {
                result += '\n';
            }
        }

        return result;
    }
}