using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class ColumnOperatorState : IState
{
    public IState GetNextState(int symbol)
    {
        if (symbol == OperatorConstants.Equal)
        {
            return new AssignmentOperatorState();
        }

        return new OperatorEndState();
    }
}