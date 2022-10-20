﻿using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class StringWordEndState : IState
{
    public IState GetNextState(int symbol)
    {
        if (LexemesSeparators.ContainSymbol(symbol))
        {
            return new StringEndState();
        }
        
        if (LexemesSeparators.VisibleSeparators.Contains((char) symbol))
        {
            return new StringEndState();
        }
        
        if (OperatorConstants.Operators.Contains((char)symbol))
        {
            return new StringEndState();
        }

        return new ErrorState();
    }
}