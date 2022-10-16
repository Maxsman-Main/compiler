using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class HexInteger : IState
{
    public IState GetNextState(char symbol)
    {
        if (IntegerConstants.NumbersHex.Contains(symbol))
        {
            return new HexInteger();
        }

        if (LexemesSeparators.ContainSymbol(symbol))
        {
            return new HexEndState();
        }

        return new ErrorState();
    }
}