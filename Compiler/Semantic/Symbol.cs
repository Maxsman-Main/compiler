namespace Compiler.Semantic;

public abstract class Symbol
{
    public string Name { get; }

    protected Symbol(string name)
    {
        Name = name;
    }

    public abstract string GetPrint(int level);
}