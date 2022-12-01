namespace Compiler.Parser.Tree;

public class Program : INodeProgram
{
    private const string Name = "Program";
    
    private readonly Variable? _identifier;
    private readonly List<INodeDeclaration> _declarations;
    private readonly INodeStatement _block;

    public Program(Variable identifier, List<INodeDeclaration> declarations, INodeStatement block)
    {
        _identifier = identifier;
        _declarations = declarations;
        _block = block;
    }

    public Program(List<INodeDeclaration> declarations, INodeStatement block)
    {
        _identifier = null;
        _declarations = declarations;
        _block = block;
    }

    public string GetPrint(int level)
    {
        var result = "";
        for (int i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += Name;
        if (_identifier is not null)
        {
            result += " " + _identifier.GetPrint(level);
        }
        result += "\n";
        foreach (var declaration in _declarations)
        {
            result += declaration.GetPrint(level + 1);
            result += "\n";
        }

        result += _block.GetPrint(level + 1);
        return result;
    }
}