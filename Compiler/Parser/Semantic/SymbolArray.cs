namespace Compiler.Parser.Semantic;

public class SymbolArray
{
    private SymbolType _itemsType;
    private int _leftBound;
    private int _rightBound;

    public SymbolArray(SymbolType type, int leftBound, int rightBound)
    {
        _itemsType = type;
        _leftBound = leftBound;
        _rightBound = rightBound;
    }
}