﻿namespace Compiler.Constants;

public static class LexemesSeparators
{
    private static char _carrigeReturn = (char)13;
    private static char _newLine = (char)10;

    private static readonly List<char> _separators = new()
    {
        ' ', _carrigeReturn, _newLine
    };

    public static int EndOfFile => -1;
    
    public static bool ContainSymbol(char symbol)
    {
        foreach(var separator in _separators)
        {
            if (separator == symbol)
            {
                return true;
            }
        }

        return false;
    }
    
    public static bool ContainSymbol(int symbol)
    {
        foreach(var separator in _separators)
        {
            if (separator == (char)symbol)
            {
                return true;
            }
        }

        return false;
    }
}