namespace Compiler.LexicalAnalyzerStateMachine.States;

public class OperatorState : IState
{
    public IState GetNextState(int symbol)
    {
        return new OperatorEndState();
    }
}