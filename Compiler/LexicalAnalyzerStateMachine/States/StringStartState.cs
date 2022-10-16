using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class StringStartState : IState
{
    public IState GetNextState(int symbol)
    {
        if (symbol == LexemesSeparators.EndOfFile)
        {
            return new ErrorState();
        }
        
        if (symbol != StringConstants.StringSymbol)
        {
            return new StringStartWordState();
        }
        
        return new CharState();
    }
}