namespace Compiler.LexicalAnalyzerStateMachine.States;

public class OctEndState : IState, IEndState
{
    public IState GetNextState(char symbol)
    {
        return new OctEndState();
    }
}