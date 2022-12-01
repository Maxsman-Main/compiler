namespace Compiler.Parser.Tree;

public class CompoundStatement : INodeStatement
{
    private readonly List<INodeStatement> _body;

    public CompoundStatement(List<INodeStatement> body)
    {
        _body = body;
    }
    
    public string GetPrint(int level)
    {
        Console.WriteLine(_body.Count);
    
        var result = "";
        for (int i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += "CompoundStatement";
        result += '\n';

        for (int i = 0; i < _body.Count; i++)
        {
            result += _body[i].GetPrint(level + 1);
            if (i != _body.Count - 1)
            {
                result += '\n';
            }
        }

        return result;
    }
}