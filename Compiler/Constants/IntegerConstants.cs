using Compiler.Lexeme;

namespace Compiler.Constants;

public static class IntegerConstants
{
    private static List<char> _numbersDecimal = new()
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
    };
    
    private static List<char> _numbersHex = new()
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
        'A', 'B', 'C', 'D', 'E', 'F', 'a', 'b', 'c', 'e', 'f'
    };

    private static List<char> _numbersOct = new()
    {
        '0', '1', '2', '3', '4', '5', '6', '7'
    };

    private static List<char> _numbersBin = new()
    {
        '0', '1'
    };

    private static char _hexSymbol = '$';
    private static char _octSymbol = '&';
    private static char _binSymbol = '%';
    private static char _integerPlusSybmol = '+';
    private static char _integerMinusSybmol = '-';
    public static List<char> NumbersDecimal => _numbersDecimal;
    public static List<char> NumbersHex => _numbersHex;
    public static List<char> NumbersOct => _numbersOct;
    public static List<char> NumbersBin => _numbersBin;
    public static char HexSymbol => _hexSymbol;
    public static char OctSymbol => _octSymbol;
    public static char BinSymbol => _binSymbol;
    public static char PlusSign => _integerPlusSybmol;
    public static char MinusSign => _integerMinusSybmol;
}