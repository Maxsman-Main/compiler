namespace Compiler.LexicalAnalyzerStateMachine.States;

public class HexEndState : IState, IEndState
{
    public IState GetNextState(char symbol)
    {
        return new HexEndState();
    }
}