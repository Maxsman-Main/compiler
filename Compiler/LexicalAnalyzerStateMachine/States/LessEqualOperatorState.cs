namespace Compiler.LexicalAnalyzerStateMachine.States;

public class LessEqualOperatorState : IState
{
    public IState GetNextState(int symbol)
    {
        return new OperatorEndState();
    }
}