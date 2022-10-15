namespace Compiler.LexicalAnalyzerStateMachine.States;

public class StringEndState : IState, IEndState
{
    public IState GetNextState(char symbol)
    {
        return new StringEndState();
    }
}