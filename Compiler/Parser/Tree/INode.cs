using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public interface INode
{
    public string GetPrint(int level);
}

public interface INodeExpression : INode
{
    public void Generate(Generator.Generator generator);
    public SymbolType GetExpressionType();
}

public interface IBound : INode
{
    public int LeftBound { get; }
    public int RightBound { get; }
}

public interface INodeStatement : INode
{
    public void Generate(Generator.Generator generator);
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