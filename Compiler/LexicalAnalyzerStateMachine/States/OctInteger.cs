using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class OctInteger : IState
{
    public IState GetNextState(char symbol)
    {
        if (IntegerConstants.NumbersOct.Contains(symbol))
        {
            return new OctInteger();
        }

        return new ErrorState();
    }
}