namespace Compiler.LexicalAnalyzerStateMachine.States;

public class EqualOperatorState : IState
{
    public IState GetNextState(int symbol)
    {
        return new OperatorEndState();
    }
}