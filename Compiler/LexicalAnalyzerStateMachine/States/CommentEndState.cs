namespace Compiler.LexicalAnalyzerStateMachine.States;

public class CommentEndState : IState, IEndState
{
    public IState GetNextState(int symbol)
    {
        return new CommentEndState();
    }
}