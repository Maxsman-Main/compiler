namespace Compiler.Parser.Tree;

public partial class CompoundStatement : INodeStatement
{
    private readonly List<INodeStatement> _body;

    public CompoundStatement(List<INodeStatement> body)
    {
        _body = body;
    }

    public void Generate(Generator.Generator generator)
    {
        foreach (var statement in _body)
        {
            statement.Generate(generator);
        }
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