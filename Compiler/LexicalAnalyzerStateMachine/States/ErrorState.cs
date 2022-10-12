namespace Compiler.LexicalAnalyzerStateMachine.States;

public class ErrorState : IState
{
    public IState GetNextState(char symbol)
    {
        return new ErrorState();
    }
}