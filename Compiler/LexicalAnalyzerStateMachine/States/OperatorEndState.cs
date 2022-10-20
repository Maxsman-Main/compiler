namespace Compiler.LexicalAnalyzerStateMachine.States;

public class OperatorEndState : IState, IEndState
{
    public IState GetNextState(int symbol)
    {
        return new OperatorEndState();
    }
}