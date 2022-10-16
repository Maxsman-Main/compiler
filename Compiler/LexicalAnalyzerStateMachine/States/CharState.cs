using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class CharState : IState
{
    public IState GetNextState(char symbol)
    {
        if (LexemesSeparators.ContainSymbol(symbol))
        {
            return new CharEndState();
        }

        return new ErrorState();
    }
}