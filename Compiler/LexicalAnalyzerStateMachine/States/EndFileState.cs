namespace Compiler.LexicalAnalyzerStateMachine.States;

public class EndFileState : IState, IEndState
{
    public IState GetNextState(char symbol)
    {
        return new EndFileState();
    }
}