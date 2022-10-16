namespace Compiler.LexicalAnalyzerStateMachine.States;

public class CharEndState : IState, IEndState
{
    public IState GetNextState(char symbol)
    {
        return new CharEndState();
    }
}