using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class FloatState : IState
{
    public IState GetNextState(char symbol)
    {
        if (FloatConstants.NumbersFloat.Contains(symbol))
        {
            return new FloatState();
        }

        return new ErrorState();
    }
}