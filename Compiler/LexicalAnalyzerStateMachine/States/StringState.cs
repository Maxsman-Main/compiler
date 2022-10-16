using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class StringState : IState
{
    public IState GetNextState(int symbol)
    {
        if (symbol == LexemesSeparators.EndOfFile)
        {
            return new ErrorState();
        }
        
        if (symbol == StringConstants.StringSymbol)
        {
            return new StringWordEndState();
        }

        return new StringState();
    }
}