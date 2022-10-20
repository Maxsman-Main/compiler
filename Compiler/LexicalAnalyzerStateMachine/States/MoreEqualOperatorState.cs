namespace Compiler.LexicalAnalyzerStateMachine.States;

public class MoreEqualOperatorState : IState
{
    public IState GetNextState(int symbol)
    {
        return new OperatorEndState();
    }
}