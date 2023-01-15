namespace Compiler.Parser.Tree;

public class Bound : IBound
{
    public int LeftBound { get; }
    public int RightBound { get; }

    public Bound(int leftBound, int rightBound)
    {
        LeftBound = leftBound;
        RightBound = rightBound;
    }

    public string GetPrint(int level)
    {
        var result = "";
        for (int i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += LeftBound + ".." + RightBound;

        return result;
    }
}