namespace Compiler.Semantic;

public class SymbolArray : SymbolType
{
    private SymbolType _itemsType;
    private int _leftBound;
    private int _rightBound;

    public SymbolArray(string name, SymbolType type, int leftBound, int rightBound) : base(name)
    {
        _itemsType = type;
        _leftBound = leftBound;
        _rightBound = rightBound;
    }

    public int GetIndexesCount()
    {
        var result = 1;
        if (_itemsType is SymbolArray symbolArray)
        {
            result += symbolArray.GetIndexesCount();
        }

        return result;
    }
}