namespace Compiler.Parser.Tree;

public class AssignStatement
{
    private readonly INodeExpression _left;
    private readonly INodeExpression _right;

    public AssignStatement(INodeExpression left, INodeExpression right)
    {
        _left = left;
        _right = right;
    }
}