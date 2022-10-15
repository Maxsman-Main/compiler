using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class StringStartWordState : IState
{
    public IState GetNextState(char symbol)
    {
        if (symbol == StringConstants.StringSymbol)
        {
            return new CharState();
        }
        
        return new StringState();
    }
}