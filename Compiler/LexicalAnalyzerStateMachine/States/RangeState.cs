namespace Compiler.LexicalAnalyzerStateMachine.States;

public class RangeState : IState
{
    public IState GetNextState(int symbol)
    {
        return new RangeState();
    }
}