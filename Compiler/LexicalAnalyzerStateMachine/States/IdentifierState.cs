﻿using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class IdentifierState : IState
{
    public IState GetNextState(int symbol)
    {
        if (symbol == LexemesSeparators.EndOfFile)
        {
            return new IdentifierEndState();
        }
        
        if (IdentifierConstants.Symbols.Contains((char) symbol))
        {
            return new IdentifierState();
        }
        
        if (LexemesSeparators.ContainSymbol(symbol))
        {
            return new IdentifierEndState();
        }

        return new ErrorState();
    }
}