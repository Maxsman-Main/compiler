namespace Compiler.LexicalAnalyzerStateMachine.States;

public class BinEndState : IState, IEndState
{
    public IState GetNextState(int symbol)
    {
        return new BinEndState();
    }
}