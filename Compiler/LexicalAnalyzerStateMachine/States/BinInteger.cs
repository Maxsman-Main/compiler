﻿using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class BinInteger : IState
{
    public IState GetNextState(int symbol)
    {
        if (symbol == LexemesSeparators.EndOfFile)
        {
            return new BinEndState();
        }
        
        if(LexemesSeparators.VisibleSeparators.Contains((char)symbol))
        {
            return new BinEndState();
        }
        
        if (IntegerConstants.NumbersBin.Contains((char)symbol))
        {
            return new BinInteger();
        }

        if (LexemesSeparators.ContainSymbol(symbol))
        {
            return new BinEndState();
        }

        if (OperatorConstants.Operators.Contains((char)symbol))
        {
            return new BinEndState();
        }
        
        return new ErrorState("Incorrect lexeme, 0-1 or some separator excepted, but " +(char)symbol + " found");
    }
}