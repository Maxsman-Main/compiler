namespace Compiler.Parser.Semantic;

public abstract class Symbol
{
    private string _name;

    protected Symbol(string name)
    {
        _name = name;
    }
}