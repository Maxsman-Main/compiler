namespace Compiler.Parser.Tree;

public class WhileStatement
{
    private readonly INodeExpression _condition;
    private readonly INodeStatement _body;

    public WhileStatement(INodeExpression condition, INodeStatement body)
    {
        _condition = condition;
        _body = body;
    }
}