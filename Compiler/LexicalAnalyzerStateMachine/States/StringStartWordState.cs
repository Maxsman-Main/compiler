using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class StringStartWordState : IState
{
    public IState GetNextState(int symbol)
    {
        if (symbol == LexemesSeparators.EndOfFile)
        {
            return new ErrorState();
        }
        
        if (symbol == StringConstants.StringSymbol)
        {
            return new CharState();
        }
        
        return new StringState();
    }
}