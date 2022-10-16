namespace Compiler.LexicalAnalyzerStateMachine.States;

public class BinEndState : IState, IEndState
{
    public IState GetNextState(char symbol)
    {
        return new BinEndState();
    }
}