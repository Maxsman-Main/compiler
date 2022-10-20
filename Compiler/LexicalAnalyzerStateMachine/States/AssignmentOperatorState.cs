namespace Compiler.LexicalAnalyzerStateMachine.States;

public class AssignmentOperatorState : IState
{
    public IState GetNextState(int symbol)
    {
        return new OperatorEndState();
    }
}