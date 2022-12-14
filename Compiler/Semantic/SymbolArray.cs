using Type = Compiler.Parser.Tree.Type;

namespace Compiler.Semantic;

public class SymbolArray : SymbolType
{
    private int _leftBound;
    private int _rightBound;

    public SymbolType ItemsType { get; }

    public SymbolArray(string name, SymbolType type, int leftBound, int rightBound) : base(name)
    {
        ItemsType = type;
        _leftBound = leftBound;
        _rightBound = rightBound;
    }

    public int GetIndexesCount()
    {
        var result = 1;
        if (ItemsType is SymbolArray symbolArray)
        {
            result += symbolArray.GetIndexesCount();
        }

        return result;
    }

    public override string GetPrint(int level)
    {
        var result = "";
        for (var i = 0; i < level; i++)
        {
            result += " ";
        }

        result += Name + " of " +  ItemsType.GetPrint(0);

        return result;
    }
}