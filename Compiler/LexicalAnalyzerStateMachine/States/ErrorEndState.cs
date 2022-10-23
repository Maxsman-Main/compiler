namespace Compiler.LexicalAnalyzerStateMachine.States;

public class ErrorEndState : IState, IEndState
{
    public IState GetNextState(int symbol)
    {
        return new ErrorEndState();
    }
}