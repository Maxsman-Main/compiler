namespace Compiler.LexicalAnalyzerStateMachine.States;

public class FloatEndState : IState, IEndState
{
    public IState GetNextState(int symbol)
    {
        return new FloatEndState();
    }
}