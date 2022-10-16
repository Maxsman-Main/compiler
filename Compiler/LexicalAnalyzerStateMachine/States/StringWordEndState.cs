using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class StringWordEndState : IState
{
    public IState GetNextState(int symbol)
    {
        if (LexemesSeparators.ContainSymbol(symbol))
        {
            return new StringEndState();
        }

        return new ErrorState();
    }
}