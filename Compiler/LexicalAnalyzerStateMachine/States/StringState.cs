using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class StringState : IState
{
    public IState GetNextState(char symbol)
    {
        if (symbol == StringConstants.StringSymbol)
        {
            return new StringWordEndState();
        }

        return new StringState();
    }
}