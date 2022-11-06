namespace Compiler.LexicalAnalyzerStateMachine.States;

public class ErrorEndState : IState, IEndState, IErrorState
{
    private readonly string _message;

    public string Message => _message;

    public ErrorEndState(string message)
    {
        _message = message;
    }
    
    public IState GetNextState(int symbol)
    {
        
        return new ErrorEndState(_message);
    }
}