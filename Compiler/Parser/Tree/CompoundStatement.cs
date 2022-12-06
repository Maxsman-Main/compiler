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
        var result = "";
        for (int i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += "CompoundStatement";

        if (_body.Count > 0)
        {
            Console.WriteLine(_body[0].GetPrint(0));
            result += '\n';
        }

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