namespace Compiler.LexicalAnalyzerStateMachine.States;

public class EndState : IState
{
    public IState GetNextState(int symbol)
    {
        return new EndState();
    }
}