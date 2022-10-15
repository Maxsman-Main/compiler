namespace Compiler.LexicalAnalyzerStateMachine.States;

public class CharState : IState
{
    public IState GetNextState(char symbol)
    {
        return new EndState();
    }
}