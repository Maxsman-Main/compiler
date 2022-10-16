namespace Compiler.LexicalAnalyzerStateMachine.States;

public class FloatEndState : IState, IEndState
{
    public IState GetNextState(char symbol)
    {
        return new FloatEndState();
    }
}