namespace Compiler.Parser.Tree;

public interface INode
{
    public string GetPrint(int level);
}

public interface INodeExpression : INode
{
    
}

public interface INodeStatement : INode
{
    
}