namespace Compiler.Generator;

public class DirectoryMaker
{
    public void MakeDirectory(string name)
    {
        var nameWithoutFormat = name.TakeWhile(lit => lit != '.').Aggregate("", (current, lit) => current + lit);
        Directory.CreateDirectory("./" + nameWithoutFormat);
    }
}