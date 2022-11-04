using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class ErrorState : IState
{
    public IState GetNextState(int symbol)
    {
        return new ErrorEndState();
    }
}