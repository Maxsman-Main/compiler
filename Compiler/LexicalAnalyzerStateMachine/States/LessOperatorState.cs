using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class LessOperatorState : IState
{
    public IState GetNextState(int symbol)
    {
        if (symbol == OperatorConstants.Equal)
        {
            return new LessEqualOperatorState();
        }

        if (symbol == OperatorConstants.MoreSign)
        {
            return new EqualOperatorState();
        }

        return new OperatorEndState();
    }
}