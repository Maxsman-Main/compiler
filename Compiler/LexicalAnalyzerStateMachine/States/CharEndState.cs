namespace Compiler.LexicalAnalyzerStateMachine.States;

public class CharEndState : IState, IEndState
{
    public IState GetNextState(int symbol)
    {
        return new CharEndState();
    }
}