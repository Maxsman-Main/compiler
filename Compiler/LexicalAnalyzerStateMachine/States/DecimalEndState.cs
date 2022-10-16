namespace Compiler.LexicalAnalyzerStateMachine.States;

public class DecimalEndState : IState, IEndState
{
    public IState GetNextState(int symbol)
    {
        return new DecimalEndState();
    }
}