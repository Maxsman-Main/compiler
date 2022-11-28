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

public interface INodeType : INode
{
    
}

public interface INodeDeclaration : INode
{
    
}

public interface INodeProgram : INode
{
    
}

public interface IMainBlockNode : INode
{
    
}