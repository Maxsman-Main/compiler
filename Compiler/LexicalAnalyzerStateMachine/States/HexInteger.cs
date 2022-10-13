using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class HexInteger : IState
{
    public IState GetNextState(char symbol)
    {
        if (Constant.NumbersHex.Contains(symbol))
        {
            return new HexInteger();
        }

        return new ErrorState();
    }
}