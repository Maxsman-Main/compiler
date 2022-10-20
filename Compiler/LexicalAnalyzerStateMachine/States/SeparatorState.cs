namespace Compiler.LexicalAnalyzerStateMachine.States;

public class SeparatorState : IState
{
    public IState GetNextState(int symbol)
    {
        return new SeparatorEndState();
    }
}