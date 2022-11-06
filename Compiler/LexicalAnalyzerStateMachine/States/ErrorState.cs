using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class ErrorState : IState
{
    private readonly string _message;

    public ErrorState(string message)
    {
        _message = message;
    }
    
    public IState GetNextState(int symbol)
    {
        return new ErrorEndState(_message);
    }
}