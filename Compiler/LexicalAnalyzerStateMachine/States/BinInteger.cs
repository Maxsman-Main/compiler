using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class BinInteger : IState
{
    public IState GetNextState(char symbol)
    {
        if (IntegerConstants.NumbersBin.Contains(symbol))
        {
            return new BinInteger();
        }

        if (LexemesSeparators.ContainSymbol(symbol))
        {
            return new BinEndState();
        }

        return new ErrorState();
    }
}