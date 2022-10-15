namespace Compiler.LexicalAnalyzerStateMachine.States;

public class DecimalEndState : IState, IEndState
{
    public IState GetNextState(char symbol)
    {
        return new DecimalEndState();
    }
}