namespace Compiler.Parser.Tree;

public class NullStatement : INodeStatement
{
    public void Generate(Generator.Generator generator)
    {
    }

    public string GetPrint(int level)
    {
        return "";
    }
}