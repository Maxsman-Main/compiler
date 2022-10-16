namespace Compiler.LexicalAnalyzerStateMachine.States;

public class OctEndState : IState, IEndState
{
    public IState GetNextState(int symbol)
    {
        return new OctEndState();
    }
}