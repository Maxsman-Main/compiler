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

        if (LexemesSeparators.ContainSymbol(symbol))
        {
            return new EndState();
        }

        return new ErrorState();
    }
}