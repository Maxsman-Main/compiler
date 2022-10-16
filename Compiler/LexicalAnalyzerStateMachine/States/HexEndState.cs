namespace Compiler.LexicalAnalyzerStateMachine.States;

public class HexEndState : IState, IEndState
{
    public IState GetNextState(int symbol)
    {
        return new HexEndState();
    }
}