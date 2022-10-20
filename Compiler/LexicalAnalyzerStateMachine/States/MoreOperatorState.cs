using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class MoreOperatorState : IState
{
    public IState GetNextState(int symbol)
    {
        if (symbol == OperatorConstants.Equal)
        {
            return new MoreEqualOperatorState();
        }

        return new OperatorEndState();
    }
}