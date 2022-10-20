namespace Compiler.LexicalAnalyzerStateMachine.States;

public class SeparatorEndState : IState, IEndState
{
    public IState GetNextState(int symbol)
    {
        return new SeparatorEndState();
    }
}