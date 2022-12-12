namespace Compiler.Parser.Tree;

public interface INode
{
    public string GetPrint(int level);
}

public interface INodeExpression : INode
{
    
}

public interface IBound : INode
{
    public int LeftBound { get; }
    public int RightBound { get; }
}

public interface INodeStatement : INode
{
    
}

public interface IVariable : INode
{
    public string Name { get; }
}

public interface INodeType : INode
{
    
}

public interface INodeArrayType : INodeType
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