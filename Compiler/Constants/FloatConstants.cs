namespace Compiler.Constants;

public class FloatConstants
{
    private static List<char> _numbersFloat = new()
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
    };

    private static char _floatSymbol = '.';

    public static List<char> NumbersFloat => _numbersFloat;
    public static char FloatSymbol => _floatSymbol;
}