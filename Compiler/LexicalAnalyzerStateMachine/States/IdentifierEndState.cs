namespace Compiler.LexicalAnalyzerStateMachine.States;

public class IdentifierEndState : IState, IEndState
{
    public IState GetNextState(int symbol)
    {
        return new IdentifierEndState();
    }
}