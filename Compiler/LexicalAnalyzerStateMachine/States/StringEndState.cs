﻿namespace Compiler.LexicalAnalyzerStateMachine.States;

public class StringEndState : IState, IEndState
{
    public IState GetNextState(int symbol)
    {
        return new StringEndState();
    }
}