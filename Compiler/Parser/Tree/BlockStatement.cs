namespace Compiler.Parser.Tree;

public class BlockStatement
{
    private readonly List<INodeStatement> _body;

    public BlockStatement(List<INodeStatement> body)
    {
        _body = body;
    }
}