namespace Compiler.Parser.Tree;

public class Bound : INode
{
    private readonly int _leftBound;
    private readonly int _rightBound;
    
    public Bound(int leftBound, int rightBound)
    {
        _leftBound = leftBound;
        _rightBound = rightBound;
    }
    
    public string GetPrint(int level)
    {
        var result = "";
        for (int i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += _leftBound + ".." + _rightBound;

        return result;
    }
}