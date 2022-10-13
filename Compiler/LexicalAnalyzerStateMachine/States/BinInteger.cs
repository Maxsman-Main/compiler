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

        return new ErrorState();
    }
}