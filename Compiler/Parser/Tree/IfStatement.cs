namespace Compiler.Parser.Tree;

public class IfStatement
{
    private readonly INodeExpression _condition;
    private readonly INodeStatement _body;
    private readonly INodeStatement _elsePart;
    
    public IfStatement(INodeExpression condition, INodeStatement body, INodeStatement elsePart)
    {
        _condition = condition;
        _body = body;
        _elsePart = elsePart;
    }
}